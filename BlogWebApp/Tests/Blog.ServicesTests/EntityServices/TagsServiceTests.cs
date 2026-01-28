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
using Blog.Data.Specifications.Base;
using Xunit;

namespace Blog.ServicesTests.EntityServices;

/// <summary>
/// Tags service tests.
/// </summary>
public class TagsServiceTests
{
    #region Fields

    /// <summary>
    /// The tags service.
    /// </summary>
    private readonly ITagsService _tagsService;

    /// <summary>
    /// The tags repository mock.
    /// </summary>
    private readonly Mock<IRepository<Tag>> _tagsRepositoryMock;

    /// <summary>
    /// The fixture.
    /// </summary>
    private readonly Fixture _fixture;

    #endregion

    #region Ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="TagsServiceTests"/> class.
    /// </summary>
    public TagsServiceTests()
    {
        _tagsRepositoryMock = new Mock<IRepository<Tag>>();
        _tagsService = new TagsService(_tagsRepositoryMock.Object);
        _fixture = new Fixture();
    }

    #endregion

    #region Uthilities

    private IPostprocessComposer<Tag> SetupTagFixture(string title = null)
    {
        var tagSetup = _fixture.Build<Tag>()
            .Without(x => x.PostsTagsRelations);
        if (!string.IsNullOrWhiteSpace(title))
        {
            tagSetup.With(x => x.Title, title);
        }

        return tagSetup;
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
        var tagsList =
            SetupTagFixture()
                .CreateMany(random.Next(100));

        _tagsRepositoryMock.Setup(x => x.GetAll())
            .Returns(tagsList.AsQueryable());

        //Act
        _tagsService.GetAll();

        //Assert
        _tagsRepositoryMock.Verify(x => x.GetAll(), Times.Once);
    }

    /// <summary>
    /// Get all tags.
    /// Should return tags when tags exists.
    /// </summary>
    /// <param name="notEqualCount">The not equal count.</param>
    [Theory]
    [InlineData(0)]
    public void GetAll_WhenTagsExists_ShouldReturnTags(int notEqualCount)
    {
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture()
                .CreateMany(random.Next(100));

        _tagsRepositoryMock.Setup(x => x.GetAll())
            .Returns(() => tagsList.AsQueryable());

        //Act
        var tags = _tagsService.GetAll();

        //Assert
        Assert.NotNull(tags);
        Assert.NotEmpty(tags);
        Assert.NotEqual(notEqualCount, tags.ToList().Count);
    }

    /// <summary>
    /// Get all tags.
    /// Should return nothing when tags does not exist.
    /// </summary>
    [Fact]
    public void GetAll_WhenTagsDoesNotExists_ShouldReturnNothing()
    {
        //Arrange
        _tagsRepositoryMock.Setup(x => x.GetAll())
            .Returns(() => new List<Tag>().AsQueryable());

        //Act
        var tags = _tagsService.GetAll();

        //Assert
        Assert.Empty(tags);
    }

    /// <summary>
    /// Get all tags.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public void GetAll_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        _tagsRepositoryMock.Setup(x => x.GetAll())
            .Throws(() => new Exception("Test exception"));

        //Assert
        Assert.Throws<Exception>(() => _tagsService.GetAll());
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
        var tagsList =
            SetupTagFixture()
                .CreateMany(random.Next(100))
                .ToList();

        _tagsRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(tagsList);

        //Act
        await _tagsService.GetAllAsync();

        //Assert
        _tagsRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    /// <summary>
    /// Get all async tags.
    /// Should return tags when tags exists.
    /// </summary>
    /// <param name="notEqualCount">The not equal count.</param>
    [Theory]
    [InlineData(0)]
    public async Task GetAllAsync_WhenTagsExists_ShouldReturnTags(int notEqualCount)
    {
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture()
                .CreateMany(random.Next(100))
                .ToList();

        _tagsRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(() => tagsList);

        //Act
        var tags = await _tagsService.GetAllAsync();

        //Assert
        Assert.NotNull(tags);
        Assert.NotEmpty(tags);
        Assert.NotEqual(notEqualCount, tags.ToList().Count);
    }

    /// <summary>
    /// Get all async tags.
    /// Should return nothing when tags does not exist.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_WhenTagsDoesNotExists_ShouldReturnNothing()
    {
        //Arrange
        _tagsRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(() => []);

        //Act
        var tags = await _tagsService.GetAllAsync();

        //Assert
        Assert.Empty(tags);
    }

    /// <summary>
    /// Get all async tags.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_WhenRepositoryExceptionIsThrown_ShouldThrowException()
    {
        //Arrange
        _tagsRepositoryMock.Setup(x => x.GetAllAsync())
            .ThrowsAsync(new Exception("Test exception"));

        //Assert
        await Assert.ThrowsAsync<Exception>(() => _tagsService.GetAllAsync());
    }

    #endregion

    #region Get all function With Specification

    /// <summary>
    /// Verify that function Get All with specification has been called.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag ")]
    public void Verify_FunctionGetAll_WithSpecification_HasBeenCalled(string tagSearch)
    {
        //Arrange
        var random = new Random();

        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
        _tagsRepositoryMock.Setup(x => x.GetAll(specification))
            .Returns(() => tagsList.Where(x => x.Title.Contains(tagSearch)).AsQueryable());

        //Act
        _tagsService.GetAll(specification);

        //Assert
        _tagsRepositoryMock.Verify(x => x.GetAll(specification), Times.Once);
    }

    /// <summary>
    /// Get all tags with specification.
    /// Should return tags with contains specification when tags exists.
    /// </summary>
    /// <param name="notEqualCount">The not equal count.</param>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData(0, "Tag ")]
    public void GetAll_WithContainsSpecification_WhenTagsExists_ShouldReturnTags(int notEqualCount, string tagSearch)
    {
        //Test failed
        //Arrange
        var random = new Random();

        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
        _tagsRepositoryMock.Setup(x => x.GetAll(specification))
            .Returns(() => tagsList.Where(x => x.Title.Contains(tagSearch)).AsQueryable());

        //Act
        var tags = _tagsService.GetAll(specification);

        //Assert
        Assert.NotNull(tags);
        Assert.NotEmpty(tags);
        Assert.NotEqual(notEqualCount, tags.ToList().Count);
    }


    /// <summary>
    /// Get all tags with specification.
    /// Should return tag with equal specification when tags exists.
    /// </summary>
    /// <param name="equalCount">The equal count.</param>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData(1, "Tag 0")]
    public void GetAll_WithEqualsSpecification_WhenTagsExists_ShouldReturnTag(int equalCount, string tagSearch)
    {
        //Arrange
        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(1);

        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.GetAll(specification))
            .Returns(() => tagsList.Where(x => x.Title.Contains(tagSearch)).AsQueryable());

        //Act
        var tags = _tagsService.GetAll(specification);

