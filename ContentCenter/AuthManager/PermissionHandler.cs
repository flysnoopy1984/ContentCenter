using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.AuthManager
{
    public class PermissionHandler : AuthorizationHandler<PermissionItem>
    {
      
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionItem requirement)
        {
            if (requirement.Role == "Admin")
                context.Succeed(requirement);
            else
                context.Fail();

            return Task.CompletedTask;
        }
    }
}
