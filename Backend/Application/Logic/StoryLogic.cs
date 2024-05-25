using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Model;

namespace Application.Logic;

public class StoryLogic : IStoryLogic
{
    private readonly IStoryDAO storyDAO;
    private readonly IDepartmentLogic departmentLogic;

    public StoryLogic(IStoryDAO storyDAO, IDepartmentLogic departmentLogic)
    {
        this.storyDAO = storyDAO;
        this.departmentLogic = departmentLogic;
    }
    
    public async Task<Story> CreateStoryAsync(string title, string body, int departmentId)
    {
        IEnumerable<Story> sts = await GetStoriesAsync(title, body);
        if(sts.Any())
        {
            throw new Exception($"A story with the same title and body already exists");
        }

        Department? department = await departmentLogic.GetDepartmentByIdAsync(departmentId);
        if(department == null)
        {
            throw new Exception($"Could not find department with id {departmentId}");
        }

        StoryDTO story = new StoryDTO(title, body, departmentId);

        Story created = await storyDAO.CreateStoryAsync(story);
        return created;
    }

    public async Task<Story> DeleteStoryAsync(int storyId)
    {
        Story? existing = await GetStoryByIdAsync(storyId);
        if(existing == null)
        {
            throw new Exception($"A story with id {storyId} doesn't exist");
        }
        await storyDAO.DeleteStoryAsync(storyId);
        return existing;
    }

    public async Task<IEnumerable<Story>> GetStoriesAsync(string? title, string? body)
    {
        IEnumerable<Story> stories = await storyDAO.GetAllStoriesAsync();

        if(!String.IsNullOrEmpty(title))
        {
            stories = stories.Where(s => s.title.Contains(title));
        }
        if(!String.IsNullOrEmpty(body))
        {
            stories = stories.Where(s => s.body.Contains(body));
        }

        return stories;
    }

    public async Task<Story?>? GetStoryByIdAsync(int storyId)
    {
        Story? existing = await storyDAO.GetStoryByIdAsync(storyId);
        if(existing == null)
        {
            throw new Exception($"No story was found with id: {storyId}");
        }
        return existing;
    }
}