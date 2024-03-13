using Application.LogicInterfaces;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI;

[ApiController]
[Route("[controller]")]
public class TabloidController : ControllerBase
{
    private readonly ITabloidLogic tabloidLogic;
    public TabloidController(ITabloidLogic tabloidLogic)
    {
        this.tabloidLogic = tabloidLogic;
    }

    [HttpPost]
    public async Task<ActionResult<Tabloid>> CreateTabloid()
    {
        try
        {
            Tabloid? tabloid = await tabloidLogic.CreateTabloidAsync();
            return Created($"/tabloid", tabloid);
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<Tabloid>> GetTabloidAsync()
    {
        try
        {
            Tabloid tabloid = await tabloidLogic.GetTabloidAsync();
            return Ok(tabloid);
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}