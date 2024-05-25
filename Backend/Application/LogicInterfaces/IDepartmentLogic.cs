using Domain.Model;

namespace Application.LogicInterfaces;

public interface IDepartmentLogic
{
    Task<Department> CreateDepartmentAsync(string name);
    Task<Department> DeleteDepartmentAsync(int depId);
    Task<IEnumerable<Department>> GetDepartmentsAsync();
    Task<Department> GetDepartmentByIdAsync(int depId);
}