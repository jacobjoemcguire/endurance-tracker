using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnduranceTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var objectId = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
            ?? User.FindFirst("oid")?.Value;
        var name = User.FindFirst("name")?.Value
            ?? User.Identity?.Name;

        return Ok(new
        {
            ObjectId = objectId,
            DisplayName = name,
            Email = User.FindFirst("preferred_username")?.Value
        });
    }
}
