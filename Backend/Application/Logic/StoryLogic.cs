using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Model;

namespace Application.Logic;

public class StoryLogic : IStoryLogic
{
    private readonly IStoryDAO storyDAO;

    public StoryLogic(IStoryDAO storyDAO)
    {
        this.storyDAO = storyDAO;
    }
    
    public async Task<Story> CreateStoryAsync(string title, string body)
    {
        IEnumerable<Story> existing = await GetStoriesAsync(title, body);
        if(existing != null)
        {
            throw new Exception($"A story with the same title and body already exists");
        }

        StoryDTO story = new StoryDTO(title, body);

        Story created = await storyDAO.CreateStoryAsync(story);
        return created;
    }

    public async Task<Story> DeleteStoryAsync(int storyId)
    {
        return await storyDAO.DeleteStoryAsync(storyId);
    }

    public async Task<IEnumerable<Story>> GetStoriesAsync(string? title, string? body)
    {
        IEnumerable<Story> stories = await storyDAO.GetAllStoriesAsync();

        if(title != null)
        {
            stories.Where(s => s.title.Equals(title));
        }
        if(body != null)
        {
            stories.Where(s => s.body.Equals(body));
        }

        return stories;
    }

    public async Task<Story> GetStoryByIdAsync(int storyId)
    {
        return await storyDAO.GetStoryByIdAsync(storyId);
    }
}
