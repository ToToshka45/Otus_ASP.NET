using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess;
using PromoCodeFactory.WebHost.Controllers;
using PromoCodeFactory.WebHost.Models;
using System;
using Xunit;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitDBAsyncTests : IClassFixture<TestFixture_DB>
    {
        private readonly PartnersController _partnersController;
        private readonly IRepository<Partner> _partnersRepository;

        public SetPartnerPromoCodeLimitDBAsyncTests( TestFixture_DB testFixture )
        {
            var serviceProvider = testFixture.ServiceProvider;
            var dbContext = serviceProvider.GetService<DataContext>();
            _partnersRepository = serviceProvider.GetService<IRepository<Partner>>();
            _partnersController = new PartnersController( _partnersRepository );
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerLimitIsSet_LimitSavedToDB()
        {
            // Arrange
            var partnerId = Guid.Parse( "0B1FB4BF-4974-4EED-A707-2736D68FBAB2" );
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.Now + TimeSpan.FromDays( 7 ),
                Limit = 10,
            };

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync( partnerId, request );

            // Assert
            result.Should().BeAssignableTo<CreatedAtActionResult>();

            var createdAtActionResult = result as CreatedAtActionResult;
            var createdLimitId = (Guid) createdAtActionResult.RouteValues[ "limitId" ];
            var returnedPartnerId = (Guid) createdAtActionResult.RouteValues[ "id" ];

            returnedPartnerId.Should().Be( partnerId );

            var returnedPartner = await _partnersRepository.GetByIdAsync( returnedPartnerId );

            // Убеждаемся что лимит был создан в БД
            returnedPartner.PartnerLimits.Should().Contain( limit => limit.Id == createdLimitId );
        }

        public void Dispose()
        {

        }
    }
}