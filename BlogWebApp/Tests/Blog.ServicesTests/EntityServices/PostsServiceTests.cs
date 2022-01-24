using AutoMapper;
using Blog.Data.Models;
using Blog.Data.Repository;
using Blog.Services;
using Blog.Services.Interfaces;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Blog.ServicesTests.EntityServices
{
    /// <summary>
    /// Posts service tests.
    /// </summary>
    public class PostsServiceTests
    {
        /// <summary>
        /// The posts service.
        /// </summary>
        private readonly IPostsService _postsService;

        /// <summary>
        /// The posts repository mock.
        /// </summary>
        private readonly Mock<IRepository<Post>> _postsRepositoryMock;

        /// <summary>
        /// The comments service mock.
        /// </summary>
        private readonly Mock<ICommentsService> _commentsServiceMock;

        /// <summary>
        /// The mapper mock.
        /// </summary>
        private readonly Mock<IMapper> _mapper;

        /// <summary>
        /// The posts tags relations service mock.
        /// </summary>
        private readonly Mock<IPostsTagsRelationsService> _postsTagsRelationsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsServiceTests"/> class.
        /// </summary>
        public PostsServiceTests()
        {
            _postsRepositoryMock = new Mock<IRepository<Post>>();
            _commentsServiceMock = new Mock<ICommentsService>();
            _mapper = new Mock<IMapper>();
            _postsTagsRelationsService = new Mock<IPostsTagsRelationsService>();
            _postsService = new PostsService(_postsRepositoryMock.Object, _commentsServiceMock.Object, _mapper.Object, _postsTagsRelationsService.Object);
        }

        /// <summary>
        /// Find post.
        /// Should return post when post exists.
        /// </summary>
        [Fact]
        public void Find_ShouldReturnPost_WhenPostExists()
        {
            //Arrange
            var random = new Random();
            var postId = random.Next(52);
            var newPost = new Post
            {
                Id = postId,
                Title = $"Created from ServicesTests {postId}",
                Description = $"Created from ServicesTests {postId}",
                Content = $"Created from ServicesTests {postId}",
                ImageUrl = $"Created from ServicesTests {postId}",
            };
            _postsRepositoryMock.Setup(x => x.GetById(postId))
                .Returns(newPost);

            //Act
            var post = _postsService.Find(postId);

            //Assert
            Assert.Equal(postId, post.Id);
        }

        /// <summary>
        /// Find post.
        /// Should return nothing when post does not exists.
        /// </summary>
        [Fact]
        public void Find_ShouldReturnNothing_WhenPostDoesNotExists()
        {
            //Arrange
            var random = new Random();
            var postId = random.Next(52);
            _postsRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(() => null);

            //Act
            var post = _postsService.Find(postId);

            //Assert
            Assert.Null(post);
        }

        /// <summary>
        /// Async find post.
        /// Should return post when post exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task FindAsync_ShouldReturnPost_WhenPostExists()
        {
            //Arrange
            var random = new Random();
            var postId = random.Next(52);
            var newPost = new Post
            {
                Id = postId,
                Title = $"Created from ServicesTests {postId}",
                Description = $"Created from ServicesTests {postId}",
                Content = $"Created from ServicesTests {postId}",
                ImageUrl = $"Created from ServicesTests {postId}",
            };
            _postsRepositoryMock.Setup(x => x.GetByIdAsync(postId))
                .ReturnsAsync(newPost);

            //Act
            var post = await _postsService.FindAsync(postId);

            //Assert
            Assert.Equal(postId, post.Id);
        }

        /// <summary>
        /// Async find post.
        /// Should return nothing when post does not exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task FindAsync_ShouldReturnNothing_WhenPostDoesNotExists()
        {
            //Arrange
            var random = new Random();
            var postId = random.Next(52);
            _postsRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var post = await _postsService.FindAsync(postId);

            //Assert
            Assert.Null(post);
        }

        /// <summary>
        /// Insert post.
        /// Should return post when post created.
        /// </summary>
        [Fact]
        public void Insert_ShouldReturnPost_WhenPostExists()
        {
            //Arrange
            var random = new Random();
            var postId = random.Next(52);
            var newPost = new Post
            {
                Title = $"Created from ServicesTests {postId}",
                Description = $"Created from ServicesTests {postId}",
                Content = $"Created from ServicesTests {postId}",
                ImageUrl = $"Created from ServicesTests {postId}",
            };

            _postsRepositoryMock.Setup(x => x.Insert(newPost))
                .Callback(() => {
                    newPost.Id = postId;
                });

            //Act
            _postsService.Insert(newPost);

            //Assert
            Assert.NotEqual(0, newPost.Id);
        }

        /// <summary>
        /// Async insert post.
        /// Should return post when post created.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task InsertAsync_ShouldReturnPost_WhenPostExists()
        {
            //Arrange
            var random = new Random();
            var postId = random.Next(52);
            var newPost = new Post
            {
                Title = $"Created from ServicesTests {postId}",
                Description = $"Created from ServicesTests {postId}",
                Content = $"Created from ServicesTests {postId}",
                ImageUrl = $"Created from ServicesTests {postId}",
            };

            _postsRepositoryMock.Setup(x => x.InsertAsync(newPost))
                .Callback(() => {
                    newPost.Id = postId;
                });

            //Act
            await _postsService.InsertAsync(newPost);

            //Assert
            Assert.NotEqual(0, newPost.Id);
        }
    }
}
