using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories;

/// <summary>
/// UOW.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IPreferenceRepository _preferenceRepository;
    private readonly IPromocodeRepository _promocodeRepository;

    private DatabaseContext _context;

    public ICustomerRepository CustomerRepository => _customerRepository;
    public IPreferenceRepository PreferenceRepository => _preferenceRepository;
    public IPromocodeRepository PromocodeRepository => _promocodeRepository;

    public UnitOfWork( DatabaseContext context )
    {
        _context = context;

        _customerRepository = new CustomerRepository( context );
        _preferenceRepository = new PreferenceRepository( context );
        _promocodeRepository = new PromocodeRepository( context );
    }

    public async Task SaveChangesAsync( CancellationToken cancellationToken = default )
    {
        await _context.SaveChangesAsync( cancellationToken );
    }
}
