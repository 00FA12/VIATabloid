using Domain.Model;

namespace Application.DaoInterfaces;

public interface ITabloidDao
{
    Task<Tabloid> CreateTabloidAsync();
    Task<Tabloid?> GetTabloidAsync();
    Task<Tabloid> UpdateTabloidAsync(Tabloid tabloid);
}