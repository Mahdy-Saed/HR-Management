using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HR_Carrer.Controllers
{
    public static class ControllerExtension
    {

        public static bool TryGetUserId(this ControllerBase controller, out Guid userId)
        {

           
            var userIdString = controller.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(userIdString, out userId);
        }


    }
}
