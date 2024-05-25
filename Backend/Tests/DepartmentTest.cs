using Application.DaoInterfaces;
using Application.Logic;
using Application.LogicInterfaces;
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
    public async Task CreateDepartmentAsync_ShouldCreateAndReturnDepartment()
    {
        //Arrange
        Department department = new Department
        {
            name = "testdpt"
        };
        _mockDepartmentDAO.Setup(db => db.CreateDepartmentAsync(It.IsAny<DepartmentDTO>())).ReturnsAsync(department);

        //Act
        var result = await _departmentLogic.CreateDepartmentAsync("testdpt");

        //Assert
        Assert.Equal("testdpt", result.name);
        _mockDepartmentDAO.Verify(db => db.CreateDepartmentAsync(It.Is<DepartmentDTO>(dto => dto.name == "testdpt")), Times.Once);
    }

    [Fact]
    public async Task CreateDepartmentAsync_ShouldNotCreateDepartment()
    {
        //Arrange
        var existingDepartments = new List<Department>
        {
            new Department{name = "testdpt"}
        };
        _mockDepartmentDAO.Setup(d => d.GetDepartmentsAsync()).ReturnsAsync(existingDepartments);


        //Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _departmentLogic.CreateDepartmentAsync("testdpt"));
        Assert.Equal("A department with the same name already exists", exception.Message);
    }

    [Fact]
    public async Task GetDepartmentsAsync_ShouldReturnListOfDepartments()
    {
        //Arrange
        Department department = new Department
        {
            name = "testdpt"
        };
        IList<Department> departmentsList = [department];
        IEnumerable<Department> departments = departmentsList;
        _mockDepartmentDAO.Setup(db => db.GetDepartmentsAsync()).ReturnsAsync(departments.AsEnumerable());

        //Act
        var result = await _departmentLogic.GetDepartmentsAsync();

        //Assert
        Assert.Single(result);
        Assert.Equal("testdpt", result.FirstOrDefault(d => d.name == "testdpt").name);
        _mockDepartmentDAO.Verify(db => db.GetDepartmentsAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteDepartmentAsync_ShouldDeleteAndReturnDepartment()
    {
        //Arrange
        Department department = new Department
        {
            name = "testdpt",
            id = 1
        };
        _mockDepartmentDAO.Setup(db => db.DeleteDepartmentAsync(1)).ReturnsAsync(department);
        _mockDepartmentDAO.Setup(db => db.GetDepartmentsAsync()).ReturnsAsync(Enumerable.Empty<Department>());

        //Act
        var deleteResult = await _departmentLogic.DeleteDepartmentAsync(department.id);
        var departmentsAfterDeletion = await _departmentLogic.GetDepartmentsAsync();


        //Assert
        Assert.Equal("testdpt", deleteResult.name);
        Assert.Empty(departmentsAfterDeletion);
        _mockDepartmentDAO.Verify(db => db.DeleteDepartmentAsync(It.Is<int>(id => id == 1)), Times.Once);
        _mockDepartmentDAO.Verify(db => db.GetDepartmentsAsync(), Times.Once);

    }

    [Fact]
    public async Task GetDepartmentByIdAsync_ShouldReturnDepartment()
    {
        //Arrange
        Department department = new Department
        {
            name = "testdpt",
            id = 1
        };
        _mockDepartmentDAO.Setup(db => db.GetDepartmentByIdAsync(It.IsAny<int>())).ReturnsAsync(department);

        //Act
        var result = await _departmentLogic.GetDepartmentByIdAsync(1);

        //Assert
        Assert.Equal(1, result.id);
        Assert.Equal("testdpt", result.name);
        _mockDepartmentDAO.Verify(db => db.GetDepartmentByIdAsync(It.Is<int>(id => id == 1)), Times.Once);
    }
}