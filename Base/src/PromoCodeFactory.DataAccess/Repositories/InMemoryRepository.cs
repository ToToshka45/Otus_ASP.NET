using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected ICollection<T> Data { get; set; }

        public InMemoryRepository( IEnumerable<T> data )
        {
            Data = new Collection<T>( data.ToList() );
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult( (IEnumerable<T>) Data );
        }

        public Task<T> GetByIdAsync( Guid id )
        {
            return Task.FromResult( Data.FirstOrDefault( x => x.Id == id ) );
        }

        public Task<T> AddAsync( T newItem )
        {
            if ( Data is ICollection<T> collection )
            {
                Data.Add( newItem );
            }

            return Task.FromResult( newItem );
        }

        public Task<T> UpdateAsync( T updItem )
        {
            var item = Data.FirstOrDefault( x => x.Id == updItem.Id );

            if ( item is not null )
            {
                Data.Remove( item );
            }

            Data.Add( updItem );

            return Task.FromResult( updItem );
        }

        public Task<bool> DeleteByIdAsync( Guid id )
        {
            bool result = false;

            var item = Data.FirstOrDefault( x => x.Id == id );

            if ( item is not null )
            {
                Data.Remove( item );
                result = true;
            }

            return Task.FromResult( result );
        }
    }
}