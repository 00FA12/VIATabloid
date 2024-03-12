using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Domain.Model;

namespace Application;

public class StoryLogic : IStoryLogic
{
    private readonly IStoryDAO storyDAO;

    public StoryLogic(IStoryDAO storyDAO)
    {
        this.storyDAO = storyDAO;
    }
    
    public async Task<Story> CreateStoryAsync(string title, string body)
    {
        IEnumerable<Story> existing = await FindStoriesAsync(title, body);
        if(existing != null)
        {
            throw new Exception($"A story with the same title and body already exists");
        }

        Story story = new Story{
            title = title,
            body = body
        };

        Story created = await storyDAO.CreateStoryAsync(story);
        return created;
    }

    public async Task<Story> DeleteStoryAsync(int storyId)
    {
        return await storyDAO.DeleteStoryAsync(storyId);
    }

    public async Task<IEnumerable<Story>> FindStoriesAsync(string? title, string? body)
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

    public async Task<IEnumerable<Story>> GetAllStoriesAsync()
    {
        return await storyDAO.GetAllStoriesAsync();
    }

    public async Task<Story> GetStoryByIdAsync(int storyId)
    {
        return await storyDAO.GetStoryByIdAsync(storyId);
    }
}
