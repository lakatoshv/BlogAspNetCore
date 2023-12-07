using AutoFixture;
using AutoFixture.Dsl;
using AutoMapper;
using Blog.CommonServices.Interfaces;
using Blog.Core.Enums;
using Blog.Core.Infrastructure;
using Blog.Core.Infrastructure.Pagination;
using Blog.Data.Models;
using Blog.Data.Repository;
using Blog.Data.Specifications;
using Blog.EntityServices;
using Blog.EntityServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Blog.ServicesTests.EntityServices;

/// <summary>
/// Posts service tests.
/// </summary>
public class PostsServiceTests
{
    #region Fields

    /// <summary>
    /// The posts service.
    /// </summary>
    private readonly IPostsService _postsService;

    /// <summary>
    /// The posts repository mock.
    /// </summary>
    private readonly Mock<IRepository<Post>> _postsRepositoryMock;

    /// <summary>
    /// The fixture.
    /// </summary>
    private readonly Fixture _fixture;

    #endregion

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="PostsServiceTests"/> class.
    /// </summary>
    public PostsServiceTests()
    {
        _postsRepositoryMock = new Mock<IRepository<Post>>();
        var commentsServiceMock = new Mock<ICommentsService>();
        var mapper = new Mock<IMapper>();
        var postsTagsRelationsService = new Mock<IPostsTagsRelationsService>();
        _postsService = new PostsService(_postsRepositoryMock.Object, commentsServiceMock.Object, mapper.Object, postsTagsRelationsService.Object);
        _fixture = new Fixture();
    }

    #endregion

    #region Uthilities

    private IPostprocessComposer<Post> SetupPostFixture(string postTitle = null, int tagsCount = 10, int commentsCount = 10)
    {
        var applicationUser =
            _fixture.Build<ApplicationUser>()
                .Without(x => x.Roles)
                .Without(x => x.Claims)
                .Without(x => x.Logins)
                .Without(x => x.RefreshTokens)
                .Without(x => x.Profile)
                .Without(x => x.Posts)
                .Without(x => x.Comments)
                .Without(x => x.ReceivedMessages)
                .Without(x => x.SentMessages)
                .Create();

        var comments =
            _fixture.Build<Comment>()
                .With(x => x.User, applicationUser)
                .Without(x => x.Post)
                .CreateMany(commentsCount).ToList();

        var postsTagsRelations =
            _fixture.Build<PostsTagsRelations>()
                .With(x => x.Tag,
                    _fixture.Build<Tag>()
                        .Without(x => x.PostsTagsRelations)
                        .Create())
                .Without(x => x.Post)
                .CreateMany(tagsCount).ToList();

        var category =
            _fixture.Build<Category>()
                .Without(x => x.Posts)
                .Without(x => x.ParentCategory)
                .Without(x => x.Categories)
                .Create();

        var postSetup =
            _fixture.Build<Post>()
                .With(x => x.Author, applicationUser)
                .With(x => x.Comments, comments)
                .With(x => x.PostsTagsRelations, postsTagsRelations)
                .With(x => x.Category, category);


        if (!string.IsNullOrWhiteSpace(postTitle))
        {
            postSetup.With(x => x.Title, postTitle);
        }

        return postSetup;
    }

    #endregion

    #region Tests

    #region Get All

    #region Get All function

    /// <summary>
    /// Verify that function Get All has been called.
    /// </summary>
    [Fact]
    public void Verify_FunctionGetAll_HasBeenCalled()
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .CreateMany(random.Next(100));

        _postsRepositoryMock.Setup(x => x.GetAll())
            .Returns(postsList.AsQueryable());

        //Act
        _postsService.GetAll();

