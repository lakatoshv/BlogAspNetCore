using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Dsl;
using Blog.Core.Enums;
using Blog.Core.Infrastructure;
using Blog.Core.Infrastructure.Pagination;
using Blog.Core.TableFilters;
using Blog.Data.Models;
using Blog.Data.Repository;
using Blog.Data.Specifications;
using Blog.EntityServices;
using Blog.EntityServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Blog.ServicesTests.EntityServices;

/// <summary>
/// Comments service tests.
/// </summary>
public class CommentsServiceTests
{
    #region Fields

    /// <summary>
    /// The comments service.
    /// </summary>
    private readonly ICommentsService _commentsService;

    /// <summary>
    /// The comments repository mock.
    /// </summary>
    private readonly Mock<IRepository<Comment>> _commentsRepositoryMock;

    /// <summary>
    /// The fixture.
    /// </summary>
    private readonly Fixture _fixture;

    #endregion

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentsServiceTests"/> class.
    /// </summary>
    public CommentsServiceTests()
    {
        _commentsRepositoryMock = new Mock<IRepository<Comment>>();
        _commentsService = new CommentsService(_commentsRepositoryMock.Object);
        _fixture = new Fixture();
    }

    #endregion

    #region Uthilities

    private IPostprocessComposer<Comment> SetupCommentFixture(string commentBody = null)
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
        var post =
            _fixture.Build<Post>()
                .With(x => x.Author, applicationUser)
                .Without(x => x.Category)
                .Without(x => x.Comments)
                .Without(x => x.PostsTagsRelations)
                .Create();

        var commentSetup =
            _fixture.Build<Comment>()
                .With(x => x.User, applicationUser)
                .With(x => x.Post, post);
        if (!string.IsNullOrWhiteSpace(commentBody))
        {
            commentSetup.With(x => x.CommentBody, commentBody);
        }

