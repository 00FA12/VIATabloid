using System.Diagnostics.Contracts;

namespace Domain.DTOs;

public class StoryDTO
{
    public string title {get; set;}
    public string body {get; set;}
    public int departmentId{get; set;}
    public StoryDTO(string title, string body, int departmentId)
    {
        this.title = title;
        this.body = body;
        this.departmentId = departmentId;
    }
}
