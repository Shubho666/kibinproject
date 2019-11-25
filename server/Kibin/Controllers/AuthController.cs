// using Identity.ExternalClaims.Data;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Kibin.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using Kibin.Services;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Kibin.Controllers {
    // [Route("auth")]
    [Route ("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase {

        [HttpGet ("login")]

        public IActionResult SignInWithGitLab (string returnUrl = "http://localhost:4200/dashboard") {
            return Redirect ("https://gitlab-cgi.stackroute.in/oauth/authorize?client_id="+Environment.GetEnvironmentVariable("ClientID")+"&redirect_uri="+Environment.GetEnvironmentVariable("CallbackURL")+"&response_type=code&state=" + this.Randomize (50) + "&scope=api+read_user+sudo+read_repository+openid");
        }

        [HttpGet ("callback")]
        public IActionResult Callback ([FromQuery] string code, [FromQuery] string state) {

            Console.WriteLine (code);
            Console.WriteLine (state);

            ResponseModel c;
            GitlabUserResponse userObj;
            string token;
            using (HttpClient client = new HttpClient ()) {
                var a = new AuthModel () {
                client_id = Environment.GetEnvironmentVariable("ClientID"),
                client_secret = Environment.GetEnvironmentVariable("ClientSecret"),
                code = code,
                grant_type = "authorization_code",
                redirect_uri = Environment.GetEnvironmentVariable("CallbackURL")
                };
                var client_ = new RestClient ("http://gitlab-cgi.stackroute.in/oauth/token");
                var request = new RestRequest (Method.POST);
                request.AddJsonBody (a);
                IRestResponse response = client_.Execute (request);
                c = JsonConvert.DeserializeObject<ResponseModel> (response.Content);
                //var client2 = new RestClient("https://gitlab-cgi.stackroute.in/api/v4/user");
                var client2 = new RestClient ("https://gitlab-cgi.stackroute.in/api/v4/user?access_token=" + c.access_token);
                var request2 = new RestRequest (Method.GET);
                //request.AddHeader("Authorization", "token "+c.access_token);
                IRestResponse res2 = client2.Execute (request2);
                userObj = JsonConvert.DeserializeObject<GitlabUserResponse> (res2.Content);
                // var settings = new KibinDatabaseSettings () {
                //     ConnectionString = "mongodb://localhost:27017",
                //     DatabaseName = "KibinDbKanban"
                // };

                var ctx = new MongoDBContext();
                var UsersService = new UsersService (ctx);
                var _user =UsersService.GetByUsername (userObj.username);
                if (_user == null) {
                    var newUser = new User {
                    username = userObj.username,
                    avatar_url = userObj.avatar_url,
                    email=userObj.email,
                    admin = false,
                    projectDetails = new ProjectDetails[] { new ProjectDetails () },
                    accessToken = c.access_token
                    };
                    _user = UsersService.Create (newUser);
                } else {
                    UsersService.UpdateAccessToken (_user.Id, c.access_token);
                }
                 string securityKey = "this_is_the_security_key_for_token_validation";
                var symmetricSecurityKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (securityKey));
                var signingCredentials = new SigningCredentials (symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

                var claims = new List<Claim> ();
                claims.Add (new Claim (ClaimTypes.Role, "Member"));
                claims.Add (new Claim ("id", _user.Id));
                claims.Add (new Claim ("username", _user.username));
                Console.WriteLine(_user.username);
                var _token = new JwtSecurityToken (
                    issuer: "Kibin",
                    audience: "members",
                    expires : DateTime.Now.AddDays (2),
                    claims : claims,
                    signingCredentials : signingCredentials
                );
                token = new JwtSecurityTokenHandler ().WriteToken (_token);
                Response.Cookies.Append("jwt",token);
            }
            return Redirect(Environment.GetEnvironmentVariable("ClientURL")+"/dashboard");
            // return Ok(token);

        }

        private string Randomize (int length) {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random ();
            for (int i = 0; i < stringChars.Length; i++) {
                stringChars[i] = chars[random.Next (chars.Length)];
            }
            var finalString = new String (stringChars);
            return finalString;
        }
        // [HttpGet("getdetails")]
        // public static async Task<IActionResult> GetDetails([FromQuery]string accessToken){

        // }

    }
    public class AuthModel {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string code { get; set; }
        public string grant_type { get; set; }
        public string redirect_uri { get; set; }
    }
    public class ResponseModel {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public long expires_in { get; set; }
        public string refresh_token { get; set; }
        // public string scope { get; set; }
        // public long created_at { get; set; }
    }
}