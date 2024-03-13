using Domain.DTOs;
using Domain.Model;

namespace Application.LogicInterfaces;

public interface ITabloidLogic
{
    Task<Tabloid> CreateTabloidAsync();
    Task<Tabloid> GetTabloidAsync();
    Task<Department> AddDepartmentAsync(int departmentId);
    Task<Department> RemoveDepartmentAsync(int departmentId);
    Task<Tabloid> UpdateTabloidAsync(Tabloid tabloid);
}
