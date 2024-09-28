using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T> : IEfRepository<T> where T : BaseEntity
    {
        public readonly DbContext Context;
        public readonly DbSet<T> _entitySet;

        public EfRepository( DbContext context )
        {
            Context = context;
            _entitySet = Context.Set<T>();
        }

        /// <summary>
        /// Получить сущность по Id.
        /// </summary>
        /// <param name="id"> Id сущности. </param>
        /// <param name="cancellationToken"></param>
        /// <returns> Найденную сущность или <see langword="null" />.</returns>
        public virtual async Task<T> GetAsync( Guid id, CancellationToken cancellationToken = default )
        {
            return await _entitySet.FindAsync( id, cancellationToken );
        }

        /// <summary>
        /// Получить набор сущностей по набору Id.
        /// </summary>
        /// <param name="ids"> Ids сущностей. </param>
        /// <param name="cancellationToken"></param>
        /// <returns> Cущность. </returns>
        public virtual async Task<List<T>> GetAsyncByIds( IEnumerable<Guid> ids, CancellationToken cancellationToken = default )
        {
            var query = _entitySet
                .Where( item => ids.Any( id => item.Id == id ) )
                .AsQueryable();

            return await query.ToListAsync( cancellationToken );
        }

        /// <summary>
        /// Запросить все сущности в базе.
        /// </summary>
        /// <param name="cancellationToken"> Токен отмены </param>
        /// <returns> Список сущностей. </returns>
        public virtual async Task<List<T>> GetAllAsync( CancellationToken cancellationToken = default )
        {
            return await _entitySet.ToListAsync( cancellationToken );
        }

        /// <summary>
        /// Добавить в базу одну сущность.
        /// </summary>
        /// <param name="entity"> Сущность для добавления. </param>
        /// <returns> Добавленная сущность или <see langword="null" /> если передан <see langword="null" />.</returns>
        public virtual async Task<T> AddAsync( T entity, CancellationToken cancellationToken = default )
        {
            if ( entity == null )
            {
                return null;
            }

            var objToReturn = await _entitySet.AddAsync( entity, cancellationToken );
            return objToReturn.Entity;
        }

        /// <summary>
        /// Добавить в базу массив сущностей.
        /// </summary>
        /// <param name="entities"> Массив сущностей. </param>
        public virtual async Task AddRangeAsync( IEnumerable<T> entities, CancellationToken cancellationToken = default )
        {
            await _entitySet.AddRangeAsync( entities, cancellationToken );
        }

        /// <summary>
        /// Для сущности проставить состояние - что она изменена.
        /// </summary>
        /// <param name="entity"> Сущность для изменения. </param>
        /// <returns> Была ли сущность обновлена. </returns>
        public virtual async Task<bool> UpdateAsync( T entity, CancellationToken cancellationToken = default )
        {
            if ( entity == null )
            {
                return false;
            }

            Context.Entry( entity ).State = EntityState.Modified;

            return true;
        }

        /// <summary>
        /// Удалить сущность.
        /// </summary>
        /// <param name="id"> Id удалённой сущности. </param>
        /// <returns> Была ли сущность удалена. </returns>
        public virtual async Task<bool> DeleteAsync( Guid id, CancellationToken cancellationToken = default )
        {
            var obj = await _entitySet.FindAsync( id, cancellationToken );
            if ( obj == null )
            {
                return false;
            }

            _entitySet.Remove( obj );

            return true;
        }

        /// <summary>
        /// Удалить сущность.
        /// </summary>
        /// <param name="entity"> Сущность для удаления. </param>
        /// <returns> Была ли сущность удалена. </returns>
        public virtual async Task<bool> DeleteAsync( T entity, CancellationToken cancellationToken = default )
        {
            if ( entity == null )
            {
                return false;
            }

            Context.Entry( entity ).State = EntityState.Deleted;

            return true;
        }

        /// <summary>
        /// Сохранить изменения.
        /// </summary>
        public virtual async Task SaveChangesAsync( CancellationToken cancellationToken = default )
        {
            await Context.SaveChangesAsync( cancellationToken );
        }
    }
}
