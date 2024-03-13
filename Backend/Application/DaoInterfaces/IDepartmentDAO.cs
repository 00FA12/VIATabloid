using Domain.Model;

namespace Application.DaoInterfaces;

public interface IDepartmentDAO
{
    Task<Department> CreateDepartmentAsync(Department department);
    //this parameter needs to be changed to departmentDTO
    Task<Department> DeleteDepartmentAsync(int depId);
    Task<IEnumerable<Department>> GetDepartmentsAsync();
    Task<Department> GetDepartmentByIdAsync(int depId);
}
