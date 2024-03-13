namespace Domain.Model;

public class Department
{
    public int id {get; set;}
    public string name {get; set;}
    public IEnumerable<int> stories {get; set;}

}
