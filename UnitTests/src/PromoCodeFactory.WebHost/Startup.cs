using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.DataAccess;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.DataAccess.Repositories;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.WebHost
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddMvcOptions(x=> 
                x.SuppressAsyncSuffixInActionNames = false);
            services.AddScoped( typeof( IRepository<Partner> ), typeof( EfRepository<Partner> ) );
            services.AddScoped<IDbInitializer, EfDbInitializer>();

            services.AddDbContext<DataContext>(x =>
            {
                x.UseSqlite("Filename=PromoCodeFactoryDb.sqlite");
                //x.UseNpgsql(Configuration.GetConnectionString("PromoCodeFactoryDb"));
                x.UseSnakeCaseNamingConvention();
                x.UseLazyLoadingProxies();
            });

            services.AddTransient<DbContext, DataContext>();

            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory API Doc";
                options.Version = "1.0";
            });
        }

        public IServiceProvider GetServiceProvider( IServiceCollection services )
        {
            var serviceProvider = services
                .BuildServiceProvider();

            //using ( var scope = services.CreateScope() )
            //{
                var db = serviceProvider.GetRequiredService<DataContext>();
                db.Database.EnsureDeleted();
                db.Database.Migrate();
                Seed( db );
            //}

            return serviceProvider;
        }

        private static void Seed( DataContext dataContext )
        {
            var partner = new Partner()
            {
                Id = Guid.Parse( "0B1FB4BF-4974-4EED-A707-2736D68FBAB2" ),
                Name = "GameBy",
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
                {
                    new PartnerPromoCodeLimit()
                    {
                        Id = Guid.Parse("E17C1E81-ED9C-4EEF-9857-FA29963813D7"),
                        CreateDate = DateTime.Now - TimeSpan.FromDays( 3 ),
                        EndDate = DateTime.Now + TimeSpan.FromDays( 7 ),
                        Limit = 10
                    }
                }
            };

            dataContext.Add<Partner>( partner );

            dataContext.SaveChanges();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi(x =>
            {
                x.DocExpansion = "list";
            });
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            dbInitializer.InitializeDb();
        }
    }
}