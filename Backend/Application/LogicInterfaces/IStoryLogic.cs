using Domain.Model;

namespace Application.LogicInterfaces;

public interface IStoryLogic
{   
    Task<Story> GetStoryByIdAsync(int storyId);

    Task<Story> CreateStoryAsync(string title, string body);
    Task<Story> DeleteStoryAsync(int storyId);
    Task<IEnumerable<Story>> GetStoriesAsync(string? title, string? body);
}
