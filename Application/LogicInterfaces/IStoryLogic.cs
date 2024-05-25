using Domain.Model;

namespace Application.LogicInterfaces;

public interface IStoryLogic
{   
    Task<Story> CreateStoryAsync(string title, string body, int departmentId);
    Task<Story> DeleteStoryAsync(int storyId);
    Task<Story?>? GetStoryByIdAsync(int storyId);
    Task<IEnumerable<Story>> GetStoriesAsync(string? title, string? body);
}
