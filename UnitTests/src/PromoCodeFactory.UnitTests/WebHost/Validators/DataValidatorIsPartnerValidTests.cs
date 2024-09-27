using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.UnitTests.WebHost.DefaultDataCreateHelpers;
using PromoCodeFactory.WebHost.Controllers;
using PromoCodeFactory.WebHost.Validators;
using Xunit;

namespace PromoCodeFactory.UnitTests.WebHost.Validators;

public class DataValidatorIsPartnerValidTests
{
    private readonly PartnersController _partnersController;

    public DataValidatorIsPartnerValidTests()
    {
        var fixture = new Fixture().Customize( new AutoMoqCustomization() );
        _partnersController = fixture.Build<PartnersController>().OmitAutoProperties().Create();
    }

    [Fact]
    public async void PartnerIsValid_PartnerIsNull_ReturnsNotFound()
    {
        // Arrange
        Partner partner = null;

        // Act
        var result = _partnersController.PartnerIsValid( partner, out var aResult );

        // Assert
        result.Should().BeFalse();
        aResult.Should().BeAssignableTo<NotFoundResult>();
    }

    [Fact]
    public async void PartnerIsValid_PartnerIsNotActive_ReturnsBadRequest()
    {
        // Arrange
        Partner partner = DefaultPartnerHelper.CreateBasePartner();
        partner.IsActive = false;

        // Act
        var result = _partnersController.PartnerIsValid( partner, out var aResult );

        // Assert
        result.Should().BeFalse();
        aResult.Should().BeAssignableTo<BadRequestObjectResult>();
    }
}
