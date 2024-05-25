namespace Domain.DTOs;

public class DepartmentDTO
{
    public string name {get; set;}

    public DepartmentDTO(string name)
    {
        this.name = name;
    }
}