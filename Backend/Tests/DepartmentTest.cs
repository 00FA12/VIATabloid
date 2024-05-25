using System.Data.Common;
using Application.DaoInterfaces;
using Application.Logic;
using Application.LogicInterfaces;
using DBConnection;
using Domain.DTOs;
using Domain.Model;
using Moq;
using Xunit;

namespace Tests;

public class DepartmentTest
{
    private readonly Mock<IDepartmentDAO> _mockDepartmentDAO;
    private readonly Mock<IStoryDAO> _mockStoryDAO;
    private readonly IDepartmentLogic _departmentLogic;

    public DepartmentTest()
    {
        _mockDepartmentDAO = new Mock<IDepartmentDAO>();
        _mockStoryDAO = new Mock<IStoryDAO>();
        _departmentLogic = new DepartmentLogic(_mockDepartmentDAO.Object, _mockStoryDAO.Object);
    }

    [Fact]
    public async Task CreateDepartmentAsync()
    {
        Department department = new Department
        {
            name = "testdpt"
        };
        _mockDepartmentDAO.Setup(db => db.CreateDepartmentAsync(It.IsAny<DepartmentDTO>())).ReturnsAsync(department);

        var result = await _departmentLogic.CreateDepartmentAsync("testdpt");

        Assert.Equal("testdpt", result.name);
    }

    [Fact]
    public async Task GetDepartmentsAsync()
    {
        Department department = new Department
        {
            name = "testdpt"
        };
        IList<Department> departmentsList = new List<Department>();
        departmentsList.Add(department);
        IEnumerable<Department> departments = departmentsList;

        _mockDepartmentDAO.Setup(db => db.GetDepartmentsAsync()).ReturnsAsync(departments.AsEnumerable());


        var result = await _departmentLogic.GetDepartmentsAsync();
        Assert.Equal("testdpt", result.FirstOrDefault(d => d.name == "testdpt").name);
    }

    [Fact]
    public async Task DeleteDepartmentAsync()
    {
        Department department = new Department
        {
            name = "testdpt"
        };

        _mockDepartmentDAO.Setup(db => db.DeleteDepartmentAsync(It.IsAny<int>())).ReturnsAsync(department);

        var result = await _departmentLogic.DeleteDepartmentAsync(department.id);
        var results = await _departmentLogic.GetDepartmentsAsync();
        Assert.Equal("testdpt", result.name);
        Assert.Equal([], results);
    }

    [Fact]
    public async Task GetDepartmentByIdAsync()
    {
        Department department = new Department
        {
            name = "testdpt",
            id = 1
        };

        _mockDepartmentDAO.Setup(db => db.GetDepartmentByIdAsync(It.IsAny<int>())).ReturnsAsync(department);

        var result = await _departmentLogic.GetDepartmentByIdAsync(1);
        Assert.Equal(1, result.id);
    }

    // [Fact]
    // public async Task AddStoryAsync()
    // {
    //     Department department = new Department
    //     {
    //         name = "testdpt",
    //         id = 1
    //     };

    //     _mockDepartmentDAO.Setup(db => db.GetDepartmentByIdAsync(It.IsAny<int>())).ReturnsAsync(department);

    //     Story story = new Story
    //     {
    //         title = "testStory",
    //         body = "testBody",
    //         id = 1
    //     };
    //     _mockStoryDAO.Setup(db => db.CreateStoryAsync(It.IsAny<StoryDTO>())).ReturnsAsync(story);

    //     var result = await _departmentLogic.AddStoryAsync(1, story.id);
    //     Assert.Equal(story.title, result.title);
    // }

    // [Fact]
    // public async Task RemoveStoryAsync()
    // {

    // }
}