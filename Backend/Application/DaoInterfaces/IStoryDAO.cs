using Domain.Model;

namespace Application.DaoInterfaces;

public interface IStoryDAO
{
    Task<Story> CreateStoryAsync(Story story);
    Task<Story> DeleteStoryAsync(int storyId);
    Task<IEnumerable<Story>> GetAllStoriesAsync();
    Task<Story> GetStoryByIdAsync(int storyId);
}