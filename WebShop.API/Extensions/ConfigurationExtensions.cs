using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Core.Domain.Entities;
using WebShop.Data;

namespace ProductListing.API.Extensions
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


    }
}
