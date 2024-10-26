using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess;
using PromoCodeFactory.DataAccess.Repositories;
using PromoCodeFactory.UnitTests.WebHost.DefaultDataCreateHelpers;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Threading;
using Xunit;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitDBAsyncUsingMockTests_AutoFixture
    {
        private readonly IRepository<Partner> _partnersRepository;
        private readonly Mock<DataContext> _dataContextMock;

        public SetPartnerPromoCodeLimitDBAsyncUsingMockTests_AutoFixture()
        {
            var fixture = new Fixture().Customize( new AutoMoqCustomization() );
            _dataContextMock = fixture.Freeze<Mock<DataContext>>();
            _partnersRepository = fixture.Build<EfRepository<Partner>>().OmitAutoProperties().Create();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerLimitIsSet_LimitSavedToDB()
        {
            // Arrange
            var partnerId = Guid.Parse( "FF87F725-1001-4873-ABC0-8FFC3413E11C" );
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.Now + TimeSpan.FromDays( 7 ),
                Limit = 10,
            };

            Partner partner = DefaultPartnerCreatorHelper.CreateBasePartner();

            var newLimit = new PartnerPromoCodeLimit()
            {
                Limit = request.Limit,
                Partner = partner,
                PartnerId = partner.Id,
                CreateDate = DateTime.Now,
                EndDate = request.EndDate
            };

            partner.PartnerLimits.Add( newLimit );

            _dataContextMock
                .Setup( repo => repo.SaveChangesAsync( true, CancellationToken.None ) )
                .ReturnsAsync( 1 );

            // Act
            await _partnersRepository.UpdateAsync( partner );

            // Assert
            _dataContextMock.Verify( m => m.SaveChangesAsync( It.IsAny<CancellationToken>() ), Times.Exactly( 1 ) );
        }

        public void Dispose()
        {

        }
    }
}