        return commentSetup;
    }

    #endregion

    #region Tests

    #region Get All service tests

    #region Get All function

    /// <summary>
    /// Verify that function Get All has been called.
    /// </summary>
    [Fact]
    public void Verify_FunctionGetAll_HasBeenCalled()
    {
        //Arrange
        var random = new Random();
        var commentsList =
            SetupCommentFixture()
                .CreateMany(random.Next(100));

        _commentsRepositoryMock.Setup(x => x.GetAll())
            .Returns(commentsList.AsQueryable());

        //Act
        _commentsService.GetAll();

        //Assert
        _commentsRepositoryMock.Verify(x => x.GetAll(), Times.Once);
    }

    /// <summary>
    /// Get all comments.
    /// Should return comments when comments exists.
    /// </summary>
    /// <param name="notEqualCount">The not equal count.</param>
    [Theory]
    [InlineData(0)]
    public void GetAll_WhenCommentsExists_ShouldReturnComments(int notEqualCount)
    {
        //Arrange
        var random = new Random();
        var commentsList =
            SetupCommentFixture()
                .CreateMany(random.Next(100));

        _commentsRepositoryMock.Setup(x => x.GetAll())
            .Returns(() => commentsList.AsQueryable());

        //Act
        var comments = _commentsService.GetAll();

        //Assert
        Assert.NotNull(comments);
        Assert.NotEmpty(comments);
        Assert.NotEqual(notEqualCount, comments.ToList().Count);
    }

    /// <summary>
    /// Get all comments.
    /// Should return nothing when comments does not exists.
    /// </summary>
    [Fact]
    public void GetAll_WhenCommentsDoesNotExists_ShouldReturnNothing()
    {
        //Arrange
        _commentsRepositoryMock.Setup(x => x.GetAll())
            .Returns(() => new List<Comment>().AsQueryable());

        //Act
        var comments = _commentsService.GetAll();

        //Assert
        Assert.Empty(comments);
    }

    /// <summary>
    /// Get all comments.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public void GetAll_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        _commentsRepositoryMock.Setup(x => x.GetAll())
            .Throws(() => new Exception("Test exception"));

        //Assert
        Assert.Throws<Exception>(() => _commentsService.GetAll());
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
        var commentsList =
            SetupCommentFixture()
                .CreateMany(random.Next(100));

        _commentsRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(() => commentsList.ToList());

        //Act
        await _commentsService.GetAllAsync();

        //Assert
        _commentsRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    /// <summary>
    /// Get all async comments.
    /// Should return comments when comments exists.
    /// </summary>
    /// <param name="notEqualCount">The not equal count.</param>
    [Theory]
    [InlineData(0)]
    public async Task GetAllAsync_WhenCommentsExists_ShouldReturnComments(int notEqualCount)
    {
        //Arrange
        var random = new Random();
        var commentsList =
            SetupCommentFixture()
                .CreateMany(random.Next(100));

        _commentsRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(() => commentsList.ToList());

        //Act
        var comments = await _commentsService.GetAllAsync();

        //Assert
        Assert.NotNull(comments);
        Assert.NotEmpty(comments);
        Assert.NotEqual(notEqualCount, comments.ToList().Count);
    }

    /// <summary>
    /// Get all async comments.
    /// Should return nothing when comments does not exists.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_WhenCommentsDoesNotExists_ShouldReturnNothing()
    {
        //Arrange
        _commentsRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(() => []);

        //Act
        var comments = await _commentsService.GetAllAsync();

        //Assert
        Assert.Empty(comments);
    }

    /// <summary>
    /// Get all async comments.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_WhenRepositoryExceptionIsThrown_ShouldThrowException()
    {
        //Arrange
        _commentsRepositoryMock.Setup(x => x.GetAllAsync())
            .ThrowsAsync(new Exception("Test exception"));

        //Assert
        await Assert.ThrowsAsync<Exception>(() => _commentsService.GetAllAsync());
    }

    #endregion

    #region Get all function With Specification

    /// <summary>
    /// Verify that function Get All with specification has been called.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment ")]
    public void Verify_FunctionGetAll_WithSpecification_HasBeenCalled(string commentBodySearch)
    {
        //Arrange
        var random = new Random();
        var commentsList =
            SetupCommentFixture(commentBodySearch)
                .CreateMany(random.Next(100));

        var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.GetAll(It.IsAny<CommentSpecification>()))
            .Returns(() => commentsList.Where(x => x.CommentBody.Contains(commentBodySearch)).AsQueryable());

        //Act
        _commentsService.GetAll(specification);

        //Assert
        _commentsRepositoryMock.Verify(x => x.GetAll(specification), Times.Once);
    }

    /// <summary>
    /// Get all comments with specification.
    /// Should return comments with contains specification when comments exists.
    /// </summary>
    /// <param name="notEqualCount">The not equal count.</param>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData(0, "Comment ")]
    public void GetAll_WithContainsSpecification_WhenCommentsExists_ShouldReturnComments(int notEqualCount, string commentBodySearch)
    {
        //Arrange
        var random = new Random();
        var commentsList =
            SetupCommentFixture(commentBodySearch)
                .With(x => x.CommentBody, commentBodySearch)
                .CreateMany(random.Next(100));

        var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.GetAll(It.IsAny<CommentSpecification>()))
            .Returns(() => commentsList.Where(x => x.CommentBody.Contains(commentBodySearch)).AsQueryable());

        //Act
        var comments = _commentsService.GetAll(specification);

        //Assert
        Assert.NotNull(comments);
        Assert.NotEmpty(comments);
        Assert.NotEqual(notEqualCount, comments.ToList().Count);
    }

    /// <summary>
    /// Get all comments with specification.
    /// Should return comment with equal specification when comments exists.
    /// </summary>
    /// <param name="equalCount">The equal count.</param>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData(1, "Comment 0")]
    public void GetAll_WithEqualsSpecification_WhenCommentsExists_ShouldReturnComment(int equalCount, string commentBodySearch)
    {
        //Arrange
        var commentsList =
            SetupCommentFixture(commentBodySearch)
                .With(x => x.CommentBody, commentBodySearch)
                .CreateMany(1);

        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.GetAll(It.IsAny<CommentSpecification>()))
            .Returns(() => commentsList.Where(x => x.CommentBody.Contains(commentBodySearch)).AsQueryable());

        //Act
        var comments = _commentsService.GetAll(specification);

        //Assert
        Assert.NotNull(comments);
        Assert.NotEmpty(comments);
        Assert.Equal(equalCount, comments.ToList().Count);
    }

    /// <summary>
    /// Get all comments with specification.
    /// Should return nothing with  when comments does not exists.
    /// </summary>
    /// <param name="equalCount">The equal count.</param>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData(0, "Comment -1")]
    public void GetAll_WithEqualSpecification_WhenCommentsExists_ShouldReturnNothing(int equalCount, string commentBodySearch)
    {
        //Arrange
        var random = new Random();
        var commentsList =
            SetupCommentFixture("Comment 1")
                .With(x => x.CommentBody, "Comment 1")
                .CreateMany(random.Next(100));

        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.GetAll(It.IsAny<CommentSpecification>()))
            .Returns(() => commentsList.Where(x => x.CommentBody.Contains(commentBodySearch)).AsQueryable());

        //Act
        var comments = _commentsService.GetAll(specification);

        //Assert
        Assert.NotNull(comments);
        Assert.Empty(comments);
        Assert.Equal(equalCount, comments.ToList().Count);
    }

    /// <summary>
    /// Get all comments.
    /// Should return nothing with  when comments does not exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment 0")]
    public void GetAll_WithEqualSpecification_WhenCommentDoesNotExists_ShouldReturnNothing(string commentBodySearch)
    {
        //Arrange
        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.GetAll(It.IsAny<CommentSpecification>()))
            .Returns(() => new List<Comment>().AsQueryable());

        //Act
        var comments = _commentsService.GetAll(specification);

        //Assert
        Assert.Empty(comments);
    }

    #endregion

    #region Get all async function With Specification

    /// <summary>
    /// Verify that function Get All Async with specification has been called.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment ")]
    public async Task Verify_FunctionGetAllAsync_WithSpecification_HasBeenCalled(string commentBodySearch)
    {
        //Arrange
        var random = new Random();
        var commentsList =
            SetupCommentFixture(commentBodySearch)
                .CreateMany(random.Next(100));

        var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CommentSpecification>()))
            .ReturnsAsync(() => commentsList.Where(x => x.CommentBody.Contains(commentBodySearch)).ToList());

        //Act
        await _commentsService.GetAllAsync(specification);

        //Assert
        _commentsRepositoryMock.Verify(x => x.GetAllAsync(specification), Times.Once);
    }

    /// <summary>
    /// Get all async comments with specification.
    /// Should return comments with contains specification when comments exists.
    /// </summary>
    /// <param name="notEqualCount">The not equal count.</param>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData(0, "Comment ")]
    public async Task GetAllAsync_WithContainsSpecification_WhenCommentsExists_ShouldReturnComments(int notEqualCount, string commentBodySearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var commentsList =
            SetupCommentFixture(commentBodySearch)
                .With(x => x.CommentBody, commentBodySearch)
                .CreateMany(random.Next(100));

        var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CommentSpecification>()))
            .ReturnsAsync(() => commentsList.Where(x => x.CommentBody.Contains(commentBodySearch)).ToList());

        //Act
        var comments = await _commentsService.GetAllAsync(specification);

        //Assert
        Assert.NotNull(comments);
        Assert.NotEmpty(comments);
        Assert.NotEqual(notEqualCount, comments.ToList().Count);
    }

    /// <summary>
    /// Get all async comments with specification.
    /// Should return comment with equal specification when comments exists.
    /// </summary>
    /// <param name="equalCount">The equal count.</param>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData(1, "Comment 0")]
    public async Task GetAllAsync_WithEqualsSpecification_WhenCommentsExists_ShouldReturnComment(int equalCount, string commentBodySearch)
    {
        //Arrange
        var commentsList =
            SetupCommentFixture(commentBodySearch)
                .With(x => x.CommentBody, commentBodySearch)
                .CreateMany(1);

        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CommentSpecification>()))
            .ReturnsAsync(() => commentsList.Where(x => x.CommentBody.Contains(commentBodySearch)).ToList());

        //Act
        var comments = await _commentsService.GetAllAsync(specification);

        //Assert
        Assert.NotNull(comments);
        Assert.NotEmpty(comments);
        Assert.Equal(equalCount, comments.ToList().Count);
    }

    /// <summary>
    /// Get all async comments with specification.
    /// Should return nothing with  when comments does not exists.
    /// </summary>
    /// <param name="equalCount">The equal count.</param>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData(0, "Comment -1")]
    public async Task GetAllAsync_WithEqualSpecification_WhenCommentsExists_ShouldReturnNothing(int equalCount, string commentBodySearch)
    {
        //Arrange
        var random = new Random();
        var commentsList =
            SetupCommentFixture("Comment 1")
                .With(x => x.CommentBody, "Comment 1")
                .CreateMany(random.Next(100));

        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CommentSpecification>()))
            .ReturnsAsync(() => commentsList.Where(x => x.CommentBody.Contains(commentBodySearch)).ToList());

        //Act
        var comments = await _commentsService.GetAllAsync(specification);

        //Assert
        Assert.NotNull(comments);
        Assert.Empty(comments);
        Assert.Equal(equalCount, comments.ToList().Count);
    }

    /// <summary>
    /// Get all async comments.
    /// Should return nothing with  when comments does not exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment 0")]
    public async Task GetAllAsync_WithEqualSpecification_WhenCommentDoesNotExists_ShouldReturnNothing(string commentBodySearch)
    {
        //Arrange
        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CommentSpecification>()))
            .ReturnsAsync(() => []);

        //Act
        var comments = await _commentsService.GetAllAsync(specification);

        //Assert
        Assert.NotNull(comments);
        Assert.Empty(comments);
        Assert.Equal(0, comments.ToList().Count);
    }

    #endregion

    #endregion

    #region Find service tests

    #region Find function

    /// <summary>
    /// Verify that function Find has been called.
    /// </summary>
    [Fact]
    public void Verify_FunctionFind_HasBeenCalled()
    {
        //Arrange
        var commentId = _fixture.Create<int>();
        var newComment = SetupCommentFixture().Create();
        _commentsRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(() => newComment);

        //Act
        _commentsService.Find(commentId);

        //Assert
        _commentsRepositoryMock.Verify(x => x.GetById(commentId), Times.Once);
    }

    /// <summary>
    /// Find comment.
    /// Should return comment when comment exists.
    /// </summary>
    [Fact]
    public void Find_WhenCommentExists_ShouldReturnComment()
    {
        //Arrange
        var newComment = SetupCommentFixture().Create();
        _commentsRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(() => newComment);

        //Act
        var comment = _commentsService.Find(newComment.Id);

        //Assert
        Assert.Equal(newComment.Id, comment.Id);
    }

    /// <summary>
    /// Find comment.
    /// Should return nothing when comment does not exists.
    /// </summary>
    [Fact]
    public void Find_WhenCommentDoesNotExists_ShouldReturnNothing()
    {
        //Arrange
        var commentId = _fixture.Create<int>();
        _commentsRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(() => null);

        //Act
        var comment = _commentsService.Find(commentId);

        //Assert
        Assert.Null(comment);
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
        var commentId = _fixture.Create<int>();
        var newComment = SetupCommentFixture().Create();

        _commentsRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => newComment);

        //Act
        await _commentsService.FindAsync(commentId);

        //Assert
        _commentsRepositoryMock.Verify(x => x.GetByIdAsync(commentId), Times.Once);
    }

    /// <summary>
    /// Async find comment.
    /// Should return comment when comment exists.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task FindAsync_WhenCommentExists_ShouldReturnComment()
    {
        //Arrange
        var newComment = SetupCommentFixture().Create();
        _commentsRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => newComment);

        //Act
        var comment = await _commentsService.FindAsync(newComment.Id);

        //Assert
        Assert.Equal(newComment.Id, comment.Id);
    }

    /// <summary>
    /// Async find comment.
    /// Should return nothing when comment does not exists.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task FindAsync_WhenCommentDoesNotExists_ShouldReturnNothing()
    {
        //Arrange
        var commentId = _fixture.Create<int>();

        _commentsRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => null);

        //Act
        var comment = await _commentsService.FindAsync(commentId);

        //Assert
        Assert.Null(comment);
    }

    #endregion

    #endregion

    #region Insert service tests

    #region Insert function

    /// <summary>
    /// Verify that function Insert has been called.
    /// </summary>
    [Fact]
    public void Verify_FunctionInsert_HasBeenCalled()
    {
        //Arrange
        var commentId = _fixture.Create<int>();
        var newComment = SetupCommentFixture().Create();

        _commentsRepositoryMock.Setup(x => x.Insert(It.IsAny<Comment>()))
            .Callback(() =>
            {
                newComment.Id = commentId;
            });

        //Act
        _commentsService.Insert(newComment);

        //Assert
        _commentsRepositoryMock.Verify(x => x.Insert(newComment), Times.Once);
    }

    /// <summary>
    /// Insert comment.
    /// Should return comment when comment created.
    /// </summary>
    [Fact]
    public void Insert_WhenCommentExists_ShouldReturnComment()
    {
        //Arrange
        var commentId = _fixture.Create<int>();
        var newComment = SetupCommentFixture().Create();

        _commentsRepositoryMock.Setup(x => x.Insert(It.IsAny<Comment>()))
            .Callback(() =>
            {
                newComment.Id = commentId;
            });

        //Act
        _commentsService.Insert(newComment);

        //Assert
        Assert.NotEqual(0, newComment.Id);
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
        var commentId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newComments =
            SetupCommentFixture()
                .With(x => x.Id, 1)
                .CreateMany(random.Next(10))
                .ToList();

        _commentsRepositoryMock.Setup(x => x.Insert(It.IsAny<Comment>()))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newComments[i].Id = commentId + i;
                }
            });

        //Act
        _commentsService.Insert(newComments);

        //Assert
        _commentsRepositoryMock.Verify(x => x.Insert(newComments), Times.Once);
    }

    /// <summary>
    /// Insert Enumerable comments.
    /// Should return comments when comments created.
    /// </summary>
    [Fact]
    public void InsertEnumerable_WhenCommentsExists_ShouldReturnComments()
    {
        //Arrange
        var random = new Random();
        var commentId = _fixture.Create<int>(); ;
        var itemsCount = random.Next(10);
        var newComments =
            SetupCommentFixture()
                .With(x => x.Id, 1)
                .CreateMany(itemsCount)
                .ToList();

        _commentsRepositoryMock.Setup(x => x.Insert(It.IsAny<Comment>()))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newComments[i].Id = commentId + i;
                }
            });

        //Act
        _commentsService.Insert(newComments);

        //Assert
        newComments.ForEach(x =>
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
        var commentId = _fixture.Create<int>();
        var newComment = SetupCommentFixture().Create();

        _commentsRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<Comment>()))
            .Callback(() =>
            {
                newComment.Id = commentId;
            });

        //Act
        await _commentsService.InsertAsync(newComment);

        //Assert
        _commentsRepositoryMock.Verify(x => x.InsertAsync(newComment), Times.Once);
    }

    /// <summary>
    /// Async insert comment.
    /// Should return comment when comment created.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task InsertAsync_WhenCommentExists_ShouldReturnComment()
    {
        //Arrange
        var commentId = _fixture.Create<int>();
        var newComment = SetupCommentFixture().Create();

        _commentsRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<Comment>()))
            .Callback(() =>
            {
                newComment.Id = commentId;
            });

        //Act
        await _commentsService.InsertAsync(newComment);

        //Assert
        Assert.NotEqual(0, newComment.Id);
    }

    #endregion

    #region Insert Async Enumerable function

    /// <summary>
    /// Verify that function Insert Async Enumerable has been called.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task Verify_FunctionInsertAsyncEnumerable_HasBeenCalled()
    {
        //Arrange
        var random = new Random();
        var commentId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newComments =
            SetupCommentFixture()
                .With(x => x.Id, 1)
                .CreateMany(itemsCount)
                .ToList();

        _commentsRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<Comment>()))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newComments[i].Id = commentId + i;
                }
            });

        //Act
        await _commentsService.InsertAsync(newComments);

        //Assert
        _commentsRepositoryMock.Verify(x => x.InsertAsync(newComments), Times.Once);
    }

    /// <summary>
    /// Insert Async Enumerable comments.
    /// Should return comments when comments created.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task InsertAsyncEnumerable_WhenCommentsExists_ShouldReturnComments()
    {
        //Arrange
        var random = new Random();
        var commentId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newComments =
            SetupCommentFixture()
                .With(x => x.Id, 1)
                .CreateMany(itemsCount)
                .ToList();

        _commentsRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<Comment>()))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newComments[i].Id = commentId + i;
                }
            });

        //Act
        await _commentsService.InsertAsync(newComments);

        //Assert
        newComments.ForEach(x =>
        {
            Assert.NotEqual(0, x.Id);
        });
    }

    #endregion

    #endregion

    #region Update service tests

    #region Upadate function

    /// <summary>
    /// Verify that function Update has been called.
    /// </summary>
    /// <param name="newCommentBody">The new CommentBody.</param>
    [Theory]
    [InlineData("New CommentBody")]
    public void Verify_FunctionUpdate_HasBeenCalled(string newCommentBody)
    {
        //Arrange
        var commentId = _fixture.Create<int>();
        var newComment = SetupCommentFixture().Create();

        _commentsRepositoryMock.Setup(x => x.Insert(newComment))
            .Callback(() =>
            {
                newComment.Id = commentId;
            });
        _commentsRepositoryMock.Setup(x => x.GetById(commentId))
            .Returns(() => newComment);

        //Act
        _commentsService.Insert(newComment);
        var comment = _commentsService.Find(commentId);
        comment.CommentBody = newCommentBody;
        _commentsService.Update(comment);

        //Assert
        _commentsRepositoryMock.Verify(x => x.Update(comment), Times.Once);
    }

    /// <summary>
    /// Update comment.
    /// Should return comment when comment updated.
    /// </summary>
    /// <param name="newCommentBody">The new CommentBody.</param>
    [Theory]
    [InlineData("New CommentBody")]
    public void Update_WhenCommentExists_ShouldReturnComment(string newCommentBody)
    {
        //Arrange
        var commentId = _fixture.Create<int>();
        var newComment = SetupCommentFixture().Create();

        _commentsRepositoryMock.Setup(x => x.Insert(newComment))
            .Callback(() =>
            {
                newComment.Id = commentId;
            });
        _commentsRepositoryMock.Setup(x => x.GetById(commentId))
            .Returns(() => newComment);

        //Act
        _commentsService.Insert(newComment);
        var comment = _commentsService.Find(commentId);
        comment.CommentBody = newCommentBody;
        _commentsService.Update(comment);

        //Assert
        Assert.Equal(newCommentBody, comment.CommentBody);
    }

    #endregion

    #region Upadate Enumerable function

    /// <summary>
    /// Verify that function Update Enumerable has been called.
    /// </summary>
    /// <param name="newCommentBody">The new CommentBody.</param>
    [Theory]
    [InlineData("New CommentBody")]
    public void Verify_FunctionUpdateEnumerable_HasBeenCalled(string newCommentBody)
    {
        //Arrange
        var random = new Random();
        var commentId = _fixture.Create<int>();
        var itemsCount = random.Next(100);
        var newComments = SetupCommentFixture()
            .CreateMany(itemsCount)
            .ToList();

        _commentsRepositoryMock.Setup(x => x.Insert(newComments))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newComments[i].Id = commentId + i;
                }
            });

        //Act
        _commentsService.Insert(newComments);
        for (var i = 0; i < itemsCount; i++)
        {
            newComments[i].CommentBody = $"{newCommentBody} {i}";
        }
        _commentsService.Update(newComments);

        //Assert
        _commentsRepositoryMock.Verify(x => x.Update(newComments), Times.Once);
    }

    /// <summary>
    /// Update Enumerable comment.
    /// Should return comment when comment updated.
    /// </summary>
    /// <param name="newCommentBody">The new CommentBody.</param>
    [Theory]
    [InlineData("New CommentBody")]
    public void UpdateEnumerable_WhenCommentExists_ShouldReturnComment(string newCommentBody)
    {
        //Arrange
        var random = new Random();
        var commentId = _fixture.Create<int>();
        var itemsCount = random.Next(100);
        var newComments = SetupCommentFixture()
            .CreateMany(itemsCount)
            .ToList();

        _commentsRepositoryMock.Setup(x => x.Insert(newComments))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newComments[i].Id = commentId + i;
                }
            });

        //Act
        _commentsService.Insert(newComments);
        for (var i = 0; i < itemsCount; i++)
        {
            newComments[i].CommentBody = $"{newCommentBody} {i}";
        }
        _commentsService.Update(newComments);

        //Assert

        for (var i = 0; i < itemsCount; i++)
        {
            Assert.Equal($"{newCommentBody} {i}", newComments[i].CommentBody);
        }
    }

    #endregion

    #region Update Async function

    /// <summary>
    /// Verify that function Update Async has been called.
    /// Should return comment when comment updated.
    /// </summary>
    /// <param name="newCommentBody">The new CommentBody.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("New CommentBody")]
    public async Task Verify_FunctionUpdateAsync_HasBeenCalled(string newCommentBody)
    {
        //Arrange
        var commentId = _fixture.Create<int>();
        var newComment = SetupCommentFixture()
            .Create();

        _commentsRepositoryMock.Setup(x => x.InsertAsync(newComment))
            .Callback(() =>
            {
                newComment.Id = commentId;
            });
        _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
            .ReturnsAsync(() => newComment);

        //Act
        await _commentsService.InsertAsync(newComment);
        var comment = await _commentsService.FindAsync(commentId);
        comment.CommentBody = newCommentBody;
        await _commentsService.UpdateAsync(comment);

        //Assert
        _commentsRepositoryMock.Verify(x => x.UpdateAsync(comment), Times.Once);
    }

    /// <summary>
    /// Async update comment.
    /// Should return comment when comment updated.
    /// </summary>
    /// <param name="newCommentBody">The new CommentBody.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("New CommentBody")]
    public async Task UpdateAsync_WhenCommentExists_ShouldReturnComment(string newCommentBody)
    {
        //Arrange
        var commentId = _fixture.Create<int>();
        var newComment = SetupCommentFixture().Create();

        _commentsRepositoryMock.Setup(x => x.InsertAsync(newComment))
            .Callback(() =>
            {
                newComment.Id = commentId;
            });
        _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
            .ReturnsAsync(() => newComment);

        //Act
        await _commentsService.InsertAsync(newComment);
        var comment = await _commentsService.FindAsync(commentId);
        comment.CommentBody = newCommentBody;
        await _commentsService.UpdateAsync(comment);

        //Assert
        Assert.Equal(newCommentBody, comment.CommentBody);
    }

    #endregion

    #region Upadate Async Enumerable function

    /// <summary>
    /// Verify that function Update Async Enumerable has been called.
    /// </summary>
    /// <param name="newCommentBody">The new CommentBody.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("New CommentBody")]
    public async Task Verify_FunctionUpdateAsyncEnumerable_HasBeenCalled(string newCommentBody)
    {
        //Arrange
        var random = new Random();
        var commentId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newComments = SetupCommentFixture()
            .CreateMany(itemsCount)
            .ToList();

        _commentsRepositoryMock.Setup(x => x.InsertAsync(newComments))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newComments[i].Id = commentId + i;
                }
            });

        //Act
        await _commentsService.InsertAsync(newComments);
        for (var i = 0; i < itemsCount; i++)
        {
            newComments[i].CommentBody = $"{newCommentBody} {i}";
        }
        await _commentsService.UpdateAsync(newComments);

        //Assert
        _commentsRepositoryMock.Verify(x => x.UpdateAsync(newComments), Times.Once);
    }

    /// <summary>
    /// Update Async Enumerable comment.
    /// Should return comment when comment updated.
    /// </summary>
    /// <param name="newCommentBody">The new CommentBody.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("New CommentBody")]
    public async Task UpdateAsyncEnumerable_WhenCommentExists_ShouldReturnComment(string newCommentBody)
    {
        //Arrange
        var random = new Random();
        var commentId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newComments = SetupCommentFixture()
            .CreateMany(itemsCount)
            .ToList();

        _commentsRepositoryMock.Setup(x => x.InsertAsync(newComments))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newComments[i].Id = commentId + i;
                }
            });

        //Act
        await _commentsService.InsertAsync(newComments);
        for (var i = 0; i < itemsCount; i++)
        {
            newComments[i].CommentBody = $"{newCommentBody} {i}";
        }
        await _commentsService.UpdateAsync(newComments);

        //Assert

        for (var i = 0; i < itemsCount; i++)
        {
            Assert.Equal($"{newCommentBody} {i}", newComments[i].CommentBody);
        }
    }

    #endregion

    #endregion

    #region Delete service tests

    #region Delete By Id function

    /// <summary>
    /// Verify that function Delete By Id has been called.
    /// </summary>
    [Fact]
    public void Verify_FunctionDeleteById_HasBeenCalled()
    {
        //Arrange
        var newComment = SetupCommentFixture().Create();
        var commentId = newComment.Id;
        _commentsRepositoryMock.Setup(x => x.GetById(commentId))
            .Returns(() => newComment);

        //Act
        _commentsService.Insert(newComment);
        var comment = _commentsService.Find(commentId);
        _commentsService.Delete(commentId);
        _commentsRepositoryMock.Setup(x => x.GetById(commentId))
            .Returns(() => null);
        _commentsService.Find(commentId);

        //Assert
        _commentsRepositoryMock.Verify(x => x.Delete(newComment), Times.Once);
    }

    /// <summary>
    /// Delete By Id comment.
    /// Should return nothing when comment is deleted.
    /// </summary>
    [Fact]
    public void DeleteById_WhenCommentIsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var newComment = SetupCommentFixture().Create();
        var commentId = newComment.Id;

        _commentsRepositoryMock.Setup(x => x.Insert(newComment))
            .Callback(() =>
            {
                newComment.Id = commentId;
            });
        _commentsRepositoryMock.Setup(x => x.GetById(commentId))
            .Returns(() => newComment);

        //Act
        _commentsService.Insert(newComment);
        var comment = _commentsService.Find(commentId);
        _commentsService.Delete(commentId);
        _commentsRepositoryMock.Setup(x => x.GetById(commentId))
            .Returns(() => null);
        var deletedComment = _commentsService.Find(commentId);

        //Assert
        Assert.Null(deletedComment);
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
        var newComment = SetupCommentFixture().Create();
        var commentId = newComment.Id;

        _commentsRepositoryMock.Setup(x => x.GetById(commentId))
            .Returns(() => newComment);

        //Act
        _commentsService.Insert(newComment);
        var comment = _commentsService.Find(commentId);
        _commentsService.Delete(comment);
        _commentsRepositoryMock.Setup(x => x.GetById(commentId))
            .Returns(() => null);
        _commentsService.Find(commentId);

        //Assert
        _commentsRepositoryMock.Verify(x => x.Delete(comment), Times.Once);
    }

    /// <summary>
    /// Delete By Object comment.
    /// Should return nothing when comment is deleted.
    /// </summary>
    [Fact]
    public void DeleteByObject_WhenCommentIsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var newComment = SetupCommentFixture().Create();
        var commentId = newComment.Id;

        _commentsRepositoryMock.Setup(x => x.Insert(newComment))
            .Callback(() =>
            {
                newComment.Id = commentId;
            });
        _commentsRepositoryMock.Setup(x => x.GetById(commentId))
            .Returns(() => newComment);

        //Act
        _commentsService.Insert(newComment);
        var comment = _commentsService.Find(commentId);
        _commentsService.Delete(comment);
        _commentsRepositoryMock.Setup(x => x.GetById(commentId))
            .Returns(() => null);
        var deletedComment = _commentsService.Find(commentId);

        //Assert
        Assert.Null(deletedComment);
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
        var commentId = random.Next(52);
        var itemsCount = random.Next(10);
        var newComments = SetupCommentFixture()
            .CreateMany(itemsCount)
            .ToList();

        _commentsRepositoryMock.Setup(x => x.Insert(newComments))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newComments[i].Id = commentId + i;
                }
            });

        //Act
        _commentsService.Insert(newComments);
        _commentsService.Delete(newComments);

        //Assert
        _commentsRepositoryMock.Verify(x => x.Delete(newComments), Times.Once);
    }

    /// <summary>
    /// Delete By Enumerable comment.
    /// Should return nothing when comment is deleted.
    /// </summary>
    [Fact]
    public void DeleteByEnumerable_WhenCommentIsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var random = new Random();
        var commentId = random.Next(52);
        var itemsCount = random.Next(10);
        var newComments = SetupCommentFixture()
            .CreateMany(itemsCount)
            .ToList();

        _commentsRepositoryMock.Setup(x => x.Insert(newComments))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newComments[i].Id = commentId + i;
                }
            });
        _commentsRepositoryMock.Setup(x => x.Delete(newComments))
            .Callback(() =>
            {
                newComments = null;
            });

        //Act
        _commentsService.Insert(newComments);
        _commentsService.Delete(newComments);

        //Assert
        Assert.Null(newComments);
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
        var newComment = SetupCommentFixture().Create();
        var commentId = newComment.Id;
        _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
            .ReturnsAsync(() => newComment);

        //Act
        await _commentsService.InsertAsync(newComment);
        var comment = await _commentsService.FindAsync(commentId);
        await _commentsService.DeleteAsync(commentId);

        //Assert
        _commentsRepositoryMock.Verify(x => x.DeleteAsync(comment), Times.Once);
    }

    /// <summary>
    /// Async delete by id comment.
    /// Should return nothing when comment is deleted.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task DeleteAsyncById_WhenCommentIsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var newComment = SetupCommentFixture().Create();
        var commentId = newComment.Id;

        _commentsRepositoryMock.Setup(x => x.InsertAsync(newComment))
            .Callback(() =>
            {
                newComment.Id = commentId;
            });
        _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
            .ReturnsAsync(() => newComment);

        //Act
        await _commentsService.InsertAsync(newComment);
        var comment = await _commentsService.FindAsync(commentId);
        await _commentsService.DeleteAsync(comment.Id);
        _commentsRepositoryMock.Setup(x => x.GetByIdAsync(comment.Id))
            .ReturnsAsync(() => null);
        var deletedComment = await _commentsService.FindAsync(comment.Id);

        //Assert
        Assert.Null(deletedComment);
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
        var newComment = SetupCommentFixture().Create();
        var commentId = newComment.Id;
        _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
            .ReturnsAsync(() => newComment);

        //Act
        await _commentsService.InsertAsync(newComment);
        var comment = await _commentsService.FindAsync(commentId);
        await _commentsService.DeleteAsync(comment);

        //Assert
        _commentsRepositoryMock.Verify(x => x.DeleteAsync(comment), Times.Once);
    }

    /// <summary>
    /// Async delete by object comment.
    /// Should return nothing when comment is deleted.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task DeleteAsyncByObject_WhenCommentIsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var newComment = SetupCommentFixture().Create();
        var commentId = newComment.Id;

        _commentsRepositoryMock.Setup(x => x.InsertAsync(newComment))
            .Callback(() =>
            {
                newComment.Id = commentId;
            });
        _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
            .ReturnsAsync(() => newComment);

        //Act
        await _commentsService.InsertAsync(newComment);
        var comment = await _commentsService.FindAsync(commentId);
        await _commentsService.DeleteAsync(comment);
        _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
            .ReturnsAsync(() => null);
        var deletedComment = await _commentsService.FindAsync(commentId);

        //Assert
        Assert.Null(deletedComment);
    }

    #endregion

    #region Delete Async By Enumerable function

    /// <summary>
    /// Verify that function Delete Async By Enumerable has been called.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task Verify_FunctionDeleteAsyncByEnumerable_HasBeenCalled()
    {
        //Arrange
        var random = new Random();
        var commentId = random.Next(52);
        var itemsCount = random.Next(10);
        var newComments = SetupCommentFixture()
            .CreateMany(random.Next(100))
            .ToList();

        _commentsRepositoryMock.Setup(x => x.InsertAsync(newComments))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newComments[i].Id = commentId + i;
                }
            });

        //Act
        await _commentsService.InsertAsync(newComments);
        await _commentsService.DeleteAsync(newComments);

        //Assert
        _commentsRepositoryMock.Verify(x => x.DeleteAsync(newComments), Times.Once);
    }

    /// <summary>
    /// Delete Async By Enumerable comment.
    /// Should return nothing when comment is deleted.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task DeleteAsyncByEnumerable_WhenCommentIsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var random = new Random();
        var commentId = random.Next(52);
        var itemsCount = random.Next(10);
        var newComments = SetupCommentFixture()
            .CreateMany(random.Next(100))
            .ToList();

        _commentsRepositoryMock.Setup(x => x.InsertAsync(newComments))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newComments[i].Id = commentId + i;
                }
            });
        _commentsRepositoryMock.Setup(x => x.DeleteAsync(newComments))
            .Callback(() =>
            {
                newComments = null;
            });

        //Act
        await _commentsService.InsertAsync(newComments);
        await _commentsService.DeleteAsync(newComments);

        //Assert
        Assert.Null(newComments);
    }

    #endregion

    #endregion

    #region Any service tests

    #region Any function With Specification

    /// <summary>
    /// Verify that function Any with specification has been called.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment ")]
    public void Verify_FunctionAny_WithSpecification_HasBeenCalled(string commentBodySearch)
    {
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .With(x => x.CommentBody, commentBodySearch)
            .CreateMany(random.Next(100))
            .ToList();

        var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => commentsList.Any(x => x.CommentBody.Contains(commentBodySearch)));

        //Act
        _commentsService.Any(specification);

        //Assert
        _commentsRepositoryMock.Verify(x => x.Any(specification), Times.Once);
    }

    /// <summary>
    /// Check if there are any comments with specification.
    /// Should return true with contains specification when comments exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment ")]
    public void Any_WithContainsSpecification_WhenCommentsExists_ShouldReturnTrue(string commentBodySearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .With(x => x.CommentBody, commentBodySearch)
            .CreateMany(random.Next(100))
            .ToList();

        var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => commentsList.Any(x => x.CommentBody.Contains(commentBodySearch)));

        //Act
        var areAnyComments = _commentsService.Any(specification);

        //Assert
        Assert.True(areAnyComments);
    }

    /// <summary>
    /// Check if there are any comments with specification.
    /// Should return comment with equal specification when comments exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment 0")]
    public void Any_WithEqualsSpecification_WhenCommentsExists_ShouldReturnTrue(string commentBodySearch)
    {
        //Arrange
        var commentsList = SetupCommentFixture()
            .With(x => x.CommentBody, commentBodySearch)
            .CreateMany(1)
            .ToList();

        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => commentsList.Any(x => x.CommentBody.Contains(commentBodySearch)));

        //Act
        var areAnyComments = _commentsService.Any(specification);

        //Assert
        Assert.True(areAnyComments);
    }

    /// <summary>
    /// Check if there are any comments with specification.
    /// Should return false with when comments does not exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment -1")]
    public void Any_WithEqualSpecification_WhenCommentsExists_ShouldReturnFalse(string commentBodySearch)
    {
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .CreateMany(random.Next(100))
            .ToList();

        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => commentsList.Any(x => x.CommentBody.Contains(commentBodySearch)));

        //Act
        var areAnyComments = _commentsService.Any(specification);

        //Assert
        Assert.False(areAnyComments);
    }

    /// <summary>
    /// Check if there are any comments with specification.
    /// Should return false with when comments does not exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment 0")]
    public void Any_WithEqualSpecification_WhenCommentDoesNotExists_ShouldReturnNothing(string commentBodySearch)
    {
        //Arrange
        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => false);

        //Act
        var areAnyComments = _commentsService.Any(specification);

        //Assert
        Assert.False(areAnyComments);
    }

    #endregion

    #region Any Async function With Specification

    /// <summary>
    /// Verify that function Any Async with specification has been called.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Comment ")]
    public async Task Verify_FunctionAnyAsync_WithSpecification_HasBeenCalled(string commentBodySearch)
    {
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .With(x => x.CommentBody, commentBodySearch)
            .CreateMany(random.Next(100))
            .ToList();

        var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => commentsList.Any(x => x.CommentBody.Contains(commentBodySearch)));

        //Act
        await _commentsService.AnyAsync(specification);

        //Assert
        _commentsRepositoryMock.Verify(x => x.AnyAsync(specification), Times.Once);
    }

    /// <summary>
    /// Async check if there are any comments with specification.
    /// Should return true with contains specification when comments exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Comment ")]
    public async Task AnyAsync_WithContainsSpecification_WhenCommentsExists_ShouldReturnTrue(string commentBodySearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .With(x => x.CommentBody, commentBodySearch)
            .CreateMany(random.Next(100))
            .ToList();

        var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => commentsList.Any(x => x.CommentBody.Contains(commentBodySearch)));

        //Act
        var areAnyComments = await _commentsService.AnyAsync(specification);

        //Assert
        Assert.True(areAnyComments);
    }

    /// <summary>
    /// Async check if there are any comments with specification.
    /// Should return comment with equal specification when comments exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Comment 0")]
    public async Task AnyAsync_WithEqualsSpecification_WhenCommentsExists_ShouldReturnTrue(string commentBodySearch)
    {
        //Arrange
        var commentsList = SetupCommentFixture()
            .With(x => x.CommentBody, commentBodySearch)
            .With(x => x.CommentBody, commentBodySearch)
            .CreateMany(1)
            .ToList();

        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => commentsList.Any(x => x.CommentBody.Contains(commentBodySearch)));

        //Act
        var areAnyComments = await _commentsService.AnyAsync(specification);

        //Assert
        Assert.True(areAnyComments);
    }

    /// <summary>
    /// Async check if there are any comments with specification.
    /// Should return false with when comments does not exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Comment -1")]
    public async Task AnyAsync_WithEqualSpecification_WhenCommentsExists_ShouldReturnFalse(string commentBodySearch)
    {
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .CreateMany(random.Next(100))
            .ToList();

        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => commentsList.Any(x => x.CommentBody.Contains(commentBodySearch)));

        //Act
        var areAnyComments = await _commentsService.AnyAsync(specification);

        //Assert
        Assert.False(areAnyComments);
    }

    /// <summary>
    /// Async check if there are any comments with specification.
    /// Should return false with when comments does not exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Comment 0")]
    public async Task AnyAsync_WithEqualSpecification_WhenCommentDoesNotExists_ShouldReturnNothing(string commentBodySearch)
    {
        //Arrange
        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => false);

        //Act
        var areAnyComments = await _commentsService.AnyAsync(specification);

        //Assert
        Assert.False(areAnyComments);
    }

    #endregion

    #endregion

    #region First Or Default function With Specification

    /// <summary>
    /// Verify that function First Or Default with specification has been called.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment ")]
    public void Verify_FunctionFirstOrDefault_WithSpecification_HasBeenCalled(string commentBodySearch)
    {
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .With(x => x.CommentBody, commentBodySearch)
            .CreateMany(random.Next(100))
            .ToList();

        var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => commentsList.FirstOrDefault(x => x.CommentBody.Contains(commentBodySearch)));

        //Act
        _commentsService.FirstOrDefault(specification);

        //Assert
        _commentsRepositoryMock.Verify(x => x.FirstOrDefault(specification), Times.Once);
    }

    /// <summary>
    /// Get first or default comment with specification.
    /// Should return comment with contains specification when comments exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment ")]
    public void FirstOrDefault_WithContainsSpecification_WhenCommentsExists_ShouldReturnTrue(string commentBodySearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .With(x => x.CommentBody, commentBodySearch)
            .CreateMany(random.Next(100))
            .ToList();

        var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => commentsList.FirstOrDefault(x => x.CommentBody.Contains(commentBodySearch)));

        //Act
        var comment = _commentsService.FirstOrDefault(specification);

        //Assert
        Assert.NotNull(comment);
        Assert.IsType<Comment>(comment);
    }

    /// <summary>
    /// Get first or default comment with specification.
    /// Should return comment with equal specification when comments exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment 0")]
    public void FirstOrDefault_WithEqualsSpecification_WhenCommentsExists_ShouldReturnTrue(string commentBodySearch)
    {
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .With(x => x.CommentBody, commentBodySearch)
            .CreateMany(random.Next(100))
            .ToList();

        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => commentsList.FirstOrDefault(x => x.CommentBody.Contains(commentBodySearch)));

        //Act
        var comment = _commentsService.FirstOrDefault(specification);

        //Assert
        Assert.NotNull(comment);
        Assert.IsType<Comment>(comment);
    }

    /// <summary>
    /// Get first or default comment with specification.
    /// Should return nothing with when comment does not exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment -1")]
    public void FirstOrDefault_WithEqualSpecification_WhenCommentsExists_ShouldReturnNothing(string commentBodySearch)
    {
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .CreateMany(random.Next(100))
            .ToList();

        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => commentsList.FirstOrDefault(x => x.CommentBody.Contains(commentBodySearch)));

        //Act
        var comment = _commentsService.FirstOrDefault(specification);

        //Assert
        Assert.Null(comment);
    }

    /// <summary>
    /// Get first or default comment with specification.
    /// Should return nothing with when comments does not exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment 0")]
    public void FirstOrDefault_WithEqualSpecification_WhenCommentDoesNotExists_ShouldReturnNothing(string commentBodySearch)
    {
        //Arrange
        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => null);

        //Act
        var comment = _commentsService.FirstOrDefault(specification);

        //Assert
        Assert.Null(comment);
    }

    #endregion

    #region Last Or Default function With Specification

    /// <summary>
    /// Verify that function Last Or Default with specification has been called.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment ")]
    public void Verify_FunctionLastOrDefault_WithSpecification_HasBeenCalled(string commentBodySearch)
    {
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .With(x => x.CommentBody, commentBodySearch)
            .CreateMany(random.Next(100))
            .ToList();

        var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => commentsList.LastOrDefault(x => x.CommentBody.Contains(commentBodySearch)));

        //Act
        _commentsService.LastOrDefault(specification);

        //Assert
        _commentsRepositoryMock.Verify(x => x.LastOrDefault(specification), Times.Once);
    }

    /// <summary>
    /// Get last or default comment with specification.
    /// Should return comment with contains specification when comments exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment ")]
    public void LastOrDefault_WithContainsSpecification_WhenCommentsExists_ShouldReturnTrue(string commentBodySearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .With(x => x.CommentBody, commentBodySearch)
            .CreateMany(random.Next(100))
            .ToList();

        var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => commentsList.LastOrDefault(x => x.CommentBody.Contains(commentBodySearch)));

        //Act
        var comment = _commentsService.LastOrDefault(specification);

        //Assert
        Assert.NotNull(comment);
        Assert.IsType<Comment>(comment);
    }

    /// <summary>
    /// Get last or default comment with specification.
    /// Should return comment with equal specification when comments exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment 0")]
    public void LastOrDefault_WithEqualsSpecification_WhenCommentsExists_ShouldReturnTrue(string commentBodySearch)
    {
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .With(x => x.CommentBody, commentBodySearch)
            .CreateMany(random.Next(100))
            .ToList();

        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => commentsList.LastOrDefault(x => x.CommentBody.Contains(commentBodySearch)));

        //Act
        var comment = _commentsService.LastOrDefault(specification);

        //Assert
        Assert.NotNull(comment);
        Assert.IsType<Comment>(comment);
    }

    /// <summary>
    /// Get last or default comment with specification.
    /// Should return nothing with when comment does not exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment -1")]
    public void LastOrDefault_WithEqualSpecification_WhenCommentsExists_ShouldReturnNothing(string commentBodySearch)
    {
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .CreateMany(random.Next(100))
            .ToList();

        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => commentsList.LastOrDefault(x => x.CommentBody.Contains(commentBodySearch)));

        //Act
        var comment = _commentsService.LastOrDefault(specification);

        //Assert
        Assert.Null(comment);
    }

    /// <summary>
    /// Get last or default comment with specification.
    /// Should return nothing with when comments does not exists.
    /// </summary>
    /// <param name="commentBodySearch">The CommentBody search.</param>
    [Theory]
    [InlineData("Comment 0")]
    public void LastOrDefault_WithEqualSpecification_WhenCommentDoesNotExists_ShouldReturnNothing(string commentBodySearch)
    {
        //Arrange
        var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
        _commentsRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => null);

        //Act
        var comment = _commentsService.LastOrDefault(specification);

        //Assert
        Assert.Null(comment);
    }

    #endregion

    #region Search async function

    /// <summary>
    /// Search the specified query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="commentsList">The comments list.</param>
    /// <returns>PagedListResult.</returns>
    protected static PagedListResult<Comment> Search(SearchQuery<Comment> query, List<Comment> commentsList)
    {
        var sequence = commentsList.AsQueryable();

        // Applying filters
        if (query.Filters is { Count: > 0 })
        {
            sequence = query.Filters
                .Aggregate(sequence, (current, filterClause) => current.Where(filterClause));
        }

        // Include Properties
        if (!string.IsNullOrWhiteSpace(query.IncludeProperties))
        {
            var properties = query.IncludeProperties.Split([","], StringSplitOptions.RemoveEmptyEntries);

            sequence = properties.Aggregate(sequence, (current, includeProperty) => current.Include(includeProperty));
        }

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
            sequence = ((IOrderedQueryable<Comment>)sequence).OrderBy(x => true);
        }

        // Counting the total number of object.
        var resultCount = sequence.Count();

        var result = (query.Take > 0)
            ? sequence.Skip(query.Skip).Take(query.Take).ToList()
            : sequence.ToList();

        // Debug info of what the query looks like
        // Console.WriteLine(sequence.ToString());

        // Setting up the return object.
        var hasNext = (query.Skip > 0 || query.Take > 0) && (query.Skip + query.Take < resultCount);

        return new PagedListResult<Comment>
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
    [InlineData("Comment ", 0, 10, "CommentBody", OrderType.Ascending)]
    [InlineData("Comment ", 10, 10, "CommentBody", OrderType.Ascending)]
    [InlineData("Comment ", 10, 20, "CommentBody", OrderType.Ascending)]
    [InlineData("Comment ", 0, 100, "CommentBody", OrderType.Ascending)]
    public async Task Verify_FunctionSearchAsync_HasBeenCalled(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .With(x => x.CommentBody, search)
            .CreateMany(random.Next(100))
            .ToList();

        var query = new SearchQuery<Comment>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Comment>(fieldName, orderType));

        query.AddFilter(x => x.CommentBody.ToUpper().Contains($"{search}".ToUpper()));

        _commentsRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => Search(query, commentsList));

        //Act
        await _commentsService.SearchAsync(query);

        //Assert
        _commentsRepositoryMock.Verify(x => x.SearchAsync(query), Times.Once);
    }

    /// <summary>
    /// Search async comments.
    /// Should return comments when comments exists.
    /// </summary>
    /// <param name="search">The search.</param>
    /// <param name="start">The start.</param>
    /// <param name="take">The take.</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="orderType">The order type.</param>
    [Theory]
    [InlineData("Comment ", 0, 10, "CommentBody", OrderType.Ascending)]
    [InlineData("Comment ", 10, 10, "CommentBody", OrderType.Ascending)]
    [InlineData("Comment ", 10, 20, "CommentBody", OrderType.Ascending)]
    [InlineData("Comment ", 0, 100, "CommentBody", OrderType.Ascending)]
    public async Task SearchAsync_WhenCommentsExists_ShouldReturnComments(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .With(x => x.CommentBody, search)
            .CreateMany(random.Next(100))
            .ToList();

        var query = new SearchQuery<Comment>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Comment>(fieldName, orderType));

        query.AddFilter(x => x.CommentBody.ToUpper().Contains($"{search}".ToUpper()));

        _commentsRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => Search(query, commentsList));

        //Act
        var comments = await _commentsService.SearchAsync(query);

        //Assert
        Assert.NotNull(comments);
        Assert.NotEmpty(comments.Entities);
    }

    /// <summary>
    /// Search async comments with specification.
    /// Should return comment with equal specification when comments exists.
    /// </summary>
    /// <param name="search">The search.</param>
    /// <param name="start">The start.</param>
    /// <param name="take">The take.</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="orderType">The order type.</param>
    [Theory]
    [InlineData("Search", 0, 10, "CommentBody", OrderType.Ascending)]
    [InlineData("Search", 10, 10, "CommentBody", OrderType.Ascending)]
    [InlineData("Search", 10, 20, "CommentBody", OrderType.Ascending)]
    [InlineData("Search", 0, 100, "CommentBody", OrderType.Ascending)]
    public async Task SearchAsync_WithEqualsSpecification_WhenCommentsExists_ShouldReturnComment(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var commentsList = SetupCommentFixture()
            .With(x => x.CommentBody, search)
            .CreateMany(start + 1)
            .ToList();

        var query = new SearchQuery<Comment>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Comment>(fieldName, orderType));

        query.AddFilter(x => x.CommentBody.ToUpper().Contains($"{search}".ToUpper()));

        _commentsRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => Search(query, commentsList));

        //Act
        var comments = await _commentsService.SearchAsync(query);

        //Assert
        Assert.NotNull(comments);
        Assert.NotEmpty(comments.Entities);
        Assert.Single(comments.Entities);
    }

    /// <summary>
    /// Search async comments with specification.
    /// Should return nothing with  when comments does not exists.
    /// </summary>
    /// <param name="search">The search.</param>
    /// <param name="start">The start.</param>
    /// <param name="take">The take.</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="orderType">The order type.</param>
    [Theory]
    [InlineData("Comment -0", 0, 10, "CommentBody", OrderType.Ascending)]
    [InlineData("Comment -11", 10, 10, "CommentBody", OrderType.Ascending)]
    [InlineData("Comment -11", 10, 20, "CommentBody", OrderType.Ascending)]
    [InlineData("Comment -11", 0, 100, "CommentBody", OrderType.Ascending)]
    public async Task SearchAsync_WithEqualSpecification_WhenCommentsExists_ShouldReturnNothing(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var random = new Random();
        var commentsList = SetupCommentFixture()
            .CreateMany(random.Next(100))
            .ToList();

        var query = new SearchQuery<Comment>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Comment>(fieldName, orderType));

        query.AddFilter(x => x.CommentBody.ToUpper().Equals($"{search}".ToUpper()));

        _commentsRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => Search(query, commentsList));

        //Act
        var comments = await _commentsService.SearchAsync(query);

        //Assert
        Assert.NotNull(comments);
        Assert.Empty(comments.Entities);
    }

    /// <summary>
    /// Search async comments.
    /// Should return nothing when comments does not exists.
    /// </summary>
    /// <param name="search">The search.</param>
    /// <param name="start">The start.</param>
    /// <param name="take">The take.</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="orderType">The order type.</param>
    [Theory]
    [InlineData("Comment 0", 0, 10, "CommentBody", OrderType.Ascending)]
    [InlineData("Comment 11", 10, 10, "CommentBody", OrderType.Ascending)]
    [InlineData("Comment 11", 10, 20, "CommentBody", OrderType.Ascending)]
    [InlineData("Comment 11", 0, 100, "CommentBody", OrderType.Ascending)]
    public async Task SearchAsync_WhenCommentsDoesNotExists_ShouldReturnNothing(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var query = new SearchQuery<Comment>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Comment>(fieldName, orderType));

        query.AddFilter(x => x.CommentBody.ToUpper().Contains($"{search}".ToUpper()));

        _commentsRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => new PagedListResult<Comment>());

        //Act
        var comments = await _commentsService.SearchAsync(query);

        //Assert
        Assert.Null(comments.Entities);
    }

    #endregion

    #region SearchBySequenceAsync function

    /// <summary>
    /// Search by sequence async.
    /// When no entities exist should return empty result.
    /// </summary>
    [Fact]
    public async Task SearchBySequenceAsync_WhenNoEntitiesExist_ShouldReturnEmpty()
    {
        var data = new List<Comment>().AsQueryable();
        var query = new SearchQuery<Comment> { Skip = 0, Take = 5 };
        var expected = new PagedListResult<Comment> { Entities = new List<Comment>(), Count = 0 };

        _commentsRepositoryMock.Setup(r => r.SearchBySquenceAsync(query, data)).ReturnsAsync(expected);

        var result = await _commentsService.SearchBySequenceAsync(query, data);

        Assert.NotNull(result);
        Assert.Empty(result.Entities);
    }

    /// <summary>
    /// Search by sequence async.
    /// When entities exist and query is null should return paged list.
    /// </summary>
    [Fact]
    public async Task SearchBySequenceAsync_WhenEntitiesExistAndQueryIsNull_ShouldReturnPagedList()
    {
        var data = SetupCommentFixture().CreateMany(5).AsQueryable();
        var expected = new PagedListResult<Comment> { Entities = data.ToList(), Count = 5 };

        _commentsRepositoryMock.Setup(r => r.SearchBySquenceAsync(null, data)).ReturnsAsync(expected);

        var result = await _commentsService.SearchBySequenceAsync(null, data);

        Assert.NotNull(result);
        Assert.Equal(5, result.Entities.Count());
    }

    /// <summary>
    /// Search by sequence async.
    /// When entities exist and sequence is null should return paged list.
    /// </summary>
    [Fact]
    public async Task SearchBySequenceAsync_WhenEntitiesExistAndSequenceIsNullIsNull_ShouldReturnPagedList()
    {
        var query = new SearchQuery<Comment> { Skip = 0, Take = 5 };
        var expected = new PagedListResult<Comment> { Entities = null, Count = 5 };

        _commentsRepositoryMock.Setup(r => r.SearchBySquenceAsync(query, null)).ReturnsAsync(expected);

        var result = await _commentsService.SearchBySequenceAsync(query, null);

        Assert.NotNull(result);
        Assert.Null(result.Entities);
    }

    #endregion

    #region GenerateQuery

    /// <summary>
    /// GenerateQuery.
    /// When called should return search query.
    /// </summary>
    [Fact]
    public void GenerateQuery_WhenCalled_ShouldReturnSearchQuery()
    {
        var tableFilter = new TableFilter();
        var expected = new SearchQuery<Comment>();

        _commentsRepositoryMock.Setup(r => r.GenerateQuery(tableFilter, null)).Returns(expected);

        var result = _commentsService.GenerateQuery(tableFilter);

        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// GenerateQuery.
    /// When include properties provided should return search query.
    /// </summary>
    [Fact]
    public void GenerateQuery_WhenIncludePropertiesProvided_ShouldReturnSearchQuery()
    {
        var tableFilter = new TableFilter();
        var expected = new SearchQuery<Comment>();

        _commentsRepositoryMock.Setup(r => r.GenerateQuery(tableFilter, "CommentBody")).Returns(expected);

        var result = _commentsService.GenerateQuery(tableFilter, "CommentBody");

        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// GenerateQuery.
    /// When called and table filter is null should return null result.
    /// </summary>
    [Fact]
    public void GenerateQuery_WhenCalledAndTableFilterIsNull_ShouldReturnNullResult()
    {
        var tableFilter = new TableFilter();
        var expected = new SearchQuery<Comment>();

        _commentsRepositoryMock.Setup(r => r.GenerateQuery(tableFilter, null)).Returns(expected);

        var result = _commentsService.GenerateQuery(null);

        Assert.Null(result);
    }

    #endregion

    #endregion
}