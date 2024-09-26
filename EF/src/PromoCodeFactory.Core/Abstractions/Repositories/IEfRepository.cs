using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IEfRepository<T>
        where T : BaseEntity
    {
        /// <summary>
        /// Получить сущность по Id.
        /// </summary>
        /// <param name="id"> Id сущности. </param>
        /// <returns> Cущность. </returns>
        T Get( Guid id );

        /// <summary>
        /// Получить сущность по Id.
        /// </summary>
        /// <param name="id"> Id сущности. </param>
        /// <param name="cancellationToken"></param>
        /// <returns> Cущность. </returns>
        Task<T> GetAsync( Guid id, CancellationToken cancellationToken );

        /// <summary>
        /// Запросить все сущности в базе.
        /// </summary>
        /// <returns> IQueryable массив сущностей.</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Запросить все сущности в базе.
        /// </summary>
        /// <param name="cancellationToken"> Токен отмены. </param>
        /// <returns> Список сущностей. </returns>
        Task<List<T>> GetAllAsync( CancellationToken cancellationToken );

        /// <summary>
        /// Удалить сущность.
        /// </summary>
        /// <param name="id"> Id удалённой сущности. </param>
        /// <returns> Была ли сущность удалена. </returns>
        bool Delete( Guid id );

        /// <summary>
        /// Удалить сущность.
        /// </summary>
        /// <param name="entity"> Cущность для удаления. </param>
        /// <returns> Была ли сущность удалена. </returns>
        bool Delete( T entity );

        /// <summary>
        /// Удалить сущности.
        /// </summary>
        /// <param name="entities"> Коллекция сущностей для удаления. </param>
        /// <returns> Была ли операция удаления успешна. </returns>
        bool DeleteRange( IEnumerable<T> entities );

        /// <summary>
        /// Для сущности проставить состояние - что она изменена.
        /// </summary>
        /// <param name="entity"> Сущность для изменения. </param>
        void Update( T entity );

        /// <summary>
        /// Добавить в базу одну сущность.
        /// </summary>
        /// <param name="entity"> Сущность для добавления. </param>
        /// <returns> Добавленная сущность. </returns>
        T Add( T entity );

        /// <summary>
        /// Добавить в базу одну сущность.
        /// </summary>
        /// <param name="entity"> Сущность для добавления. </param>
        /// <returns> Добавленная сущность. </returns>
        Task<T> AddAsync( T entity );

        /// <summary>
        /// Добавить в базу массив сущностей.
        /// </summary>
        /// <param name="entities"> Массив сущностей. </param>
        void AddRange( IEnumerable<T> entities );

        /// <summary>
        /// Добавить в базу массив сущностей.
        /// </summary>
        /// <param name="entities"> Массив сущностей. </param>
        Task AddRangeAsync( IEnumerable<T> entities );

        /// <summary>
        /// Сохранить изменения.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Сохранить изменения.
        /// </summary>
        Task SaveChangesAsync( CancellationToken cancellationToken = default );
    }
}
