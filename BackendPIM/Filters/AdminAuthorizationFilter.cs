using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BackendPIM.Filters;

public class AdminAuthorizationFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var token = context.HttpContext.Session.GetString("Token");
        var perfil = context.HttpContext.Session.GetString("Perfil");

        if (string.IsNullOrEmpty(token) || perfil != "Administrador")
        {
            context.Result = new RedirectToActionResult(
                "Index",
                "Login",
                null
            );
        }
    }
}