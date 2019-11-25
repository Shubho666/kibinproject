using Kibin.Models;
using Kibin.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Kibin.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public TokenController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet]
        public ActionResult<List<Token>> Get() =>
            _tokenService.Get();

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Token> Get(string id)
        {
            var book = _tokenService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpGet("userId/{userId}")]
        public ActionResult<List<Token>> GetByUserId(string userId)
        {
            // Console.WriteLine(userId);
            var book = _tokenService.GetByUserId(userId);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }


        [HttpPost]
        public ActionResult<Token> Create(Token book)
        {
            _tokenService.Create(book);

            return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Token bookIn)
        {
            var book = _tokenService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _tokenService.Update(id, bookIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _tokenService.Get(id);
            Console.WriteLine(book.token);

            if (book == null)
            {
                return NotFound();
            }

            _tokenService.Remove(book.Id);

            return NoContent();
        }

        [HttpGet("accessToken")] 
        public ActionResult GetToken(string appname,DateTime expiryDate)
        {
            string securityKey = "this_is_the_security_key_for_token_validation";
            var claims = new List<Claim> ();
            claims.Add (new Claim ("appname",appname));           
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var jwttoken = new JwtSecurityToken(
                issuer: "Kibin",
                audience: "members",
                expires: expiryDate,
                claims:claims,
                signingCredentials: signingCredentials
                );
            string token=new JwtSecurityTokenHandler ().WriteToken (jwttoken);
            Console.WriteLine(token);
            var userid=HttpContext.User.Claims.Where(c=>c.Type=="id").FirstOrDefault().Value;
            Console.WriteLine(userid); 
            var storeToken=new Token{
                appName=appname,
                expiryDate=expiryDate,
                token=token,
                userId=userid
            };
            Create(storeToken);
            return Ok(token);
        }
    }
}