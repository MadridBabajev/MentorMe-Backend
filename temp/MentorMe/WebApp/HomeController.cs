using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

/// <summary>
/// This controller redirects the user to the Swagger documentation page
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// Handles the redirection
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {
        return Redirect("/swagger");
    }
}