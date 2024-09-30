using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.EntityFramework;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class PreferenceRepository : EfRepository<Preference>, IPreferenceRepository
    {
        public PreferenceRepository( DatabaseContext context ) : base( context )
        {
        }

        /// <summary>
        /// Получить сущность по Id.
        /// </summary>
        /// <param name="id"> Id сущности. </param>
        /// <param name="cancellationToken"> Токен отмены </param>
        /// <returns> Найденную сущность или <see langword="null" />.</returns>
        public override async Task<Preference> GetAsync( Guid id, CancellationToken cancellationToken = default )
        {
            var query = _entitySet.AsQueryable();
            query = query.Where( p => p.Id == id );

            return await query.SingleOrDefaultAsync( cancellationToken );
        }

        /// <summary>
        /// Получить сущность по Name.
        /// </summary>
        /// <param name="name"> Name сущности. </param>
        /// <param name="cancellationToken"> Токен отмены </param>
        /// <returns> Найденную сущность или <see langword="null" />.</returns>
        public async Task<Preference> GetByNameAsync( string name, CancellationToken cancellationToken = default )
        {
            var query = _entitySet.AsQueryable();
            query = query.Where( p => p.Name == name );

            return await query.SingleOrDefaultAsync( cancellationToken );
        }
    }
}
