using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.WebHost.Validators;

public static class DataValidator
{
    public static bool PartnerIsValid( this ControllerBase controller, Partner partner, out IActionResult actionResult )
    {
        actionResult = null;

        // Если партнёр равен null, то ошибка
        if ( partner == null )
        {
            actionResult = controller.NotFound();
            return false;
        }

        // Если партнер заблокирован, то ошибка
        if ( !partner.IsActive )
        {
            actionResult = controller.BadRequest( "Данный партнер не активен" );
            return false;
        }

        return true;
    }
}
