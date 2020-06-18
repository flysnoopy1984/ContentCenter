using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.AuthManager
{
    public class PermissionItem:IAuthorizationRequirement
    {
        public string Role { get; set; }
    }
}
