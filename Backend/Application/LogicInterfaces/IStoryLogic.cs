using Domain.Model;

namespace Application.LogicInterfaces;

public interface IStoryLogic
{   
    Task<Story> GetStoryByIdAsync(int storyId);
    Task<IEnumerable<Story>> GetAllStoriesAsync();

    Task<Story> CreateStoryAsync(string title, string body);
    Task<Story> DeleteStoryAsync(int storyId);
    Task<IEnumerable<Story>> FindStoriesAsync(string? title, string? body);
}
