using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Kibin.Models;
using Kibin.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
// using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Swagger;
//jwt package
using Kibin.RabbitMQ;
using System.Text;
using Kibin.SignalR_Hub;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace Kibin {
    public class Startup {
        public Startup (IConfiguration configuration,IHostingEnvironment env) {
            Configuration = configuration;
            RabbitMQReceiver client=new RabbitMQReceiver();
             
        }

        public IConfiguration Configuration { get; }
        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {

            string securityKey = "this_is_the_security_key_for_token_validation";
            var symmetricSecurityKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (securityKey));
            var signingCredentials = new SigningCredentials (symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            services.AddSwaggerGen (c => {
                c.SwaggerDoc ("v1", new Info { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
               {
                   Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                   Name = "Authorization",
                   In = "header",
                   Type = "apiKey"
               });
               c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                   { "Bearer", Enumerable.Empty<string>() },
               });
            });
            services.AddCors (o => o.AddPolicy ("CorsPolicy", builder => {
                builder
                    .AllowAnyMethod ()
                    .AllowAnyHeader ()
                    .AllowCredentials ()
                    .WithOrigins (Environment.GetEnvironmentVariable("ClientURL"));
            }));

            services.AddSignalR ();

            services.AddAuthentication (options => {

                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "gitlab";

                })
                .AddJwtBearer (options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "Kibin",
                    ValidAudience = "members",
                    IssuerSigningKey = symmetricSecurityKey
                    };
                })
                .AddCookie ();
                

            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_2);

            //services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_2);

            // services.Configure<KibinDatabaseSettings> (
            //     Configuration.GetSection (nameof (KibinDatabaseSettings)));

            // services.AddSingleton<IKibinDatabaseSettings> (sp =>
            //     sp.GetRequiredService<IOptions<KibinDatabaseSettings>> ().Value);


            services.AddSingleton<IMongoDBContext, MongoDBContext>();

            services.AddSingleton<UsersService> ();
            services.AddSingleton<IKanbanUSService, KanbanUSService> ();
            // services.AddSingleton<ITokenService, TokenService> ();
            services.AddSingleton<IProjectService, ProjectService> ();
            services.AddSingleton<EpicService> ();
            services.AddSingleton<ListService> ();
            services.AddSingleton<TokenService> ();
            services.AddSingleton<LoggerActivityService> ();

            services.AddSingleton<ReportService>();
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }
            app.UseSwagger ();
            app.UseSwaggerUI (c => {
                c.SwaggerEndpoint ("/boards/swagger/v1/swagger.json", "My API V1");
            });
            //app.UseHttpsRedirection ();
            app.UseAuthentication ();
            
            app.UseCors ("CorsPolicy");

            app.UseSignalR (routes => {
                routes.MapHub<ChatHub> ("/notify");
                //routes.MapHub<NotifyHub>("/234");
            });
            app.UseMvc ();
        }
    }
}