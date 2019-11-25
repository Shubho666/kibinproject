using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Kibin.Models;
using Kibin.Services;
using Kibin.SignalR_Hub;
//jwt package
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Kibin.RabbitMQ;
using Swashbuckle.AspNetCore.Swagger;

namespace Kibin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
           // RabbitMQReceiver client = new RabbitMQReceiver();
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            string securityKey = "this_is_the_security_key_for_token_validation";
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "Kibin",
                        ValidAudience = "members",
                        IssuerSigningKey = symmetricSecurityKey
                    };
                });
            //services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
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
            // services.Configure<KibinDatabaseSettings>(
            // Configuration.GetSection(nameof(KibinDatabaseSettings)));
            // services.AddSingleton<IKibinDatabaseSettings>(sp =>
            //sp.GetRequiredService<IOptions<KibinDatabaseSettings>>().Value);
            services.AddSingleton<IMongoDBContext, MongoDBContext>();
            //services.AddSingleton<UsersService>();
            //services.AddSingleton<IKanbanUSService, KanbanUSService>();
            services.AddSingleton<IWorkSpaceService, WorkSpaceService>();
            services.AddSingleton<IEpicIdZoneService, EpicIdZoneService>();
            services.AddSingleton<IUserStoryIdZoneService, UserStoryIdZoneService>();
            services.AddSingleton<IPrivateSpaceService, PrivateSpaceService>();
            services.AddSingleton<LoggerActivityService>();
            services.AddCors(o => o.AddPolicy("AllowAllHeaders", builder => {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins(Environment.GetEnvironmentVariable("ClientURL"));
            }));
            services.AddSignalR();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            //app.UseHttpsRedirection ();
            app.UseAuthentication();
            app.UseCors("AllowAllHeaders");
            app.UseSignalR(routes =>
            {
                routes.MapHub<ColHub>("/colhub");
            });
            app.UseMvc();
        }
    }
}




