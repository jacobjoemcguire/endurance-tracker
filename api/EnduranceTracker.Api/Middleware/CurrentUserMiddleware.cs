using EnduranceTracker.Core.Entities;
using EnduranceTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EnduranceTracker.Api.Middleware;

public class CurrentUserMiddleware
{
    private readonly RequestDelegate _next;

    public CurrentUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var objectId = context.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value
                ?? context.User.FindFirst("oid")?.Value;

            if (!string.IsNullOrEmpty(objectId))
            {
                var user = await dbContext.Users
                    .FirstOrDefaultAsync(u => u.EntraObjectId == objectId);

                if (user == null)
                {
                    user = new User
                    {
                        Id = Guid.NewGuid(),
                        EntraObjectId = objectId,
                        DisplayName = context.User.FindFirst("name")?.Value
                            ?? context.User.Identity.Name
                            ?? "Unknown"
                    };
                    dbContext.Users.Add(user);
                    await dbContext.SaveChangesAsync();
                }

                context.Items["CurrentUser"] = user;
            }
        }

        await _next(context);
    }
}
