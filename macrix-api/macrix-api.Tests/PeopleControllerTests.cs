using macrix_api.Controllers;
using macrix_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace macrix_api.Tests;

public class PeopleControllerTests
{
    private static long testPersonValidId = 1L;
    private static long testPersoInvalidId = -1L;

    [Fact]
    public async Task GetPeopleEntities_ReturnsAll()
    {
        //Arrange
        var controller = CreateController();

        //Act
        var actionResult = await controller.GetPeopleEntities();
        var value = actionResult.Value;

        //Assert
        Assert.NotNull(value);
        Assert.Equal(PersonEntityFakeData.Count, value!.Count());
    }

    [Fact]
    public async Task GetPersonEntity_ValidId_ReturnsSameId()
    {
        //Arrange
        var controller = CreateController();

        //Act
        var actionResult = await controller.GetPersonEntity(testPersonValidId);
        var value = actionResult.Value;

        //Assert
        Assert.NotNull(value);
        Assert.Equal(testPersonValidId, value!.id);
    }

    [Fact]
    public async Task GetPersonEntity_InvalidId_Returns404()
    {
        //Arrange
        var controller = CreateController();

        //Act
        var actionResult = await controller.GetPersonEntity(testPersoInvalidId);
        var result = actionResult.Result as NotFoundResult;

        //Assert
        Assert.NotNull(result);
        Assert.Equal(404, result!.StatusCode);
    }

    [Fact]
    public async Task DeletePersonEntity_ValidId_Returns204()
    {
        //Arrange
        var controller = CreateController();

        //Act
        var actionResult = await controller.DeletePersonEntity(testPersonValidId);

        //Assert
        Assert.IsType<NoContentResult>(actionResult);
    }

    [Fact]
    public async Task DeletePersonEntity_InvalidId_Returns404()
    {
        //Arrange
        var controller = CreateController();

        //Act
        var actionResult = await controller.DeletePersonEntity(testPersoInvalidId);

        //Assert
        Assert.IsType<NotFoundResult>(actionResult);
    }

    private PeopleController CreateController()
    {
        var mockSet = PersonEntityFakeData.data.AsQueryable().BuildMockDbSet();
        mockSet.Setup(x => x.FindAsync(testPersonValidId)).ReturnsAsync((object[] ids) =>
        {
            var id = (long)ids[0];
            return PersonEntityFakeData.data.FirstOrDefault(x => x.id == id);
        });
        var mockContext = new Mock<PeopleDbContext>();
        mockContext.Setup(c => c.PeopleEntities).Returns(mockSet.Object);
        return new PeopleController(mockContext.Object);
    }
}