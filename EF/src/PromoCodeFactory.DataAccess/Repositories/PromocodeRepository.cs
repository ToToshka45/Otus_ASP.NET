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
    public class PromocodeRepository : EfRepository<PromoCode>, IPromocodeRepository
    {
        public PromocodeRepository( DatabaseContext context ) : base( context )
        {
        }

        /// <summary>
        /// Получить сущность по Id.
        /// </summary>
        /// <param name="id"> Id сущности. </param>
        /// <param name="cancellationToken"> Токен отмены </param>
        /// <returns> Найденную сущность или <see langword="null" />.</returns>
        public override async Task<PromoCode> GetAsync( Guid id, CancellationToken cancellationToken )
        {
            var query = _entitySet.AsQueryable();
            query = query
                .Where( p => p.Id == id )
                .Include( p => p.Preference );

            return await query.SingleOrDefaultAsync( cancellationToken );
        }

        /// <summary>
        /// Запросить все сущности в базе.
        /// </summary>
        /// <param name="cancellationToken"> Токен отмены </param>
        /// <param name="asNoTracking"> Вызвать с AsNoTracking. </param>
        /// <returns> Список сущностей. </returns>
        public override async Task<List<PromoCode>> GetAllAsync( CancellationToken cancellationToken )
        {
            var query = _entitySet.AsQueryable();
            query = query
                .Include( p => p.Preference );

            return await query.ToListAsync( cancellationToken );
        }
    }
}
