using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StoryController : ControllerBase
{
    private readonly IStoryLogic storyLogic;
    public StoryController(IStoryLogic storyLogic)
    {
        this.storyLogic = storyLogic;
    }

    [HttpPost]
    public async Task<ActionResult<Story>> CreateStoryAsync(StoryDTO storyDTO)
    {
        try
        {
            Story? story = await storyLogic.CreateStoryAsync(storyDTO.title, storyDTO.body, storyDTO.departmentId);
            return Created($"/stories/{story.id}", story);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Story>>> GetStoriesAsync(string? title, string? body)
    {
        try
        {
            IEnumerable<Story> stories = await storyLogic.GetStoriesAsync(title, body);
            return Ok(stories);
            //If the search fields are null, the program returns all of the stories
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet ("{id:int}")]
    public async Task<ActionResult<Story?>> GetStoryById([FromRoute] int id)
    {
        try
        {
            Story? story = await storyLogic.GetStoryByIdAsync(id)!;
            return Ok(story);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete ("{id:int}")]
    public async Task<ActionResult> DeleteStoryAsync([FromRoute] int id)
    {        
        try
        {
            await storyLogic.DeleteStoryAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}