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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Blog.ServicesTests.EntityServices;

/// <summary>
/// Messages service tests.
/// </summary>
public class MessagesServiceTests
{
    #region Fields

    /// <summary>
    /// The messages service.
    /// </summary>
    private readonly IMessagesService _messagesService;

    /// <summary>
    /// The messages repository mock.
    /// </summary>
    private readonly Mock<IRepository<Message>> _messagesRepositoryMock;

    /// <summary>
    /// The fixture.
    /// </summary>
    private readonly Fixture _fixture;

    #endregion

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="MessagesServiceTests"/> class.
    /// </summary>
    public MessagesServiceTests()
    {
        _messagesRepositoryMock = new Mock<IRepository<Message>>();
        _messagesService = new MessagesService(_messagesRepositoryMock.Object);
        _fixture = new Fixture();
    }

    #endregion

    #region Uthilities

    private IPostprocessComposer<Message> SetupMessageFixture(string messageBody = null)
    {
        var recipient =
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

        var sender =
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

        var messageSetup =
            _fixture.Build<Message>()
                .With(x => x.Recipient, recipient)
                .With(x => x.Sender, sender);
        if (!string.IsNullOrWhiteSpace(messageBody))
        {
            messageSetup.With(x => x.Body, messageBody);
        }

        return messageSetup;
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
        var messagesList =
            SetupMessageFixture()
                .CreateMany(random.Next(100));

        _messagesRepositoryMock.Setup(x => x.GetAll())
            .Returns(messagesList.AsQueryable());

        //Act
        _messagesService.GetAll();

        //Assert
        _messagesRepositoryMock.Verify(x => x.GetAll(), Times.Once);
    }

    /// <summary>
    /// Get all messages.
    /// Should return messages when messages exists.
    /// </summary>
    /// <param name="notEqualCount">The not equal count.</param>
    [Theory]
    [InlineData(0)]
    public void GetAll_WhenMessagesExists_ShouldReturnMessages(int notEqualCount)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .CreateMany(random.Next(100));

        _messagesRepositoryMock.Setup(x => x.GetAll())
            .Returns(messagesList.AsQueryable());

        //Act
        var messages = _messagesService.GetAll();

