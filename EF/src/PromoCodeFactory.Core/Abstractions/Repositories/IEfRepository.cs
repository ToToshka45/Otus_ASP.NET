using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
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
        /// <param name="cancellationToken"></param>
        /// <returns> Cущность. </returns>
        Task<T> GetAsync( Guid id, CancellationToken cancellationToken = default );

        /// <summary>
        /// Получить набор сущностей по набору Id.
        /// </summary>
        /// <param name="ids"> Ids сущностей. </param>
        /// <param name="cancellationToken"></param>
        /// <returns> Cущность. </returns>
        Task<List<T>> GetAsyncByIds( IEnumerable<Guid> ids, CancellationToken cancellationToken = default );

        /// <summary>
        /// Запросить все сущности в базе.
        /// </summary>
        /// <param name="cancellationToken"> Токен отмены. </param>
        /// <returns> Список сущностей. </returns>
        Task<List<T>> GetAllAsync( CancellationToken cancellationToken = default );

        /// <summary>
        /// Удалить сущность.
        /// </summary>
        /// <param name="id"> Id удалённой сущности. </param>
        /// <returns> Была ли сущность удалена. </returns>
        Task<bool> DeleteAsync( Guid id, CancellationToken cancellationToken = default );

        /// <summary>
        /// Удалить сущность.
        /// </summary>
        /// <param name="entity"> Cущность для удаления. </param>
        /// <returns> Была ли сущность удалена. </returns>
        Task<bool> DeleteAsync( T entity, CancellationToken cancellationToken = default );

        /// <summary>
        /// Для сущности проставить состояние - что она изменена.
        /// </summary>
        /// <param name="entity"> Сущность для изменения. </param>
        /// <returns> Была ли сущность обновлена. </returns>
        Task<bool> UpdateAsync( T entity, CancellationToken cancellationToken = default );

        /// <summary>
        /// Добавить в базу одну сущность.
        /// </summary>
        /// <param name="entity"> Сущность для добавления. </param>
        /// <returns> Добавленная сущность. </returns>
        Task<T> AddAsync( T entity, CancellationToken cancellationToken = default );

        /// <summary>
        /// Добавить в базу массив сущностей.
        /// </summary>
        /// <param name="entities"> Массив сущностей. </param>
        Task AddRangeAsync( IEnumerable<T> entities, CancellationToken cancellationToken = default );

        /// <summary>
        /// Сохранить изменения.
        /// </summary>
        Task SaveChangesAsync( CancellationToken cancellationToken = default );
    }
}
