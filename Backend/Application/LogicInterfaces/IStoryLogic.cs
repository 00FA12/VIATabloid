using Domain.Model;

namespace Application.LogicInterfaces;

public interface IStoryLogic
{   
    Task<Story> GetStoryAsync(int storyId);
    Task<IEnumerable<Story>> GetAllStoriesAsync();

    Task<Story> CreateStoryAsync(string title, string body);
    Task<Story> DeleteStoryAsync(int storyId);
}
