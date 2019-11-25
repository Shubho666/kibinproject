// using System;
// using System.Collections.Generic;
// using System.IdentityModel.Tokens.Jwt;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.IdentityModel.Tokens;

// namespace Kibin.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class AuthController : ControllerBase
//     {
//         [HttpPost("token")]
//         public ActionResult GetToken()
//         {
//             string securityKey = "this_is_the_security_key_for_token_validation";
//             var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
//             var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
//             var token = new JwtSecurityToken(
//                 issuer: "shubho",
//                 audience: "readers",
//                 expires: DateTime.Now.AddHours(24),
//                 signingCredentials: signingCredentials
//                 );
//             return Ok(new JwtSecurityTokenHandler().WriteToken(token));
//         }
//     }
// }