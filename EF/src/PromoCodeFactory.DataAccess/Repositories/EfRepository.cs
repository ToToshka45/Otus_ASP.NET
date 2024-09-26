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
        /// Получить сущность по ID.
        /// </summary>
        /// <param name="id"> Id сущности. </param>
        /// <returns> Найденную сущность или <see langword="null" />.</returns>
        public virtual T Get( Guid id )
        {
            return _entitySet.Find( id );
        }

        /// <summary>
        /// Получить сущность по Id.
        /// </summary>
        /// <param name="id"> Id сущности. </param>
        /// <param name="cancellationToken"></param>
        /// <returns> Найденную сущность или <see langword="null" />.</returns>
        public virtual async Task<T> GetAsync( Guid id, CancellationToken cancellationToken )
        {
            return await _entitySet.FindAsync( id, cancellationToken );
        }

        /// <summary>
        /// Запросить все сущности в базе.
        /// </summary>
        /// <returns> IQueryable массив сущностей. </returns>
        public virtual IQueryable<T> GetAll()
        {
            return _entitySet;
        }

        /// <summary>
        /// Запросить все сущности в базе.
        /// </summary>
        /// <param name="cancellationToken"> Токен отмены </param>
        /// <returns> Список сущностей. </returns>
        public virtual async Task<List<T>> GetAllAsync( CancellationToken cancellationToken )
        {
            return await GetAll().ToListAsync( cancellationToken );
        }

        /// <summary>
        /// Добавить в базу сущность.
        /// </summary>
        /// <param name="entity"> Cущность для добавления. </param>
        /// <returns> Добавленная сущность или <see langword="null" /> если передан <see langword="null" />.</returns>
        public virtual T Add( T entity )
        {
            if ( entity == null )
            {
                return null;
            }

            var objToReturn = _entitySet.Add( entity );
            return objToReturn.Entity;
        }

        /// <summary>
        /// Добавить в базу одну сущность.
        /// </summary>
        /// <param name="entity"> Сущность для добавления. </param>
        /// <returns> Добавленная сущность или <see langword="null" /> если передан <see langword="null" />.</returns>
        public virtual async Task<T> AddAsync( T entity )
        {
            if ( entity == null )
            {
                return null;
            }

            var objToReturn = await _entitySet.AddAsync( entity );
            return objToReturn.Entity;
        }

        /// <summary>
        /// Добавить в базу массив сущностей.
        /// </summary>
        /// <param name="entities"> Массив сущностей. </param>
        public virtual void AddRange( IEnumerable<T> entities )
        {
            if ( entities == null || !entities.Any() )
            {
                return;
            }

            _entitySet.AddRange( entities );
        }

        /// <summary>
        /// Добавить в базу массив сущностей.
        /// </summary>
        /// <param name="entities"> Массив сущностей. </param>
        public virtual async Task AddRangeAsync( IEnumerable<T> entities )
        {
            await _entitySet.AddRangeAsync( entities );
        }

        /// <summary>
        /// Для сущности проставить состояние - что она изменена.
        /// </summary>
        /// <param name="entity"> Сущность для изменения. </param>
        public virtual void Update( T entity )
        {
            if ( entity == null )
            {
                return;
            }

            Context.Entry( entity ).State = EntityState.Modified;
        }

        /// <summary>
        /// Удалить сущность.
        /// </summary>
        /// <param name="id"> Id удалённой сущности. </param>
        /// <returns> Была ли сущность удалена. </returns>
        public virtual bool Delete( Guid id )
        {
            var obj = _entitySet.Find( id );
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
        public virtual bool Delete( T entity )
        {
            if ( entity == null )
            {
                return false;
            }

            Context.Entry( entity ).State = EntityState.Deleted;

            return true;
        }

        /// <summary>
        /// Удалить сущности.
        /// </summary>
        /// <param name="entities"> Коллекция сущностей для удаления. </param>
        /// <returns> Была ли операция завершена успешно. </returns>
        public virtual bool DeleteRange( IEnumerable<T> entities )
        {
            if ( entities == null || !entities.Any() )
            {
                return false;
            }

            _entitySet.RemoveRange( entities );

            return true;
        }

        /// <summary>
        /// Сохранить изменения.
        /// </summary>
        public virtual void SaveChanges()
        {
            Context.SaveChanges();
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