        //Assert
        Assert.NotNull(messages);
        Assert.NotEmpty(messages);
        Assert.NotEqual(notEqualCount, messages.ToList().Count);
    }

    /// <summary>
    /// Get all messages.
    /// Should return nothing when messages does not exists.
    /// </summary>
    [Fact]
    public void GetAll_WhenMessagesDoesNotExists_ShouldReturnNothing()
    {
        //Arrange
        _messagesRepositoryMock.Setup(x => x.GetAll())
            .Returns(() => new List<Message>().AsQueryable());

        //Act
        var messages = _messagesService.GetAll();

        //Assert
        Assert.Empty(messages);
    }

    /// <summary>
    /// Get all messages.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public void GetAll_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        _messagesRepositoryMock.Setup(x => x.GetAll())
            .Throws(() => new Exception("Test exception"));

        //Assert
        Assert.Throws<Exception>(() => _messagesService.GetAll());
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
        var messagesList =
            SetupMessageFixture()
                .CreateMany(random.Next(100))
                .ToList();

        _messagesRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(messagesList);

        //Act
        await _messagesService.GetAllAsync();

        //Assert
        _messagesRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    /// <summary>
    /// Get all async messages.
    /// Should return messages when messages exists.
    /// </summary>
    /// <param name="notEqualCount">The not equal count.</param>
    [Theory]
    [InlineData(0)]
    public async Task GetAllAsync_WhenMessagesExists_ShouldReturnMessages(int notEqualCount)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .CreateMany(random.Next(100))
                .ToList();

        _messagesRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(() => messagesList);

        //Act
        var messages = await _messagesService.GetAllAsync();

        //Assert
        Assert.NotNull(messages);
        Assert.NotEmpty(messages);
        Assert.NotEqual(notEqualCount, messages.ToList().Count);
    }

    /// <summary>
    /// Get all async messages.
    /// Should return nothing when messages does not exists.
    /// </summary>
    [Fact]
    public void GetAllAsync_WhenMessagesDoesNotExists_ShouldReturnNothing()
    {
        //Arrange
        _messagesRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(() => []);

        //Act
        var messages = _messagesService.GetAll();

        //Assert
        Assert.Empty(messages);
    }

    /// <summary>
    /// Get all async messages.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_WhenRepositoryExceptionIsThrown_ShouldThrowException()
    {
        //Arrange
        _messagesRepositoryMock.Setup(x => x.GetAllAsync())
            .ThrowsAsync(new Exception("Test exception"));

        //Assert
        await Assert.ThrowsAsync<Exception>(() => _messagesService.GetAllAsync());
    }

    #endregion

    #region Get all function With Specification

    /// <summary>
    /// Verify that function Get All with specification has been called.
    /// </summary>
    /// <param name="bodySearch">The body search.</param>
    [Theory]
    [InlineData("Test body")]
    public void Verify_FunctionGetAll_WithSpecification_HasBeenCalled(string bodySearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Body, bodySearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Body.Contains(bodySearch));
        _messagesRepositoryMock.Setup(x => x.GetAll(specification))
            .Returns(() => messagesList.Where(x => x.Body.Contains(bodySearch)).AsQueryable());

        //Act
        _messagesService.GetAll(specification);

        //Assert
        _messagesRepositoryMock.Verify(x => x.GetAll(specification), Times.Once);
    }

    /// <summary>
    /// Get all messages with specification.
    /// Should return messages with contains specification when messages exists.
    /// </summary>
    /// <param name="notEqualCount">The not equal count.</param>
    /// <param name="bodySearch">The body search.</param>
    [Theory]
    [InlineData(0, "Test body")]
    public void GetAll_WithContainsSpecification_WhenMessagesExists_ShouldReturnMessages(int notEqualCount, string bodySearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Body, bodySearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Body.Contains(bodySearch));
        _messagesRepositoryMock.Setup(x => x.GetAll(specification))
            .Returns(() => messagesList.Where(x => x.Body.Contains(bodySearch)).AsQueryable());

        //Act
        var messages = _messagesService.GetAll(specification);

        //Assert
        Assert.NotNull(messages);
        Assert.NotEmpty(messages);
        Assert.NotEqual(notEqualCount, messages.ToList().Count);
    }

    /// <summary>
    /// Get all messages with specification.
    /// Should return message with equal specification when messages exists.
    /// </summary>
    /// <param name="equalCount">The equal count.</param>
    /// <param name="bodySearch">The body search.</param>
    [Theory]
    [InlineData(1, "Test body0")]
    public void GetAll_WithEqualsSpecification_WhenMessagesExists_ShouldReturnMessage(int equalCount, string bodySearch)
    {
        //Arrange
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Body, bodySearch)
                .CreateMany(1);

        var specification = new MessageSpecification(x => x.Body.Contains(bodySearch));
        _messagesRepositoryMock.Setup(x => x.GetAll(specification))
            .Returns(() => messagesList.Where(x => x.Body.Contains(bodySearch)).AsQueryable());

        //Act
        var messages = _messagesService.GetAll(specification);

        //Assert
        Assert.NotNull(messages);
        Assert.NotEmpty(messages);
        Assert.Equal(equalCount, messages.ToList().Count);
    }

    /// <summary>
    /// Get all messages with specification.
    /// Should return nothing with  when messages does not exists.
    /// </summary>
    /// <param name="equalCount">The equal count.</param>
    /// <param name="bodySearch">The body search.</param>
    [Theory]
    [InlineData(0, "Test body-1")]
    public void GetAll_WithEqualSpecification_WhenMessagesExists_ShouldReturnNothing(int equalCount, string bodySearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Body.Contains(bodySearch));
        _messagesRepositoryMock.Setup(x => x.GetAll(specification))
            .Returns(() => messagesList.Where(x => x.Body.Contains(bodySearch)).AsQueryable());

        //Act
        var messages = _messagesService.GetAll(specification);

        //Assert
        Assert.NotNull(messages);
        Assert.Empty(messages);
        Assert.Equal(equalCount, messages.ToList().Count);
    }

    /// <summary>
    /// Get all messages.
    /// Should return nothing with  when messages does not exists.
    /// </summary>
    /// <param name="bodySearch">The message search.</param>
    [Theory]
    [InlineData("Tag 0")]
    public void GetAll_WithEqualSpecification_WhenMessagesDoesNotExists_ShouldReturnNothing(string bodySearch)
    {
        //Arrange
        var specification = new MessageSpecification(x => x.Body.Equals(bodySearch));
        _messagesRepositoryMock.Setup(x => x.GetAll(specification))
            .Returns(() => new List<Message>().AsQueryable());

        //Act
        var messages = _messagesService.GetAll();

        //Assert
        Assert.Empty(messages);
    }

    /// <summary>
    /// Get all messages with specification.
    /// Should return messages when specification is empty.
    /// </summary>
    [Theory]
    [InlineData(0, "message ")]
    public void GetAll_WithContainsSpecification_WhenSpecificationIsEmpty_ShouldReturnMessages(int notEqualCount, string messageBodySearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Body, messageBodySearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification();
        _messagesRepositoryMock.Setup(x => x.GetAll(It.IsAny<MessageSpecification>()))
            .Returns(() => messagesList.Where(x => x.Body.Contains(messageBodySearch)).AsQueryable());

        //Act
        var messages = _messagesService.GetAll(specification);

        //Assert
        Assert.NotNull(messages);
        Assert.NotEmpty(messages);
        Assert.NotEqual(notEqualCount, messages.ToList().Count);
    }

    /// <summary>
    /// Get all messages with specification.
    /// Should return messages when specification is null.
    /// </summary>
    [Theory]
    [InlineData(0, "message ")]
    public void GetAll_WithContainsSpecification_WhenSpecificationIsNull_ShouldReturnMessages(int notEqualCount, string messageBodySearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture(messageBodySearch)
                .With(x => x.Body, messageBodySearch)
                .CreateMany(random.Next(100));

        MessageSpecification specification = null;
        _messagesRepositoryMock.Setup(x => x.GetAll(It.IsAny<MessageSpecification>()))
            .Returns(() => messagesList.Where(x => x.Body.Contains(messageBodySearch)).AsQueryable());

        //Act
        var messages = _messagesService.GetAll(specification);

        //Assert
        Assert.NotNull(messages);
        Assert.NotEmpty(messages);
        Assert.NotEqual(notEqualCount, messages.ToList().Count);
    }

    /// <summary>
    /// Get all messages with specification.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Theory]
    [InlineData("message 0")]
    public void GetAll_WithEqualsSpecification_WhenRepositoryThrowsException_ShouldThrowException(string search)
    {
        //Arrange
        var specification = new MessageSpecification(x => x.Body.Equals(search));
        _messagesRepositoryMock.Setup(x => x.GetAll(It.IsAny<MessageSpecification>()))
            .Throws(() => new Exception("Test exception"));

        //Assert
        Assert.Throws<Exception>(() => _messagesService.GetAll(specification));
    }

    #endregion

    #region Get all async function With Specification

    /// <summary>
    /// Verify that function Get All Async with specification has been called.
    /// </summary>
    /// <param name="bodySearch">The body search.</param>
    [Theory]
    [InlineData("Test body")]
    public async Task Verify_FunctionGetAllAsync_WithSpecification_HasBeenCalled(string bodySearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Body, bodySearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Body.Contains(bodySearch));
        _messagesRepositoryMock.Setup(x => x.GetAllAsync(specification))
            .ReturnsAsync(() => messagesList.Where(x => x.Body.Contains(bodySearch)).ToList());

        //Act
        await _messagesService.GetAllAsync(specification);

        //Assert
        _messagesRepositoryMock.Verify(x => x.GetAllAsync(specification), Times.Once);
    }

    /// <summary>
    /// Get all async messages with specification.
    /// Should return messages with contains specification when messages exists.
    /// </summary>
    /// <param name="notEqualCount">The not equal count.</param>
    /// <param name="bodySearch">The body search.</param>
    [Theory]
    [InlineData(0, "Test body")]
    public async Task GetAllAsync_WithContainsSpecification_WhenMessagesExists_ShouldReturnMessages(int notEqualCount, string bodySearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Body, bodySearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Body.Contains(bodySearch));
        _messagesRepositoryMock.Setup(x => x.GetAllAsync(specification))
            .ReturnsAsync(() => messagesList.Where(x => x.Body.Contains(bodySearch)).ToList());

        //Act
        var messages = await _messagesService.GetAllAsync(specification);

        //Assert
        Assert.NotNull(messages);
        Assert.NotEmpty(messages);
        Assert.NotEqual(notEqualCount, messages.ToList().Count);
    }

    /// <summary>
    /// Get all async messages with specification.
    /// Should return message with equal specification when messages exists.
    /// </summary>
    /// <param name="equalCount">The equal count.</param>
    /// <param name="bodySearch">The body search.</param>
    [Theory]
    [InlineData(1, "Test body0")]
    public async Task GetAllAsync_WithEqualsSpecification_WhenMessagesExists_ShouldReturnMessage(int equalCount, string bodySearch)
    {
        //Arrange
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Body, bodySearch)
                .CreateMany(1);

        var specification = new MessageSpecification(x => x.Body.Contains(bodySearch));
        _messagesRepositoryMock.Setup(x => x.GetAllAsync(specification))
            .ReturnsAsync(() => messagesList.Where(x => x.Body.Contains(bodySearch)).ToList());

        //Act
        var messages = await _messagesService.GetAllAsync(specification);

        //Assert
        Assert.NotNull(messages);
        Assert.NotEmpty(messages);
        Assert.Equal(equalCount, messages.ToList().Count);
    }

    /// <summary>
    /// Get all async messages with specification.
    /// Should return nothing with  when messages does not exists.
    /// </summary>
    /// <param name="equalCount">The equal count.</param>
    /// <param name="bodySearch">The body search.</param>
    [Theory]
    [InlineData(0, "Test body-1")]
    public async Task GetAllAsync_WithEqualSpecification_WhenMessagesExists_ShouldReturnNothing(int equalCount, string bodySearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Body.Contains(bodySearch));
        _messagesRepositoryMock.Setup(x => x.GetAllAsync(specification))
            .ReturnsAsync(() => messagesList.Where(x => x.Body.Contains(bodySearch)).ToList());

        //Act
        var messages = await _messagesService.GetAllAsync(specification);

        //Assert
        Assert.NotNull(messages);
        Assert.Empty(messages);
        Assert.Equal(equalCount, messages.ToList().Count);
    }

    /// <summary>
    /// Get all async messages.
    /// Should return nothing with  when messages does not exists.
    /// </summary>
    /// <param name="bodySearch">The message search.</param>
    [Theory]
    [InlineData("Tag 0")]
    public async Task GetAllAsync_WithEqualSpecification_WhenMessagesDoesNotExists_ShouldReturnNothing(string bodySearch)
    {
        //Arrange
        var specification = new MessageSpecification(x => x.Body.Equals(bodySearch));
        _messagesRepositoryMock.Setup(x => x.GetAllAsync(specification))
            .ReturnsAsync(() => []);

        //Act
        var messages = await _messagesService.GetAllAsync();

        //Assert
        Assert.Null(messages);
    }

    /// <summary>
    /// Get all Async messages with specification.
    /// Should return messages when specification is empty.
    /// </summary>
    [Theory]
    [InlineData(0, "message ")]
    public async Task GetAllAsync_WithContainsSpecification_WhenSpecificationIsEmpty_ShouldReturnMessages(int notEqualCount, string messageBodySearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Body, messageBodySearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification();
        _messagesRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<MessageSpecification>()))
            .ReturnsAsync(() => messagesList.Where(x => x.Body.Contains(messageBodySearch)).ToList());

        //Act
        var messages = await _messagesService.GetAllAsync(specification);

        //Assert
        Assert.NotNull(messages);
        Assert.NotEmpty(messages);
        Assert.NotEqual(notEqualCount, messages.ToList().Count);
    }

    /// <summary>
    /// Get all async messages with specification.
    /// Should return messages when specification is null.
    /// </summary>
    [Theory]
    [InlineData(0, "message ")]
    public async Task GetAllAsync_WithContainsSpecification_WhenSpecificationIsNull_ShouldReturnMessages(int notEqualCount, string messageBodySearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture(messageBodySearch)
                .With(x => x.Body, messageBodySearch)
                .CreateMany(random.Next(100));

        MessageSpecification specification = null;
        _messagesRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<MessageSpecification>()))
            .ReturnsAsync(() => messagesList.Where(x => x.Body.Contains(messageBodySearch)).ToList());

        //Act
        var messages = await _messagesService.GetAllAsync(specification);

        //Assert
        Assert.NotNull(messages);
        Assert.NotEmpty(messages);
        Assert.NotEqual(notEqualCount, messages.ToList().Count);
    }

    /// <summary>
    /// Get all Async messages with specification.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Theory]
    [InlineData("message 0")]
    public async Task GetAllAsync_WithEqualsSpecification_WhenRepositoryThrowsException_ShouldThrowException(string search)
    {
        //Arrange
        var specification = new MessageSpecification(x => x.Body.Equals(search));
        _messagesRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<MessageSpecification>()))
            .ThrowsAsync(new Exception("Test exception"));

        //Assert
        await Assert.ThrowsAsync<Exception>(() => _messagesService.GetAllAsync(specification));
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
        var newMessage =
            SetupMessageFixture()
                .Create();
        _messagesRepositoryMock.Setup(x => x.GetById(newMessage.Id))
            .Returns(() => newMessage);

        //Act
        _messagesService.Find(newMessage.Id);

        //Assert
        _messagesRepositoryMock.Verify(x => x.GetById(newMessage.Id), Times.Once);
    }

    /// <summary>
    /// Find message.
    /// Should return message when message exists.
    /// </summary>
    [Fact]
    public void Find_WhenMessageExists_ShouldReturnMessage()
    {
        //Arrange
        var newMessage =
            SetupMessageFixture()
                .Create();
        _messagesRepositoryMock.Setup(x => x.GetById(newMessage.Id))
            .Returns(() => newMessage);

        //Act
        var message = _messagesService.Find(newMessage.Id);

        //Assert
        Assert.Equal(newMessage.Id, message.Id);
    }

    /// <summary>
    /// Find message.
    /// Should return nothing when message does not exists.
    /// </summary>
    [Fact]
    public void Find_WhenMessageDoesNotExists_ShouldReturnNothing()
    {
        //Arrange
        var messageId = _fixture.Create<int>();
        _messagesRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(() => null);

        //Act
        var tag = _messagesService.Find(messageId);

        //Assert
        Assert.Null(tag);
    }

    /// <summary>
    /// Find message.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public void Find_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var messageId = _fixture.Create<int>();
        _messagesRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Throws(() => new Exception("Test exception"));

        //Assert
        Assert.Throws<Exception>(() => _messagesService.Find(messageId));
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
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.GetByIdAsync(newMessage.Id))
            .ReturnsAsync(() => newMessage);

        //Act
        await _messagesService.FindAsync(newMessage.Id);

        //Assert
        _messagesRepositoryMock.Verify(x => x.GetByIdAsync(newMessage.Id), Times.Once);
    }

    /// <summary>
    /// Async find message.
    /// Should return message when message exists.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task FindAsync_WhenMessageExists_ShouldReturnMessage()
    {
        //Arrange
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.GetByIdAsync(newMessage.Id))
            .ReturnsAsync(() => newMessage);

        //Act
        var tag = await _messagesService.FindAsync(newMessage.Id);

        //Assert
        Assert.Equal(newMessage.Id, tag.Id);
    }

    /// <summary>
    /// Async find message.
    /// Should return nothing when message does not exists.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task FindAsync_WhenMessageDoesNotExists_ShouldReturnNothing()
    {
        //Arrange
        var messageId = _fixture.Create<int>();

        _messagesRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => null);

        //Act
        var message = await _messagesService.FindAsync(messageId);

        //Assert
        Assert.Null(message);
    }

    /// <summary>
    /// Async find message.
    /// Should throw exception when repository throws exception.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task FindAsync_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var messageId = _fixture.Create<int>();

        _messagesRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception("Test exception"));

        //Assert
        await Assert.ThrowsAsync<Exception>(() => _messagesService.FindAsync(messageId));
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
        var messageId = _fixture.Create<int>();
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.Insert(newMessage))
            .Callback(() =>
            {
                newMessage.Id = messageId;
            });

        //Act
        _messagesService.Insert(newMessage);

        //Assert
        _messagesRepositoryMock.Verify(x => x.Insert(newMessage), Times.Once);
    }

    /// <summary>
    /// Insert message.
    /// Should return message when message created.
    /// </summary>
    [Fact]
    public void Insert_WhenMessageExists_ShouldReturnMessage()
    {
        //Arrange
        var messageId = _fixture.Create<int>();
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.Insert(newMessage))
            .Callback(() =>
            {
                newMessage.Id = messageId;
            });

        //Act
        _messagesService.Insert(newMessage);

        //Assert
        Assert.NotEqual(0, newMessage.Id);
    }

    /// <summary>
    /// Insert message.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public void Insert_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var newMessage = SetupMessageFixture().Create();

        _messagesRepositoryMock.Setup(x => x.Insert(It.IsAny<Message>()))
            .Throws(new Exception("Test exception"));

        //Assert
        Assert.Throws<Exception>(() => _messagesService.Insert(newMessage));
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
        var messageId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newMessages =
            SetupMessageFixture()
                .CreateMany(random.Next(100))
                .ToList();

        _messagesRepositoryMock.Setup(x => x.Insert(newMessages))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newMessages[i].Id = messageId + i;
                }
            });

        //Act
        _messagesService.Insert(newMessages);

        //Assert
        _messagesRepositoryMock.Verify(x => x.Insert(newMessages), Times.Once);
    }

    /// <summary>
    /// Insert Enumerable comment.
    /// Should return comments when comments created.
    /// </summary>
    [Fact]
    public void InsertEnumerable_WhenCommentsExists_ShouldReturnComments()
    {
        //Arrange
        var random = new Random();
        var messageId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newMessages =
            SetupMessageFixture()
                .CreateMany(random.Next(100))
                .ToList();

        _messagesRepositoryMock.Setup(x => x.Insert(newMessages))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newMessages[i].Id = messageId + i;
                }
            });

        //Act
        _messagesService.Insert(newMessages);

        //Assert
        newMessages.ForEach(x =>
        {
            Assert.NotEqual(0, x.Id);
        });
    }

    #endregion

    #region Insert Async function

    /// <summary>
    /// Verify that function Insert Async has been called.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task Verify_FunctionInsertAsync_HasBeenCalled()
    {
        //Arrange
        var messageId = _fixture.Create<int>();
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessage))
            .Callback(() =>
            {
                newMessage.Id = messageId;
            });

        //Act
        await _messagesService.InsertAsync(newMessage);

        //Assert
        _messagesRepositoryMock.Verify(x => x.InsertAsync(newMessage), Times.Once);
    }

    /// <summary>
    /// Async insert message.
    /// Should return message when message created.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task InsertAsync_WhenMessageExists_ShouldReturnMessage()
    {
        //Arrange
        var messageId = _fixture.Create<int>();
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessage))
            .Callback(() =>
            {
                newMessage.Id = messageId;
            });

        //Act
        await _messagesService.InsertAsync(newMessage);

        //Assert
        Assert.NotEqual(0, newMessage.Id);
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
        var messageId = random.Next(52);
        var itemsCount = random.Next(10);
        var newMessages =
            SetupMessageFixture()
                .CreateMany(random.Next(100))
                .ToList();

        _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessages))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newMessages[i].Id = messageId + i;
                }
            });

        //Act
        await _messagesService.InsertAsync(newMessages);

        //Assert
        _messagesRepositoryMock.Verify(x => x.InsertAsync(newMessages), Times.Once);
    }

    /// <summary>
    /// Insert Async Enumerable comment.
    /// Should return comments when comments created.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task InsertAsyncEnumerable_WhenCommentsExists_ShouldReturnComments()
    {
        //Arrange
        var random = new Random();
        var messageId = random.Next(52);
        var itemsCount = random.Next(10);
        var newMessages =
            SetupMessageFixture()
                .CreateMany(random.Next(100))
                .ToList();

        _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessages))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newMessages[i].Id = messageId + i;
                }
            });

        //Act
        await _messagesService.InsertAsync(newMessages);

        //Assert
        newMessages.ForEach(x =>
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
    /// <param name="newMessageSubject">The new message subject.</param>
    [Theory]
    [InlineData("New subject")]
    public void Verify_FunctionUpdate_HasBeenCalled(string newMessageSubject)
    {
        //Arrange
        var messageId = _fixture.Create<int>();
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.Insert(newMessage))
            .Callback(() =>
            {
                newMessage.Id = messageId;
            });
        _messagesRepositoryMock.Setup(x => x.GetById(messageId))
            .Returns(() => newMessage);

        //Act
        _messagesService.Insert(newMessage);
        var message = _messagesService.Find(messageId);
        message.Subject = newMessageSubject;
        _messagesService.Update(message);

        //Assert
        _messagesRepositoryMock.Verify(x => x.Update(message), Times.Once);
    }

    /// <summary>
    /// Update message.
    /// Should return message when message updated.
    /// </summary>
    /// <param name="newMessageSubject">The new message subject.</param>
    [Theory]
    [InlineData("New subject")]
    public void Update_WhenMessageExists_ShouldReturnMessage(string newMessageSubject)
    {
        //Arrange
        var messageId = _fixture.Create<int>();
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.Insert(newMessage))
            .Callback(() =>
            {
                newMessage.Id = messageId;
            });
        _messagesRepositoryMock.Setup(x => x.GetById(messageId))
            .Returns(() => newMessage);

        //Act
        _messagesService.Insert(newMessage);
        var message = _messagesService.Find(messageId);
        message.Subject = newMessageSubject;
        _messagesService.Update(message);

        //Assert
        Assert.Equal(newMessageSubject, message.Subject);
    }

    #endregion

    #region Upadate Enumerable function

    /// <summary>
    /// Verify that function Update has been called.
    /// </summary>
    /// <param name="newMessageSubject">The new message subject.</param>
    [Theory]
    [InlineData("New subject")]
    public void Verify_FunctionUpdateEnumerable_HasBeenCalled(string newMessageSubject)
    {
        //Arrange
        var random = new Random();
        var messageId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newMessages =
            SetupMessageFixture()
                .CreateMany(random.Next(100))
                .ToList();

        _messagesRepositoryMock.Setup(x => x.Insert(newMessages))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newMessages[i].Id = messageId + i;
                }
            });

        //Act
        _messagesService.Insert(newMessages);
        for (var i = 0; i < itemsCount; i++)
        {
            newMessages[i].Subject = $"{newMessageSubject} {i}";
        }
        _messagesService.Update(newMessages);

        //Assert
        _messagesRepositoryMock.Verify(x => x.Update(newMessages), Times.Once);
    }

    /// <summary>
    /// Update Enumerable message.
    /// Should return message when message updated.
    /// </summary>
    /// <param name="newMessageSubject">The new message subject.</param>
    [Theory]
    [InlineData("New subject")]
    public void UpdateEnumerable_WhenMessageExists_ShouldReturnMessage(string newMessageSubject)
    {
        //Arrange
        var random = new Random();
        var messageId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newMessages =
            SetupMessageFixture()
                .CreateMany(random.Next(100))
                .ToList();

        _messagesRepositoryMock.Setup(x => x.Insert(newMessages))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newMessages[i].Id = messageId + i;
                }
            });

        //Act
        _messagesService.Insert(newMessages);
        for (var i = 0; i < itemsCount; i++)
        {
            newMessages[i].Subject = $"{newMessageSubject} {i}";
        }
        _messagesService.Update(newMessages);

        //Assert
        for (var i = 0; i < itemsCount; i++)
        {
            Assert.Equal($"{newMessageSubject} {i}", newMessages[i].Subject);
        }
    }

    #endregion

    #region Update Async function

    /// <summary>
    /// Verify that function Update Async has been called.
    /// Should return tag when tag updated.
    /// </summary>
    /// <param name="newMessageSubject">The new message subject.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("New Tag")]
    public async Task Verify_FunctionUpdateAsync_HasBeenCalled(string newMessageSubject)
    {
        //Arrange
        var messageId = _fixture.Create<int>();
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessage))
            .Callback(() =>
            {
                newMessage.Id = messageId;
            });
        _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
            .ReturnsAsync(() => newMessage);

        //Act
        await _messagesService.InsertAsync(newMessage);
        var message = await _messagesService.FindAsync(messageId);
        message.Subject = newMessageSubject;
        await _messagesService.UpdateAsync(message);

        //Assert
        _messagesRepositoryMock.Verify(x => x.UpdateAsync(message), Times.Once);
    }

    /// <summary>
    /// Async update message.
    /// Should return message when message updated.
    /// </summary>
    /// <param name="newMessageSubject">The new message subject.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("New Tag")]
    public async Task UpdateAsync_WhenMessageExists_ShouldReturnMessage(string newMessageSubject)
    {
        //Arrange
        var messageId = _fixture.Create<int>();
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessage))
            .Callback(() =>
            {
                newMessage.Id = messageId;
            });
        _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
            .ReturnsAsync(() => newMessage);

        //Act
        await _messagesService.InsertAsync(newMessage);
        var message = await _messagesService.FindAsync(messageId);
        message.Subject = newMessageSubject;
        await _messagesService.UpdateAsync(message);

        //Assert
        Assert.Equal(newMessageSubject, message.Subject);
    }

    #endregion

    #region Upadate Async Enumerable function

    /// <summary>
    /// Verify that function Update Async has been called.
    /// </summary>
    /// <param name="newMessageSubject">The new message subject.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("New subject")]
    public async Task Verify_FunctionUpdateAsyncEnumerable_HasBeenCalled(string newMessageSubject)
    {
        //Arrange
        var random = new Random();
        var messageId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newMessages =
            SetupMessageFixture()
                .CreateMany(random.Next(100))
                .ToList();

        _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessages))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newMessages[i].Id = messageId + i;
                }
            });

        //Act
        await _messagesService.InsertAsync(newMessages);
        for (var i = 0; i < itemsCount; i++)
        {
            newMessages[i].Subject = $"{newMessageSubject} {i}";
        }
        await _messagesService.UpdateAsync(newMessages);

        //Assert
        _messagesRepositoryMock.Verify(x => x.UpdateAsync(newMessages), Times.Once);
    }

    /// <summary>
    /// Update Async Enumerable message.
    /// Should return message when message updated.
    /// </summary>
    /// <param name="newMessageSubject">The new message subject.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("New subject")]
    public async Task UpdateAsyncEnumerable_WhenMessageExists_ShouldReturnMessage(string newMessageSubject)
    {
        //Arrange
        var random = new Random();
        var messageId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newMessages =
            SetupMessageFixture()
                .CreateMany(random.Next(100))
                .ToList();

        _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessages))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newMessages[i].Id = messageId + i;
                }
            });

        //Act
        await _messagesService.InsertAsync(newMessages);
        for (var i = 0; i < itemsCount; i++)
        {
            newMessages[i].Subject = $"{newMessageSubject} {i}";
        }
        await _messagesService.UpdateAsync(newMessages);

        //Assert
        for (var i = 0; i < itemsCount; i++)
        {
            Assert.Equal($"{newMessageSubject} {i}", newMessages[i].Subject);
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
        var messageId = _fixture.Create<int>();
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.Insert(newMessage))
            .Callback(() =>
            {
                newMessage.Id = messageId;
            });
        _messagesRepositoryMock.Setup(x => x.GetById(messageId))
            .Returns(() => newMessage);

        //Act
        _messagesService.Insert(newMessage);
        var message = _messagesService.Find(messageId);
        _messagesService.Delete(messageId);
        _messagesRepositoryMock.Setup(x => x.GetById(messageId))
            .Returns(() => null);
        _messagesService.Find(messageId);

        //Assert
        _messagesRepositoryMock.Verify(x => x.Delete(newMessage), Times.Once);
    }

    /// <summary>
    /// Delete By Id message.
    /// Should return nothing when message is deleted.
    /// </summary>
    [Fact]
    public void DeleteById_WhenMessageDeleted_ShouldReturnNothing()
    {
        //Arrange
        var messageId = _fixture.Create<int>();
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.Insert(newMessage))
            .Callback(() =>
            {
                newMessage.Id = messageId;
            });
        _messagesRepositoryMock.Setup(x => x.GetById(messageId))
            .Returns(() => newMessage);

        //Act
        _messagesService.Insert(newMessage);
        var message = _messagesService.Find(messageId);
        _messagesService.Delete(messageId);
        _messagesRepositoryMock.Setup(x => x.GetById(messageId))
            .Returns(() => null);
        var deletedMessage = _messagesService.Find(messageId);

        //Assert
        Assert.Null(deletedMessage);
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
        var messageId = _fixture.Create<int>();
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.Insert(newMessage))
            .Callback(() =>
            {
                newMessage.Id = messageId;
            });
        _messagesRepositoryMock.Setup(x => x.GetById(messageId))
            .Returns(() => newMessage);

        //Act
        _messagesService.Insert(newMessage);
        var message = _messagesService.Find(messageId);
        _messagesService.Delete(message);
        _messagesRepositoryMock.Setup(x => x.GetById(messageId))
            .Returns(() => null);
        _messagesService.Find(messageId);

        //Assert
        _messagesRepositoryMock.Verify(x => x.Delete(message), Times.Once);
    }

    /// <summary>
    /// Delete By Object message.
    /// Should return nothing when message is deleted.
    /// </summary>
    [Fact]
    public void DeleteByObject_WhenMessageDeleted_ShouldReturnNothing()
    {
        //Arrange
        var messageId = _fixture.Create<int>();
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.Insert(newMessage))
            .Callback(() =>
            {
                newMessage.Id = messageId;
            });
        _messagesRepositoryMock.Setup(x => x.GetById(messageId))
            .Returns(() => newMessage);

        //Act
        _messagesService.Insert(newMessage);
        var message = _messagesService.Find(messageId);
        _messagesService.Delete(message);
        _messagesRepositoryMock.Setup(x => x.GetById(messageId))
            .Returns(() => null);
        var deletedMessage = _messagesService.Find(messageId);

        //Assert
        Assert.Null(deletedMessage);
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
        var messageId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newMessages =
            SetupMessageFixture()
                .CreateMany(itemsCount)
                .ToList();

        _messagesRepositoryMock.Setup(x => x.Insert(newMessages))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newMessages[i].Id = messageId + i;
                }
            });

        //Act
        _messagesService.Insert(newMessages);
        _messagesService.Delete(newMessages);

        //Assert
        _messagesRepositoryMock.Verify(x => x.Delete(newMessages), Times.Once);
    }

    /// <summary>
    /// Delete By Enumerable message.
    /// Should return nothing when message is deleted.
    /// </summary>
    [Fact]
    public void DeleteByEnumerable_WhenMessageDeleted_ShouldReturnNothing()
    {
        //Arrange
        var random = new Random();
        var messageId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newMessages =
            SetupMessageFixture()
                .CreateMany(itemsCount)
                .ToList();

        _messagesRepositoryMock.Setup(x => x.Insert(newMessages))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newMessages[i].Id = messageId + i;
                }
            });
        _messagesRepositoryMock.Setup(x => x.Delete(newMessages))
            .Callback(() =>
            {
                newMessages = null;
            });

        //Act
        _messagesService.Insert(newMessages);
        _messagesService.Delete(newMessages);

        //Assert
        Assert.Null(newMessages);
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
        var messageId = _fixture.Create<int>();
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessage))
            .Callback(() =>
            {
                newMessage.Id = messageId;
            });
        _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
            .ReturnsAsync(() => newMessage);

        //Act
        await _messagesService.InsertAsync(newMessage);
        await _messagesService.DeleteAsync(messageId);

        //Assert
        _messagesRepositoryMock.Verify(x => x.DeleteAsync(newMessage), Times.Once);
    }

    /// <summary>
    /// Async delete by id message.
    /// Should return nothing when message is deleted.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task DeleteAsyncById_WhenMessageIsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var messageId = _fixture.Create<int>();
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessage))
            .Callback(() =>
            {
                newMessage.Id = messageId;
            });
        _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
            .ReturnsAsync(() => newMessage);

        //Act
        await _messagesService.InsertAsync(newMessage);
        await _messagesService.DeleteAsync(messageId);
        _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
            .ReturnsAsync(() => null);
        var deletedMessage = await _messagesService.FindAsync(messageId);

        //Assert
        Assert.Null(deletedMessage);
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
        var messageId = _fixture.Create<int>();
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessage))
            .Callback(() =>
            {
                newMessage.Id = messageId;
            });
        _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
            .ReturnsAsync(() => newMessage);

        //Act
        await _messagesService.InsertAsync(newMessage);
        var comment = await _messagesService.FindAsync(messageId);
        await _messagesService.DeleteAsync(comment);

        //Assert
        _messagesRepositoryMock.Verify(x => x.DeleteAsync(comment), Times.Once);
    }

    /// <summary>
    /// Async delete by object message.
    /// Should return nothing when message is deleted.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task DeleteAsyncByObject_WhenMessageIsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var messageId = _fixture.Create<int>();
        var newMessage =
            SetupMessageFixture()
                .Create();

        _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessage))
            .Callback(() =>
            {
                newMessage.Id = messageId;
            });
        _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
            .ReturnsAsync(() => newMessage);

        //Act
        await _messagesService.InsertAsync(newMessage);
        var message = await _messagesService.FindAsync(messageId);
        await _messagesService.DeleteAsync(message);
        _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
            .ReturnsAsync(() => null);
        var deletedMessage = await _messagesService.FindAsync(messageId);

        //Assert
        Assert.Null(deletedMessage);
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
        var messageId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newMessages =
            SetupMessageFixture()
                .CreateMany(itemsCount)
                .ToList();

        _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessages))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newMessages[i].Id = messageId + i;
                }
            });

        //Act
        await _messagesService.InsertAsync(newMessages);
        await _messagesService.DeleteAsync(newMessages);

        //Assert
        _messagesRepositoryMock.Verify(x => x.DeleteAsync(newMessages), Times.Once);
    }

    /// <summary>
    /// Delete Async By Enumerable message.
    /// Should return nothing when message is deleted.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task DeleteAsyncByEnumerable_WhenMessageDeleted_ShouldReturnNothing()
    {
        //Arrange
        var random = new Random();
        var messageId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newMessages =
            SetupMessageFixture()
                .CreateMany(itemsCount)
                .ToList();

        _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessages))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newMessages[i].Id = messageId + i;
                }
            });
        _messagesRepositoryMock.Setup(x => x.DeleteAsync(newMessages))
            .Callback(() =>
            {
                newMessages = null;
            });

        //Act
        await _messagesService.InsertAsync(newMessages);
        await _messagesService.DeleteAsync(newMessages);

        //Assert
        Assert.Null(newMessages);
    }

    #endregion

    #endregion

    #region Any

    #region Any function With Specification

    /// <summary>
    /// Verify that function Any with specification has been called.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    [Theory]
    [InlineData("Test subject")]
    public void Verify_FunctionAny_WithSpecification_HasBeenCalled(string subjectSearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Body, subjectSearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => messagesList.Any(x => x.Subject.Contains(subjectSearch)));

        //Act
        _messagesService.Any(specification);

        //Assert
        _messagesRepositoryMock.Verify(x => x.Any(specification), Times.Once);
    }

    /// <summary>
    /// Check if there are any messages with specification.
    /// Should return true with contains specification when messages exists.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    [Theory]
    [InlineData("Test subject")]
    public void Any_WithContainsSpecification_WhenMessagesExists_ShouldReturnTrue(string subjectSearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Subject, subjectSearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => messagesList.Any(x => x.Subject.Contains(subjectSearch)));

        //Act
        var areAnyMessages = _messagesService.Any(specification);

        //Assert
        Assert.True(areAnyMessages);
    }

    /// <summary>
    /// Check if there are any messages with specification.
    /// Should return true with equal specification when messages exists.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    [Theory]
    [InlineData("Test subject0")]
    public void Any_WithEqualsSpecification_WhenMessagesExists_ShouldReturnTrue(string subjectSearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Subject, subjectSearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => messagesList.Any(x => x.Subject.Contains(subjectSearch)));

        //Act
        var areAnyMessages = _messagesService.Any(specification);

        //Assert
        Assert.True(areAnyMessages);
    }

    /// <summary>
    /// Check if there are any messages with specification.
    /// Should return false with when messages does not exists.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    [Theory]
    [InlineData("Test subject-1")]
    public void Any_WithEqualSpecification_WhenMessagesExists_ShouldReturnFalse(string subjectSearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => messagesList.Any(x => x.Subject.Contains(subjectSearch)));

        //Act
        var areAnyMessages = _messagesService.Any(specification);

        //Assert
        Assert.False(areAnyMessages);
    }

    /// <summary>
    /// Check if there are any messages with specification.
    /// Should return false with when messages does not exists.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    [Theory]
    [InlineData("Test subject0")]
    public void Any_WithEqualSpecification_WhenMessagesDoesNotExists_ShouldReturnNothing(string subjectSearch)
    {
        //Arrange
        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => false);

        //Act
        var areAnyMessages = _messagesService.Any(specification);

        //Assert
        Assert.False(areAnyMessages);
    }

    #endregion

    #region Any Async function With Specification

    /// <summary>
    /// Verify that function Any Async with specification has been called.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Test subject")]
    public async Task Verify_FunctionAnyAsync_WithSpecification_HasBeenCalled(string subjectSearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Body, subjectSearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => messagesList.Any(x => x.Subject.Contains(subjectSearch)));

        //Act
        await _messagesService.AnyAsync(specification);

        //Assert
        _messagesRepositoryMock.Verify(x => x.AnyAsync(specification), Times.Once);
    }

    /// <summary>
    /// Async check if there are any messages with specification.
    /// Should return true with contains specification when messages exists.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Test subject")]
    public async Task AnyAsync_WithContainsSpecification_WhenMessagesExists_ShouldReturnTrue(string subjectSearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Subject, subjectSearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => messagesList.Any(x => x.Subject.Contains(subjectSearch)));

        //Act
        var areAnyMessages = await _messagesService.AnyAsync(specification);

        //Assert
        Assert.True(areAnyMessages);
    }

    /// <summary>
    /// Async check if there are any messages with specification.
    /// Should return true with equal specification when messages exists.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Test subject0")]
    public async Task AnyAsync_WithEqualsSpecification_WhenMessagesExists_ShouldReturnTrue(string subjectSearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Subject, subjectSearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => messagesList.Any(x => x.Subject.Contains(subjectSearch)));

        //Act
        var areAnyMessages = await _messagesService.AnyAsync(specification);

        //Assert
        Assert.True(areAnyMessages);
    }

    /// <summary>
    /// Async check if there are any messages with specification.
    /// Should return false with when messages does not exists.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Test subject-1")]
    public async Task AnyAsync_WithEqualSpecification_WhenMessagesExists_ShouldReturnFalse(string subjectSearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => messagesList.Any(x => x.Subject.Contains(subjectSearch)));

        //Act
        var areAnyMessages = await _messagesService.AnyAsync(specification);

        //Assert
        Assert.False(areAnyMessages);
    }

    /// <summary>
    /// Async check if there are any messages with specification.
    /// Should return false with when messages does not exists.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Test subject0")]
    public async Task AnyAsync_WithEqualSpecification_WhenMessagesDoesNotExists_ShouldReturnNothing(string subjectSearch)
    {
        //Arrange
        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => false);

        //Act
        var areAnyMessages = await _messagesService.AnyAsync(specification);

        //Assert
        Assert.False(areAnyMessages);
    }

    #endregion

    #endregion

    #region First Or Default function With Specification

    /// <summary>
    /// Verify that function First Or Default with specification has been called.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    [Theory]
    [InlineData("Test subject")]
    public void Verify_FunctionFirstOrDefault_WithSpecification_HasBeenCalled(string subjectSearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Subject, subjectSearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => messagesList.FirstOrDefault(x => x.Subject.Contains(subjectSearch)));

        //Act
        _messagesService.FirstOrDefault(specification);

        //Assert
        _messagesRepositoryMock.Verify(x => x.FirstOrDefault(specification), Times.Once);
    }

    /// <summary>
    /// Get first or default message with specification.
    /// Should return message with contains specification when messages exists.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    [Theory]
    [InlineData("Test subject")]
    public void FirstOrDefault_WithContainsSpecification_WhenMessagesExists_ShouldReturnMessage(string subjectSearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Subject, subjectSearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => messagesList.FirstOrDefault(x => x.Subject.Contains(subjectSearch)));

        //Act
        var message = _messagesService.FirstOrDefault(specification);

        //Assert
        Assert.NotNull(message);
        Assert.IsType<Message>(message);
    }

    /// <summary>
    /// Get first or default message with specification.
    /// Should return message with equal specification when messages exists.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    [Theory]
    [InlineData("Test subject0")]
    public void FirstOrDefault_WithEqualsSpecification_WhenMessagesExists_ShouldReturnMessage(string subjectSearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Subject, subjectSearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => messagesList.FirstOrDefault(x => x.Subject.Contains(subjectSearch)));

        //Act
        var message = _messagesService.FirstOrDefault(specification);

        //Assert
        Assert.NotNull(message);
        Assert.IsType<Message>(message);
    }

    /// <summary>
    /// Get first or default message with specification.
    /// Should return nothing with when messages does not exists.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    [Theory]
    [InlineData("Test subject-1")]
    public void FirstOrDefault_WithEqualSpecification_WhenMessagesExists_ShouldReturnNothing(string subjectSearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => messagesList.FirstOrDefault(x => x.Subject.Contains(subjectSearch)));

        //Act
        var message = _messagesService.FirstOrDefault(specification);

        //Assert
        Assert.Null(message);
    }

    /// <summary>
    /// Get first or default message with specification.
    /// Should return nothing with when messages does not exists.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    [Theory]
    [InlineData("Test subject0")]
    public void FirstOrDefault_WithEqualSpecification_WhenMessagesDoesNotExists_ShouldReturnNothing(string subjectSearch)
    {
        //Arrange
        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => null);

        //Act
        var message = _messagesService.FirstOrDefault(specification);

        //Assert
        Assert.Null(message);
    }

    #endregion

    #region Last Or Default function With Specification

    /// <summary>
    /// Verify that function Last Or Default with specification has been called.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    [Theory]
    [InlineData("Test subject")]
    public void Verify_WithSpecification_HasBeenCalled_FunctionLastOrDefault(string subjectSearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Subject, subjectSearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => messagesList.LastOrDefault(x => x.Subject.Contains(subjectSearch)));

        //Act
        _messagesService.LastOrDefault(specification);

        //Assert
        _messagesRepositoryMock.Verify(x => x.LastOrDefault(specification), Times.Once);
    }

    /// <summary>
    /// Get last or default message with specification.
    /// Should return message with contains specification when messages exists.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    [Theory]
    [InlineData("Test subject")]
    public void LastOrDefault_WithContainsSpecification_WhenMessagesExists_ShouldReturnMessage(string subjectSearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Subject, subjectSearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => messagesList.LastOrDefault(x => x.Subject.Contains(subjectSearch)));

        //Act
        var message = _messagesService.LastOrDefault(specification);

        //Assert
        Assert.NotNull(message);
        Assert.IsType<Message>(message);
    }

    /// <summary>
    /// Get last or default message with specification.
    /// Should return message with equal specification when messages exists.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    [Theory]
    [InlineData("Test subject0")]
    public void LastOrDefault_WithEqualsSpecification_WhenMessagesExists_ShouldReturnMessage(string subjectSearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Subject, subjectSearch)
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => messagesList.LastOrDefault(x => x.Subject.Contains(subjectSearch)));

        //Act
        var message = _messagesService.LastOrDefault(specification);

        //Assert
        Assert.NotNull(message);
        Assert.IsType<Message>(message);
    }

    /// <summary>
    /// Get last or default message with specification.
    /// Should return nothing with when messages does not exists.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    [Theory]
    [InlineData("Test subject-1")]
    public void LastOrDefault_WithEqualSpecification_WhenMessagesExists_ShouldReturnNothing(string subjectSearch)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .CreateMany(random.Next(100));

        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => messagesList.LastOrDefault(x => x.Subject.Contains(subjectSearch)));

        //Act
        var message = _messagesService.LastOrDefault(specification);

        //Assert
        Assert.Null(message);
    }

    /// <summary>
    /// Get last or default message with specification.
    /// Should return nothing with when messages does not exists.
    /// </summary>
    /// <param name="subjectSearch">The subject search.</param>
    [Theory]
    [InlineData("Test subject0")]
    public void LastOrDefault_WithEqualSpecification_WhenMessagesDoesNotExists_ShouldReturnNothing(string subjectSearch)
    {
        //Arrange
        var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
        _messagesRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => null);

        //Act
        var message = _messagesService.LastOrDefault(specification);

        //Assert
        Assert.Null(message);
    }

    #endregion

    #region Search async function

    /// <summary>
    /// Search the specified query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="messagesList">The messages list.</param>
    /// <returns>PagedListResult.</returns>
    protected static PagedListResult<Message> Search(SearchQuery<Message> query, List<Message> messagesList)
    {
        var sequence = messagesList.AsQueryable();

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
            sequence = ((IOrderedQueryable<Message>)sequence).OrderBy(x => true);
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

        return new PagedListResult<Message>
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
    [InlineData("Test subject ", 0, 10, "Subject", OrderType.Ascending)]
    [InlineData("Test subject ", 10, 10, "Subject", OrderType.Ascending)]
    [InlineData("Test subject ", 10, 20, "Subject", OrderType.Ascending)]
    [InlineData("Test subject ", 0, 100, "Subject", OrderType.Ascending)]
    public async Task Verify_FunctionSearchAsync_HasBeenCalled(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Body, search)
                .CreateMany(random.Next(100))
                .ToList();

        var query = new SearchQuery<Message>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Message>(fieldName, orderType));

        query.AddFilter(x => x.Body.ToUpper().Contains($"{search}".ToUpper()));

        _messagesRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => Search(query, messagesList));

        //Act
        await _messagesService.SearchAsync(query);

        //Assert
        _messagesRepositoryMock.Verify(x => x.SearchAsync(query), Times.Once);
    }

    /// <summary>
    /// Search async messages.
    /// Should return messages when messages exists.
    /// </summary>
    /// <param name="search">The search.</param>
    /// <param name="start">The start.</param>
    /// <param name="take">The take.</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="orderType">The order type.</param>
    [Theory]
    [InlineData("Test subject ", 0, 10, "Subject", OrderType.Ascending)]
    [InlineData("Test subject ", 10, 10, "Subject", OrderType.Ascending)]
    [InlineData("Test subject ", 10, 20, "Subject", OrderType.Ascending)]
    [InlineData("Test subject ", 0, 100, "Subject", OrderType.Ascending)]
    public async Task SearchAsync_WhenMessagesExists_ShouldReturnMessages(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Body, search)
                .CreateMany(random.Next(100))
                .ToList();

        var query = new SearchQuery<Message>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Message>(fieldName, orderType));

        query.AddFilter(x => x.Body.ToUpper().Contains($"{search}".ToUpper()));

        _messagesRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => Search(query, messagesList));

        //Act
        await _messagesService.SearchAsync(query);

        //Act
        var messages = await _messagesService.SearchAsync(query);

        //Assert
        Assert.NotNull(messages);
        Assert.NotEmpty(messages.Entities);
    }

    /// <summary>
    /// Search async messages with specification.
    /// Should return message with equal specification when messages exists.
    /// </summary>
    /// <param name="search">The search.</param>
    /// <param name="start">The start.</param>
    /// <param name="take">The take.</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="orderType">The order type.</param>
    [Theory]
    [InlineData("Test subject 1", 0, 10, "Subject", OrderType.Ascending)]
    [InlineData("Test subject 10", 10, 10, "Subject", OrderType.Ascending)]
    [InlineData("Test subject 11", 10, 20, "Subject", OrderType.Ascending)]
    [InlineData("Test subject 20", 0, 100, "Subject", OrderType.Ascending)]
    public async Task SearchAsync_WithEqualsSpecification_WhenCMessagesExists_ShouldReturnMessage(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var messagesList =
            SetupMessageFixture()
                .With(x => x.Body, search)
                .CreateMany(start + 1)
                .ToList();

        var query = new SearchQuery<Message>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Message>(fieldName, orderType));

        query.AddFilter(x => x.Body.ToUpper().Equals($"{search}".ToUpper()));

        _messagesRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => Search(query, messagesList));

        //Act
        await _messagesService.SearchAsync(query);

        //Act
        var messages = await _messagesService.SearchAsync(query);

        //Assert
        Assert.NotNull(messages);
        Assert.NotEmpty(messages.Entities);
        Assert.Single(messages.Entities);
    }

    /// <summary>
    /// Search async messages with specification.
    /// Should return nothing with  when messages does not exists.
    /// </summary>
    /// <param name="search">The search.</param>
    /// <param name="start">The start.</param>
    /// <param name="take">The take.</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="orderType">The order type.</param>
    [Theory]
    [InlineData("Test subject -1", 0, 10, "Subject", OrderType.Ascending)]
    [InlineData("Test subject -10", 10, 10, "Subject", OrderType.Ascending)]
    [InlineData("Test subject -11", 10, 20, "Subject", OrderType.Ascending)]
    [InlineData("Test subject -20", 0, 100, "Subject", OrderType.Ascending)]
    public async Task SearchAsync_WithEqualSpecification_WhenMessagesExists_ShouldReturnNothing(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var random = new Random();
        var messagesList =
            SetupMessageFixture()
                .CreateMany(random.Next(100))
                .ToList();

        var query = new SearchQuery<Message>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Message>(fieldName, orderType));

        query.AddFilter(x => x.Subject.ToUpper().Contains($"{search}".ToUpper()));

        _messagesRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => Search(query, messagesList));

        //Act
        await _messagesService.SearchAsync(query);

        //Act
        var messages = await _messagesService.SearchAsync(query);

        //Assert
        Assert.NotNull(messages);
        Assert.Empty(messages.Entities);
    }

    /// <summary>
    /// Search async messages.
    /// Should return nothing when messages does not exists.
    /// </summary>
    /// <param name="search">The search.</param>
    /// <param name="start">The start.</param>
    /// <param name="take">The take.</param>
    /// <param name="fieldName">The field name.</param>
    /// <param name="orderType">The order type.</param>
    [Theory]
    [InlineData("Test subject 1", 0, 10, "Subject", OrderType.Ascending)]
    [InlineData("Test subject 10", 10, 10, "Subject", OrderType.Ascending)]
    [InlineData("Test subject 11", 10, 20, "Subject", OrderType.Ascending)]
    [InlineData("Test subject 20", 0, 100, "Subject", OrderType.Ascending)]
    public async Task SearchAsync_WhenMessagesDoesNotExists_ShouldReturnNothing(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var query = new SearchQuery<Message>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Message>(fieldName, orderType));

        query.AddFilter(x => x.Body.ToUpper().Contains($"{search}".ToUpper()));

        _messagesRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => new PagedListResult<Message>());

        //Act
        var comments = await _messagesService.SearchAsync(query);

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
        var data = new List<Message>().AsQueryable();
        var query = new SearchQuery<Message> { Skip = 0, Take = 5 };
        var expected = new PagedListResult<Message> { Entities = new List<Message>(), Count = 0 };

        _messagesRepositoryMock.Setup(r => r.SearchBySquenceAsync(query, data)).ReturnsAsync(expected);

        var result = await _messagesService.SearchBySequenceAsync(query, data);

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
        var data = SetupMessageFixture().CreateMany(5).AsQueryable();
        var expected = new PagedListResult<Message> { Entities = data.ToList(), Count = 5 };

        _messagesRepositoryMock.Setup(r => r.SearchBySquenceAsync(null, data)).ReturnsAsync(expected);

        var result = await _messagesService.SearchBySequenceAsync(null, data);

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
        var query = new SearchQuery<Message> { Skip = 0, Take = 5 };
        var expected = new PagedListResult<Message> { Entities = null, Count = 5 };

        _messagesRepositoryMock.Setup(r => r.SearchBySquenceAsync(query, null)).ReturnsAsync(expected);

        var result = await _messagesService.SearchBySequenceAsync(query, null);

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
        var expected = new SearchQuery<Message>();

        _messagesRepositoryMock.Setup(r => r.GenerateQuery(tableFilter, null)).Returns(expected);

        var result = _messagesService.GenerateQuery(tableFilter);

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
        var expected = new SearchQuery<Message>();

        _messagesRepositoryMock.Setup(r => r.GenerateQuery(tableFilter, "SenderId")).Returns(expected);

        var result = _messagesService.GenerateQuery(tableFilter, "SenderId");

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
        var expected = new SearchQuery<Message>();

        _messagesRepositoryMock.Setup(r => r.GenerateQuery(tableFilter, null)).Returns(expected);

        var result = _messagesService.GenerateQuery(null);

        Assert.Null(result);
    }

    #endregion

    #endregion
}