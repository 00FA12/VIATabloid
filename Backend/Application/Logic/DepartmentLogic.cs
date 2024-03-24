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

    public async Task<Story> AddStoryAsync(int departmentId, int storyId)
    {
        Department? existing = await departmentDao.GetDepartmentByIdAsync(departmentId);
        if(existing == null)
        {
            throw new Exception($"there is no department with id {departmentId}");
        }
        Story? existing2 = await storyDAO.GetStoryByIdAsync(storyId)!;
        if(existing2 == null)
        {
            throw new Exception($"the story wasn't created succesfully");
        }
        if(existing.stories == null)
        {
            existing.stories = new List<int>();
        }
        List<int> stories = existing.stories.ToList();
        stories.Add(storyId);
        existing.stories = stories;
        await departmentDao.UpdateDepartmentAsync(existing);
        return existing2;
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

    public async Task<Story> RemoveStoryAsync(int storyId)
    {
        IEnumerable<Department> dps = await departmentDao.GetDepartmentsAsync();
        int depId = -1;
        foreach(var dep in dps)
        {
            int? stId = dep.stories.FirstOrDefault(s => s == storyId);
            if(stId == storyId)
            {
                depId = dep.id;
            }
        }
        if(depId == -1)
        {
            throw new Exception($"there is no department with id {depId}");
        }
        Department existing = await GetDepartmentByIdAsync(depId); 
        Story? existing2 = await storyDAO.GetStoryByIdAsync(storyId);
        if(existing2 == null)
        {
            throw new Exception($"there is no story with id {storyId}");
        }

        List<int> stories = existing.stories.ToList();
        stories.Remove(storyId);
        existing.stories = stories;
        await departmentDao.UpdateDepartmentAsync(existing);
        return existing2;
    }
}
