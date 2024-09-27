using AutoFixture.AutoMoq;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Controllers;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using PromoCodeFactory.UnitTests.WebHost.DefaultDataCreateHelpers;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests
    {
        private readonly PartnersController _partnersController;
        private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;

        public SetPartnerPromoCodeLimitAsyncTests()
        {
            var fixture = new Fixture().Customize( new AutoMoqCustomization() );
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
            Partner partner = DefaultPartnerHelper.CreateBasePartner();
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
            Partner partner = DefaultPartnerHelper.CreateBasePartner();

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
            Partner partner = DefaultPartnerHelper.CreateBasePartner();
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
            Partner partner = DefaultPartnerHelper.CreateBasePartner();
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
            Partner partner = DefaultPartnerHelper.CreateBasePartner();

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
            Partner partner = DefaultPartnerHelper.CreateBasePartner();
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
            Partner partner = DefaultPartnerHelper.CreateBasePartner();
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

        public void Dispose()
        {

        }
    }
}