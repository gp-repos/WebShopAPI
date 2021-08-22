using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using UserManagement.Core.Domain.Entities;
using WebShop.Data;

namespace WebShop.API.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<AppUser>(q => { q.User.RequireUniqueEmail = true; });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddTokenProvider("WebShopApi", typeof(DataProtectorTokenProvider<AppUser>));
            builder.AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration Configuration)
        {
            var jwtSettings = Configuration.GetSection("JWT");
            var key = Configuration.GetSection("AuthSecureKey").Value;
            if (string.IsNullOrEmpty(key))
                throw new Exception("JWT AuthSecureKey not specified");

            services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    };
                });
        }

        public static void ConfigureSwaggerDoc(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();

            services.AddSwaggerGen(c =>
            {
                // c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebShop API", Version = "v1" });
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(
                        description.GroupName,
                        new OpenApiInfo()
                        {
                            Title = $"WebShop API v{description.ApiVersion}",
                            Version = description.ApiVersion.ToString(),
                        });
                }

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "JWT Authorization header using the Bearer scheme",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = JwtBearerDefaults.AuthenticationScheme,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
             {
                 opt.ReportApiVersions = true;
                 opt.AssumeDefaultVersionWhenUnspecified = true;
                 opt.DefaultApiVersion = new ApiVersion(1, 0);
               //  opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
             });

            services.AddVersionedApiExplorer(opt => opt.GroupNameFormat = "'v'VVV");

        }
    }

}
