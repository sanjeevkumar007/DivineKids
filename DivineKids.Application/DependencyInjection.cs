using DivineKids.Application.Contracts;
using DivineKids.Application.Features.Catalog;
using DivineKids.Application.Features.Categories;
using DivineKids.Application.Features.MainCategories;
using DivineKids.Application.Features.Patients;
using DivineKids.Application.Features.Prodoucts;
using Microsoft.Extensions.DependencyInjection;

namespace DivineKids.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IMainCategoryService, MainCategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IPatientsService, PatientsService>();
        return services;
    }
}