using FPT_Exchange_Data;
using FPT_Exchange_Service.Categories;
using FPT_Exchange_Service.Products;
using FPT_Exchange_Service.Stations;
using FPT_Exchange_Service.Users;

namespace Wallet_API.Configurations
{
    public static class AppConfig
    {
        public static void AddDependenceInjection(this IServiceCollection services)
        {
            services.AddScoped<IGoogleAuth, GoogleAuth>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IStationService, StationService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

    }
}
