using Domain.DTOs;
using Domain.Model;

namespace Application.DaoInterfaces;

public interface IDepartmentDAO
{
    Task<Department> CreateDepartmentAsync(DepartmentDTO department);
    Task<Department> DeleteDepartmentAsync(int depId);
    Task<IEnumerable<Department>> GetDepartmentsAsync();
    Task<Department> GetDepartmentByIdAsync(int depId);
}