        //Assert
        _postsRepositoryMock.Verify(x => x.GetAll(), Times.Once);
    }

    /// <summary>
    /// Get all posts.
    /// Should return posts when posts exists.
    /// </summary>
    /// <param name="notEqualCount">The not equal count.</param>
    [Theory]
    [InlineData(0)]
    public void GetAll_WhenPostsExists_ShouldReturnPosts(int notEqualCount)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .CreateMany(random.Next(100));

        _postsRepositoryMock.Setup(x => x.GetAll())
            .Returns(() => postsList.AsQueryable());

        //Act
        var posts = _postsService.GetAll();

        //Assert
        Assert.NotNull(posts);
        Assert.NotEmpty(posts);
        Assert.NotEqual(notEqualCount, posts.ToList().Count);
    }

    /// <summary>
    /// Get all posts.
    /// Should return nothing when posts does not exists.
    /// </summary>
    [Fact]
    public void GetAll_WhenPostDoesNotExists_ShouldReturnNothing()
    {
        //Arrange
        _postsRepositoryMock.Setup(x => x.GetAll())
            .Returns(() => new List<Post>().AsQueryable());

        //Act
        var posts = _postsService.GetAll();

        //Assert
        Assert.Empty(posts);
    }

    #endregion

    #region Get All Async function

    /// <summary>
    /// Verify that function Get All Async has been called.
    /// </summary>
    [Fact]
    public async Task Verify_FunctionGetAllAsync_HasBeenCalled()
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .CreateMany(random.Next(100))
                .ToList();

        _postsRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(postsList);

        //Act
        await _postsService.GetAllAsync();

        //Assert
        _postsRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    /// <summary>
    /// Get all async posts.
    /// Should return posts when posts exists.
    /// </summary>
    /// <param name="notEqualCount">The not equal count.</param>
    [Theory]
    [InlineData(0)]
    public async Task GetAllAsync_WhenPostsExists_ShouldReturnPosts(int notEqualCount)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .CreateMany(random.Next(100))
                .ToList();

        _postsRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(() => postsList);

        //Act
        var posts = await _postsService.GetAllAsync();

        //Assert
        Assert.NotNull(posts);
        Assert.NotEmpty(posts);
        Assert.NotEqual(notEqualCount, posts.ToList().Count);
    }

    /// <summary>
    /// Get all async posts.
    /// Should return nothing when posts does not exists.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_WhenPostDoesNotExists_ShouldReturnNothing()
    {
        //Arrange
        _postsRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(() => []);

        //Act
        var posts = await _postsService.GetAllAsync();

        //Assert
        Assert.Empty(posts);
    }

    #endregion

    #region Get all function With Specification

    /// <summary>
    /// Verify that function Get All with specification has been called.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests ")]
    public void Verify_FunctionGetAll_WithSpecification_HasBeenCalled(string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Contains(titleSearch));
        _postsRepositoryMock.Setup(x => x.GetAll(specification))
            .Returns(() => postsList.Where(x => x.Title.Contains(titleSearch)).AsQueryable());

        //Act
        _postsService.GetAll(specification);

        //Assert
        _postsRepositoryMock.Verify(x => x.GetAll(specification), Times.Once);
    }

    /// <summary>
    /// Get all posts with specification.
    /// Should return posts with contains specification when posts exists.
    /// </summary>
    /// <param name="notEqualCount">The not equal count.</param>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData(0, "Created from ServicesTests ")]
    public void GetAll_WithContainsSpecification_WhenPostsExists_ShouldReturnPosts(int notEqualCount, string titleSearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Contains(titleSearch));
        _postsRepositoryMock.Setup(x => x.GetAll(specification))
            .Returns(() => postsList.Where(x => x.Title.Contains(titleSearch)).AsQueryable());

        //Act
        var posts = _postsService.GetAll(specification);

        //Assert
        Assert.NotNull(posts);
        Assert.NotEmpty(posts);
        Assert.NotEqual(notEqualCount, posts.ToList().Count);
    }

    /// <summary>
    /// Get all posts with specification.
    /// Should return post with equal specification when posts exists.
    /// </summary>
    /// <param name="equalCount">The equal count.</param>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData(1, "Created from ServicesTests 0")]
    public void GetAll_WithEqualsSpecification_WhenPostsExists_ShouldReturnPost(int equalCount, string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(1);

        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.GetAll(specification))
            .Returns(() => postsList.Where(x => x.Title.Contains(titleSearch)).AsQueryable());

        //Act
        var posts = _postsService.GetAll(specification);

        //Assert
        Assert.NotNull(posts);
        Assert.NotEmpty(posts);
        Assert.Equal(equalCount, posts.ToList().Count);
    }

    /// <summary>
    /// Get all posts.
    /// Should return nothing with  when posts does not exists.
    /// </summary>
    /// <param name="equalCount">The equal count.</param>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData(0, "Created from ServicesTests -1")]
    public void GetAll_WithEqualSpecification_WhenPostsExists_ShouldReturnNothing(int equalCount, string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.GetAll(specification))
            .Returns(() => postsList.Where(x => x.Title.Contains(titleSearch)).AsQueryable());

        //Act
        var posts = _postsService.GetAll(specification);

        //Assert
        Assert.NotNull(posts);
        Assert.Empty(posts);
        Assert.Equal(equalCount, posts.ToList().Count);
    }

    /// <summary>
    /// Get all posts.
    /// Should return nothing with  when posts does not exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests 0")]
    public void GetAll_WithEqualSpecification_WhenPostDoesNotExists_ShouldReturnNothing(string titleSearch)
    {
        //Arrange
        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.GetAll(specification))
            .Returns(() => new List<Post>().AsQueryable());

        //Act
        var posts = _postsService.GetAll();

        //Assert
        Assert.Empty(posts);
    }

    #endregion

    #region Get all async function With Specification

    /// <summary>
    /// Verify that function Get All Async with specification has been called.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests ")]
    public async Task Verify_FunctionGetAllAsync_WithSpecification_HasBeenCalled(string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(random.Next(100))
                .ToList();

        var specification = new PostSpecification(x => x.Title.Contains(titleSearch));
        _postsRepositoryMock.Setup(x => x.GetAllAsync(specification))
            .ReturnsAsync(() => postsList.Where(x => x.Title.Contains(titleSearch)).ToList());

        //Act
        await _postsService.GetAllAsync(specification);

        //Assert
        _postsRepositoryMock.Verify(x => x.GetAllAsync(specification), Times.Once);
    }

    /// <summary>
    /// Get all async posts with specification.
    /// Should return posts with contains specification when posts exists.
    /// </summary>
    /// <param name="notEqualCount">The not equal count.</param>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData(0, "Created from ServicesTests ")]
    public async Task GetAllAsync_WithContainsSpecification_WhenPostsExists_ShouldReturnPosts(int notEqualCount, string titleSearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(random.Next(100))
                .ToList();

        var specification = new PostSpecification(x => x.Title.Contains(titleSearch));
        _postsRepositoryMock.Setup(x => x.GetAllAsync(specification))
            .ReturnsAsync(() => postsList.Where(x => x.Title.Contains(titleSearch)).ToList());

        //Act
        var posts = await _postsService.GetAllAsync(specification);

        //Assert
        Assert.NotNull(posts);
        Assert.NotEmpty(posts);
        Assert.NotEqual(notEqualCount, posts.ToList().Count);
    }

    /// <summary>
    /// Get all async posts with specification.
    /// Should return post with equal specification when posts exists.
    /// </summary>
    /// <param name="equalCount">The equal count.</param>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData(1, "Created from ServicesTests 0")]
    public async Task GetAllAsync_WithEqualsSpecification_WhenPostsExists_ShouldReturnPost(int equalCount, string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(1)
                .ToList();

        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.GetAllAsync(specification))
            .ReturnsAsync(() => postsList.Where(x => x.Title.Contains(titleSearch)).ToList());

        //Act
        var posts = await _postsService.GetAllAsync(specification);

        //Assert
        Assert.NotNull(posts);
        Assert.NotEmpty(posts);
        Assert.Equal(equalCount, posts.ToList().Count);
    }

    /// <summary>
    /// Get all async posts.
    /// Should return nothing with  when posts does not exists.
    /// </summary>
    /// <param name="equalCount">The equal count.</param>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData(0, "Created from ServicesTests -1")]
    public async Task GetAllAsync_WithEqualSpecification_WhenPostsExists_ShouldReturnNothing(int equalCount, string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .CreateMany(random.Next(100))
                .ToList();

        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.GetAllAsync(specification))
            .ReturnsAsync(() => postsList.Where(x => x.Title.Contains(titleSearch)).ToList());

        //Act
        var posts = await _postsService.GetAllAsync(specification);

        //Assert
        Assert.NotNull(posts);
        Assert.Empty(posts);
        Assert.Equal(equalCount, posts.ToList().Count);
    }

    /// <summary>
    /// Get all async posts.
    /// Should return nothing with  when posts does not exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests 0")]
    public async Task GetAllAsync_WithEqualSpecification_WhenPostDoesNotExists_ShouldReturnNothing(string titleSearch)
    {
        //Arrange
        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.GetAllAsync(specification))
            .ReturnsAsync(() => []);

        //Act
        var posts = await _postsService.GetAllAsync();

        //Assert
        Assert.Null(posts);
    }

    #endregion

    #endregion

    #region Find

    #region Find function

    /// <summary>
    /// Verify that function Find has been called.
    /// </summary>
    [Fact]
    public void Verify_FunctionFind_HasBeenCalled()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.GetById(postId))
            .Returns(() => newPost);

        //Act
        _postsService.Find(postId);

        //Assert
        _postsRepositoryMock.Verify(x => x.GetById(postId), Times.Once);
    }

    /// <summary>
    /// Find post.
    /// Should return post when post exists.
    /// </summary>
    [Fact]
    public void Find_WhenPostExists_ShouldReturnPost()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.GetById(postId))
            .Returns(() => newPost);

        //Act
        var post = _postsService.Find(postId);

        //Assert
        Assert.Equal(post.Id, post.Id);
    }

    /// <summary>
    /// Find post.
    /// Should return nothing when post does not exists.
    /// </summary>
    [Fact]
    public void Find_WhenPostDoesNotExists_ShouldReturnNothing()
    {
        //Arrange
        var postId = _fixture.Create<int>();

        _postsRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(() => null);

        //Act
        var post = _postsService.Find(postId);

        //Assert
        Assert.Null(post);
    }

    #endregion

    #region Find Async function 

    /// <summary>
    /// Verify that function Find Async has been called.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task Verify_FunctionFindAsync_HasBeenCalled()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.GetByIdAsync(postId))
            .ReturnsAsync(() => newPost);

        //Act
        await _postsService.FindAsync(postId);

        //Assert
        _postsRepositoryMock.Verify(x => x.GetByIdAsync(postId), Times.Once);
    }

    /// <summary>
    /// Async find post.
    /// Should return post when post exists.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task FindAsync_WhenPostExists_ShouldReturnPost()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.GetByIdAsync(postId))
            .ReturnsAsync(() => newPost);

        //Act
        var post = await _postsService.FindAsync(postId);

        //Assert
        Assert.Equal(post.Id, post.Id);
    }

    /// <summary>
    /// Async find post.
    /// Should return nothing when post does not exists.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task FindAsync_WhenPostDoesNotExists_ShouldReturnNothing()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        
        _postsRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => null);

        //Act
        var post = await _postsService.FindAsync(postId);

        //Assert
        Assert.Null(post);
    }

    #endregion

    #endregion

    #region Insert

    #region Insert function

    /// <summary>
    /// Verify that function Insert has been called.
    /// </summary>
    [Fact]
    public void Verify_FunctionInsert_HasBeenCalled()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.Insert(newPost))
            .Callback(() =>
            {
                newPost.Id = postId;
            });

        //Act
        _postsService.Insert(newPost);

        //Assert
        _postsRepositoryMock.Verify(x => x.Insert(newPost), Times.Once);
    }

    /// <summary>
    /// Insert post.
    /// Should return post when post created.
    /// </summary>
    [Fact]
    public void Insert_WhenPostExists_ShouldReturnPost()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.Insert(newPost))
            .Callback(() =>
            {
                newPost.Id = postId;
            });

        //Act
        _postsService.Insert(newPost);

        //Assert
        Assert.NotEqual(0, newPost.Id);
    }

    #endregion

    #region Insert Enumerable function

    /// <summary>
    /// Verify that function Insert Enumerable has been called.
    /// </summary>
    [Fact]
    public void Verify_FunctionInsertEnumerable_HasBeenCalled()
    {
        //Arrange
        var random = new Random();
        var postId = _fixture.Create<int>();
        var itemsCount = random.Next(100);
        var newPosts =
            SetupPostFixture()
                .CreateMany(itemsCount)
                .ToList();

        _postsRepositoryMock.Setup(x => x.Insert(newPosts))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newPosts[i].Id = postId + i;
                }
            });

        //Act
        _postsService.Insert(newPosts);

        //Assert
        _postsRepositoryMock.Verify(x => x.Insert(newPosts), Times.Once);
    }

    /// <summary>
    /// Insert Enumerable posts.
    /// Should return posts when posts created.
    /// </summary>
    [Fact]
    public void InsertEnumerable_WhenPostsExists_ShouldReturnPosts()
    {
        //Arrange
        var random = new Random();
        var postId = _fixture.Create<int>();
        var itemsCount = random.Next(100);
        var newPosts =
            SetupPostFixture()
                .CreateMany(itemsCount)
                .ToList();

        _postsRepositoryMock.Setup(x => x.Insert(newPosts))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newPosts[i].Id = postId + i;
                }
            });

        //Act
        _postsService.Insert(newPosts);

        //Assert
        newPosts.ForEach(x =>
        {
            Assert.NotEqual(0, x.Id);
        });
    }

    #endregion

    #region Insert Async function

    /// <summary>
    /// Verify that function Insert Async has been called.
    /// </summary>
    [Fact]
    public async Task Verify_FunctionInsertAsync_HasBeenCalled()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.InsertAsync(newPost))
            .Callback(() =>
            {
                newPost.Id = postId;
            });

        //Act
        await _postsService.InsertAsync(newPost);

        //Assert
        _postsRepositoryMock.Verify(x => x.InsertAsync(newPost), Times.Once);
    }

    /// <summary>
    /// Async insert post.
    /// Should return post when post created.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task InsertAsync_WhenPostExists_ShouldReturnPost()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.InsertAsync(newPost))
            .Callback(() =>
            {
                newPost.Id = postId;
            });

        //Act
        await _postsService.InsertAsync(newPost);

        //Assert
        Assert.NotEqual(0, newPost.Id);
    }

    #endregion

    #region Insert Async Enumerable function

    /// <summary>
    /// Verify that function Insert Enumerable has been called.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task Verify_FunctionInsertAsyncEnumerable_HasBeenCalled()
    {
        //Arrange
        var random = new Random();
        var postId = _fixture.Create<int>();
        var itemsCount = random.Next(100);
        var newPosts =
            SetupPostFixture()
                .CreateMany(itemsCount)
                .ToList();

        _postsRepositoryMock.Setup(x => x.InsertAsync(newPosts))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newPosts[i].Id = postId + i;
                }
            });

        //Act
        await _postsService.InsertAsync(newPosts);

        //Assert
        _postsRepositoryMock.Verify(x => x.InsertAsync(newPosts), Times.Once);
    }

    /// <summary>
    /// Insert Async Enumerable posts.
    /// Should return posts when posts created.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task InsertAsyncEnumerable_WhenPostsExists_ShouldReturnPosts()
    {
        //Arrange
        var random = new Random();
        var postId = _fixture.Create<int>();
        var itemsCount = random.Next(100);
        var newPosts =
            SetupPostFixture()
                .CreateMany(itemsCount)
                .ToList();

        _postsRepositoryMock.Setup(x => x.InsertAsync(newPosts))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newPosts[i].Id = postId + i;
                }
            });

        //Act
        await _postsService.InsertAsync(newPosts);

        //Assert
        newPosts.ForEach(x =>
        {
            Assert.NotEqual(0, x.Id);
        });
    }

    #endregion

    #endregion

    #region Update

    #region Upadate function

    /// <summary>
    /// Verify that function Update has been called.
    /// </summary>
    /// <param name="newTitle">The new title.</param>
    [Theory]
    [InlineData("New title")]
    public void Verify_FunctionUpdate_HasBeenCalled(string newTitle)
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.Insert(newPost))
            .Callback(() =>
            {
                newPost.Id = postId;
            });
        _postsRepositoryMock.Setup(x => x.GetById(postId))
            .Returns(() => newPost);

        //Act
        _postsService.Insert(newPost);
        var post = _postsService.Find(postId);
        post.Title = newTitle;
        _postsService.Update(post);

        //Assert
        _postsRepositoryMock.Verify(x => x.Update(post), Times.Once);
    }

    /// <summary>
    /// Update post.
    /// Should return post when post updated.
    /// </summary>
    /// <param name="newTitle">The new title.</param>
    [Theory]
    [InlineData("New title")]
    public void Update_WhenPostExists_ShouldReturnPost(string newTitle)
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.Insert(newPost))
            .Callback(() =>
            {
                newPost.Id = postId;
            });
        _postsRepositoryMock.Setup(x => x.GetById(postId))
            .Returns(() => newPost);

        //Act
        _postsService.Insert(newPost);
        var post = _postsService.Find(postId);
        post.Title = newTitle;
        _postsService.Update(post);

        //Assert
        Assert.Equal(newTitle, post.Title);
    }

    #endregion

    #region Upadate Enumerable function

    /// <summary>
    /// Verify that function Update Enumerable has been called.
    /// </summary>
    /// <param name="newTitle">The new title.</param>
    [Theory]
    [InlineData("New title")]
    public void Verify_FunctionUpdateEnumerable_HasBeenCalled(string newTitle)
    {
        //Arrange
        var random = new Random();
        var postId = _fixture.Create<int>();
        var itemsCount = random.Next(100);
        var newPosts =
            SetupPostFixture()
                .CreateMany(itemsCount)
                .ToList();

        _postsRepositoryMock.Setup(x => x.Insert(newPosts))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newPosts[i].Id = postId + i;
                }
            });

        //Act
        _postsService.Insert(newPosts);
        newPosts.ForEach(post =>
        {
            post.Title = newTitle;
        });
        for (var i = 0; i < itemsCount; i++)
        {
            newPosts[i].Title = $"{newTitle} {i}";
        }
        _postsService.Update(newPosts);

        //Assert
        _postsRepositoryMock.Verify(x => x.Update(newPosts), Times.Once);
    }

    /// <summary>
    /// Update Enumerable post.
    /// Should return post when post updated.
    /// </summary>
    /// <param name="newTitle">The new title.</param>
    [Theory]
    [InlineData("New title")]
    public void UpdateEnumerable_WhenPostExists_ShouldReturnPost(string newTitle)
    {
        //Arrange
        var random = new Random();
        var postId = _fixture.Create<int>();
        var itemsCount = random.Next(100);
        var newPosts =
            SetupPostFixture()
                .CreateMany(itemsCount)
                .ToList();

        _postsRepositoryMock.Setup(x => x.Insert(newPosts))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newPosts[i].Id = postId + i;
                }
            });

        //Act
        _postsService.Insert(newPosts);
        for (var i = 0; i < itemsCount; i++)
        {
            newPosts[i].Title = $"{newTitle} {i}";
        }
        _postsService.Update(newPosts);

        //Assert
        for (var i = 0; i < itemsCount; i++)
        {
            Assert.Equal($"{newTitle} {i}", newPosts[i].Title);
        }
    }

    #endregion

    #region Update Async function

    /// <summary>
    /// Verify that function Update Async has been called.
    /// Should return post when post updated.
    /// </summary>
    /// <param name="newTitle">The new title.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("New title")]
    public async Task Verify_FunctionUpdateAsync_HasBeenCalled(string newTitle)
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.InsertAsync(newPost))
            .Callback(() =>
            {
                newPost.Id = postId;
            });
        _postsRepositoryMock.Setup(x => x.GetByIdAsync(postId))
            .ReturnsAsync(() => newPost);

        //Act
        await _postsService.InsertAsync(newPost);
        var post = await _postsService.FindAsync(postId);
        post.Title = newTitle;
        await _postsService.UpdateAsync(post);

        //Assert
        _postsRepositoryMock.Verify(x => x.UpdateAsync(post), Times.Once);
    }

    /// <summary>
    /// Async update post.
    /// Should return post when post updated.
    /// </summary>
    /// <param name="newTitle">The new title.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("New title")]
    public async Task UpdateAsync_WhenPostExists_ShouldReturnPost(string newTitle)
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.InsertAsync(newPost))
            .Callback(() =>
            {
                newPost.Id = postId;
            });
        _postsRepositoryMock.Setup(x => x.GetByIdAsync(postId))
            .ReturnsAsync(() => newPost);

        //Act
        await _postsService.InsertAsync(newPost);
        var post = await _postsService.FindAsync(postId);
        post.Title = newTitle;
        await _postsService.UpdateAsync(post);

        //Assert
        Assert.Equal(newTitle, post.Title);
    }

    #endregion

    #region Upadate Async Enumerable function

    /// <summary>
    /// Verify that function Update Enumerable has been called.
    /// </summary>
    /// <param name="newTitle">The new title.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("New title")]
    public async Task Verify_FunctionUpdateAsyncEnumerable_HasBeenCalled(string newTitle)
    {
        //Arrange
        var random = new Random();
        var postId = _fixture.Create<int>();
        var itemsCount = random.Next(100);
        var newPosts =
            SetupPostFixture()
                .CreateMany(itemsCount)
                .ToList();

        _postsRepositoryMock.Setup(x => x.InsertAsync(newPosts))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newPosts[i].Id = postId + i;
                }
            });

        //Act
        await _postsService.InsertAsync(newPosts);
        newPosts.ForEach(post =>
        {
            post.Title = newTitle;
        });
        for (var i = 0; i < itemsCount; i++)
        {
            newPosts[i].Title = $"{newTitle} {i}";
        }
        await _postsService.UpdateAsync(newPosts);

        //Assert
        _postsRepositoryMock.Verify(x => x.UpdateAsync(newPosts), Times.Once);
    }

    /// <summary>
    /// Update Enumerable post.
    /// Should return post when post updated.
    /// </summary>
    /// <param name="newTitle">The new title.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("New title")]
    public async Task UpdateAsyncEnumerable_WhenPostExists_ShouldReturnPost(string newTitle)
    {
        //Arrange
        var random = new Random();
        var postId = _fixture.Create<int>();
        var itemsCount = random.Next(100);
        var newPosts =
            SetupPostFixture()
                .CreateMany(itemsCount)
                .ToList();

        _postsRepositoryMock.Setup(x => x.InsertAsync(newPosts))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newPosts[i].Id = postId + i;
                }
            });

        //Act
        await _postsService.InsertAsync(newPosts);
        for (var i = 0; i < itemsCount; i++)
        {
            newPosts[i].Title = $"{newTitle} {i}";
        }
        await _postsService.UpdateAsync(newPosts);

        //Assert
        for (var i = 0; i < itemsCount; i++)
        {
            Assert.Equal($"{newTitle} {i}", newPosts[i].Title);
        }
    }

    #endregion

    #endregion

    #region Delete

    #region Delete By Id function

    /// <summary>
    /// Verify that function Delete By Id has been called.
    /// </summary>
    [Fact]
    public void Verify_FunctionDeleteById_HasBeenCalled()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost =
            SetupPostFixture()
                .Create();

        _postsRepositoryMock.Setup(x => x.GetById(postId))
            .Returns(() => newPost);

        //Act
        _postsService.Insert(newPost);
        var post = _postsService.Find(postId);
        _postsService.Delete(postId);
        _postsRepositoryMock.Setup(x => x.GetById(postId))
            .Returns(() => null);
        _postsService.Find(postId);

        //Assert
        _postsRepositoryMock.Verify(x => x.Delete(newPost), Times.Once);
    }

    /// <summary>
    /// Delete By Id post.
    /// Should return nothing when post is deleted.
    /// </summary>
    [Fact]
    public void DeleteById_WhenPostIsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost =
            SetupPostFixture()
                .Create();

        _postsRepositoryMock.Setup(x => x.Insert(newPost))
            .Callback(() =>
            {
                newPost.Id = postId;
            });
        _postsRepositoryMock.Setup(x => x.GetById(postId))
            .Returns(() => newPost);

        //Act
        _postsService.Insert(newPost);
        var post = _postsService.Find(postId);
        _postsService.Delete(postId);
        _postsRepositoryMock.Setup(x => x.GetById(postId))
            .Returns(() => null);
        var deletedPost = _postsService.Find(postId);

        //Assert
        Assert.Null(deletedPost);
    }

    #endregion

    #region Delete By Object function

    /// <summary>
    /// Verify that function Delete By Object has been called.
    /// </summary>
    [Fact]
    public void Verify_FunctionDeleteByObject_HasBeenCalled()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.GetById(postId))
            .Returns(() => newPost);

        //Act
        _postsService.Insert(newPost);
        var post = _postsService.Find(postId);
        _postsService.Delete(post);
        _postsRepositoryMock.Setup(x => x.GetById(postId))
            .Returns(() => null);
        _postsService.Find(postId);

        //Assert
        _postsRepositoryMock.Verify(x => x.Delete(post), Times.Once);
    }

    /// <summary>
    /// Delete By Object post.
    /// Should return nothing when post is deleted.
    /// </summary>
    [Fact]
    public void DeleteByObject_WhenPostIsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.Insert(newPost))
            .Callback(() =>
            {
                newPost.Id = postId;
            });
        _postsRepositoryMock.Setup(x => x.GetById(postId))
            .Returns(() => newPost);

        //Act
        _postsService.Insert(newPost);
        var post = _postsService.Find(postId);
        _postsService.Delete(post);
        _postsRepositoryMock.Setup(x => x.GetById(postId))
            .Returns(() => null);
        var deletedPost = _postsService.Find(postId);

        //Assert
        Assert.Null(deletedPost);
    }

    #endregion

    #region Delete By Enumerable function

    /// <summary>
    /// Verify that function Delete By Enumerable has been called.
    /// </summary>
    [Fact]
    public void Verify_FunctionDeleteByEnumerable_HasBeenCalled()
    {
        //Arrange
        var random = new Random();
        var postId = random.Next(52);
        var itemsCount = random.Next(10);

        var newPosts =
            SetupPostFixture()
                .CreateMany(itemsCount)
                .ToList();

        _postsRepositoryMock.Setup(x => x.Insert(newPosts))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newPosts[i].Id = postId + i;
                }
            });

        //Act
        _postsService.Insert(newPosts);
        _postsService.Delete(newPosts);

        //Assert
        _postsRepositoryMock.Verify(x => x.Delete(newPosts), Times.Once);
    }

    /// <summary>
    /// Delete By Enumerable post.
    /// Should return nothing when post is deleted.
    /// </summary>
    [Fact]
    public void DeleteByEnumerable_WhenPostIsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var random = new Random();
        var postId = random.Next(52);
        var itemsCount = random.Next(10);

        var newPosts =
            SetupPostFixture()
                .CreateMany(itemsCount)
                .ToList();

        _postsRepositoryMock.Setup(x => x.Insert(newPosts))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newPosts[i].Id = postId + i;
                }
            });

        _postsRepositoryMock.Setup(x => x.Delete(newPosts))
            .Callback(() =>
            {
                newPosts = null;
            });

        //Act
        _postsService.Insert(newPosts);
        _postsService.Delete(newPosts);

        //Assert
        Assert.Null(newPosts);
    }

    #endregion

    #region Delete Async By Id function

    /// <summary>
    /// Verify that function Delete Async By Id has been called.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task Verify_FunctionDeleteAsyncById_HasBeenCalled()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.GetByIdAsync(postId))
            .ReturnsAsync(() => newPost);

        //Act
        await _postsService.InsertAsync(newPost);
        await _postsService.DeleteAsync(postId);

        //Assert
        _postsRepositoryMock.Verify(x => x.DeleteAsync(newPost), Times.Once);
    }

    /// <summary>
    /// Async delete by id post.
    /// Should return nothing when post is deleted.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task DeleteAsyncById_WhenPostIsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.InsertAsync(newPost))
            .Callback(() =>
            {
                newPost.Id = postId;
            });
        _postsRepositoryMock.Setup(x => x.GetByIdAsync(postId))
            .ReturnsAsync(() => newPost);

        //Act
        await _postsService.InsertAsync(newPost);
        await _postsService.DeleteAsync(postId);
        _postsRepositoryMock.Setup(x => x.GetByIdAsync(postId))
            .ReturnsAsync(() => null);
        var deletedPost = await _postsService.FindAsync(postId);

        //Assert
        Assert.Null(deletedPost);
    }

    #endregion

    #region Delete Async By Object function

    /// <summary>
    /// Verify that function Delete Async By Object has been called.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task Verify_FunctionDeleteAsyncByObject_HasBeenCalled()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.GetByIdAsync(postId))
            .ReturnsAsync(() => newPost);

        //Act
        await _postsService.InsertAsync(newPost);
        var post = await _postsService.FindAsync(postId);
        await _postsService.DeleteAsync(post);

        //Assert
        _postsRepositoryMock.Verify(x => x.DeleteAsync(post), Times.Once);
    }

    /// <summary>
    /// Async delete by object post.
    /// Should return nothing when post is deleted.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task DeleteAsyncByObject_WhenPostIsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var postId = _fixture.Create<int>();
        var newPost = SetupPostFixture().Create();

        _postsRepositoryMock.Setup(x => x.InsertAsync(newPost))
            .Callback(() =>
            {
                newPost.Id = postId;
            });
        _postsRepositoryMock.Setup(x => x.GetByIdAsync(postId))
            .ReturnsAsync(() => newPost);

        //Act
        await _postsService.InsertAsync(newPost);
        var post = await _postsService.FindAsync(postId);
        await _postsService.DeleteAsync(post);
        _postsRepositoryMock.Setup(x => x.GetByIdAsync(postId))
            .ReturnsAsync(() => null);
        var deletedPost = await _postsService.FindAsync(postId);

        //Assert
        Assert.Null(deletedPost);
    }

    #endregion

    #region Delete Async By Enumerable function

    /// <summary>
    /// Verify that function Delete Async By Enumerable has been called.
    /// </summary>
    [Fact]
    public async Task Verify_FunctionDeleteAsyncByEnumerable_HasBeenCalled()
    {
        //Arrange
        var random = new Random();
        var postId = random.Next(52);
        var itemsCount = random.Next(10);

        var newPosts =
            SetupPostFixture()
                .CreateMany(itemsCount)
                .ToList();

        _postsRepositoryMock.Setup(x => x.InsertAsync(newPosts))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newPosts[i].Id = postId + i;
                }
            });

        //Act
        await _postsService.InsertAsync(newPosts);
        await _postsService.DeleteAsync(newPosts);

        //Assert
        _postsRepositoryMock.Verify(x => x.DeleteAsync(newPosts), Times.Once);
    }

    /// <summary>
    /// Delete Async By Enumerable post.
    /// Should return nothing when post is deleted.
    /// </summary>
    [Fact]
    public async Task DeleteAsyncByEnumerable_WhenPostIsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var random = new Random();
        var postId = random.Next(52);
        var itemsCount = random.Next(10);

        var newPosts =
            SetupPostFixture()
                .CreateMany(itemsCount)
                .ToList();

        _postsRepositoryMock.Setup(x => x.InsertAsync(newPosts))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newPosts[i].Id = postId + i;
                }
            });
        _postsRepositoryMock.Setup(x => x.DeleteAsync(newPosts))
            .Callback(() =>
            {
                newPosts = null;
            });

        //Act
        await _postsService.InsertAsync(newPosts);
        await _postsService.DeleteAsync(newPosts);

        //Assert
        Assert.Null(newPosts);
    }

    #endregion

    #endregion

    #region Any

    #region Any function With Specification

    /// <summary>
    /// Verify that function Any with specification has been called.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests ")]
    public void Verify_FunctionAny_WithSpecification_HasBeenCalled(string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Contains(titleSearch));
        _postsRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => postsList.Any(x => x.Title.Contains(titleSearch)));

        //Act
        _postsService.Any(specification);

        //Assert
        _postsRepositoryMock.Verify(x => x.Any(specification), Times.Once);
    }

    /// <summary>
    /// Check if there are any posts with specification.
    /// Should return true with contains specification when posts exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests ")]
    public void Any_WithContainsSpecification_WhenPostsExists_ShouldReturnTrue(string titleSearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Contains(titleSearch));
        _postsRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => postsList.Any(x => x.Title.Contains(titleSearch)));

        //Act
        var areAnyPosts = _postsService.Any(specification);

        //Assert
        Assert.True(areAnyPosts);
    }

    /// <summary>
    /// Check if there are any posts with specification.
    /// Should return post with equal specification when posts exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests 0")]
    public void Any_WithEqualsSpecification_WhenPostsExists_ShouldReturnTrue(string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => postsList.Any(x => x.Title.Contains(titleSearch)));

        //Act
        var areAnyPosts = _postsService.Any(specification);

        //Assert
        Assert.True(areAnyPosts);
    }

    /// <summary>
    /// Check if there are any posts with specification.
    /// Should return false with when posts does not exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests -1")]
    public void Any_WithEqualSpecification_WhenPostsExists_ShouldReturnFalse(string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => postsList.Any(x => x.Title.Contains(titleSearch)));

        //Act
        var areAnyPosts = _postsService.Any(specification);

        //Assert
        Assert.False(areAnyPosts);
    }

    /// <summary>
    /// Check if there are any posts with specification.
    /// Should return false with when posts does not exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests 0")]
    public void Any_WithEqualSpecification_WhenPostDoesNotExists_ShouldReturnNothing(string titleSearch)
    {
        //Arrange
        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => false);

        //Act
        var areAnyPosts = _postsService.Any(specification);

        //Assert
        Assert.False(areAnyPosts);
    }

    #endregion

    #region Any Async function With Specification

    /// <summary>
    /// Verify that function Any Async with specification has been called.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Created from ServicesTests ")]
    public async Task Verify_FunctionAnyAsync_WithSpecification_HasBeenCalled(string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Contains(titleSearch));
        _postsRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => postsList.Any(x => x.Title.Contains(titleSearch)));

        //Act
        await _postsService.AnyAsync(specification);

        //Assert
        _postsRepositoryMock.Verify(x => x.AnyAsync(specification), Times.Once);
    }

    /// <summary>
    /// Async check if there are any posts with specification.
    /// Should return true with contains specification when posts exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Created from ServicesTests ")]
    public async Task AnyAsync_WithContainsSpecification_WhenPostsExists_ShouldReturnTrue(string titleSearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Contains(titleSearch));
        _postsRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => postsList.Any(x => x.Title.Contains(titleSearch)));

        //Act
        var areAnyPosts = await _postsService.AnyAsync(specification);

        //Assert
        Assert.True(areAnyPosts);
    }

    /// <summary>
    /// Async check if there are any posts with specification.
    /// Should return post with equal specification when posts exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Created from ServicesTests 0")]
    public async Task AnyAsync_WithEqualsSpecification_WhenPostsExists_ShouldReturnTrue(string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => postsList.Any(x => x.Title.Contains(titleSearch)));

        //Act
        var areAnyPosts = await _postsService.AnyAsync(specification);

        //Assert
        Assert.True(areAnyPosts);
    }

    /// <summary>
    /// Async check if there are any posts with specification.
    /// Should return false with when posts does not exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Created from ServicesTests -1")]
    public async Task AnyAsync_WithEqualSpecification_WhenPostsExists_ShouldReturnFalse(string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => postsList.Any(x => x.Title.Contains(titleSearch)));

        //Act
        var areAnyPosts = await _postsService.AnyAsync(specification);

        //Assert
        Assert.False(areAnyPosts);
    }

    /// <summary>
    /// Async check if there are any posts with specification.
    /// Should return false with when posts does not exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Created from ServicesTests 0")]
    public async Task AnyAsync_WithEqualSpecification_WhenPostDoesNotExists_ShouldReturnNothing(string titleSearch)
    {
        //Arrange
        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => false);

        //Act
        var areAnyPosts = await _postsService.AnyAsync(specification);

        //Assert
        Assert.False(areAnyPosts);
    }

    #endregion

    #endregion

    #region First Or Default function With Specification

    /// <summary>
    /// Verify that function First Or Default with specification has been called.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests ")]
    public void Verify_FunctionFirstOrDefault_WithSpecification_HasBeenCalled(string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Contains(titleSearch));
        _postsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => postsList.FirstOrDefault(x => x.Title.Contains(titleSearch)));

        //Act
        _postsService.FirstOrDefault(specification);

        //Assert
        _postsRepositoryMock.Verify(x => x.FirstOrDefault(specification), Times.Once);
    }

    /// <summary>
    /// Get first or default post with specification.
    /// Should return post with contains specification when posts exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests ")]
    public void FirstOrDefault_WithContainsSpecification_WhenPostsExists_ShouldReturnTrue(string titleSearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Contains(titleSearch));
        _postsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => postsList.FirstOrDefault(x => x.Title.Contains(titleSearch)));

        //Act
        var post = _postsService.FirstOrDefault(specification);

        //Assert
        Assert.NotNull(post);
        Assert.IsType<Post>(post);
    }

    /// <summary>
    /// Get first or default post with specification.
    /// Should return post with equal specification when posts exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests 0")]
    public void FirstOrDefault_WithEqualsSpecification_WhenPostsExists_ShouldReturnTrue(string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => postsList.FirstOrDefault(x => x.Title.Contains(titleSearch)));

        //Act
        var post = _postsService.FirstOrDefault(specification);

        //Assert
        Assert.NotNull(post);
        Assert.IsType<Post>(post);
    }

    /// <summary>
    /// Get first or default post with specification.
    /// Should return nothing with when post does not exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests -1")]
    public void FirstOrDefault_WithEqualSpecification_WhenPostsExists_ShouldReturnNothing(string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => postsList.FirstOrDefault(x => x.Title.Contains(titleSearch)));

        //Act
        var post = _postsService.FirstOrDefault(specification);

        //Assert
        Assert.Null(post);
    }

    /// <summary>
    /// Get first or default post with specification.
    /// Should return nothing with when posts does not exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests 0")]
    public void FirstOrDefault_WithEqualSpecification_WhenPostDoesNotExists_ShouldReturnNothing(string titleSearch)
    {
        //Arrange
        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => null);

        //Act
        var post = _postsService.FirstOrDefault(specification);

        //Assert
        Assert.Null(post);
    }

    #endregion

    #region Last Or Default function With Specification

    /// <summary>
    /// Verify that function Last Or Default with specification has been called.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests ")]
    public void Verify_FunctionLastOrDefault_WithSpecification_HasBeenCalled(string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Contains(titleSearch));
        _postsRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => postsList.LastOrDefault(x => x.Title.Contains(titleSearch)));

        //Act
        _postsService.LastOrDefault(specification);

        //Assert
        _postsRepositoryMock.Verify(x => x.LastOrDefault(specification), Times.Once);
    }

    /// <summary>
    /// Get last or default post with specification.
    /// Should return post with contains specification when posts exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests ")]
    public void LastOrDefault_WithContainsSpecification_WhenPostsExists_ShouldReturnTrue(string titleSearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Contains(titleSearch));
        _postsRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => postsList.LastOrDefault(x => x.Title.Contains(titleSearch)));

        //Act
        var post = _postsService.LastOrDefault(specification);

        //Assert
        Assert.NotNull(post);
        Assert.IsType<Post>(post);
    }

    /// <summary>
    /// Get last or default post with specification.
    /// Should return post with equal specification when posts exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests 0")]
    public void LastOrDefault_WithEqualsSpecification_WhenPostsExists_ShouldReturnTrue(string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, titleSearch)
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => postsList.LastOrDefault(x => x.Title.Contains(titleSearch)));

        //Act
        var post = _postsService.LastOrDefault(specification);

        //Assert
        Assert.NotNull(post);
        Assert.IsType<Post>(post);
    }

    /// <summary>
    /// Get last or default post with specification.
    /// Should return nothing with when post does not exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests -1")]
    public void LastOrDefault_WithEqualSpecification_WhenPostsExists_ShouldReturnNothing(string titleSearch)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .CreateMany(random.Next(100));

        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => postsList.LastOrDefault(x => x.Title.Contains(titleSearch)));

        //Act
        var post = _postsService.LastOrDefault(specification);

        //Assert
        Assert.Null(post);
    }

    /// <summary>
    /// Get last or default post with specification.
    /// Should return nothing with when posts does not exists.
    /// </summary>
    /// <param name="titleSearch">The title search.</param>
    [Theory]
    [InlineData("Created from ServicesTests 0")]
    public void LastOrDefault_WithEqualSpecification_WhenPostDoesNotExists_ShouldReturnNothing(string titleSearch)
    {
        //Arrange
        var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
        _postsRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => null);

        //Act
        var post = _postsService.LastOrDefault(specification);

        //Assert
        Assert.Null(post);
    }

    #endregion

    #region Search async function

    /// <summary>
    /// Search the specified query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="postsList">The posts list.</param>
    /// <returns>PagedListResult.</returns>
    protected static PagedListResult<Post> Search(SearchQuery<Post> query, List<Post> postsList)
    {
        var sequence = postsList.AsQueryable();

        // Applying filters
        if (query.Filters is { Count: > 0 })
        {
            foreach (var filterClause in query.Filters)
            {
                sequence = sequence.Where(filterClause);
                var a = sequence.Select(x => x).ToList();
            }
        }

        // Include Properties
        if (!string.IsNullOrWhiteSpace(query.IncludeProperties))
        {
            var properties = query.IncludeProperties.Split([","], StringSplitOptions.RemoveEmptyEntries);

            sequence = properties.Aggregate(sequence, (current, includeProperty) => current.Include(includeProperty));
        }
        var b = sequence.ToList();

        // Resolving Sort Criteria
        // This code applies the sorting criterias sent as the parameter
        if (query.SortCriterias is { Count: > 0 })
        {
            var sortCriteria = query.SortCriterias[0];
            var orderedSequence = sortCriteria.ApplyOrdering(sequence, false);

            if (query.SortCriterias.Count > 1)
            {
                for (var i = 1; i < query.SortCriterias.Count; i++)
                {
                    var sc = query.SortCriterias[i];
                    orderedSequence = sc.ApplyOrdering(orderedSequence, true);
                }
            }

            sequence = orderedSequence;
        }
        else
        {
            sequence = ((IOrderedQueryable<Post>)sequence).OrderBy(x => true);
        }

        // Counting the total number of object.
        var resultCount = sequence.Count();

        var result = (query.Take > 0)
            ? sequence.Skip(query.Skip).Take(query.Take).ToList()
            : sequence.ToList();

        // Debug info of what the query looks like
        // Console.WriteLine(sequence.ToString());

        // Setting up the return object.
        bool hasNext = (query.Skip > 0 || query.Take > 0) && (query.Skip + query.Take < resultCount);
        return new PagedListResult<Post>
        {
            Entities = result,
            HasNext = hasNext,
            HasPrevious = query.Skip > 0,
            Count = resultCount,
        };
    }

    /// <summary>
    /// Verify that function Search async has been called.
    /// </summary>
    /// <param name="search">The search.</param>
    /// <param name="start">The start.</param>
    /// <param name="take">The take.</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="orderType">The order type.</param>
    [Theory]
    [InlineData("Created from ServicesTests ", 0, 10, "Title", OrderType.Ascending)]
    [InlineData("Created from ServicesTests ", 10, 10, "Title", OrderType.Ascending)]
    [InlineData("Created from ServicesTests ", 10, 20, "Title", OrderType.Ascending)]
    [InlineData("Created from ServicesTests ", 0, 100, "Title", OrderType.Ascending)]
    public async Task Verify_FunctionSearchAsync_HasBeenCalled(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, search)
                .CreateMany(random.Next(100))
                .ToList();

        var query = new SearchQuery<Post>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Post>(fieldName, orderType));

        query.AddFilter(x => x.Title.ToUpper().Contains($"{search}".ToUpper()));

        _postsRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => Search(query, postsList));

        //Act
        await _postsService.SearchAsync(query);

        //Assert
        _postsRepositoryMock.Verify(x => x.SearchAsync(query), Times.Once);
    }

    /// <summary>
    /// Search async posts.
    /// Should return posts when posts exists.
    /// </summary>
    /// <param name="search">The search.</param>
    /// <param name="start">The start.</param>
    /// <param name="take">The take.</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="orderType">The order type.</param>
    [Theory]
    [InlineData("Created from ServicesTests ", 0, 10, "Title", OrderType.Ascending)]
    [InlineData("Created from ServicesTests ", 10, 10, "Title", OrderType.Ascending)]
    [InlineData("Created from ServicesTests ", 10, 20, "Title", OrderType.Ascending)]
    [InlineData("Created from ServicesTests ", 0, 100, "Title", OrderType.Ascending)]
    public async Task SearchAsync_WhenPostsExists_ShouldReturnPosts(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, search)
                .CreateMany(random.Next(100))
                .ToList();

        var query = new SearchQuery<Post>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Post>(fieldName, orderType));

        query.AddFilter(x => x.Title.ToUpper().Contains($"{search}".ToUpper()));

        _postsRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => Search(query, postsList));

        //Act
        var posts = await _postsService.SearchAsync(query);

        //Assert
        Assert.NotNull(posts);
        Assert.NotEmpty(posts.Entities);
    }

    /// <summary>
    /// Search async posts with specification.
    /// Should return post with equal specification when posts exists.
    /// </summary>
    /// <param name="search">The search.</param>
    /// <param name="start">The start.</param>
    /// <param name="take">The take.</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="orderType">The order type.</param>
    [Theory]
    [InlineData("Created from ServicesTests 0", 0, 10, "Title", OrderType.Ascending)]
    [InlineData("Created from ServicesTests 11", 10, 10, "Title", OrderType.Ascending)]
    [InlineData("Created from ServicesTests 11", 10, 20, "Title", OrderType.Ascending)]
    [InlineData("Created from ServicesTests 11", 0, 100, "Title", OrderType.Ascending)]
    public async Task SearchAsync_WithEqualsSpecification_WhenPostsExists_ShouldReturnPost(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var postsList =
            SetupPostFixture()
                .With(x => x.Title, search)
                .CreateMany(start + 1)
                .ToList();

        var query = new SearchQuery<Post>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Post>(fieldName, orderType));

        query.AddFilter(x => x.Title.ToUpper().Contains($"{search}".ToUpper()));

        _postsRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => Search(query, postsList));

        //Act
        var posts = await _postsService.SearchAsync(query);

        //Assert
        Assert.NotNull(posts);
        Assert.NotEmpty(posts.Entities);
        Assert.Single(posts.Entities);
    }

    /// <summary>
    /// Search async posts with specification.
    /// Should return nothing with  when posts exists.
    /// </summary>
    /// <param name="search">The search.</param>
    /// <param name="start">The start.</param>
    /// <param name="take">The take.</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="orderType">The order type.</param>
    [Theory]
    [InlineData("Created from ServicesTests -0", 0, 10, "Title", OrderType.Ascending)]
    [InlineData("Created from ServicesTests -11", 10, 10, "Title", OrderType.Ascending)]
    [InlineData("Created from ServicesTests -11", 10, 20, "Title", OrderType.Ascending)]
    [InlineData("Created from ServicesTests -11", 0, 100, "Title", OrderType.Ascending)]
    public async Task SearchAsync_WithEqualSpecification_WhenPostsExists_ShouldReturnNothing(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var random = new Random();
        var postsList =
            SetupPostFixture()
                .CreateMany(random.Next(100))
                .ToList();

        var query = new SearchQuery<Post>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Post>(fieldName, orderType));

        query.AddFilter(x => x.Title.ToUpper().Contains($"{search}".ToUpper()));

        _postsRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => Search(query, postsList));

        //Act
        var posts = await _postsService.SearchAsync(query);

        //Assert
        Assert.NotNull(posts);
        Assert.Empty(posts.Entities);
    }

    /// <summary>
    /// Search async posts.
    /// Should return nothing when posts does not exists.
    /// </summary>
    /// <param name="search">The search.</param>
    /// <param name="start">The start.</param>
    /// <param name="take">The take.</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="orderType">The order type.</param>
    [Theory]
    [InlineData("Created from ServicesTests 0", 0, 10, "Title", OrderType.Ascending)]
    [InlineData("Created from ServicesTests 11", 10, 10, "Title", OrderType.Ascending)]
    [InlineData("Created from ServicesTests 11", 10, 20, "Title", OrderType.Ascending)]
    [InlineData("Created from ServicesTests 11", 0, 100, "Title", OrderType.Ascending)]
    public async Task SearchAsync_WhenPostsDoesNotExists_ShouldReturnNothing(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var query = new SearchQuery<Post>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Post>(fieldName, orderType));

        query.AddFilter(x => x.Title.ToUpper().Contains($"{search}".ToUpper()));

        _postsRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => new PagedListResult<Post>());

        //Act
        var posts = await _postsService.SearchAsync(query);

        //Assert
        Assert.Null(posts.Entities);
    }

    #endregion

    #region NotTestedYet
    //GenerateQuery(TableFilter tableFilter, string includeProperties = null)
    //GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
    #endregion

    #endregion
}