using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess;
using PromoCodeFactory.UnitTests.WebHost.DefaultDataCreateHelpers;
using PromoCodeFactory.WebHost.Controllers;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests
    {
        private readonly PartnersController _partnersController;
        private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;
        private readonly Mock<DataContext> _dataContextMock;

        public SetPartnerPromoCodeLimitAsyncTests()
        {
            var fixture = new Fixture().Customize( new AutoMoqCustomization() );
            _dataContextMock = fixture.Freeze<Mock<DataContext>>();
            _partnersRepositoryMock = fixture.Freeze<Mock<IRepository<Partner>>>();
            _partnersController = fixture.Build<PartnersController>().OmitAutoProperties().Create();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerIsNotFound_ReturnsNotFound()
        {
            // Arrange
            var partnerId = Guid.Parse( "FF87F725-1001-4873-ABC0-8FFC3413E11C" );
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.Now + TimeSpan.FromDays(7),
                Limit = 10,
            };
            Partner partner = null;

            _partnersRepositoryMock
                .Setup(repo => repo.GetByIdAsync( partnerId ) )
                .ReturnsAsync( partner );

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync( partnerId, request );

            // Assert
            result.Should().BeAssignableTo<NotFoundResult>();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerIsNotActive_ReturnsBadRequest()
        {
            // Arrange
            var partnerId = Guid.Parse( "FF87F725-1001-4873-ABC0-8FFC3413E11C" );
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.Now + TimeSpan.FromDays( 7 ),
                Limit = 10,
            };
            Partner partner = DefaultPartnerCreatorHelper.CreateBasePartner();
            partner.IsActive = false;

            _partnersRepositoryMock
                .Setup( repo => repo.GetByIdAsync( partnerId ) )
                .ReturnsAsync( partner );

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync( partnerId, request );

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerLimitIsBelowZero_ReturnsBadRequest() // Дополнительный тест
        {
            // Arrange
            var partnerId = Guid.Parse( "FF87F725-1001-4873-ABC0-8FFC3413E11C" );
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.Now + TimeSpan.FromDays( 7 ),
                Limit = -1,
            };
            Partner partner = DefaultPartnerCreatorHelper.CreateBasePartner();

            _partnersRepositoryMock
                .Setup( repo => repo.GetByIdAsync( partnerId ) )
                .ReturnsAsync( partner );

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync( partnerId, request );

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerLimitIsSet_NumberIssuedPromoCodesIsZero()
        {
            // Arrange
            var partnerId = Guid.Parse( "FF87F725-1001-4873-ABC0-8FFC3413E11C" );
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.Now + TimeSpan.FromDays( 7 ),
                Limit = 10,
            };
            Partner partner = DefaultPartnerCreatorHelper.CreateBasePartner();
            partner.NumberIssuedPromoCodes = 3;

            _partnersRepositoryMock
                .Setup( repo => repo.GetByIdAsync( partnerId ) )
                .ReturnsAsync( partner );

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync( partnerId, request );

            // Assert
            result.Should().BeAssignableTo<CreatedAtActionResult>();
            partner.NumberIssuedPromoCodes.Should().Be(0);
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerLimitIsSet_PreviousLimitIsCanceld()
        {
            // Arrange
            var partnerId = Guid.Parse( "FF87F725-1001-4873-ABC0-8FFC3413E11C" );
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.Now + TimeSpan.FromDays( 7 ),
                Limit = 10,
            };
            Partner partner = DefaultPartnerCreatorHelper.CreateBasePartner();
            var partnerLimit = partner.PartnerLimits.FirstOrDefault();

            _partnersRepositoryMock
                .Setup( repo => repo.GetByIdAsync( partnerId ) )
                .ReturnsAsync( partner );

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync( partnerId, request );

            // Assert
            result.Should().BeAssignableTo<CreatedAtActionResult>();
            partnerLimit.CancelDate.Should().HaveValue();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_PartnerLimitIsSet_LimitIsGreaterThanZero()
        {
            // Arrange
            var partnerId = Guid.Parse( "FF87F725-1001-4873-ABC0-8FFC3413E11C" );
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.Now + TimeSpan.FromDays( 7 ),
                Limit = 10,
            };
            Partner partner = DefaultPartnerCreatorHelper.CreateBasePartner();

            _partnersRepositoryMock
                .Setup( repo => repo.GetByIdAsync( partnerId ) )
                .ReturnsAsync( partner );

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync( partnerId, request );

            // Assert
            result.Should().BeAssignableTo<CreatedAtActionResult>();

            var createdAtActionResult = result as CreatedAtActionResult;
            var createdLimitId = (Guid) createdAtActionResult.RouteValues[ "limitId" ];

            var partnerLimit = partner.PartnerLimits.FirstOrDefault( limit => limit.Id == createdLimitId );
            partnerLimit.Limit.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_CurrentLimitIsEmpty_PartnerLimitIsSet() // Дополнительный тест
        {
            // Arrange
            var partnerId = Guid.Parse( "FF87F725-1001-4873-ABC0-8FFC3413E11C" );
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.Now + TimeSpan.FromDays( 7 ),
                Limit = 10,
            };
            Partner partner = DefaultPartnerCreatorHelper.CreateBasePartner();
            partner.PartnerLimits.Clear();

            _partnersRepositoryMock
                .Setup( repo => repo.GetByIdAsync( partnerId ) )
                .ReturnsAsync( partner );

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync( partnerId, request );

            // Assert
            result.Should().BeAssignableTo<CreatedAtActionResult>();
            partner.PartnerLimits.Should().HaveCount(1);
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_CurrentLimitIsEnded_NumberIssuedPromoCodesIsNotModified() // Дополнительный тест
        {
            // Arrange
            var numberIssuedPromoCodes = 3;
            var partnerId = Guid.Parse( "FF87F725-1001-4873-ABC0-8FFC3413E11C" );
            var request = new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = DateTime.Now + TimeSpan.FromDays( 7 ),
                Limit = 10,
            };
            Partner partner = DefaultPartnerCreatorHelper.CreateBasePartner();
            partner.NumberIssuedPromoCodes = numberIssuedPromoCodes;
            partner.PartnerLimits.FirstOrDefault().EndDate = DateTime.Now - TimeSpan.FromDays( 1 );

            _partnersRepositoryMock
                .Setup( repo => repo.GetByIdAsync( partnerId ) )
                .ReturnsAsync( partner );

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync( partnerId, request );

            // Assert
            result.Should().BeAssignableTo<CreatedAtActionResult>();
            partner.NumberIssuedPromoCodes.Should().Be( numberIssuedPromoCodes );
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

            _partnersRepositoryMock
                .Setup( repo => repo.GetByIdAsync( partnerId ) )
                .ReturnsAsync( partner );

            _partnersRepositoryMock
                .Setup( repo => repo.UpdateAsync( partner ) );

            _dataContextMock
                .Setup( repo => repo.SaveChangesAsync( CancellationToken.None ) );

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync( partnerId, request );

            // Assert
            result.Should().BeAssignableTo<CreatedAtActionResult>();

            //var createdAtActionResult = result as CreatedAtActionResult;
            //var createdLimitId = (Guid) createdAtActionResult.RouteValues[ "limitId" ];
            //var returnedPartnerId = (Guid) createdAtActionResult.RouteValues[ "id" ];

            //returnedPartnerId.Should().Be( partnerId );
            //createdLimitId.Should().NotBeEmpty();

            _partnersRepositoryMock.Verify( m => m.GetByIdAsync( It.IsAny<Guid>() ), Times.Exactly( 1 ) );
            _partnersRepositoryMock.Verify( m => m.UpdateAsync( It.IsAny<Partner>() ), Times.Exactly( 1 ) );
            _dataContextMock.Verify( m => m.SaveChangesAsync( It.IsAny<CancellationToken>() ), Times.Exactly( 1 ) );
        }

        public void Dispose()
        {

        }
    }
}