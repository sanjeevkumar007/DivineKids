using DivineKids.Application.Contracts;
using DivineKids.Application.Features.Dtos;
using DivineKids.Domain.Entities;

namespace DivineKids.Application.Features.Catalog;
public class CatalogService(
    IGenericRepository<MainCategory> mainCategoryRepo,
    IGenericRepository<Category> categoryRepo,
    IGenericRepository<Product> productRepo) : ICatalogService
{
    private readonly IGenericRepository<MainCategory> _mainCategoryRepo = mainCategoryRepo ?? throw new ArgumentNullException(nameof(mainCategoryRepo));
    private readonly IGenericRepository<Category> _categoryRepo = categoryRepo ?? throw new ArgumentNullException(nameof(categoryRepo));
    private readonly IGenericRepository<Product> _productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));

    public async Task<CatalogTreeDto> GetCatalogTreeAsync(CancellationToken cancellationToken)
    {
        var mainTask = await _mainCategoryRepo.GetAllAsync(cancellationToken);
        var catTask = await _categoryRepo.GetAllAsync(cancellationToken);
        var prodTask = await _productRepo.GetAllAsync(cancellationToken);

        //await Task.WhenAll(mainTask, catTask, prodTask);

        //var mainCategories = mainTask.Result;
        //var categories = catTask.Result;
        //var products = prodTask.Result;

        var mainCategories = mainTask;
        var categories = catTask;
        var products = prodTask;

        var catsByMain = categories.GroupBy(c => c.MainCategoryId)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<Category>)g.ToList());
        var prodsByCat = products.GroupBy(p => p.CategoryId)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<Product>)g.ToList());

        var tree = new CatalogTreeDto
        {
            MainCategories = [.. mainCategories
                .OrderBy(mc => mc.Id)
                .Select(mc => new MainCategoryNode
                {
                    Id = mc.Id,
                    Name = mc.Name,
                    Categories = [.. (catsByMain.TryGetValue(mc.Id, out var cs) ? cs : [])
                        .OrderBy(c => c.Name)
                        .Select(c => new CategoryNode
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Products = [.. (prodsByCat.TryGetValue(c.Id, out var ps) ? ps : [])
                                .OrderBy(p => p.Name)
                                .Select(p => new ProductLeaf
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Price = p.Price,
                                    ImageUrl = p.ImageUrl
                                })]
                        })]
                })]
        };

        return tree;
    }
}