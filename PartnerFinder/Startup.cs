﻿using System.Text;
using Data.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PartnerFinder.Extensions;
using Service.Models;
using Service.Services;

namespace PartnerFinder
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
            services.ConfigureMvc();

            //Inject ApplicationSetting
            services.Configure<ApplicationSetting>(Configuration.GetSection("ApplicationSettings"));

            var connectionString = Configuration.GetConnectionString("HomeComputer");
            services.ConfigureDbContext(connectionString);

            services.ConfigureIdentity();

            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JwtSecret"]);
            services.ConfigureAuthenticationWithJwtBearer(key);

            services.ConfigureMapper();

            services.AddCors();

            services.AddHttpContextAccessor();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.RegisterAllTypes(typeof(AppDbContext).Assembly, "Repository", ServiceLifetime.Scoped);
            services.RegisterAllTypes(typeof(AppDbContext).Assembly, "Service", ServiceLifetime.Scoped);

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

            app.ConfigureExceptionHandler();
            app.UseHttpsRedirection();

            string[] origins =
            {
                Configuration["ApplicationSettings:ClientUrl"],
                Configuration["ApplicationSettings:ClientUrl1"]
            };
            app.UseCors(builder =>
                builder.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
            );

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
