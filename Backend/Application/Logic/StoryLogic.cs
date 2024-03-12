using Application.LogicInterfaces;
using Domain.Model;

namespace Application;

public class StoryLogic : IStoryLogic
{
    public Task<Story> CreateStoryAsync(string title, string body)
    {
        throw new NotImplementedException();
    }

    public Task<Story> DeleteStoryAsync(int storyId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Story>> GetAllStoriesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Story> GetStoryAsync(int storyId)
    {
        throw new NotImplementedException();
    }
}
