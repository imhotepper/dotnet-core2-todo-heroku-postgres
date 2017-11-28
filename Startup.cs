using System;
using System.Linq;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
//using HerokuPostgresConnectionStringParser;

namespace app2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMediatR();//typeof(CreateTodoHandler));


            var enumerator = Environment.GetEnvironmentVariables();



            var connectionString = string.Empty;// enumerator["DATABASE_URL"]?.ToString();

            //Get Database Connection 
            string _connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (!string.IsNullOrWhiteSpace(_connectionString))
            {
               
                _connectionString.Replace("//", "");

                char[] delimiterChars = { '/', ':', '@', '?' };
                string[] strConn = _connectionString.Split(delimiterChars);
                strConn = strConn.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                var user = strConn[1];
                var pass = strConn[2];
                var server = strConn[3];
                var database = strConn[5];
                var port = strConn[4];
                connectionString = "host=" + server + ";port=" + port
                + ";database=" + database + ";uid=" + user
                + ";pwd=" + pass + ";sslmode=Require;Trust Server Certificate=true;Timeout=1000";

            }
            if (string.IsNullOrWhiteSpace(connectionString)) connectionString = Configuration.GetConnectionString("DbCtx");

            services.AddEntityFrameworkNpgsql().AddDbContext<DbCtx>(options => options.UseNpgsql(connectionString));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddEntityFrameworkStores<DbCtx>()
                .AddDefaultTokenProviders();
            
            
            services.AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = "JwtBearer";
                    options.DefaultChallengeScheme = "JwtBearer";            
                })
                .AddJwtBearer("JwtBearer", jwtBearerOptions =>
                {                        
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {                            
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your secret goes here")),

                        ValidateIssuer = true,
                        ValidIssuer = "TodoApi",

                        ValidateAudience = true,
                        ValidAudience = "The name of the audience",

                        ValidateLifetime = true, //validate the expiration and not before values in the token

                        ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                    };
                });
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication(); //needs to be up in the pipeline, before MVC
      
            app.UseMvc();
            
            
            //custom error on 404
            app.Run(async (context) =>
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Page not found");
            });
        }
    }
}
