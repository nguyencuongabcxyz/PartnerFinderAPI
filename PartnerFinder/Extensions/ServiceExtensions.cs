using AutoMapper;
using Data.Contexts;
using Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Service;
using System;
using System.Linq;
using System.Reflection;
using Data;

namespace PartnerFinder.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        public static void ConfigureMvc(this IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(
                   options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                )
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressUseValidationProblemDetailsForInvalidModelStateResponses = true;
                    options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(context.ModelState)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                });
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<AppDbContext>();
            services.Configure<IdentityOptions>(options => 
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            });
        }

        public static void RegisterAllTypes(this IServiceCollection services, Assembly assembly, string suffix,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var typesFromAssemblies = assembly.DefinedTypes.Where(t => !t.GetTypeInfo().IsAbstract && t.Name.EndsWith(suffix));
            foreach (var type in typesFromAssemblies)
            {
                services.Add(new ServiceDescriptor(type.GetInterfaces().First(i => i.Name.EndsWith(suffix)), type, lifetime));
            }
        }

        public static void ConfigureAuthenticationWithJwtBearer(this IServiceCollection services, byte[] key)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        public static void ConfigureMapper(this IServiceCollection services)
        {
            var mapperConfigure = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            var mapper = mapperConfigure.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void RegisterDbFactory(this IServiceCollection services, string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            services.AddScoped<IDbFactory>(s => new DbFactory(new AppDbContext(optionsBuilder.Options)));
        }

    }
}
