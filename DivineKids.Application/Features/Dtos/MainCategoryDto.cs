namespace DivineKids.Application.Features.Dtos;

public sealed record MainCategoryDto(int Id, string Name, string Description, string ImageUrl, DateTimeOffset CreatedDate, DateTimeOffset ModifiedDate);