        //Assert
        Assert.NotNull(tags);
        Assert.NotEmpty(tags);
        Assert.Equal(equalCount, tags.ToList().Count);
    }

    /// <summary>
    /// Get all tags with specification.
    /// Should return nothing with  when tags does not exist.
    /// </summary>
    /// <param name="equalCount">The equal count.</param>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData(0, "Tag -1")]
    public void GetAll_WithEqualsSpecification_WhenTagsExists_ShouldReturnNothing(int equalCount, string tagSearch)
    {
        //Arrange
        var random = new Random();

        var tagsList =
            SetupTagFixture()
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.GetAll(specification))
            .Returns(() => tagsList.Where(x => x.Title.Contains(tagSearch)).AsQueryable());

        //Act
        var tags = _tagsService.GetAll(specification);

        //Assert
        Assert.NotNull(tags);
        Assert.Empty(tags);
        Assert.Equal(equalCount, tags.ToList().Count);
    }

    /// <summary>
    /// Get all tags.
    /// Should return nothing with  when tags does not exist.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag 0")]
    public void GetAll_WithEqualSpecification_WhenTagsDoesNotExists_ShouldReturnNothing(string tagSearch)
    {
        //Arrange
        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.GetAll(specification))
            .Returns(() => new List<Tag>().AsQueryable());

        //Act
        var tags = _tagsService.GetAll();

        //Assert
        Assert.Empty(tags);
    }

    /// <summary>
    /// Get all tags with specification.
    /// Should return tags when specification is empty.
    /// </summary>
    [Theory]
    [InlineData(0, "Tag ")]
    public void GetAll_WithContainsSpecification_WhenSpecificationIsEmpty_ShouldReturnTags(int notEqualCount, string tagTitleSearch)
    {
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture(tagTitleSearch)
                .With(x => x.Title, tagTitleSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(null);
        _tagsRepositoryMock.Setup(x => x.GetAll(It.IsAny<TagSpecification>()))
            .Returns(() => tagsList.Where(x => x.Title.Contains(tagTitleSearch)).AsQueryable());

        //Act
        var tags = _tagsService.GetAll(specification);

        //Assert
        Assert.NotNull(tags);
        Assert.NotEmpty(tags);
        Assert.NotEqual(notEqualCount, tags.ToList().Count);
    }

    /// <summary>
    /// Get all tags with specification.
    /// Should return tags when specification is null.
    /// </summary>
    [Theory]
    [InlineData(0, "Tag ")]
    public void GetAll_WithContainsSpecification_WhenSpecificationIsNull_ShouldReturnTags(int notEqualCount, string tagTitleSearch)
    {
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture(tagTitleSearch)
                .With(x => x.Title, tagTitleSearch)
                .CreateMany(random.Next(100));

        _tagsRepositoryMock.Setup(x => x.GetAll(It.IsAny<TagSpecification>()))
            .Returns(() => tagsList.Where(x => x.Title.Contains(tagTitleSearch)).AsQueryable());

        //Act
        var tags = _tagsService.GetAll(null);

        //Assert
        Assert.NotNull(tags);
        Assert.NotEmpty(tags);
        Assert.NotEqual(notEqualCount, tags.ToList().Count);
    }

    /// <summary>
    /// Get all tags with specification.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Theory]
    [InlineData("Tag 0")]
    public void GetAll_WithEqualsSpecification_WhenRepositoryThrowsException_ShouldThrowException(string search)
    {
        //Arrange
        var specification = new TagSpecification(x => x.Title.Equals(search));
        _tagsRepositoryMock.Setup(x => x.GetAll(It.IsAny<TagSpecification>()))
            .Throws(() => new Exception("Test exception"));

        //Assert
        Assert.Throws<Exception>(() => _tagsService.GetAll(specification));
    }

    #endregion

    #region Get all async function With Specification

    /// <summary>
    /// Verify that function Get All Async with specification has been called.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag ")]
    public async Task Verify_FunctionGetAllAsync_WithSpecification_HasBeenCalled(string tagSearch)
    {
        //Arrange
        var random = new Random();

        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
        _tagsRepositoryMock.Setup(x => x.GetAllAsync(specification))
            .ReturnsAsync(() => tagsList.Where(x => x.Title.Contains(tagSearch)).ToList());

        //Act
        await _tagsService.GetAllAsync(specification);

        //Assert
        _tagsRepositoryMock.Verify(x => x.GetAllAsync(specification), Times.Once);
    }

    /// <summary>
    /// Get all async tags with specification.
    /// Should return tags with contains specification when tags exists.
    /// </summary>
    /// <param name="notEqualCount">The not equal count.</param>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData(0, "Tag ")]
    public async Task GetAllAsync_WithContainsSpecification_WhenTagsExists_ShouldReturnTags(int notEqualCount, string tagSearch)
    {
        //Test failed
        //Arrange
        var random = new Random();

        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
        _tagsRepositoryMock.Setup(x => x.GetAllAsync(specification))
            .ReturnsAsync(() => tagsList.Where(x => x.Title.Contains(tagSearch)).ToList());

        //Act
        var tags = await _tagsService.GetAllAsync(specification);

        //Assert
        Assert.NotNull(tags);
        Assert.NotEmpty(tags);
        Assert.NotEqual(notEqualCount, tags.ToList().Count);
    }

    /// <summary>
    /// Get all async tags with specification.
    /// Should return tag with equal specification when tags exists.
    /// </summary>
    /// <param name="equalCount">The equal count.</param>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData(1, "Tag 0")]
    public async Task GetAllAsync_WithEqualsSpecification_WhenTagsExists_ShouldReturnTag(int equalCount, string tagSearch)
    {
        //Arrange
        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(1);

        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.GetAllAsync(specification))
            .ReturnsAsync(() => tagsList.Where(x => x.Title.Contains(tagSearch)).ToList());

        //Act
        var tags = await _tagsService.GetAllAsync(specification);

        //Assert
        Assert.NotNull(tags);
        Assert.NotEmpty(tags);
        Assert.Equal(equalCount, tags.ToList().Count);
    }

    /// <summary>
    /// Get all async tags with specification.
    /// Should return nothing with  when tags does not exist.
    /// </summary>
    /// <param name="equalCount">The equal count.</param>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData(0, "Tag -1")]
    public async Task GetAllAsync_WithEqualSpecification_WhenTagsExists_ShouldReturnNothing(int equalCount, string tagSearch)
    {
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture()
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.GetAllAsync(specification))
            .ReturnsAsync(() => tagsList.Where(x => x.Title.Contains(tagSearch)).ToList());

        //Act
        var tags = await _tagsService.GetAllAsync(specification);

        //Assert
        Assert.NotNull(tags);
        Assert.Empty(tags);
        Assert.Equal(equalCount, tags.ToList().Count);
    }

    /// <summary>
    /// Get all async tags.
    /// Should return nothing with  when tags does not exist.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag 0")]
    public async Task GetAllAsync_WithEqualSpecification_WhenTagsDoesNotExists_ShouldReturnNothing(string tagSearch)
    {
        //Arrange
        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.GetAllAsync(specification))
            .ReturnsAsync(() => []);

        //Act
        var tags = await _tagsService.GetAllAsync();

        //Assert
        Assert.Null(tags);
    }

    /// <summary>
    /// Get all Async tags with specification.
    /// Should return tags when specification is empty.
    /// </summary>
    [Theory]
    [InlineData(0, "Tag ")]
    public async Task GetAllAsync_WithContainsSpecification_WhenSpecificationIsEmpty_ShouldReturnTags(int notEqualCount, string tagTitleSearch)
    {
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture(tagTitleSearch)
                .With(x => x.Title, tagTitleSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(null);
        _tagsRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<TagSpecification>()))
            .ReturnsAsync(() => tagsList.Where(x => x.Title.Contains(tagTitleSearch)).ToList());

        //Act
        var tags = await _tagsService.GetAllAsync(specification);

        //Assert
        Assert.NotNull(tags);
        Assert.NotEmpty(tags);
        Assert.NotEqual(notEqualCount, tags.ToList().Count);
    }

    /// <summary>
    /// Get all Async tags with specification.
    /// Should return tags when specification is null.
    /// </summary>
    [Theory]
    [InlineData(0, "Tag ")]
    public async Task GetAllAsync_WithContainsSpecification_WhenSpecificationIsNull_ShouldReturnTags(int notEqualCount, string tagTitleSearch)
    {
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture(tagTitleSearch)
                .With(x => x.Title, tagTitleSearch)
                .CreateMany(random.Next(100));

        _tagsRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<TagSpecification>()))
            .ReturnsAsync(() => tagsList.Where(x => x.Title.Contains(tagTitleSearch)).ToList());

        //Act
        var tags = await _tagsService.GetAllAsync(null);

        //Assert
        Assert.NotNull(tags);
        Assert.NotEmpty(tags);
        Assert.NotEqual(notEqualCount, tags.ToList().Count);
    }

    /// <summary>
    /// Get all Async tags with specification.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Theory]
    [InlineData("Tag 0")]
    public async Task GetAllAsync_WithEqualsSpecification_WhenRepositoryThrowsException_ShouldThrowException(string search)
    {
        //Arrange
        var specification = new TagSpecification(x => x.Title.Equals(search));
        _tagsRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<TagSpecification>()))
            .ThrowsAsync(new Exception("Test exception"));

        //Assert
        await Assert.ThrowsAsync<Exception>(() => _tagsService.GetAllAsync(specification));
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
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .With(x => x.Id, tagId)
                .Create();

        _tagsRepositoryMock.Setup(x => x.GetById(tagId))
            .Returns(() => newTag);

        //Act
        _tagsService.Find(tagId);

        //Assert
        _tagsRepositoryMock.Verify(x => x.GetById(tagId), Times.Once);
    }

    /// <summary>
    /// Find tag.
    /// Should return tag when tag exists.
    /// </summary>
    [Fact]
    public void Find_WhenTagExists_ShouldReturnTag()
    {
        //Arrange
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .With(x => x.Id, tagId)
                .Create();

        _tagsRepositoryMock.Setup(x => x.GetById(tagId))
            .Returns(() => newTag);

        //Act
        var tag = _tagsService.Find(tagId);

        //Assert
        Assert.Equal(tagId, tag.Id);
    }

    /// <summary>
    /// Find tag.
    /// Should return nothing when tag does not exist.
    /// </summary>
    [Fact]
    public void Find_WhenTagDoesNotExists_ShouldReturnNothing()
    {
        //Arrange
        var tagId = _fixture.Create<int>();

        _tagsRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Returns(() => null);

        //Act
        var tag = _tagsService.Find(tagId);

        //Assert
        Assert.Null(tag);
    }

    /// <summary>
    /// Find tag.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public void Find_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var tagId = _fixture.Create<int>();
        _tagsRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
            .Throws(() => new Exception("Test exception"));

        //Assert
        Assert.Throws<Exception>(() => _tagsService.Find(tagId));
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
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .With(x => x.Id, tagId)
                .Create();

        _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
            .ReturnsAsync(() => newTag);

        //Act
        await _tagsService.FindAsync(tagId);

        //Assert
        _tagsRepositoryMock.Verify(x => x.GetByIdAsync(tagId), Times.Once);
    }

    /// <summary>
    /// Async find tag.
    /// Should return tag when tag exists.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task FindAsync_WhenTagExists_ShouldReturnTag()
    {
        //Arrange
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .With(x => x.Id, tagId)
                .Create();

        _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
            .ReturnsAsync(() => newTag);

        //Act
        var tag = await _tagsService.FindAsync(tagId);

        //Assert
        Assert.Equal(tagId, tag.Id);
    }

    /// <summary>
    /// Async find tag.
    /// Should return nothing when tag does not exist.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task FindAsync_WhenTagDoesNotExists_ShouldReturnNothing()
    {
        //Arrange
        var tagId = _fixture.Create<int>();

        _tagsRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => null);

        //Act
        var tag = await _tagsService.FindAsync(tagId);

        //Assert
        Assert.Null(tag);
    }

    /// <summary>
    /// Async find tag.
    /// Should throw exception when repository throws exception.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task FindAsync_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var tagId = _fixture.Create<int>();

        _tagsRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception("Test exception"));

        //Assert
        await Assert.ThrowsAsync<Exception>(() => _tagsService.FindAsync(tagId));
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
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .With(x => x.Id, tagId)
                .Create();

        _tagsRepositoryMock.Setup(x => x.Insert(newTag))
            .Callback(() =>
            {
                newTag.Id = tagId;
            });

        //Act
        _tagsService.Insert(newTag);

        //Assert
        _tagsRepositoryMock.Verify(x => x.Insert(newTag), Times.Once);
    }

    /// <summary>
    /// Insert tag.
    /// Should return tag when tag created.
    /// </summary>
    [Fact]
    public void Insert_WhenTagExists_ShouldReturnTag()
    {
        //Arrange
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .With(x => x.Id, tagId)
                .Create();

        _tagsRepositoryMock.Setup(x => x.Insert(newTag))
            .Callback(() =>
            {
                newTag.Id = tagId;
            });

        //Act
        _tagsService.Insert(newTag);

        //Assert
        Assert.NotEqual(0, newTag.Id);
    }

    /// <summary>
    /// Insert tag.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public void Insert_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var newTag = SetupTagFixture().Create();

        _tagsRepositoryMock.Setup(x => x.Insert(It.IsAny<Tag>()))
            .Throws(new Exception("Test exception"));

        //Assert
        Assert.Throws<Exception>(() => _tagsService.Insert(newTag));
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
        var tagId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newTags =
            SetupTagFixture()
                .CreateMany(itemsCount)
                .ToList();

        _tagsRepositoryMock.Setup(x => x.Insert(newTags))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newTags[i].Id = tagId + i;
                }
            });

        //Act
        _tagsService.Insert(newTags);

        //Assert
        _tagsRepositoryMock.Verify(x => x.Insert(newTags), Times.Once);
    }

    /// <summary>
    /// Insert Enumerable tags.
    /// Should return tags when tags created.
    /// </summary>
    [Fact]
    public void InsertEnumerable_WhenTagsExists_ShouldReturnTags()
    {
        //Arrange
        var random = new Random();
        var tagId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newTags =
            SetupTagFixture()
                .CreateMany(itemsCount)
                .ToList();

        _tagsRepositoryMock.Setup(x => x.Insert(newTags))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newTags[i].Id = tagId + i;
                }
            });

        //Act
        _tagsService.Insert(newTags);

        //Assert
        newTags.ForEach(x =>
        {
            Assert.NotEqual(0, x.Id);
        });
    }

    /// <summary>
    /// Insert Enumerable tags.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public void InsertEnumerable_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var random = new Random();
        var itemsCount = random.Next(10);
        var newTags =
            SetupTagFixture()
                .CreateMany(itemsCount)
                .ToList();

        _tagsRepositoryMock.Setup(x => x.Insert(It.IsAny<IEnumerable<Tag>>()))
            .Throws(new Exception("Test exception"));

        //Assert
        Assert.Throws<Exception>(() => _tagsService.Insert(newTags));
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
        var tagId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newTags =
            SetupTagFixture()
                .CreateMany(itemsCount)
                .ToList();

        _tagsRepositoryMock.Setup(x => x.InsertAsync(newTags))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newTags[i].Id = tagId + i;
                }
            });

        //Act
        await _tagsService.InsertAsync(newTags);

        //Assert
        _tagsRepositoryMock.Verify(x => x.InsertAsync(newTags), Times.Once);
    }

    /// <summary>
    /// Insert Async Enumerable tags.
    /// Should return tags when tags created.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task InsertAsyncEnumerable_WhenTagsExists_ShouldReturnTags()
    {
        //Arrange
        var random = new Random();
        var tagId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newTags =
            SetupTagFixture()
                .CreateMany(itemsCount)
                .ToList();

        _tagsRepositoryMock.Setup(x => x.InsertAsync(newTags))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newTags[i].Id = tagId + i;
                }
            });

        //Act
        await _tagsService.InsertAsync(newTags);

        //Assert
        newTags.ForEach(x =>
        {
            Assert.NotEqual(0, x.Id);
        });
    }

    /// <summary>
    /// Async Insert Enumerable tags.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public async Task InsertAsyncEnumerable_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var random = new Random();
        var itemsCount = random.Next(10);
        var newTags =
            SetupTagFixture()
                .CreateMany(itemsCount)
                .ToList();

        _tagsRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<IEnumerable<Tag>>()))
            .ThrowsAsync(new Exception("Test exception"));

        //Assert
        await Assert.ThrowsAsync<Exception>(() => _tagsService.InsertAsync(newTags));
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
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .With(x => x.Id, tagId)
                .Create();

        _tagsRepositoryMock.Setup(x => x.InsertAsync(newTag))
            .Callback(() =>
            {
                newTag.Id = tagId;
            });

        //Act
        await _tagsService.InsertAsync(newTag);

        //Assert
        _tagsRepositoryMock.Verify(x => x.InsertAsync(newTag), Times.Once);
    }

    /// <summary>
    /// Async insert tag.
    /// Should return comment when comment created.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task InsertAsync_ShouldReturnTag_WhenTagExists()
    {
        //Arrange
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .With(x => x.Id, tagId)
                .Create();

        _tagsRepositoryMock.Setup(x => x.InsertAsync(newTag))
            .Callback(() =>
            {
                newTag.Id = tagId;
            });

        //Act
        await _tagsService.InsertAsync(newTag);

        //Assert
        Assert.NotEqual(0, newTag.Id);
    }

    /// <summary>
    /// Async Insert tag.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public async Task InsertAsync_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var newTag = SetupTagFixture().Create();

        _tagsRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<Tag>()))
            .ThrowsAsync(new Exception("Test exception"));

        //Assert
        await Assert.ThrowsAsync<Exception>(() => _tagsService.InsertAsync(newTag));
    }

    #endregion

    #endregion

    #region Update

    #region Upadate function

    /// <summary>
    /// Verify that function Update has been called.
    /// </summary>
    /// <param name="newTagTitle">The new tag title.</param>
    [Theory]
    [InlineData("New Tag")]
    public void Verify_FunctionUpdate_HasBeenCalled(string newTagTitle)
    {
        //Arrange
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .Create();

        _tagsRepositoryMock.Setup(x => x.Insert(newTag))
            .Callback(() =>
            {
                newTag.Id = tagId;
            });
        _tagsRepositoryMock.Setup(x => x.GetById(tagId))
            .Returns(() => newTag);

        //Act
        _tagsService.Insert(newTag);
        var tag = _tagsService.Find(tagId);
        tag.Title = newTagTitle;
        _tagsService.Update(tag);

        //Assert
        _tagsRepositoryMock.Verify(x => x.Update(tag), Times.Once);
    }

    /// <summary>
    /// Update tag.
    /// Should return tag when tag updated.
    /// </summary>
    /// <param name="newTagTitle">The new tag title.</param>
    [Theory]
    [InlineData("New tag title")]
    public void Update_WhenCommentExists_ShouldReturnComment(string newTagTitle)
    {
        //Arrange
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .Create();

        _tagsRepositoryMock.Setup(x => x.Insert(newTag))
            .Callback(() =>
            {
                newTag.Id = tagId;
            });
        _tagsRepositoryMock.Setup(x => x.GetById(tagId))
            .Returns(() => newTag);

        //Act
        _tagsService.Insert(newTag);
        var tag = _tagsService.Find(tagId);
        tag.Title = newTagTitle;
        _tagsService.Update(tag);

        //Assert
        Assert.Equal(newTagTitle, tag.Title);
    }

    /// <summary>
    /// Update tag.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public void Update_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var tag = SetupTagFixture().Create();

        _tagsRepositoryMock.Setup(x => x.Update(It.IsAny<Tag>()))
            .Throws(new Exception("Test exception"));

        //Assert
        Assert.Throws<Exception>(() => _tagsService.Update(tag));
    }

    #endregion

    #region Upadate Enumerable function

    /// <summary>
    /// Verify that function Update Enumerable has been called.
    /// </summary>
    /// <param name="newTagTitle">The new tag title.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("New Tag")]
    public void Verify_FunctionUpdateEnumerable_HasBeenCalled(string newTagTitle)
    {
        //Arrange
        var random = new Random();
        var tagId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newTags =
            SetupTagFixture()
                .CreateMany(itemsCount)
                .ToList();

        _tagsRepositoryMock.Setup(x => x.Insert(newTags))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newTags[i].Id = tagId + i;
                }
            });

        //Act
        _tagsService.Insert(newTags);
        newTags.ForEach(tag =>
        {
            tag.Title = newTagTitle;
        });
        for (var i = 0; i < itemsCount; i++)
        {
            newTags[i].Title = $"{newTagTitle} {i}";
        }
        _tagsService.Update(newTags);

        //Assert
        _tagsRepositoryMock.Verify(x => x.Update(newTags), Times.Once);
    }

    /// <summary>
    /// Update Enumerable tag.
    /// Should return tag when tag updated.
    /// </summary>
    /// <param name="newTagTitle">The new tag title.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("New tag title")]
    public void UpdateEnumerable_WhenCommentExists_ShouldReturnComment(string newTagTitle)
    {
        //Arrange
        var random = new Random();
        var tagId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newTags =
            SetupTagFixture()
                .CreateMany(itemsCount)
                .ToList();

        _tagsRepositoryMock.Setup(x => x.Insert(newTags))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newTags[i].Id = tagId + i;
                }
            });

        //Act
        _tagsService.Insert(newTags);
        for (var i = 0; i < itemsCount; i++)
        {
            newTags[i].Title = $"{newTagTitle} {i}";
        }
        _tagsService.Update(newTags);

        //Assert
        for (var i = 0; i < itemsCount; i++)
        {
            Assert.Equal($"{newTagTitle} {i}", newTags[i].Title);
        }
    }

    /// <summary>
    /// Update Enumerable tags.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public void UpdateEnumerable_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var random = new Random();
        var itemsCount = random.Next(100);
        var tags = SetupTagFixture()
            .CreateMany(itemsCount)
            .ToList();

        _tagsRepositoryMock.Setup(x => x.Update(It.IsAny<IEnumerable<Tag>>()))
            .Throws(new Exception("Test exception"));

        //Assert
        Assert.Throws<Exception>(() => _tagsService.Update(tags));
    }

    #endregion

    #region Update Async function

    /// <summary>
    /// Verify that function Update Async has been called.
    /// Should return tag when tag updated.
    /// </summary>
    /// <param name="newTagTitle">The new tag title.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("New Tag")]
    public async Task Verify_FunctionUpdateAsync_HasBeenCalled(string newTagTitle)
    {
        //Arrange
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .Create();

        _tagsRepositoryMock.Setup(x => x.InsertAsync(newTag))
            .Callback(() =>
            {
                newTag.Id = tagId;
            });
        _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
            .ReturnsAsync(() => newTag);

        //Act
        await _tagsService.InsertAsync(newTag);
        var tag = await _tagsService.FindAsync(tagId);
        tag.Title = newTagTitle;
        await _tagsService.UpdateAsync(tag);

        //Assert
        _tagsRepositoryMock.Verify(x => x.UpdateAsync(tag), Times.Once);
    }

    /// <summary>
    /// Async update tag.
    /// Should return tag when tag updated.
    /// </summary>
    /// <param name="newTagTitle">The new tag title.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("New Tag")]
    public async Task UpdateAsync_WhenCommentExists_ShouldReturnTag(string newTagTitle)
    {
        //Arrange
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .Create();

        _tagsRepositoryMock.Setup(x => x.InsertAsync(newTag))
            .Callback(() =>
            {
                newTag.Id = tagId;
            });
        _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
            .ReturnsAsync(() => newTag);

        //Act
        await _tagsService.InsertAsync(newTag);
        var tag = await _tagsService.FindAsync(tagId);
        tag.Title = newTagTitle;
        await _tagsService.UpdateAsync(tag);

        //Assert
        Assert.Equal(newTagTitle, tag.Title);
    }

    /// <summary>
    /// Async Update tag.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var tag = SetupTagFixture().Create();

        _tagsRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Tag>()))
            .ThrowsAsync(new Exception("Test exception"));

        //Assert
        await Assert.ThrowsAsync<Exception>(() => _tagsService.UpdateAsync(tag));
    }

    #endregion

    #region Upadate Async Enumerable function

    /// <summary>
    /// Verify that function Update Async Enumerable has been called.
    /// </summary>
    /// <param name="newTagTitle">The new tag title.</param>
    [Theory]
    [InlineData("New Tag")]
    public async Task Verify_FunctionUpdateAsyncEnumerable_HasBeenCalled(string newTagTitle)
    {
        //Arrange
        var random = new Random();
        var tagId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newTags =
            SetupTagFixture()
                .CreateMany(itemsCount)
                .ToList();

        _tagsRepositoryMock.Setup(x => x.InsertAsync(newTags))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newTags[i].Id = tagId + i;
                }
            });

        //Act
        await _tagsService.InsertAsync(newTags);
        newTags.ForEach(tag =>
        {
            tag.Title = newTagTitle;
        });
        for (var i = 0; i < itemsCount; i++)
        {
            newTags[i].Title = $"{newTagTitle} {i}";
        }
        await _tagsService.UpdateAsync(newTags);

        //Assert
        _tagsRepositoryMock.Verify(x => x.UpdateAsync(newTags), Times.Once);
    }

    /// <summary>
    /// Update Async Enumerable tag.
    /// Should return tag when tag updated.
    /// </summary>
    /// <param name="newTagTitle">The new tag title.</param>
    [Theory]
    [InlineData("New tag title")]
    public async Task UpdateAsyncEnumerable_WhenCommentExists_ShouldReturnComment(string newTagTitle)
    {
        //Arrange
        var random = new Random();
        var tagId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newTags =
            SetupTagFixture()
                .CreateMany(itemsCount)
                .ToList();

        _tagsRepositoryMock.Setup(x => x.InsertAsync(newTags))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newTags[i].Id = tagId + i;
                }
            });

        //Act
        await _tagsService.InsertAsync(newTags);
        for (var i = 0; i < itemsCount; i++)
        {
            newTags[i].Title = $"{newTagTitle} {i}";
        }
        await _tagsService.UpdateAsync(newTags);

        //Assert
        for (var i = 0; i < itemsCount; i++)
        {
            Assert.Equal($"{newTagTitle} {i}", newTags[i].Title);
        }
    }

    /// <summary>
    /// Update Async Enumerable tags.
    /// Should throw exception when repository throws exception.
    /// </summary>
    [Fact]
    public async Task UpdateAsyncEnumerable_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var random = new Random();
        var itemsCount = random.Next(100);
        var tags = SetupTagFixture()
            .CreateMany(itemsCount)
            .ToList();

        _tagsRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<IEnumerable<Tag>>()))
            .ThrowsAsync(new Exception("Test exception"));

        //Assert
        await Assert.ThrowsAsync<Exception>(() => _tagsService.UpdateAsync(tags));
    }

    #endregion

    #endregion

    #region Delete

    #region Delete By Id function

    /// <summary>
    /// Verify that function Delete By ID has been called.
    /// </summary>
    [Fact]
    public void Verify_FunctionDeleteById_HasBeenCalled()
    {
        //Arrange
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .Create();

        _tagsRepositoryMock.Setup(x => x.GetById(tagId))
            .Returns(() => newTag);

        //Act
        _tagsService.Insert(newTag);
        _tagsService.Find(tagId);
        _tagsService.Delete(tagId);
        _tagsRepositoryMock.Setup(x => x.GetById(tagId))
            .Returns(() => null);
        _tagsService.Find(tagId);

        //Assert
        _tagsRepositoryMock.Verify(x => x.Delete(newTag), Times.Once);
    }

    /// <summary>
    /// Delete By ID tag.
    /// Should return nothing when tag is deleted.
    /// </summary>
    [Fact]
    public void DeleteById_WhenTagsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .Create();

        _tagsRepositoryMock.Setup(x => x.Insert(newTag))
            .Callback(() =>
            {
                newTag.Id = tagId;
            });
        _tagsRepositoryMock.Setup(x => x.GetById(tagId))
            .Returns(() => newTag);

        //Act
        _tagsService.Insert(newTag);
        _tagsService.Find(tagId);
        _tagsService.Delete(tagId);
        _tagsRepositoryMock.Setup(x => x.GetById(tagId))
            .Returns(() => null);
        var deletedTag = _tagsService.Find(tagId);

        //Assert
        Assert.Null(deletedTag);
    }

    /// <summary>
    /// Delete By ID tag.
    /// When repository throws exception should throw exception.
    /// </summary>
    [Fact]
    public void DeleteById_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var tag = SetupTagFixture().Create();
        _tagsRepositoryMock.Setup(x => x.GetById(It.IsAny<object>()))
            .Returns(tag);
        _tagsRepositoryMock.Setup(x => x.Delete(It.IsAny<Tag>()))
            .Throws(new Exception("Repo fail"));

        //Assert
        Assert.Throws<Exception>(() => _tagsService.Delete(tag.Id));
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
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .Create();

        _tagsRepositoryMock.Setup(x => x.GetById(tagId))
            .Returns(() => newTag);

        //Act
        _tagsService.Insert(newTag);
        var tag = _tagsService.Find(tagId);
        _tagsService.Delete(tag);
        _tagsRepositoryMock.Setup(x => x.GetById(tagId))
            .Returns(() => null);
        _tagsService.Find(tagId);

        //Assert
        _tagsRepositoryMock.Verify(x => x.Delete(tag), Times.Once);
    }

    /// <summary>
    /// Delete By Object tag.
    /// Should return nothing when tag is deleted.
    /// </summary>
    [Fact]
    public void DeleteByObject_WhenTagsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .Create();

        _tagsRepositoryMock.Setup(x => x.Insert(newTag))
            .Callback(() =>
            {
                newTag.Id = tagId;
            });
        _tagsRepositoryMock.Setup(x => x.GetById(tagId))
            .Returns(() => newTag);

        //Act
        _tagsService.Insert(newTag);
        var tag = _tagsService.Find(tagId);
        _tagsService.Delete(tag);
        _tagsRepositoryMock.Setup(x => x.GetById(tagId))
            .Returns(() => null);
        var deletedTag = _tagsService.Find(tagId);

        //Assert
        Assert.Null(deletedTag);
    }

    /// <summary>
    /// Delete By Object tag.
    /// When repository throws exception should throw exception.
    /// </summary>
    [Fact]
    public void DeleteByObject_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var tag = SetupTagFixture().Create();
        _tagsRepositoryMock.Setup(x => x.Delete(It.IsAny<Tag>()))
            .Throws(new Exception("Repo fail"));

        //Assert
        Assert.Throws<Exception>(() => _tagsService.Delete(tag));
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
        var tagId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newTags =
            SetupTagFixture()
                .CreateMany(itemsCount)
                .ToList();

        _tagsRepositoryMock.Setup(x => x.Insert(newTags))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newTags[i].Id = tagId + i;
                }
            });

        //Act
        _tagsService.Insert(newTags);
        _tagsService.Delete(newTags);

        //Assert
        _tagsRepositoryMock.Verify(x => x.Delete(newTags), Times.Once);
    }

    /// <summary>
    /// Delete By Enumerable tag.
    /// Should return nothing when tag is deleted.
    /// </summary>
    [Fact]
    public void DeleteByEnumerable_WhenTagsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var random = new Random();
        var tagId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newTags =
            SetupTagFixture()
                .CreateMany(itemsCount)
                .ToList();

        _tagsRepositoryMock.Setup(x => x.Insert(newTags))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newTags[i].Id = tagId + i;
                }
            });
        _tagsRepositoryMock.Setup(x => x.Delete(newTags))
            .Callback(() =>
            {
                newTags = null;
            });

        //Act
        _tagsService.Insert(newTags);
        _tagsService.Delete(newTags);

        //Assert
        Assert.Null(newTags);
    }

    /// <summary>
    /// Delete By Enumerable tags.
    /// When repository throws exception should throw exception.
    /// </summary>
    [Fact]
    public void DeleteByEnumerable_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var random = new Random();
        var itemsCount = random.Next(10);
        var tags = SetupTagFixture()
            .CreateMany(itemsCount)
            .ToList();
        _tagsRepositoryMock.Setup(x => x.Delete(It.IsAny<IEnumerable<Tag>>()))
            .Throws(new Exception("Repo fail"));

        //Assert
        Assert.Throws<Exception>(() => _tagsService.Delete(tags));
    }

    #endregion

    #region Delete By Id Async function

    /// <summary>
    /// Verify that function Delete Async By ID has been called.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task Verify_FunctionDeleteAsyncById_HasBeenCalled()
    {
        //Arrange
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .Create();

        _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
            .ReturnsAsync(() => newTag);

        //Act
        await _tagsService.InsertAsync(newTag);
        await _tagsService.DeleteAsync(tagId);

        //Assert
        _tagsRepositoryMock.Verify(x => x.DeleteAsync(newTag), Times.Once);
    }

    /// <summary>
    /// Async delete by id tag.
    /// Should return nothing when tag is deleted.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task DeleteAsyncById_WhenTagIsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .Create();

        _tagsRepositoryMock.Setup(x => x.InsertAsync(newTag))
            .Callback(() =>
            {
                newTag.Id = tagId;
            });
        _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
            .ReturnsAsync(() => newTag);

        //Act
        await _tagsService.InsertAsync(newTag);
        await _tagsService.DeleteAsync(tagId);
        _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
            .ReturnsAsync(() => null);
        var deletedTag = await _tagsService.FindAsync(tagId);

        //Assert
        Assert.Null(deletedTag);
    }

    /// <summary>
    /// Async delete By ID tag.
    /// When repository throws exception should throw exception.
    /// </summary>
    [Fact]
    public async Task DeleteAsyncById_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var tag = SetupTagFixture().Create();
        _tagsRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<object>()))
            .ReturnsAsync(tag);
        _tagsRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Tag>()))
            .ThrowsAsync(new Exception("Repo fail"));

        //Assert
        await Assert.ThrowsAsync<Exception>(() => _tagsService.DeleteAsync(tag.Id));
    }

    #endregion

    #region Delete By Object Async function

    /// <summary>
    /// Verify that function Delete Async By Object has been called.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task Verify_FunctionDeleteAsyncByObject_HasBeenCalled()
    {
        //Arrange
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .Create();

        _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
            .ReturnsAsync(() => newTag);

        //Act
        await _tagsService.InsertAsync(newTag);
        var comment = await _tagsService.FindAsync(tagId);
        await _tagsService.DeleteAsync(comment);

        //Assert
        _tagsRepositoryMock.Verify(x => x.DeleteAsync(comment), Times.Once);
    }

    /// <summary>
    /// Async delete by object tag.
    /// Should return nothing when tag is deleted.
    /// </summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task DeleteAsyncByObject_WhenTagIsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var tagId = _fixture.Create<int>();
        var newTag =
            SetupTagFixture()
                .Create();

        _tagsRepositoryMock.Setup(x => x.InsertAsync(newTag))
            .Callback(() =>
            {
                newTag.Id = tagId;
            });
        _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
            .ReturnsAsync(() => newTag);

        //Act
        await _tagsService.InsertAsync(newTag);
        var tag = await _tagsService.FindAsync(tagId);
        await _tagsService.DeleteAsync(tag);
        _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
            .ReturnsAsync(() => null);
        var deletedTag = await _tagsService.FindAsync(tagId);

        //Assert
        Assert.Null(deletedTag);
    }

    /// <summary>
    /// Async delete By Object tag.
    /// When repository throws exception should throw exception.
    /// </summary>
    [Fact]
    public async Task DeleteAsyncByObject_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var tag = SetupTagFixture().Create();
        _tagsRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Tag>()))
            .ThrowsAsync(new Exception("Repo fail"));

        //Assert
        await Assert.ThrowsAsync<Exception>(() => _tagsService.DeleteAsync(tag));
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
        var tagId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newTags =
            SetupTagFixture()
                .CreateMany(itemsCount)
                .ToList();

        _tagsRepositoryMock.Setup(x => x.InsertAsync(newTags))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newTags[i].Id = tagId + i;
                }
            });

        //Act
        await _tagsService.InsertAsync(newTags);
        await _tagsService.DeleteAsync(newTags);

        //Assert
        _tagsRepositoryMock.Verify(x => x.DeleteAsync(newTags), Times.Once);
    }

    /// <summary>
    /// Delete Async By Enumerable tag.
    /// Should return nothing when tag is deleted.
    /// </summary>
    [Fact]
    public async Task DeleteAsyncByEnumerable_WhenTagsDeleted_ShouldReturnNothing()
    {
        //Arrange
        var random = new Random();
        var tagId = _fixture.Create<int>();
        var itemsCount = random.Next(10);
        var newTags =
            SetupTagFixture()
                .CreateMany(itemsCount)
                .ToList();

        _tagsRepositoryMock.Setup(x => x.InsertAsync(newTags))
            .Callback(() =>
            {
                for (var i = 0; i < itemsCount; i++)
                {
                    newTags[i].Id = tagId + i;
                }
            });
        _tagsRepositoryMock.Setup(x => x.DeleteAsync(newTags))
            .Callback(() =>
            {
                newTags = null;
            });

        //Act
        await _tagsService.InsertAsync(newTags);
        await _tagsService.DeleteAsync(newTags);

        //Assert
        Assert.Null(newTags);
    }

    /// <summary>
    /// Async delete By Enumerable tags.
    /// When repository throws exception should throw exception.
    /// </summary>
    [Fact]
    public async Task DeleteAsyncByEnumerable_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var random = new Random();
        var itemsCount = random.Next(10);
        var tags = SetupTagFixture()
            .CreateMany(itemsCount)
            .ToList();
        _tagsRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<IEnumerable<Tag>>()))
            .ThrowsAsync(new Exception("Repo fail"));

        //Assert
        await Assert.ThrowsAsync<Exception>(() => _tagsService.DeleteAsync(tags));
    }

    #endregion

    #endregion

    #region Any

    #region Any function With Specification

    /// <summary>
    /// Verify that function Any with specification has been called.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag ")]
    public void Verify_FunctionAny_WithSpecification_HasBeenCalled(string tagSearch)
    {
        //Arrange
        var random = new Random();

        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
        _tagsRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => tagsList.Any(x => x.Title.Contains(tagSearch)));

        //Act
        _tagsService.Any(specification);

        //Assert
        _tagsRepositoryMock.Verify(x => x.Any(specification), Times.Once);
    }

    /// <summary>
    /// Check if there are any tags with specification.
    /// Should return true with contains specification when tags exists.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag ")]
    public void Any_WithContainsSpecification_WhenTagsExists_ShouldReturnTrue(string tagSearch)
    {
        //Test failed
        //Arrange
        var random = new Random();

        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
        _tagsRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => tagsList.Any(x => x.Title.Contains(tagSearch)));

        //Act
        var areAnyTags = _tagsService.Any(specification);

        //Assert
        Assert.True(areAnyTags);
    }

    /// <summary>
    /// Check if there are any tags with specification.
    /// Should return true with equal specification when tags exists.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag 0")]
    public void Any_WithEqualsSpecification_WhenCommentsExists_ShouldReturnTrue(string tagSearch)
    {
        //Arrange
        var random = new Random();

        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => tagsList.Any(x => x.Title.Contains(tagSearch)));

        //Act
        var areAnyTags = _tagsService.Any(specification);

        //Assert
        Assert.True(areAnyTags);
    }

    /// <summary>
    /// Check if there are any tags with specification.
    /// Should return false with when tags does not exist.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag -1")]
    public void Any_WithEqualSpecification_WhenCommentsExists_ShouldReturnFalse(string tagSearch)
    {
        //Arrange
        var random = new Random();

        var tagsList =
            SetupTagFixture()
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => tagsList.Any(x => x.Title.Contains(tagSearch)));

        //Act
        var areAnyTags = _tagsService.Any(specification);

        //Assert
        Assert.False(areAnyTags);
    }

    /// <summary>
    /// Check if there are any tags with specification.
    /// Should return false with when tags does not exist.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag 0")]
    public void Any_WithEqualSpecification_WhenCommentDoesNotExists_ShouldReturnNothing(string tagSearch)
    {
        //Arrange
        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.Any(specification))
            .Returns(() => false);

        //Act
        var areAnyTags = _tagsService.Any(specification);

        //Assert
        Assert.False(areAnyTags);
    }

    /// <summary>
    /// Any.
    /// When repository throws exception should throw exception.
    /// </summary>
    [Fact]
    public void Any_WhenRepositoryThrowsException_ShouldThrowException()
    {
        //Arrange
        var specification = new TagSpecification(x => x.Title.Equals("Tag 0"));
        _tagsRepositoryMock.Setup(r => r.Any(It.IsAny<ISpecification<Tag>>()))
            .Throws(new Exception("DB error"));

        //Assert
        Assert.Throws<Exception>(() => _tagsService.Any(specification));
    }

    #endregion

    #region Any Async function With Specification

    /// <summary>
    /// Verify that function Any Async with specification has been called.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Tag ")]
    public async Task Verify_FunctionAnyAsync_WithSpecification_HasBeenCalled(string tagSearch)
    {
        //Arrange
        var random = new Random();

        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
        _tagsRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => tagsList.Any(x => x.Title.Contains(tagSearch)));

        //Act
        await _tagsService.AnyAsync(specification);

        //Assert
        _tagsRepositoryMock.Verify(x => x.AnyAsync(specification), Times.Once);
    }

    /// <summary>
    /// Async check if there are any tags with specification.
    /// Should return true with contains specification when tags exists.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Tag ")]
    public async Task AnyAsync_WithContainsSpecification_WhenTagsExists_ShouldReturnTrue(string tagSearch)
    {
        //Test failed
        //Arrange
        var random = new Random();

        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
        _tagsRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => tagsList.Any(x => x.Title.Contains(tagSearch)));

        //Act
        var areAnyTags = await _tagsService.AnyAsync(specification);

        //Assert
        Assert.True(areAnyTags);
    }

    /// <summary>
    /// Async check if there are any tags with specification.
    /// Should return true with equal specification when tags exists.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Tag 0")]
    public async Task AnyAsync_WithEqualsSpecification_WhenTagsExists_ShouldReturnTrue(string tagSearch)
    {
        //Arrange
        var random = new Random();

        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => tagsList.Any(x => x.Title.Contains(tagSearch)));

        //Act
        var areAnyTags = await _tagsService.AnyAsync(specification);

        //Assert
        Assert.True(areAnyTags);
    }

    /// <summary>
    /// Async check if there are any tags with specification.
    /// Should return false with when tags does not exist.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Tag -1")]
    public async Task AnyAsync_WithEqualSpecification_WhenTagsExists_ShouldReturnFalse(string tagSearch)
    {
        //Arrange
        var random = new Random();

        var tagsList =
            SetupTagFixture()
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => tagsList.Any(x => x.Title.Contains(tagSearch)));

        //Act
        var areAnyTags = await _tagsService.AnyAsync(specification);

        //Assert
        Assert.False(areAnyTags);
    }

    /// <summary>
    /// Async check if there are any tags with specification.
    /// Should return false with when tags does not exist.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    /// <returns>Task.</returns>
    [Theory]
    [InlineData("Tag 0")]
    public async Task AnyAsync_WithEqualSpecification_WhenTagDoesNotExists_ShouldReturnNothing(string tagSearch)
    {
        //Arrange
        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.AnyAsync(specification))
            .ReturnsAsync(() => false);

        //Act
        var areAnyTags = await _tagsService.AnyAsync(specification);

        //Assert
        Assert.False(areAnyTags);
    }

    #endregion

    #endregion

    #region First Or Default function With Specification

    /// <summary>
    /// Verify that function First Or Default with specification has been called.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag ")]
    public void Verify_FunctionFirstOrDefault_WithSpecification_HasBeenCalled(string tagSearch)
    {
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
        _tagsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => tagsList.FirstOrDefault(x => x.Title.Contains(tagSearch)));

        //Act
        _tagsService.FirstOrDefault(specification);

        //Assert
        _tagsRepositoryMock.Verify(x => x.FirstOrDefault(specification), Times.Once);
    }

    /// <summary>
    /// Get first or default tag with specification.
    /// Should return tag with contains specification when tags exists.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag ")]
    public void FirstOrDefault_WithContainsSpecification_WhenTagsExists_ShouldReturnTag(string tagSearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
        _tagsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => tagsList.FirstOrDefault(x => x.Title.Contains(tagSearch)));

        //Act
        var tag = _tagsService.FirstOrDefault(specification);

        //Assert
        Assert.NotNull(tag);
        Assert.IsType<Tag>(tag);
    }

    /// <summary>
    /// Get first or default tag with specification.
    /// Should return tag with equal specification when tags exists.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag 0")]
    public void FirstOrDefault_WithEqualsSpecification_WhenTagsExists_ShouldReturnTag(string tagSearch)
    {
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => tagsList.FirstOrDefault(x => x.Title.Contains(tagSearch)));

        //Act
        var tag = _tagsService.FirstOrDefault(specification);

        //Assert
        Assert.NotNull(tag);
        Assert.IsType<Tag>(tag);
    }

    /// <summary>
    /// Get first or default tag with specification.
    /// Should return nothing with when tags does not exist.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag -1")]
    public void FirstOrDefault_WithEqualSpecification_WhenTagsExists_ShouldReturnNothing(string tagSearch)
    {
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture()
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => tagsList.FirstOrDefault(x => x.Title.Contains(tagSearch)));

        //Act
        var tag = _tagsService.FirstOrDefault(specification);

        //Assert
        Assert.Null(tag);
    }

    /// <summary>
    /// Get first or default tag with specification.
    /// Should return nothing with when tags does not exist.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag 0")]
    public void FirstOrDefault_WithEqualSpecification_WhenTagsDoesNotExists_ShouldReturnNothing(string tagSearch)
    {
        //Arrange
        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
            .Returns(() => null);

        //Act
        var tag = _tagsService.FirstOrDefault(specification);

        //Assert
        Assert.Null(tag);
    }

    #endregion

    #region Last Or Default function With Specification

    /// <summary>
    /// Verify that function Last Or Default with specification has been called.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag ")]
    public void Verify_FunctionLastOrDefault_WithSpecification_HasBeenCalled(string tagSearch)
    {
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
        _tagsRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => tagsList.LastOrDefault(x => x.Title.Contains(tagSearch)));

        //Act
        _tagsService.LastOrDefault(specification);

        //Assert
        _tagsRepositoryMock.Verify(x => x.LastOrDefault(specification), Times.Once);
    }

    /// <summary>
    /// Get last or default tag with specification.
    /// Should return tag with contains specification when tags exists.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag ")]
    public void LastOrDefault_WithContainsSpecification_WhenTagsExists_ShouldReturnTag(string tagSearch)
    {
        //Test failed
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
        _tagsRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => tagsList.LastOrDefault(x => x.Title.Contains(tagSearch)));

        //Act
        var tag = _tagsService.LastOrDefault(specification);

        //Assert
        Assert.NotNull(tag);
        Assert.IsType<Tag>(tag);
    }

    /// <summary>
    /// Get last or default tag with specification.
    /// Should return tag with equal specification when tags exists.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag 0")]
    public void LastOrDefault_WithEqualsSpecification_WhenTagsExists_ShouldReturnTag(string tagSearch)
    {
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, tagSearch)
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => tagsList.LastOrDefault(x => x.Title.Contains(tagSearch)));

        //Act
        var tag = _tagsService.LastOrDefault(specification);

        //Assert
        Assert.NotNull(tag);
        Assert.IsType<Tag>(tag);
    }

    /// <summary>
    /// Get last or default tag with specification.
    /// Should return nothing with when tags does not exist.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag -1")]
    public void LastOrDefault_WithEqualSpecification_WhenTagsExists_ShouldReturnNothing(string tagSearch)
    {
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture()
                .CreateMany(random.Next(100));

        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => tagsList.LastOrDefault(x => x.Title.Contains(tagSearch)));

        //Act
        var tag = _tagsService.LastOrDefault(specification);

        //Assert
        Assert.Null(tag);
    }

    /// <summary>
    /// Get last or default tag with specification.
    /// Should return nothing with when tags does not exist.
    /// </summary>
    /// <param name="tagSearch">The tag search.</param>
    [Theory]
    [InlineData("Tag 0")]
    public void LastOrDefault_WithEqualSpecification_WhenTagsDoesNotExists_ShouldReturnNothing(string tagSearch)
    {
        //Arrange
        var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
        _tagsRepositoryMock.Setup(x => x.LastOrDefault(specification))
            .Returns(() => null);

        //Act
        var tag = _tagsService.LastOrDefault(specification);

        //Assert
        Assert.Null(tag);
    }

    #endregion

    #region Search async function

    /// <summary>
    /// Search the specified query.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="tagsList">The tags list.</param>
    /// <returns>PagedListResult.</returns>
    protected static PagedListResult<Tag> Search(SearchQuery<Tag> query, List<Tag> tagsList)
    {
        var sequence = tagsList.AsQueryable();

        // Applying filters
        if (query.Filters is { Count: > 0 })
        {
            foreach (var filterClause in query.Filters)
            {
                sequence = sequence.Where(filterClause);
            }
        }

        // Include Properties
        if (!string.IsNullOrWhiteSpace(query.IncludeProperties))
        {
            var properties = query.IncludeProperties.Split([","], StringSplitOptions.RemoveEmptyEntries);

            sequence = properties.Aggregate(sequence, (current, includeProperty) => current.Include(includeProperty));
        }

        // Resolving Sort Criteria
        // This code applies the sorting criteria sent as the parameter
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
            sequence = ((IOrderedQueryable<Tag>)sequence).OrderBy(x => true);
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

        return new PagedListResult<Tag>
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
    [Theory]
    [InlineData("Tag ", 0, 10, "Title", OrderType.Ascending)]
    [InlineData("Tag ", 10, 10, "Title", OrderType.Ascending)]
    [InlineData("Tag ", 10, 20, "Title", OrderType.Ascending)]
    [InlineData("Tag ", 0, 100, "Title", OrderType.Ascending)]
    public async Task Verify_FunctionSearchAsync_HasBeenCalled(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, search)
                .CreateMany(random.Next(100))
                .ToList();

        var query = new SearchQuery<Tag>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Tag>(fieldName, orderType));

        query.AddFilter(x => x.Title.ToUpper().Contains($"{search}".ToUpper()));

        _tagsRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => Search(query, tagsList));

        //Act
        await _tagsService.SearchAsync(query);

        //Assert
        _tagsRepositoryMock.Verify(x => x.SearchAsync(query), Times.Once);
    }

    /// <summary>
    /// Search async tags.
    /// Should return tags when tags exists.
    /// </summary>
    [Theory]
    [InlineData("Tag ", 0, 10, "Title", OrderType.Ascending)]
    [InlineData("Tag ", 10, 10, "Title", OrderType.Ascending)]
    [InlineData("Tag ", 10, 20, "Title", OrderType.Ascending)]
    [InlineData("Tag ", 0, 100, "Title", OrderType.Ascending)]
    public async Task SearchAsync_WhenTagsExists_ShouldReturnTags(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, search)
                .CreateMany(random.Next(100))
                .ToList();

        var query = new SearchQuery<Tag>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Tag>(fieldName, orderType));

        query.AddFilter(x => x.Title.ToUpper().Contains($"{search}".ToUpper()));

        _tagsRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => Search(query, tagsList));

        //Act
        var tags = await _tagsService.SearchAsync(query);

        //Assert
        Assert.NotNull(tags);
        Assert.NotEmpty(tags.Entities);
    }

    /// <summary>
    /// Search async tags with specification.
    /// Should return tag with equal specification when tags exists.
    /// </summary>
    [Theory]
    [InlineData("Tag 0", 0, 10, "Title", OrderType.Ascending)]
    [InlineData("Tag 11", 10, 10, "Title", OrderType.Ascending)]
    [InlineData("Tag 10", 10, 20, "Title", OrderType.Ascending)]
    [InlineData("Tag 1", 0, 100, "Title", OrderType.Ascending)]
    public async Task SearchAsync_WithEqualsSpecification_WhenTagsExists_ShouldReturnTag(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var tagsList =
            SetupTagFixture()
                .With(x => x.Title, search)
                .CreateMany(start + 1)
                .ToArray();


        var query = new SearchQuery<Tag>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Tag>(fieldName, orderType));

        query.AddFilter(x => x.Title.ToUpper().Contains($"{search}".ToUpper()));

        _tagsRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => Search(query, tagsList.ToList()));

        //Act
        var tags = await _tagsService.SearchAsync(query);

        //Assert
        Assert.NotNull(tags);
        Assert.NotEmpty(tags.Entities);
        Assert.Single(tags.Entities);
    }

    /// <summary>
    /// Search async tags with specification.
    /// Should return nothing with  when tags does not exist.
    /// </summary>
    [Theory]
    [InlineData("Tag -0", 0, 10, "Title", OrderType.Ascending)]
    [InlineData("Tag -1", 10, 10, "Title", OrderType.Ascending)]
    [InlineData("Tag -10", 10, 20, "Title", OrderType.Ascending)]
    [InlineData("Tag -90", 0, 100, "Title", OrderType.Ascending)]
    public async Task SearchAsync_WithEqualSpecification_WhenTagsExists_ShouldReturnNothing(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var random = new Random();
        var tagsList =
            SetupTagFixture()
                .CreateMany(random.Next(100))
                .ToList();

        var query = new SearchQuery<Tag>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Tag>(fieldName, orderType));

        query.AddFilter(x => x.Title.ToUpper().Contains($"{search}".ToUpper()));

        _tagsRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => Search(query, tagsList));

        //Act
        var tags = await _tagsService.SearchAsync(query);

        //Assert
        Assert.NotNull(tags);
        Assert.Empty(tags.Entities);
    }

    /// <summary>
    /// Search async tags.
    /// Should return nothing when tags does not exist.
    /// </summary>
    [Theory]
    [InlineData("Comment 0", 0, 10, "CommentBody", OrderType.Ascending)]
    [InlineData("Comment 11", 10, 10, "CommentBody", OrderType.Ascending)]
    [InlineData("Comment 11", 10, 20, "CommentBody", OrderType.Ascending)]
    [InlineData("Comment 11", 0, 100, "CommentBody", OrderType.Ascending)]
    public async Task SearchAsync_WhenTagsDoesNotExists_ShouldReturnNothing(string search, int start, int take, string fieldName, OrderType orderType)
    {
        //Arrange
        var query = new SearchQuery<Tag>
        {
            Skip = start,
            Take = take
        };

        query.AddSortCriteria(new FieldSortOrder<Tag>(fieldName, orderType));

        query.AddFilter(x => x.Title.ToUpper().Contains($"{search}".ToUpper()));

        _tagsRepositoryMock.Setup(x => x.SearchAsync(query))
            .ReturnsAsync(() => new PagedListResult<Tag>());

        //Act
        var tags = await _tagsService.SearchAsync(query);

        //Assert
        Assert.Null(tags.Entities);
    }

    /// <summary>
    /// Search async.
    /// When repository throws exception should throw exception.
    /// </summary>
    [Fact]
    public async Task SearchAsync_WhenRepositoryThrowsException_ShouldThrowException()
    {
        // Arrange
        var query = new SearchQuery<Tag>
        {
            Skip = 0,
            Take = 10
        };

        _tagsRepositoryMock
            .Setup(r => r.SearchAsync(query))
            .ThrowsAsync(new Exception("Database error"));

        // Assert
        await Assert.ThrowsAsync<Exception>(() => _tagsService.SearchAsync(query));
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
        var data = new List<Tag>().AsQueryable();
        var query = new SearchQuery<Tag> { Skip = 0, Take = 5 };
        var expected = new PagedListResult<Tag> { Entities = new List<Tag>(), Count = 0 };

        _tagsRepositoryMock.Setup(r => r.SearchBySequenceAsync(query, data)).ReturnsAsync(expected);

        var result = await _tagsService.SearchBySequenceAsync(query, data);

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
        var data = SetupTagFixture().CreateMany(5).AsQueryable();
        var expected = new PagedListResult<Tag> { Entities = data.ToList(), Count = 5 };

        _tagsRepositoryMock.Setup(r => r.SearchBySequenceAsync(null, data)).ReturnsAsync(expected);

        var result = await _tagsService.SearchBySequenceAsync(null, data);

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
        var query = new SearchQuery<Tag> { Skip = 0, Take = 5 };
        var expected = new PagedListResult<Tag> { Entities = null, Count = 5 };

        _tagsRepositoryMock.Setup(r => r.SearchBySequenceAsync(query, null)).ReturnsAsync(expected);

        var result = await _tagsService.SearchBySequenceAsync(query, null);

        Assert.NotNull(result);
        Assert.Null(result.Entities);
    }

    /// <summary>
    /// Search by sequence async.
    /// When repository throws exception should throw exception.
    /// </summary>
    [Fact]
    public async Task SearchBySequenceAsync_WhenRepositoryThrowsException_ShouldThrowException()
    {
        var data = SetupTagFixture().CreateMany(3).AsQueryable();
        var query = new SearchQuery<Tag> { Skip = 0, Take = 5 };

        _tagsRepositoryMock.Setup(r => r.SearchBySequenceAsync(query, data)).ThrowsAsync(new Exception("DB fail"));

        await Assert.ThrowsAsync<Exception>(() => _tagsService.SearchBySequenceAsync(query, data));
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
        var expected = new SearchQuery<Tag>();

        _tagsRepositoryMock.Setup(r => r.GenerateQuery(tableFilter, null)).Returns(expected);

        var result = _tagsService.GenerateQuery(tableFilter);

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
        var expected = new SearchQuery<Tag>();

        _tagsRepositoryMock.Setup(r => r.GenerateQuery(tableFilter, "Title")).Returns(expected);

        var result = _tagsService.GenerateQuery(tableFilter, "Title");

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
        var expected = new SearchQuery<Tag>();

        _tagsRepositoryMock.Setup(r => r.GenerateQuery(tableFilter, null)).Returns(expected);

        var result = _tagsService.GenerateQuery(null);

        Assert.Null(result);
    }

    #endregion

    #endregion
}