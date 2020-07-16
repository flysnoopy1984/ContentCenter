using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.ServiceSetup
{
    public static class Id4Setup
    {
        public const string apiName_ContentCenter = "ccApi";
        public static void ConfigId4Auth(this IServiceCollection services, IConfiguration configuration)
        {
            var ad4Url = configuration["Id4Config:Id4Url"];
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", o =>
                {
                    o.Authority = ad4Url;
                    o.RequireHttpsMetadata = false;
                    o.Audience = apiName_ContentCenter;
                   
                });
        }
    }
}
