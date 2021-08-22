using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using UserManagement.Core.Services.Interfaces;
using WebShop.API.Extensions;
using WebShop.Core.DataAccess.Interfaces;
using WebShop.Core.Domain.Entities;
using WebShop.Data;
using WebShop.Data.Repository;
using WebShop.Infrastructure.Services;

namespace WebShop.API
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
            services.AddDbContext<AppDbContext>(builder => builder.UseSqlServer(Configuration.GetConnectionString("sqlConnection")));

            services.AddAuthentication();
            services.ConfigureIdentity();
            services.ConfigureJWT(Configuration);

            services.AddCors(o =>
            {
                o.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddTransient<IGenericRepository<Category>, GenericRepository<Category>>();
            services.AddTransient<IGenericRepository<Product>, GenericRepository<Product>>();
            services.AddScoped<IAuthManager, AuthManager>();

            services.ConfigureVersioning();

            services.ConfigureSwaggerDoc();

            services.AddControllers().AddNewtonsoftJson(op => op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<ExceptionMiddleware>();
            }

            app.UseSwagger();
            
            //      app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "WebShop API v1"));
            app.UseSwaggerUI(opt =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    opt.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
