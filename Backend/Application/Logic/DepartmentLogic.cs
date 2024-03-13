using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Domain.Model;

namespace Application.Logic;

public class DepartmentLogic : IDepartmentLogic
{
    private readonly IDepartmentDAO departmentDao;

    public DepartmentLogic(IDepartmentDAO departmentDao)
    {
        this.departmentDao = departmentDao;
    }
    public async Task<Department> CreateDepartmentAsync(string name)
    {
        IEnumerable<Department> departments = await departmentDao.GetDepartmentsAsync();
        Department? existing = departments.FirstOrDefault(d => d.name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if(existing != null)
        {
            throw new Exception("A department with the same name already exists");
        }

        Department department = new Department{
            name = name,
            stories = new List<Story>()
        };
        //Will give error if no solution is given for ID's
        //TODO Create DTOs to be able to create objects with no Id's
        Department created = await departmentDao.CreateDepartmentAsync(department);
        return created;
    }

    public async Task<Department> DeleteDepartmentAsync(int depId)
    {
        return await departmentDao.DeleteDepartmentAsync(depId);
    }

    public async Task<Department> GetDepartmentByIdAsync(int depId)
    {
        return await departmentDao.GetDepartmentByIdAsync(depId);
    }

    public async Task<IEnumerable<Department>> GetDepartmentsAsync()
    {
        return await departmentDao.GetDepartmentsAsync();
    }

    public async Task<IEnumerable<Story>> GetStoriesByDepartmentId(int depId)
    {
        Department department = await departmentDao.GetDepartmentByIdAsync(depId);
        return department.stories;
    }
}
