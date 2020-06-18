
using ContentCenter.AuthManager;
using ContentCenter.Model.Commons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCenter.ServiceSetup
{
    public static class JwtSetup
    {
        public static void ConfigJWTAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(o => o.AddPolicy("CCPolicyAdmin",
            a => a.Requirements.Add(new PermissionItem()
            {
                Role = "Admin"
            })
           ));

            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                o => o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = CConstants.JWTIssuer,

                    ValidateAudience = true,
                    ValidAudience = CConstants.JWTAud,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(CConstants.JWTIssSerKey)),

                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                });

        }
    }
}
