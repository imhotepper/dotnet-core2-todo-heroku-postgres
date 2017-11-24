using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
            app.UseMvc();
        }
    }
}
