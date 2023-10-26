using Blog.Data.Models;
using Blog.Data.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Specifications.Base;
using Xunit;
using Blog.Core.Infrastructure.Pagination;
using Microsoft.EntityFrameworkCore;
using Blog.Core.Enums;
using Blog.Core.Infrastructure;
using AutoFixture;
using AutoFixture.Dsl;
using Blog.EntityServices;
using Blog.EntityServices.Interfaces;

namespace Blog.ServicesTests.EntityServices;

    /// <summary>
    /// Post tag relations service tests.
    /// </summary>
    public class PostTagRelationsServiceTests
    {
        #region Fields

        /// <summary>
        /// The post tag relations tags relations service.
        /// </summary>
        private readonly IPostsTagsRelationsService _postsTagsRelationsService;

        /// <summary>
        /// The post tag relations tags relations repository mock.
        /// </summary>
        private readonly Mock<IRepository<PostsTagsRelations>> _postsTagsRelationsRepositoryMock;

    /// <summary>
    /// The fixture.
    /// </summary>
    private readonly Fixture _fixture;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="PostTagRelationsServiceTests"/> class.
        /// </summary>
        public PostTagRelationsServiceTests()
        {
            var tagsServiceMock = new Mock<ITagsService>();
            _postsTagsRelationsRepositoryMock = new Mock<IRepository<PostsTagsRelations>>();
            _postsTagsRelationsService = new PostsTagsRelationsService(_postsTagsRelationsRepositoryMock.Object, tagsServiceMock.Object);
        _fixture = new Fixture();
        }

        #endregion

    #region Uthilities

    private IPostprocessComposer<PostsTagsRelations> SetupPostsTagsRelationsFixture(string postTitle = null, string tagTitle = null)
    {
        var postSetup =
            _fixture.Build<Post>()
                .Without(x => x.Author)
                .Without(x => x.Comments)
                .Without(x => x.PostsTagsRelations)
                .Without(x => x.Category);

        var tagSetup = _fixture.Build<Tag>()
            .Without(x => x.PostsTagsRelations);
        if (!string.IsNullOrWhiteSpace(tagTitle))
        {
            tagSetup.Do(x => x.Title = tagTitle);
        }

        if (!string.IsNullOrWhiteSpace(postTitle))
        {
            postSetup
                .With(x => x.Title, postTitle)
                .With(x => x.Content, postTitle)
                .With(x => x.Description, postTitle);
        }

        var tag = tagSetup.Create();
        var post = postSetup.Create();

        //TODO: There is bug in With method.
        if (!string.IsNullOrWhiteSpace(tagTitle))
        {
            tag.Title = tagTitle;
        }
        if (!string.IsNullOrWhiteSpace(postTitle))
        {
            post.Title = postTitle;
            post.Content = postTitle;
            post.Description = postTitle;
        }

        return _fixture.Build<PostsTagsRelations>()
            .With(x => x.Tag, tag)
            .With(x => x.Post, post);
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
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture()
                .CreateMany(random.Next(100));

            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll())
                .Returns(postsTagsRelationsList.AsQueryable());

            //Act
            _postsTagsRelationsService.GetAll();

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        /// <summary>
        /// Get all post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        [Theory]
        [InlineData(0)]
        public void GetAll_WhenPostTagRelationExists_ShouldReturnPostTagRelations(int notEqualCount)
        {
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture()
                .CreateMany(random.Next(100));

            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => postsTagsRelationsList.AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll();

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);
        }

        /// <summary>
        /// Get all post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">The tag title.</param>
        [Theory]
        [InlineData(0, "Post", "Tag")]
        public void GetAll_WhenPostTagRelationExists_ShouldReturnPostTagRelationsWithExistingPostAndTags(int notEqualCount, string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture(postTitle, tagTitle)
                .CreateMany(random.Next(100));

            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => postsTagsRelationsList.AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll().ToList();

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);

            void Action(PostsTagsRelations postsTagsRelation)
            {
                Assert.NotNull(postsTagsRelation.Post);
                Assert.Contains(postTitle, postsTagsRelation.Post.Title);

                Assert.NotNull(postsTagsRelation.Tag);
                Assert.Contains(tagTitle, postsTagsRelation.Tag.Title);
            }

            postsTagsRelations.ForEach(Action);
        }

    //write test for get all with paging
    public void GetAll_WhenPostTagRelationExists_ShouldReturnPostTagRelationsWithExistingPostAndTagsWithPaging()
    {
        //Arrange
        var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture()
                .CreateMany(random.Next(100));

        _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll())
            .Returns(() => postsTagsRelationsList.AsQueryable());

        //Act
        var postsTagsRelations = _postsTagsRelationsService.GetAll();

        //Assert
        Assert.NotNull(postsTagsRelations);
        Assert.NotEmpty(postsTagsRelations);
        Assert.NotEqual(0, postsTagsRelations.ToList().Count);
    }

        /// <summary>
        /// Get all post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">The tag title.</param>
        [Theory]
        [InlineData(0, "Post", "Tag")]
        public void GetAll_WhenPostTagRelationExists_ShouldReturnPostTagRelationsWithExistingPostAndTagsAndShouldContainsTheSameTagsCount(int notEqualCount, string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture()
                .CreateMany(random.Next(100));

            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => postsTagsRelationsList.AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll().ToList();

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);
        Assert.Equal(postsTagsRelationsList.ToList().Count, postsTagsRelations.Count);
        }

        /// <summary>
        /// Get all post tag relations.
        /// Should return nothing when post tag relations does not exists.
        /// </summary>
        [Fact]
        public void GetAll_WhenPostTagRelationsDoesNotExists_ShouldReturnNothing()
        {
            //Arrange
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => new List<PostsTagsRelations>().AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll();

            //Assert
            Assert.Empty(postsTagsRelations);
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
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture()
                .CreateMany(random.Next(100))
                .ToList();

            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(postsTagsRelationsList);

            //Act
            await _postsTagsRelationsService.GetAllAsync();

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        /// <summary>
        /// Get all async post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        [Theory]
        [InlineData(0)]
        public async Task GetAllAsync_WhenPostTagRelationExists_ShouldReturnPostTagRelations(int notEqualCount)
        {
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture()
                .CreateMany(random.Next(100))
                .ToList();

            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => postsTagsRelationsList);

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.GetAllAsync();

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);
        }

        /// <summary>
        /// Get all async post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">The tag title.</param>
        [Theory]
        [InlineData(0, "Post", "Tag")]
    public async Task GetAllAsync_WhenPostTagRelationExists_ShouldReturnPostTagRelationsWithExistingPostAndTags(int notEqualCount, string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture(postTitle, tagTitle)
                .CreateMany(random.Next(100))
                .ToList();

            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => postsTagsRelationsList);

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.GetAllAsync();

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);

            void Action(PostsTagsRelations postsTagsRelation)
            {
                Assert.NotNull(postsTagsRelation.Post);
                Assert.Contains(postTitle, postsTagsRelation.Post.Title);

                Assert.NotNull(postsTagsRelation.Tag);
                Assert.Contains(tagTitle, postsTagsRelation.Tag.Title);
            }

            postsTagsRelations.ToList().ForEach(Action);
        }

        /// <summary>
        /// Get all async post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">The tag title.</param>
        [Theory]
        [InlineData(0, "Post", "Tag")]
        public async Task GetAllAsync_WhenPostTagRelationExists_ShouldReturnPostTagRelationsWithExistingPostAndTagsAndShouldContainsTheSameTagsCount(int notEqualCount, string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture(postTitle, tagTitle)
                .CreateMany(random.Next(100))
                .ToList();

            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => postsTagsRelationsList);

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.GetAllAsync();

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);
        Assert.Equal(postsTagsRelationsList.Count, postsTagsRelations.Count);
        }

        /// <summary>
        /// Get all post tag relations.
        /// Should return nothing when post tag relations does not exists.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_WhenPostTagRelationsDoesNotExists_ShouldReturnNothing()
        {
            //Arrange
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(() => []);

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.GetAllAsync();

            //Assert
            Assert.Empty(postsTagsRelations);
        }

        #endregion

        #region Get all function With Specification

        /// <summary>
        /// Verify that function Get All with specification has been called.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Tag ")]
        public void Verify_FunctionGetAll_WithSpecification_HasBeenCalled(string titleSearch)
        {
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture(_fixture.Create<string>(), titleSearch)
                .CreateMany(random.Next(100))
                .ToList();

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(titleSearch)).AsQueryable());

            //Act
            _postsTagsRelationsService.GetAll(specification);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.GetAll(specification), Times.Once);
        }

        /// <summary>
        /// Get all post tag relations with specification.
        /// Should return post tag relations with contains specification when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData(0, "Created from ServicesTests ")]
        public void GetAll_WithContainsSpecification_WhenPostTagRelationsExists_ShouldReturnPostTagRelations(int notEqualCount, string titleSearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture(_fixture.Create<string>(), titleSearch)
                .CreateMany(random.Next(100))
                .ToList();

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(titleSearch)).AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);
        }

        /// <summary>
        /// Get all post tag relations with specification.
        /// Should return post with equal specification when post tag relations exists.
        /// </summary>
        /// <param name="equalCount">The equal count.</param>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData(1, "Tag 0")]
        public void GetAll_WithEqualsSpecification_WhenPostsExists_ShouldReturnPost(int equalCount, string titleSearch)
        {
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture(_fixture.Create<string>(), titleSearch)
                .CreateMany(random.Next(100))
                .ToList();

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(titleSearch)).AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.Equal(equalCount, postsTagsRelations.ToList().Count);
        }

        /// <summary>
        /// Get all post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">The tag title.</param>
        [Theory]
        [InlineData(0, "Post", "Tag")]
        public void GetAll_WithContainsSpecification_WhenPostTagRelationExists_ShouldReturnPostTagRelationsWithExistingPostAndTags(int notEqualCount, string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture(postTitle, tagTitle)
                .CreateMany(random.Next(100))
                .ToList();

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(tagTitle));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(tagTitle)).AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll(specification).ToList();

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);

            void Action(PostsTagsRelations postsTagsRelation)
            {
                Assert.NotNull(postsTagsRelation.Post);
                Assert.Contains(postTitle, postsTagsRelation.Post.Title);

                Assert.NotNull(postsTagsRelation.Tag);
                Assert.Contains(tagTitle, postsTagsRelation.Tag.Title);
            }

            postsTagsRelations.ForEach(Action);
        }

        /// <summary>
        /// Get all post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">The tag title.</param>
        [Theory]
        [InlineData(0, "Post", "Tag")]
        public void GetAll_WithContainsSpecification_WhenPostTagRelationsExists_ShouldReturnPostTagRelationsWithExistingPostAndTagsAndShouldContainsTheSameTagsCount(int notEqualCount, string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture(postTitle, tagTitle)
                .CreateMany(random.Next(100))
                .ToList();

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(tagTitle));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(tagTitle)).AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll(specification).ToList();

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);
        Assert.Equal(postsTagsRelations.Count, postsTagsRelations.Count);
        }

        /// <summary>
        /// Get all post tag relations.
        /// Should return nothing with  when post tag relations does not exists.
        /// </summary>
        /// <param name="equalCount">The equal count.</param>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData(0, "Tag -1")]
        public void GetAll_WithEqualSpecification_WhenPostTagRelationsExists_ShouldReturnNothing(int equalCount, string titleSearch)
        {
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture()
                .CreateMany(random.Next(100))
                .ToList();

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(titleSearch)).AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.Empty(postsTagsRelations);
            Assert.Equal(equalCount, postsTagsRelations.ToList().Count);
        }

        /// <summary>
        /// Get all post tag relations.
        /// Should return nothing with  when post tag relations does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests 0")]
        public void GetAll_WithEqualSpecification_WhenPostTagRelationsDoesNotExists_ShouldReturnNothing(string titleSearch)
        {
            //Arrange
            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => new List<PostsTagsRelations>().AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll();

            //Assert
            Assert.Empty(postsTagsRelations);
        }

        #endregion

        #region Get all async function With Specification

        /// <summary>
        /// Verify that function Get All Async with specification has been called.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Tag ")]
        public async Task Verify_FunctionGetAllAsync_WithSpecification_HasBeenCalled(string titleSearch)
        {
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture(_fixture.Create<string>(), titleSearch)
                .CreateMany(random.Next(100))
                .ToList();

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAllAsync(specification))
                .ReturnsAsync(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(titleSearch)).ToList());

            //Act
            await _postsTagsRelationsService.GetAllAsync(specification);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.GetAllAsync(specification), Times.Once);
        }

        /// <summary>
        /// Get all async post tag relations with specification.
        /// Should return post tag relations with contains specification when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData(0, "Created from ServicesTests ")]
    public async Task GetAllAsync_WithContainsSpecification_WhenPostTagRelationsExists_ShouldReturnPostTagRelations(int notEqualCount, string titleSearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture(_fixture.Create<string>(), titleSearch)
                .CreateMany(random.Next(100))
                .ToList();

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAllAsync(specification))
                .ReturnsAsync(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(titleSearch)).ToList());

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.GetAllAsync(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);
        }

        /// <summary>
        /// Get all async post tag relations with specification.
        /// Should return post with equal specification when post tag relations exists.
        /// </summary>
        /// <param name="equalCount">The equal count.</param>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData(1, "Tag 0")]
    public async Task GetAllAsync_WithEqualsSpecification_WhenPostsExists_ShouldReturnPost(int equalCount, string titleSearch)
        {
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture(_fixture.Create<string>(), titleSearch)
                .CreateMany(random.Next(100))
                .ToList();

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAllAsync(specification))
                .ReturnsAsync(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(titleSearch)).ToList());

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.GetAllAsync(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.Equal(equalCount, postsTagsRelations.ToList().Count);
        }

        /// <summary>
        /// Get all async post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">The tag title.</param>
        [Theory]
        [InlineData(0, "Post", "Tag")]
    public async Task GetAllAsync_WithContainsSpecification_WhenPostTagRelationExists_ShouldReturnPostTagRelationsWithExistingPostAndTags(int notEqualCount, string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture()
                .CreateMany(random.Next(100))
                .ToList();

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(tagTitle));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAllAsync(specification))
                .ReturnsAsync(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(tagTitle)).ToList());

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.GetAllAsync(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);

            void Action(PostsTagsRelations postsTagsRelation)
            {
                Assert.NotNull(postsTagsRelation.Post);
                Assert.Contains(postTitle, postsTagsRelation.Post.Title);

                Assert.NotNull(postsTagsRelation.Tag);
                Assert.Contains(tagTitle, postsTagsRelation.Tag.Title);
            }

            postsTagsRelations.ToList().ForEach(Action);
        }

        /// <summary>
        /// Get all async post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">The tag title.</param>
        [Theory]
        [InlineData(0, "Post", "Tag")]
        public async Task GetAllAsync_WithContainsSpecification_WhenPostTagRelationsExists_ShouldReturnPostTagRelationsWithExistingPostAndTagsAndShouldContainsTheSameTagsCount(int notEqualCount, string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture(_fixture.Create<string>(), postTitle)
                .CreateMany(random.Next(100))
                .ToList();

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(tagTitle));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAllAsync(specification))
                .ReturnsAsync(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(tagTitle)).ToList());

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.GetAllAsync(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);
        Assert.Equal(postsTagsRelations.Count, postsTagsRelations.Count);
        }

        /// <summary>
        /// Get all async post tag relations.
        /// Should return nothing with  when post tag relations does not exists.
        /// </summary>
        /// <param name="equalCount">The equal count.</param>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData(0, "Tag -1")]
    public async Task GetAllAsync_WithEqualSpecification_WhenPostTagRelationsExists_ShouldReturnNothing(int equalCount, string titleSearch)
        {
            //Arrange
            var random = new Random();
        var postsTagsRelationsList =
            SetupPostsTagsRelationsFixture()
                .CreateMany(random.Next(100))
                .ToList();

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAllAsync(specification))
                .ReturnsAsync(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(titleSearch)).ToList());

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.GetAllAsync(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.Empty(postsTagsRelations);
            Assert.Equal(equalCount, postsTagsRelations.ToList().Count);
        }

        /// <summary>
        /// Get all async post tag relations.
        /// Should return nothing with  when post tag relations does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests 0")]
        public async Task GetAllAsync_WithEqualSpecification_WhenPostTagRelationsDoesNotExists_ShouldReturnNothing(string titleSearch)
        {
            //Arrange
            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAllAsync(specification))
            .ReturnsAsync(() => []);

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.GetAllAsync();

            //Assert
            Assert.Null(postsTagsRelations);
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
        var id = _fixture.Create<int>();
        var newPostsTagsRelation =
            SetupPostsTagsRelationsFixture()
                .With(x => x.Id, id)
                .Create();

            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            _postsTagsRelationsService.Find(id);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.GetById(id), Times.Once);
        }

        /// <summary>
        /// Find post tag relation.
        /// Should return post when post exists.
        /// </summary>
        [Fact]
        public void Find_WhenPostTagRelationExists_ShouldReturnPostTagRelation()
        {
            //Arrange
        var id = _fixture.Create<int>();
        var newPostsTagsRelation =
            SetupPostsTagsRelationsFixture()
                .With(x => x.Id, id)
                .Create();

            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            var postsTagsRelations = _postsTagsRelationsService.Find(id);

            //Assert
            Assert.Equal(id, postsTagsRelations.Id);
        }

        /// <summary>
        /// Find post tag relation.
        /// Should return nothing when post does not exists.
        /// </summary>
        [Fact]
        public void Find_WhenPostTagRelationDoesNotExists_ShouldReturnNothing()
        {
            //Arrange
        var id = _fixture.Create<int>();
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(() => null);

            //Act
            var postsTagsRelations = _postsTagsRelationsService.Find(id);

            //Assert
            Assert.Null(postsTagsRelations);
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
        var id = _fixture.Create<int>();
        var newPostsTagsRelation =
            SetupPostsTagsRelationsFixture()
                .With(x => x.Id, id)
                .Create();

            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => newPostsTagsRelation);

            //Act
            await _postsTagsRelationsService.FindAsync(id);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        /// <summary>
        /// Async find post tag relation.
        /// Should return post when post exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task FindAsync_WhenPostTagRelationExists_ShouldReturnPostTagRelation()
        {
            //Arrange
        var id = _fixture.Create<int>();
        var newPostsTagsRelation =
            SetupPostsTagsRelationsFixture()
                .With(x => x.Id, id)
                .Create();

            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => newPostsTagsRelation);

            //Act
            var post = await _postsTagsRelationsService.FindAsync(id);

            //Assert
            Assert.Equal(id, post.Id);
        }

        /// <summary>
        /// Async find post tag relation.
        /// Should return nothing when post does not exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task FindAsync_WhenPostTagRelationsDoesNotExists_ShouldReturnNothing()
        {
            //Arrange
        var id = _fixture.Create<int>();

            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.FindAsync(id);

            //Assert
            Assert.Null(postsTagsRelations);
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
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.Insert(newPostsTagsRelation), Times.Once);
        }

        /// <summary>
        /// Insert post tag relation.
        /// Should return post when post created.
        /// </summary>
        [Fact]
        public void Insert_WhenPostExists_ShouldReturnPostTagRelation()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);

            //Assert
            Assert.NotEqual(0, newPostsTagsRelation.Id);
        }

        /// <summary>
        /// Insert post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">THe tag title.</param>
        [Theory]
        [InlineData("Post", "Tag")]
        public void Insert_WhenPostTagRelationExists_ShouldReturnPostTagRelationWithExistingPostAndTags(string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var postEntity = new Post
            {
                Title = $"{postTitle} {id}",
                Description = $"{postTitle} {id}",
                Content = $"{postTitle} {id}",
                ImageUrl = $"{postTitle} {id}",
            };
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                Id = id,
                PostId = id,
                Post = postEntity,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);

            //Assert
            Assert.NotEqual(0, newPostsTagsRelation.Id);
            Assert.NotNull(newPostsTagsRelation.Post);
            Assert.Contains(postTitle, newPostsTagsRelation.Post.Title);

            Assert.NotNull(newPostsTagsRelation.Tag);
            Assert.Contains(tagTitle, newPostsTagsRelation.Tag.Title);
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
            var id = random.Next(52);
            var itemsCount = random.Next(10);
            var newPostsTagsRelations = new List<PostsTagsRelations>();


            for (int i = 0; i < itemsCount; i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                newPostsTagsRelations.Add(new PostsTagsRelations
                {
                    PostId = id,
                    TagId = tag.Id,
                    Tag = tag
                });
            }

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelations))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newPostsTagsRelations[i].Id = id + i;
                    }
                });

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelations);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.Insert(newPostsTagsRelations), Times.Once);
        }

        /// <summary>
        /// Insert Enumerable post tag relations.
        /// Should return post tag relations when post tag relations created.
        /// </summary>
        [Fact]
        public void InsertEnumerable_WhenPostTagRelationsExists_ShouldReturnPostTagRelations()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var itemsCount = random.Next(10);
            var newPostsTagsRelations = new List<PostsTagsRelations>();


            for (int i = 0; i < itemsCount; i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                newPostsTagsRelations.Add(new PostsTagsRelations
                {
                    PostId = id,
                    TagId = tag.Id,
                    Tag = tag
                });
            }

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelations))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newPostsTagsRelations[i].Id = id + i;
                    }
                });

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelations);

            //Assert
            newPostsTagsRelations.ForEach(x =>
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
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelation);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.InsertAsync(newPostsTagsRelation), Times.Once);
        }

        /// <summary>
        /// Async insert post tag relations.
        /// Should return post when post created.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task InsertAsync_WhenPostExists_ShouldReturnPost()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelation);

            //Assert
            Assert.NotEqual(0, newPostsTagsRelation.Id);
        }

        /// <summary>
        /// Async insert post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">THe tag title.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Post", "Tag")]
        public async Task InsertAsync_WhenPostTagRelationExists_ShouldReturnPostTagRelationWithExistingPostAndTags(string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var postEntity = new Post
            {
                Id = id,
                Title = $"{postTitle} {id}",
                Description = $"{postTitle} {id}",
                Content = $"{postTitle} {id}",
                ImageUrl = $"{postTitle} {id}",
            };
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                Id = id,
                PostId = id,
                Post = postEntity,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelation);

            //Assert
            Assert.NotEqual(0, newPostsTagsRelation.Id);
            Assert.NotNull(newPostsTagsRelation.Post);
            Assert.Contains(postTitle, newPostsTagsRelation.Post.Title);

            Assert.NotNull(newPostsTagsRelation.Tag);
            Assert.Contains(tagTitle, newPostsTagsRelation.Tag.Title);
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
            var id = random.Next(52);
            var itemsCount = random.Next(10);
            var newPostsTagsRelations = new List<PostsTagsRelations>();

            for (int i = 0; i < itemsCount; i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                newPostsTagsRelations.Add(new PostsTagsRelations
                {
                    PostId = id,
                    TagId = tag.Id,
                    Tag = tag
                });
            }

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelations))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newPostsTagsRelations[i].Id = id + i;
                    }
                });

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelations);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.InsertAsync(newPostsTagsRelations), Times.Once);
        }

        /// <summary>
        /// Insert Async Enumerable post tag relations.
        /// Should return post tag relations when post tag relations created.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task InsertAsyncEnumerable_WhenPostTagRelationsExists_ShouldReturnPostTagRelations()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var itemsCount = random.Next(10);
            var newPostsTagsRelations = new List<PostsTagsRelations>();

            for (int i = 0; i < itemsCount; i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                newPostsTagsRelations.Add(new PostsTagsRelations
                {
                    PostId = id,
                    TagId = tag.Id,
                    Tag = tag
                });
            }

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelations))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newPostsTagsRelations[i].Id = id + i;
                    }
                });

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelations);

            //Assert
            newPostsTagsRelations.ForEach(x =>
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
        [Fact]
        public void Verify_FunctionUpdate_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };

            var newTag = new Tag
            {
                Id = id + 1,
                Title = $"Tag {id + 1}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);
            var postsTagsRelations = _postsTagsRelationsService.Find(id);
            postsTagsRelations.TagId = newTag.Id;
            postsTagsRelations.Tag = newTag;
            _postsTagsRelationsService.Update(postsTagsRelations);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.Update(postsTagsRelations), Times.Once);
        }

        /// <summary>
        /// Update  post tag relation.
        /// Should return post tag relation when post tag relation updated.
        /// </summary>
        [Fact]
        public void Update_WhenPostTagRelationsExists_ShouldReturnPostTagRelation()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };

            var newTag = new Tag
            {
                Id = id + 1,
                Title = $"Tag {id + 1}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);
            var postsTagsRelations = _postsTagsRelationsService.Find(id);
            postsTagsRelations.TagId = newTag.Id;
            postsTagsRelations.Tag = newTag;
            _postsTagsRelationsService.Update(newPostsTagsRelation);

            //Assert
            Assert.Equal(postsTagsRelations.TagId, newTag.Id);
            Assert.Equal(postsTagsRelations.Tag.Title, newTag.Title);
        }

        /// <summary>
        /// Insert post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">THe tag title.</param>
        [Theory]
        [InlineData("Post", "Tag")]
        public void Update_WhenPostTagRelationExists_ShouldReturnPostTagRelationWithExistingPostAndTags(string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var postEntity = new Post
            {
                Title = $"{postTitle} {id}",
                Description = $"{postTitle} {id}",
                Content = $"{postTitle} {id}",
                ImageUrl = $"{postTitle} {id}",
            };
            var tag = new Tag
            {
                Id = id,
                Title = $"{tagTitle} {id}"
            };
            var newTag = new Tag
            {
                Id = id + 1,
                Title = $"{tagTitle} {id + 1}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                Id = id,
                PostId = id,
                Post = postEntity,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);
            var postsTagsRelations = _postsTagsRelationsService.Find(id);
            postsTagsRelations.TagId = newTag.Id;
            postsTagsRelations.Tag = newTag;
            _postsTagsRelationsService.Update(newPostsTagsRelation);

            //Assert
            Assert.NotEqual(0, postsTagsRelations.Id);
            Assert.NotNull(newPostsTagsRelation.Post);
            Assert.Contains(postTitle, newPostsTagsRelation.Post.Title);

            Assert.NotNull(newPostsTagsRelation.Tag);
            Assert.Contains(tagTitle, newPostsTagsRelation.Tag.Title);
            Assert.Equal(postsTagsRelations.TagId, newTag.Id);
            Assert.Equal(postsTagsRelations.Tag.Title, newTag.Title);
        }

        #endregion

        #region Upadate Enumerable function

        /// <summary>
        /// Verify that function Update Enumerable has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionUpdateEnumerable_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var itemsCount = random.Next(10);
            var newPostsTagsRelations = new List<PostsTagsRelations>();
            var newTag = new Tag
            {
                Id = id + 1,
                Title = $"Tag {id + 1}"
            };

            for (int i = 0; i < itemsCount; i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                newPostsTagsRelations.Add(new PostsTagsRelations
                {
                    PostId = id,
                    TagId = tag.Id,
                    Tag = tag
                });
            }

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelations))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newPostsTagsRelations[i].Id = id + i;
                    }
                });

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelations);
            newPostsTagsRelations.ForEach(postsTagsRelations =>
            {
                postsTagsRelations.TagId = newTag.Id;
                postsTagsRelations.Tag = newTag;
            });
            _postsTagsRelationsService.Update(newPostsTagsRelations);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.Update(newPostsTagsRelations), Times.Once);
        }

        /// <summary>
        /// Update Enumerable post tag relation.
        /// Should return post tag relation when post tag relation updated.
        /// </summary>
        [Fact]
        public void UpdateEnumerable_WhenPostTagRelationsExists_ShouldReturnPostTagRelation()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var itemsCount = random.Next(10);
            var newPostsTagsRelations = new List<PostsTagsRelations>();
            var newTag = new Tag
            {
                Id = id + 1,
                Title = $"Tag {id + 1}"
            };

            for (int i = 0; i < itemsCount; i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                newPostsTagsRelations.Add(new PostsTagsRelations
                {
                    PostId = id,
                    TagId = tag.Id,
                    Tag = tag
                });
            }

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelations))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newPostsTagsRelations[i].Id = id + i;
                    }
                });

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelations);
            newPostsTagsRelations.ForEach(postsTagsRelations =>
            {
                postsTagsRelations.TagId = newTag.Id;
                postsTagsRelations.Tag = newTag;
            });
            _postsTagsRelationsService.Update(newPostsTagsRelations);

            //Assert
            newPostsTagsRelations.ForEach(postsTagsRelations =>
            {
                Assert.Equal(postsTagsRelations.TagId, newTag.Id);
                Assert.Equal(postsTagsRelations.Tag.Title, newTag.Title);
            });
        }

        /// <summary>
        /// Update Enumerable post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">THe tag title.</param>
        [Theory]
        [InlineData("Post", "Tag")]
        public void UpdateEnumerable_WhenPostTagRelationExists_ShouldReturnPostTagRelationWithExistingPostAndTags(string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var itemsCount = random.Next(10);
            var newPostsTagsRelations = new List<PostsTagsRelations>();
            var newTag = new Tag
            {
                Id = id + 1,
                Title = $"Tag {id + 1}"
            };

            for (int i = 0; i < itemsCount; i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                newPostsTagsRelations.Add(new PostsTagsRelations
                {
                    PostId = id,
                    TagId = tag.Id,
                    Tag = tag
                });
            }

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelations))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newPostsTagsRelations[i].Id = id + i;
                    }
                });

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelations);
            newPostsTagsRelations.ForEach(postsTagsRelations =>
            {
                postsTagsRelations.TagId = newTag.Id;
                postsTagsRelations.Tag = newTag;
            });
            _postsTagsRelationsService.Update(newPostsTagsRelations);

            //Assert
            newPostsTagsRelations.ForEach(postsTagsRelations =>
            {
                Assert.NotEqual(0, postsTagsRelations.Id);

                Assert.NotNull(postsTagsRelations.Tag);
                Assert.Contains(tagTitle, postsTagsRelations.Tag.Title);
                Assert.Equal(postsTagsRelations.TagId, newTag.Id);
                Assert.Equal(postsTagsRelations.Tag.Title, newTag.Title);
            });
        }

        #endregion

        #region Update Async function

        /// <summary>
        /// Verify that function Update Async has been called.
        /// Should return post tag relation when post tag relation updated.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task Verify_FunctionUpdateAsync_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };

            var newTag = new Tag
            {
                Id = id + 1,
                Title = $"Tag {id + 1}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => newPostsTagsRelation);

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelation);
            var postsTagsRelations = await _postsTagsRelationsService.FindAsync(id);
            postsTagsRelations.TagId = newTag.Id;
            postsTagsRelations.Tag = newTag;
            await _postsTagsRelationsService.UpdateAsync(postsTagsRelations);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.UpdateAsync(postsTagsRelations), Times.Once);
        }

        /// <summary>
        /// Async update post tag relation.
        /// Should return post tag relation when post tag relation updated.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task UpdateAsync_WhenPostTagRelationExists_ShouldReturnPostTagRelation()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };

            var newTag = new Tag
            {
                Id = id + 1,
                Title = $"Tag {id + 1}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => newPostsTagsRelation);

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelation);
            var postsTagsRelations = await _postsTagsRelationsService.FindAsync(id);
            postsTagsRelations.TagId = newTag.Id;
            postsTagsRelations.Tag = newTag;
            await _postsTagsRelationsService.UpdateAsync(newPostsTagsRelation);

            //Assert
            Assert.Equal(postsTagsRelations.TagId, newTag.Id);
            Assert.Equal(postsTagsRelations.Tag.Title, newTag.Title);
        }

        /// <summary>
        /// Update async post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">THe tag title.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Post", "Tag")]
        public async Task UpdateAsync_WhenPostTagRelationExists_ShouldReturnPostTagRelationWithExistingPostAndTags(string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var postEntity = new Post
            {
                Title = $"{postTitle} {id}",
                Description = $"{postTitle} {id}",
                Content = $"{postTitle} {id}",
                ImageUrl = $"{postTitle} {id}",
            };
            var tag = new Tag
            {
                Id = id,
                Title = $"{tagTitle} {id}"
            };
            var newTag = new Tag
            {
                Id = id + 1,
                Title = $"{tagTitle} {id + 1}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                Id = id,
                PostId = id,
                Post = postEntity,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => newPostsTagsRelation);

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelation);
            var postsTagsRelations = await _postsTagsRelationsService.FindAsync(id);
            postsTagsRelations.TagId = newTag.Id;
            postsTagsRelations.Tag = newTag;
            await _postsTagsRelationsService.UpdateAsync(newPostsTagsRelation);

            //Assert
            Assert.NotEqual(0, postsTagsRelations.Id);
            Assert.NotNull(newPostsTagsRelation.Post);
            Assert.Contains(postTitle, newPostsTagsRelation.Post.Title);

            Assert.NotNull(newPostsTagsRelation.Tag);
            Assert.Contains(tagTitle, newPostsTagsRelation.Tag.Title);
            Assert.Equal(postsTagsRelations.TagId, newTag.Id);
            Assert.Equal(postsTagsRelations.Tag.Title, newTag.Title);
        }

        #endregion

        #region Upadate Async Enumerable function

        /// <summary>
        /// Verify that function Update Async Enumerable has been called.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task Verify_FunctionUpdateAsyncEnumerable_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var itemsCount = random.Next(10);
            var newPostsTagsRelations = new List<PostsTagsRelations>();
            var newTag = new Tag
            {
                Id = id + 1,
                Title = $"Tag {id + 1}"
            };

            for (int i = 0; i < itemsCount; i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                newPostsTagsRelations.Add(new PostsTagsRelations
                {
                    PostId = id,
                    TagId = tag.Id,
                    Tag = tag
                });
            }

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelations))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newPostsTagsRelations[i].Id = id + i;
                    }
                });

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelations);
            newPostsTagsRelations.ForEach(postsTagsRelations =>
            {
                postsTagsRelations.TagId = newTag.Id;
                postsTagsRelations.Tag = newTag;
            });
            await _postsTagsRelationsService.UpdateAsync(newPostsTagsRelations);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.UpdateAsync(newPostsTagsRelations), Times.Once);
        }

        /// <summary>
        /// Update Async Enumerable post tag relation.
        /// Should return post tag relation when post tag relation updated.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task UpdateAsyncEnumerable_WhenPostTagRelationsExists_ShouldReturnPostTagRelation()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var itemsCount = random.Next(10);
            var newPostsTagsRelations = new List<PostsTagsRelations>();
            var newTag = new Tag
            {
                Id = id + 1,
                Title = $"Tag {id + 1}"
            };

            for (int i = 0; i < itemsCount; i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                newPostsTagsRelations.Add(new PostsTagsRelations
                {
                    PostId = id,
                    TagId = tag.Id,
                    Tag = tag
                });
            }

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelations))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newPostsTagsRelations[i].Id = id + i;
                    }
                });

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelations);
            newPostsTagsRelations.ForEach(postsTagsRelations =>
            {
                postsTagsRelations.TagId = newTag.Id;
                postsTagsRelations.Tag = newTag;
            });
            await _postsTagsRelationsService.UpdateAsync(newPostsTagsRelations);

            //Assert
            newPostsTagsRelations.ForEach(postsTagsRelations =>
            {
                Assert.Equal(postsTagsRelations.TagId, newTag.Id);
                Assert.Equal(postsTagsRelations.Tag.Title, newTag.Title);
            });
        }

        /// <summary>
        /// Update Async Enumerable post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">THe tag title.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Post", "Tag")]
        public async Task UpdateAsyncEnumerable_WhenPostTagRelationExists_ShouldReturnPostTagRelationWithExistingPostAndTags(string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var itemsCount = random.Next(10);
            var newPostsTagsRelations = new List<PostsTagsRelations>();
            var newTag = new Tag
            {
                Id = id + 1,
                Title = $"Tag {id + 1}"
            };

            for (int i = 0; i < itemsCount; i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                newPostsTagsRelations.Add(new PostsTagsRelations
                {
                    PostId = id,
                    TagId = tag.Id,
                    Tag = tag
                });
            }

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelations))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newPostsTagsRelations[i].Id = id + i;
                    }
                });

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelations);
            newPostsTagsRelations.ForEach(postsTagsRelations =>
            {
                postsTagsRelations.TagId = newTag.Id;
                postsTagsRelations.Tag = newTag;
            });
            await _postsTagsRelationsService.UpdateAsync(newPostsTagsRelations);

            //Assert
            newPostsTagsRelations.ForEach(postsTagsRelations =>
            {
                Assert.NotEqual(0, postsTagsRelations.Id);

                Assert.NotNull(postsTagsRelations.Tag);
                Assert.Contains(tagTitle, postsTagsRelations.Tag.Title);
                Assert.Equal(postsTagsRelations.TagId, newTag.Id);
                Assert.Equal(postsTagsRelations.Tag.Title, newTag.Title);
            });
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
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };
            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);
            var postsTagsRelations = _postsTagsRelationsService.Find(id);
            _postsTagsRelationsService.Delete(id);
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => null);
            _postsTagsRelationsService.Find(id);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.Delete(newPostsTagsRelation), Times.Once);
        }

        /// <summary>
        /// Delete By Id post tag relation.
        /// Should return nothing when post tag relation is deleted.
        /// </summary>
        [Fact]
        public void DeleteById_WhenPostTagRelationIsDeleted_ShouldReturnNothing()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };
            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);
            var postsTagsRelations = _postsTagsRelationsService.Find(id);
            _postsTagsRelationsService.Delete(id);
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => null);
            var postsTagsRelation = _postsTagsRelationsService.Find(id);

            //Assert
            Assert.Null(postsTagsRelation);
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
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };
            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);
            var postsTagsRelations = _postsTagsRelationsService.Find(id);
            _postsTagsRelationsService.Delete(postsTagsRelations);
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => null);
            _postsTagsRelationsService.Find(id);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.Delete(postsTagsRelations), Times.Once);
        }

        /// <summary>
        /// Delete By Object post tag relation.
        /// Should return nothing when post tag relation is deleted.
        /// </summary>
        [Fact]
        public void DeleteByObject_WhenPostTagRelationIsDeleted_ShouldReturnNothing()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };
            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);
            var postsTagsRelations = _postsTagsRelationsService.Find(id);
            _postsTagsRelationsService.Delete(postsTagsRelations);
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => null);
            var postsTagsRelation = _postsTagsRelationsService.Find(id);

            //Assert
            Assert.Null(postsTagsRelation);
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
            var id = random.Next(52);
            var itemsCount = random.Next(10);
            var newPostsTagsRelations = new List<PostsTagsRelations>();


            for (int i = 0; i < itemsCount; i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                newPostsTagsRelations.Add(new PostsTagsRelations
                {
                    PostId = id,
                    TagId = tag.Id,
                    Tag = tag
                });
            }

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelations))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newPostsTagsRelations[i].Id = id + i;
                    }
                });

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelations);
            _postsTagsRelationsService.Delete(newPostsTagsRelations);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.Delete(newPostsTagsRelations), Times.Once);
        }

        /// <summary>
        /// Delete By Enumerable post tag relation.
        /// Should return nothing when post tag relation is deleted.
        /// </summary>
        [Fact]
        public void DeleteByEnumerable_WhenPostTagRelationIsDeleted_ShouldReturnNothing()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var itemsCount = random.Next(10);
            var newPostsTagsRelations = new List<PostsTagsRelations>();


            for (int i = 0; i < itemsCount; i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                newPostsTagsRelations.Add(new PostsTagsRelations
                {
                    PostId = id,
                    TagId = tag.Id,
                    Tag = tag
                });
            }

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelations))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newPostsTagsRelations[i].Id = id + i;
                    }
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.Delete(newPostsTagsRelations))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newPostsTagsRelations = null;
                    }
                });

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelations);
            _postsTagsRelationsService.Delete(newPostsTagsRelations);

            //Assert
            Assert.Null(newPostsTagsRelations);
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
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };
            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => newPostsTagsRelation);

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelation);
            var postsTagsRelation = await _postsTagsRelationsService.FindAsync(id);
            await _postsTagsRelationsService.DeleteAsync(id);
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => null);
            await _postsTagsRelationsService.FindAsync(id);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.DeleteAsync(newPostsTagsRelation), Times.Once);
        }

        /// <summary>
        /// Async delete by id post tag relation.
        /// Should return nothing when post tag relation is deleted.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task DeleteAsyncById_WhenPostTagRelationIsDeleted_ShouldReturnNothing()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };
            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => newPostsTagsRelation);

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelation);
            var postsTagsRelations = await _postsTagsRelationsService.FindAsync(id);
            await _postsTagsRelationsService.DeleteAsync(id);
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => null);
            var postsTagsRelation = await _postsTagsRelationsService.FindAsync(id);

            //Assert
            Assert.Null(postsTagsRelation);
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
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };
            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => newPostsTagsRelation);

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelation);
            var postsTagsRelation = await _postsTagsRelationsService.FindAsync(id);
            await _postsTagsRelationsService.DeleteAsync(postsTagsRelation);
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => null);
            await _postsTagsRelationsService.FindAsync(id);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.DeleteAsync(postsTagsRelation), Times.Once);
        }

        /// <summary>
        /// Async delete by object post tag relation.
        /// Should return nothing when post tag relation is deleted.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task DeleteAsyncByObject_WhenPostTagRelationIsDeleted_ShouldReturnNothing()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };
            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => newPostsTagsRelation);

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelation);
            var postsTagsRelations = await _postsTagsRelationsService.FindAsync(id);
            await _postsTagsRelationsService.DeleteAsync(postsTagsRelations);
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => null);
            var postsTagsRelation = await _postsTagsRelationsService.FindAsync(id);

            //Assert
            Assert.Null(postsTagsRelation);
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
            var id = random.Next(52);
            var itemsCount = random.Next(10);
            var newPostsTagsRelations = new List<PostsTagsRelations>();


            for (int i = 0; i < itemsCount; i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                newPostsTagsRelations.Add(new PostsTagsRelations
                {
                    PostId = id,
                    TagId = tag.Id,
                    Tag = tag
                });
            }

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelations))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newPostsTagsRelations[i].Id = id + i;
                    }
                });

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelations);
            await _postsTagsRelationsService.DeleteAsync(newPostsTagsRelations);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.DeleteAsync(newPostsTagsRelations), Times.Once);
        }

        /// <summary>
        /// Delete Async By Enumerable post tag relation.
        /// Should return nothing when post tag relation is deleted.
        /// </summary>
        [Fact]
        public async Task DeleteAsyncByEnumerable_WhenPostTagRelationIsDeleted_ShouldReturnNothing()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var itemsCount = random.Next(10);
            var newPostsTagsRelations = new List<PostsTagsRelations>();


            for (int i = 0; i < itemsCount; i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                newPostsTagsRelations.Add(new PostsTagsRelations
                {
                    PostId = id,
                    TagId = tag.Id,
                    Tag = tag
                });
            }

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelations))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newPostsTagsRelations[i].Id = id + i;
                    }
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.DeleteAsync(newPostsTagsRelations))
                .Callback(() =>
                {
                    newPostsTagsRelations = null;
                });

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelations);
            await _postsTagsRelationsService.DeleteAsync(newPostsTagsRelations);

            //Assert
            Assert.Null(newPostsTagsRelations);
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
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{titleSearch} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => postsTagsRelationsList.Any(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            _postsTagsRelationsService.Any(specification);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.Any(specification), Times.Once);
        }

        /// <summary>
        /// Check if there are any post tag relations with specification.
        /// Should return true with contains specification when post tag relations exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests ")]
        public void Any_WithContainsSpecification_WhenPostTagRelationsExists_ShouldReturnTrue(string titleSearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{titleSearch} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => postsTagsRelationsList.Any(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var areAnyPosts = _postsTagsRelationsService.Any(specification);

            //Assert
            Assert.True(areAnyPosts);
        }

        /// <summary>
        /// Check if there are any post tag relations with specification.
        /// Should return post with equal specification when post tag relations exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests 0")]
        public void Any_WithEqualsSpecification_WhenPostTagRelationsExists_ShouldReturnTrue(string titleSearch)
        {
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{titleSearch} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => postsTagsRelationsList.Any(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var areAnyPosts = _postsTagsRelationsService.Any(specification);

            //Assert
            Assert.True(areAnyPosts);
        }

        /// <summary>
        /// Check if there are any post tag relations with specification.
        /// Should return false with when post tag relations does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests -1")]
        public void Any_WithEqualSpecification_WhenPostTagRelationsExists_ShouldReturnFalse(string titleSearch)
        {
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => postsTagsRelationsList.Any(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var areAnyPosts = _postsTagsRelationsService.Any(specification);

            //Assert
            Assert.False(areAnyPosts);
        }

        /// <summary>
        /// Check if there are any post tag relations with specification.
        /// Should return false with when post tag relations does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests 0")]
        public void Any_WithEqualSpecification_WhenPostTagRelationDoesNotExists_ShouldReturnNothing(string titleSearch)
        {
            //Arrange
            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => false);

            //Act
            var areAnyPosts = _postsTagsRelationsService.Any(specification);

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
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{titleSearch} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => postsTagsRelationsList.Any(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            await _postsTagsRelationsService.AnyAsync(specification);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.AnyAsync(specification), Times.Once);
        }

        /// <summary>
        /// Async check if there are any post tag relations with specification.
        /// Should return true with contains specification when post tag relations exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Created from ServicesTests ")]
        public async Task AnyAsync_WithContainsSpecification_WhenPostTagRelationsExists_ShouldReturnTrue(string titleSearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{titleSearch} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => postsTagsRelationsList.Any(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var areAnyPosts = await _postsTagsRelationsService.AnyAsync(specification);

            //Assert
            Assert.True(areAnyPosts);
        }

        /// <summary>
        /// Async check if there are any post tag relations with specification.
        /// Should return post with equal specification when post tag relations exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Created from ServicesTests 0")]
        public async Task AnyAsync_WithEqualsSpecification_WhenPostTagRelationsExists_ShouldReturnTrue(string titleSearch)
        {
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{titleSearch} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => postsTagsRelationsList.Any(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var areAnyPosts = await _postsTagsRelationsService.AnyAsync(specification);

            //Assert
            Assert.True(areAnyPosts);
        }

        /// <summary>
        /// Async check if there are any post tag relations with specification.
        /// Should return false with when post tag relations does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Created from ServicesTests -1")]
        public async Task AnyAsync_WithEqualSpecification_WhenPostTagRelationsExists_ShouldReturnFalse(string titleSearch)
        {
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => postsTagsRelationsList.Any(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var areAnyPosts = await _postsTagsRelationsService.AnyAsync(specification);

            //Assert
            Assert.False(areAnyPosts);
        }

        /// <summary>
        /// Async check if there are any post tag relations with specification.
        /// Should return false with when post tag relations does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Created from ServicesTests 0")]
        public async Task AnyAsync_WithEqualSpecification_WhenPostTagRelationDoesNotExists_ShouldReturnNothing(string titleSearch)
        {
            //Arrange
            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => false);

            //Act
            var areAnyPosts = await _postsTagsRelationsService.AnyAsync(specification);

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
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{titleSearch} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => postsTagsRelationsList.FirstOrDefault(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            _postsTagsRelationsService.FirstOrDefault(specification);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.FirstOrDefault(specification), Times.Once);
        }

        /// <summary>
        /// Get first or default post with specification.
        /// Should return post with contains specification when post tag relations exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests ")]
        public void FirstOrDefault_WithContainsSpecification_WhenPostTagRelationsExists_ShouldReturnTrue(string titleSearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{titleSearch} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => postsTagsRelationsList.FirstOrDefault(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var postsTagsRelations = _postsTagsRelationsService.FirstOrDefault(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.IsType<PostsTagsRelations>(postsTagsRelations);
        }

        /// <summary>
        /// Get first or default post with specification.
        /// Should return post with equal specification when post tag relations exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Tag 0")]
        public void FirstOrDefault_WithEqualsSpecification_WhenPostTagRelationsExists_ShouldReturnTrue(string titleSearch)
        {
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => postsTagsRelationsList.FirstOrDefault(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var postsTagsRelations = _postsTagsRelationsService.FirstOrDefault(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.IsType<PostsTagsRelations>(postsTagsRelations);
        }

        /// <summary>
        /// Get first or default post with specification.
        /// Should return nothing with when post does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Tag -1")]
        public void FirstOrDefault_WithEqualSpecification_WhenPostTagRelationsExists_ShouldReturnNothing(string titleSearch)
        {
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => postsTagsRelationsList.FirstOrDefault(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var post = _postsTagsRelationsService.FirstOrDefault(specification);

            //Assert
            Assert.Null(post);
        }

        /// <summary>
        /// Get first or default post with specification.
        /// Should return nothing with when post tag relations does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Tag 0")]
        public void FirstOrDefault_WithEqualSpecification_WhenPostTagRelationDoesNotExists_ShouldReturnNothing(string titleSearch)
        {
            //Arrange
            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => null);

            //Act
            var postsTagsRelations = _postsTagsRelationsService.FirstOrDefault(specification);

            //Assert
            Assert.Null(postsTagsRelations);
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
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{titleSearch} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => postsTagsRelationsList.LastOrDefault(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            _postsTagsRelationsService.LastOrDefault(specification);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.LastOrDefault(specification), Times.Once);
        }

        /// <summary>
        /// Get last or default post with specification.
        /// Should return post with contains specification when post tag relations exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests ")]
        public void LastOrDefault_WithContainsSpecification_WhenPostTagRelationsExists_ShouldReturnTrue(string titleSearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{titleSearch} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => postsTagsRelationsList.LastOrDefault(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var postsTagsRelations = _postsTagsRelationsService.LastOrDefault(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.IsType<PostsTagsRelations>(postsTagsRelations);
        }

        /// <summary>
        /// Get last or default post with specification.
        /// Should return post with equal specification when post tag relations exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests 0")]
        public void LastOrDefault_WithEqualsSpecification_WhenPostTagRelationsExists_ShouldReturnTrue(string titleSearch)
        {
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{titleSearch} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => postsTagsRelationsList.LastOrDefault(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var postsTagsRelations = _postsTagsRelationsService.LastOrDefault(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.IsType<PostsTagsRelations>(postsTagsRelations);
        }

        /// <summary>
        /// Get last or default post with specification.
        /// Should return nothing with when post does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests -1")]
        public void LastOrDefault_WithEqualSpecification_WhenPostTagRelationsExists_ShouldReturnNothing(string titleSearch)
        {
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => postsTagsRelationsList.LastOrDefault(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var postsTagsRelations = _postsTagsRelationsService.LastOrDefault(specification);

            //Assert
            Assert.Null(postsTagsRelations);
        }

        /// <summary>
        /// Get last or default post with specification.
        /// Should return nothing with when post tag relations does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests 0")]
        public void LastOrDefault_WithEqualSpecification_WhenPostTagRelationDoesNotExists_ShouldReturnNothing(string titleSearch)
        {
            //Arrange
            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => null);

            //Act
            var postsTagsRelations = _postsTagsRelationsService.LastOrDefault(specification);

            //Assert
            Assert.Null(postsTagsRelations);
        }

        #endregion

        #region Search async function

        /// <summary>
        /// Search the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="postsTagsRelationsList">The posts tags relations list.</param>
        /// <returns>PagedListResult.</returns>
        protected PagedListResult<PostsTagsRelations> Search(SearchQuery<PostsTagsRelations> query, List<PostsTagsRelations> postsTagsRelationsList)
        {
            var sequence = postsTagsRelationsList.AsQueryable();

            // Applying filters
            if (query.Filters != null && query.Filters.Count > 0)
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
                var properties = query.IncludeProperties.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                sequence = properties.Aggregate(sequence, (current, includeProperty) => current.Include(includeProperty));
            }
            var b = sequence.ToList();

            // Resolving Sort Criteria
            // This code applies the sorting criterias sent as the parameter
            if (query.SortCriterias != null && query.SortCriterias.Count > 0)
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
                sequence = ((IOrderedQueryable<PostsTagsRelations>)sequence).OrderBy(x => true);
            }

            var c = sequence.ToList();

            // Counting the total number of object.
            var resultCount = sequence.Count();

            var result = (query.Take > 0)
                                ? sequence.Skip(query.Skip).Take(query.Take).ToList()
                                : sequence.ToList();

            // Debug info of what the query looks like
            // Console.WriteLine(sequence.ToString());

            // Setting up the return object.
            bool hasNext = (query.Skip > 0 || query.Take > 0) && (query.Skip + query.Take < resultCount);
            return new PagedListResult<PostsTagsRelations>()
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
        [InlineData("Created from ServicesTests ", 0, 10, "Post.Title", OrderType.Ascending)]
        [InlineData("Created from ServicesTests ", 10, 10, "Post.Title", OrderType.Ascending)]
        [InlineData("Created from ServicesTests ", 10, 20, "Post.Title", OrderType.Ascending)]
        [InlineData("Created from ServicesTests ", 0, 100, "Post.Title", OrderType.Ascending)]
        public async Task Verify_FunctionSearchAsync_HasBeenCalled(string search, int start, int take, string fieldName, OrderType orderType)
        {
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{search} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }

            var query = new SearchQuery<PostsTagsRelations>
            {
                Skip = start,
                Take = take
            };

            query.AddSortCriteria(new FieldSortOrder<PostsTagsRelations>(fieldName, orderType));

            query.AddFilter(x => x.Post.Title.ToUpper().Contains($"{search}".ToUpper()));

            _postsTagsRelationsRepositoryMock.Setup(x => x.SearchAsync(query))
                .ReturnsAsync(() =>
                {
                    return Search(query, postsTagsRelationsList);
                });

            //Act
            await _postsTagsRelationsService.SearchAsync(query);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.SearchAsync(query), Times.Once);
        }

        /// <summary>
        /// Search async posts tags relations.
        /// Should return posts tags relations when posts tags relations exists.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="start">The start.</param>
        /// <param name="take">The take.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="orderType">The order type.</param>
        [Theory]
        [InlineData("Created from ServicesTests ", 0, 10, "Post.Title", OrderType.Ascending)]
        [InlineData("Created from ServicesTests ", 10, 10, "Post.Title", OrderType.Ascending)]
        [InlineData("Created from ServicesTests ", 10, 20, "Post.Title", OrderType.Ascending)]
        [InlineData("Created from ServicesTests ", 0, 100, "Post.Title", OrderType.Ascending)]
        public async Task SearchAsync_WhenPostsTagsRelationsExists_ShouldReturnPostsTagsRelations(string search, int start, int take, string fieldName, OrderType orderType)
        {
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{search} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }

            var query = new SearchQuery<PostsTagsRelations>
            {
                Skip = start,
                Take = take
            };

            query.AddSortCriteria(new FieldSortOrder<PostsTagsRelations>(fieldName, orderType));

            query.AddFilter(x => x.Post.Title.ToUpper().Contains($"{search}".ToUpper()));

            _postsTagsRelationsRepositoryMock.Setup(x => x.SearchAsync(query))
                .ReturnsAsync(() =>
                {
                    return Search(query, postsTagsRelationsList);
                });

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.SearchAsync(query);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations.Entities);
        }

        /// <summary>
        /// Search async posts tags relations with specification.
        /// Should return posts tags relation with equal specification when posts tags relations exists.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="start">The start.</param>
        /// <param name="take">The take.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="orderType">The order type.</param>
        [Theory]
        [InlineData("Created from ServicesTests 0", 0, 10, "Post.Title ", OrderType.Ascending)]
        [InlineData("Created from ServicesTests 11", 10, 10, "Post.Title", OrderType.Ascending)]
        [InlineData("Created from ServicesTests 11", 10, 20, "Post.Title", OrderType.Ascending)]
        [InlineData("Created from ServicesTests 11", 0, 100, "Post.Title", OrderType.Ascending)]
        public async Task SearchAsync_WithEqualsSpecification_WhenPostsTagsRelationsExists_ShouldReturnPostsTagsRelation(string search, int start, int take, string fieldName, OrderType orderType)
        {
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{search} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }

            var query = new SearchQuery<PostsTagsRelations>
            {
                Skip = start,
                Take = take
            };

            query.AddSortCriteria(new FieldSortOrder<PostsTagsRelations>(fieldName, orderType));

            query.AddFilter(x => x.Post.Title.ToUpper().Contains($"{search}".ToUpper()));

            _postsTagsRelationsRepositoryMock.Setup(x => x.SearchAsync(query))
                .ReturnsAsync(() =>
                {
                    return Search(query, postsTagsRelationsList);
                });

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.SearchAsync(query);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations.Entities);
            Assert.Single(postsTagsRelations.Entities);
        }

        /// <summary>
        /// Search async posts tags relations with specification.
        /// Should return nothing with  when posts tags relations exists.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="start">The start.</param>
        /// <param name="take">The take.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="orderType">The order type.</param>
        [Theory]
        [InlineData("Created from ServicesTests -0", 0, 10, "Post.Title", OrderType.Ascending)]
        [InlineData("Created from ServicesTests -11", 10, 10, "Post.Title", OrderType.Ascending)]
        [InlineData("Created from ServicesTests -11", 10, 20, "Post.Title", OrderType.Ascending)]
        [InlineData("Created from ServicesTests -11", 0, 100, "Post.Title", OrderType.Ascending)]
        public async Task SearchAsync_WithEqualSpecification_WhenPostsTagsRelationsExists_ShouldReturnNothing(string search, int start, int take, string fieldName, OrderType orderType)
        {
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{search} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }

            var query = new SearchQuery<PostsTagsRelations>
            {
                Skip = start,
                Take = take
            };

            query.AddSortCriteria(new FieldSortOrder<PostsTagsRelations>(fieldName, orderType));

            query.AddFilter(x => x.Post.Title.ToUpper().Contains($"{search}".ToUpper()));

            _postsTagsRelationsRepositoryMock.Setup(x => x.SearchAsync(query))
                .ReturnsAsync(() =>
                {
                    return Search(query, postsTagsRelationsList);
                });

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.SearchAsync(query);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.Empty(postsTagsRelations.Entities);
        }

        /// <summary>
        /// Search async posts tags relations.
        /// Should return nothing when posts tags relations does not exists.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="start">The start.</param>
        /// <param name="take">The take.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="orderType">The order type.</param>
        [Theory]
        [InlineData("Created from ServicesTests 0", 0, 10, "Post.Title", OrderType.Ascending)]
        [InlineData("Created from ServicesTests 11", 10, 10, "Post.Title", OrderType.Ascending)]
        [InlineData("Created from ServicesTests 11", 10, 20, "Post.Title", OrderType.Ascending)]
        [InlineData("Created from ServicesTests 11", 0, 100, "Post.Title", OrderType.Ascending)]
        public async Task SearchAsync_WhenPostsTagsRelationsDoesNotExists_ShouldReturnNothing(string search, int start, int take, string fieldName, OrderType orderType)
        {
            //Arrange
            var query = new SearchQuery<PostsTagsRelations>
            {
                Skip = start,
                Take = take
            };

            query.AddSortCriteria(new FieldSortOrder<PostsTagsRelations>(fieldName, orderType));

            query.AddFilter(x => x.Post.Title.ToUpper().Contains($"{search}".ToUpper()));

            _postsTagsRelationsRepositoryMock.Setup(x => x.SearchAsync(query))
                .ReturnsAsync(() => new PagedListResult<PostsTagsRelations>());

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.SearchAsync(query);

            //Assert
            Assert.Empty(postsTagsRelations.Entities);
        }

        #endregion

        #region NotTestedYet
        //GenerateQuery(TableFilter tableFilter, string includeProperties = null)
        //GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
        #endregion

    #endregion
    }