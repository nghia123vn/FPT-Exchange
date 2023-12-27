﻿using FPT_Exchange_API.Configurations.Middleware;
using FPT_Exchange_Data;
using FPT_Exchange_Service.Categories;
using FPT_Exchange_Service.ProductActivities;
using FPT_Exchange_Service.Products;
using FPT_Exchange_Service.Stations;
using FPT_Exchange_Service.Users;
using Microsoft.OpenApi.Models;

namespace FPT_Exchange_API.Configurations
{
    public static class AppConfiguration
    {
        public static void AddDependenceInjection(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IStationService, StationService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductActivityService, ProductActivityService>();


            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product Service Interface", Description = "Product_APIs for FPT-Exchange Application", Version = "v1" });
                c.DescribeAllParametersInCamelCase();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                 });
            });
        }
        public static void UseJwt(this IApplicationBuilder app)
        {
            app.UseMiddleware<JwtMiddleware>();
        }

    }
}