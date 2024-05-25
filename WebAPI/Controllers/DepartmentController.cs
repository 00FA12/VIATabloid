using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI;
[ApiController]
[Route("[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentLogic departmentLogic;
    public DepartmentController(IDepartmentLogic departmentLogic)
    {
        this.departmentLogic = departmentLogic;
    }
    [HttpPost]
    public async Task<ActionResult<Department>> CreateDepartmentAsync(DepartmentDTO departmentDTO)
    {
        try
        {
            Department? department = await departmentLogic.CreateDepartmentAsync(departmentDTO.name);
            // await tabloidLogic.AddDepartmentAsync(department.id);
            return Created($"/department/{department.id}", department);
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete ("{id:int}")]
    public async Task<ActionResult> DeleteDepartmentAsync([FromRoute] int id)
    {
        try
        {
            await departmentLogic.DeleteDepartmentAsync(id);
            return Ok();
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartmentsAsync()
    {
        try
        {
            IEnumerable<Department?>? departments = await departmentLogic.GetDepartmentsAsync();
            return Ok(departments);
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Department>> GetDepartmentByIdAsync([FromRoute] int id)
    {
        try
        {
            Department? department = await departmentLogic.GetDepartmentByIdAsync(id);
            return Ok(department);
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}