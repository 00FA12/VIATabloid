namespace Domain.Model;

public class Department
{
    public IEnumerable<Story> stories {get; set;}
    public int id {get; set;}
}
