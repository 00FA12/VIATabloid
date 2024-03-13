using Domain.Model;

namespace Application.LogicInterfaces;

public interface ITabloidLogic
{
    Task<Tabloid> CreateTabloidAsync();
    Task<Tabloid> GetTabloidAsync();
}
