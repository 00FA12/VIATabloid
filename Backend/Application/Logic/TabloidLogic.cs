using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Model;

namespace Application.Logic;

public class TabloidLogic : ITabloidLogic
{
    private readonly ITabloidDao tabloidDao;
    private readonly IDepartmentLogic departmentLogic;
    public TabloidLogic(ITabloidDao tabloidDao, IDepartmentLogic departmentLogic)
    {
        this.tabloidDao = tabloidDao;
        this.departmentLogic = departmentLogic;
    }
    public async Task<Department> AddDepartmentAsync(int departmentId)
    {
        Department? existing = await departmentLogic.GetDepartmentByIdAsync(departmentId);
        if(existing == null)
        {
            throw new Exception($"There is no department with id {departmentId}");
        }
        Tabloid? tabloid = await GetTabloidAsync();
        tabloid?.departments.Append(departmentId);
        await UpdateTabloidAsync(tabloid!);
        return existing;
    }
    public async Task<Tabloid> CreateTabloidAsync()
    {
        Tabloid? existing = await tabloidDao.GetTabloidAsync();
        if(existing != null)
        {
            throw new Exception("A Tabloid Already exists");
        }
        return await tabloidDao.CreateTabloidAsync();
    }
    public async Task<Department> RemoveDepartmentAsync(int departmentId)
    {
        Tabloid? tabloid = await GetTabloidAsync();
        IList<int> deps = tabloid!.departments.ToList();
        deps.Remove(departmentId);
        tabloid.departments = deps;
        await UpdateTabloidAsync(tabloid);
        Department result = await departmentLogic.GetDepartmentByIdAsync(departmentId);
        return result;
    }

    public async Task<Tabloid?> GetTabloidAsync()
    {
        return await tabloidDao.GetTabloidAsync();
    }

    public async Task<Tabloid> UpdateTabloidAsync(Tabloid tabloid)
    {
        return await tabloidDao.UpdateTabloidAsync(tabloid);
    }
}