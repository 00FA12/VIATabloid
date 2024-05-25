using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Model;

namespace Application.Logic;

public class DepartmentLogic : IDepartmentLogic
{
    private readonly IDepartmentDAO departmentDao;
    private readonly IStoryDAO storyDAO;

    public DepartmentLogic(IDepartmentDAO departmentDao, IStoryDAO storyDAO)
    {
        this.departmentDao = departmentDao;
        this.storyDAO = storyDAO;
    }


    public async Task<Department> CreateDepartmentAsync(string name)
    {
        IEnumerable<Department> departments = await departmentDao.GetDepartmentsAsync();
        Department? existing = departments.FirstOrDefault(d => d.name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if(existing != null)
        {
            throw new Exception("A department with the same name already exists");
        }

        DepartmentDTO department = new DepartmentDTO(name);
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

}
