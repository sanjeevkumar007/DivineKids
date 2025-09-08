namespace DivineKids.Application.Features.Dtos;

public sealed record CategoryDto(int Id, string Name, string Description, string ImageUrl, int MainCategoryId, DateTimeOffset CreatedDate, DateTimeOffset ModifiedDate);
