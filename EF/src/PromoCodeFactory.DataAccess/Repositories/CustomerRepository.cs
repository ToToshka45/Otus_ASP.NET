using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class CustomerRepository : EfRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository( DatabaseContext context ) : base( context )
        {
        }

        /// <summary>
        /// Получить сущность по Id.
        /// </summary>
        /// <param name="id"> Id сущности. </param>
        /// <param name="cancellationToken"> Токен отмены. </param>
        /// <returns> Найденную сущность или <see langword="null" />.</returns>
        public override async Task<Customer> GetAsync( Guid id, CancellationToken cancellationToken )
        {
            var query = _entitySet.AsQueryable();
            query = query
                .Where( c => c.Id == id )
                .Include( c => c.PromoCodes )
                .Include( c => c.CustomerPreferences );

            return await query.SingleOrDefaultAsync( cancellationToken );
        }

        /// <summary>
        /// Запросить все сущности в базе с указанным предпочтением.
        /// </summary>
        /// <param name="preference"> Предпочтение. </param>
        /// <param name="cancellationToken"> Токен отмены. </param>
        /// <returns> Список сущностей. </returns>
        public async Task<List<Customer>> GetAllByPreferenceAsync( string preference, CancellationToken cancellationToken )
        {
            var query =  _entitySet.Join( Context.Set<CustomerPreference>(), a => a.Id, b => b.CustomerId, ( a, b ) => new { A = a, B = b } )
                                   .Join( Context.Set<Preference>(), ab => ab.B.PreferenceId, c => c.Id, ( ab, c ) => new { A = ab.A, B = ab.B, C = c } )
                                   .Where( customerWithPreference => customerWithPreference.C.Name == preference )
                                   .Select( customerWithPreference => customerWithPreference.A ).AsQueryable();

            return await query.ToListAsync( cancellationToken );
        }
    }
}
