using AutoMapper;
using Blog.EntityServices.ControllerContext;
using Blog.EntityServices.Interfaces;
using Blog.Web.Controllers.V1;
using Moq;
using Xunit;

namespace Blog.UnitTests.Controllers;

public class PostsControllerTests
{
    [Fact]
    public void Has_GetAll()
    {
        // Arrange
        var mockControllerContext = new Mock<IControllerContext>();
        var mockPostsService = new Mock<IPostsService>();
        var mockPostsTagsRelationsService = new Mock<IPostsTagsRelationsService>();
        var mockMapper = new Mock<IMapper>();

        var controller = new PostsController(mockControllerContext.Object, mockPostsService.Object, mockPostsTagsRelationsService.Object, mockMapper.Object);

        //Act
        var actionResult = controller.Index();
        var response = actionResult.Result;

        //Assert
        Assert.NotNull(response);
        //response.Should().NotBeNull();
    }
}