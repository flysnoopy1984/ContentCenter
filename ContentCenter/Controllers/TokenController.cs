using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ContentCenter.Model.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ContentCenter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        [HttpGet("{userId}")]
        public string Get(string userId)
        {
            try
            {

              
                SecurityToken token = new JwtSecurityToken(
                    issuer:CConstants.JWTIssuer,
                    audience:CConstants.JWTAud,
                    signingCredentials:new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(CConstants.JWTIssSerKey)),
                        SecurityAlgorithms.HmacSha256),
                    expires:DateTime.Now.AddHours(2),
                    claims:new Claim[]
                    {

                    }
                    );

                var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
                return tokenStr;

            }
            catch(Exception ex)
            {
                return "Get Error When Get Toekn";
            }
           // return "No Auth to Get Token";
        }
    }
}