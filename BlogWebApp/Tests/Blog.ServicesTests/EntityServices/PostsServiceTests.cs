using AutoMapper;
using Blog.Data.Models;
using Blog.Data.Repository;
using Blog.Data.Specifications;
using Blog.Services;
using Blog.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Verify that function Get All has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionGetAll_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var postslist = new List<Post>();

            for(var i = 0; i < random.Next(100); i++)
            {
                var postId = i;
                postslist.Add(new Post
                {
                    Id = postId,
                    Title = $"Created from ServicesTests {postId}",
                    Description = $"Created from ServicesTests {postId}",
                    Content = $"Created from ServicesTests {postId}",
                    ImageUrl = $"Created from ServicesTests {postId}",
                });
            }


            _postsRepositoryMock.Setup(x => x.GetAll())
                .Returns(postslist.AsQueryable());

            //Act
            var posts = _postsService.GetAll();

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
        public void GetAll_ShouldReturnPosts_WhenPostsExists(int notEqualCount)
        {
            //Arrange
            var random = new Random();
            var postslist = new List<Post>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var postId = i;
                postslist.Add(new Post
                {
                    Id = postId,
                    Title = $"Created from ServicesTests {postId}",
                    Description = $"Created from ServicesTests {postId}",
                    Content = $"Created from ServicesTests {postId}",
                    ImageUrl = $"Created from ServicesTests {postId}",
                });
            }


            _postsRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => postslist.AsQueryable());

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
        public void GetAll_ShouldReturnNothing_WhenPostDoesNotExists()
        {
            //Arrange
            _postsRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => new List<Post>().AsQueryable());

            //Act
            var posts = _postsService.GetAll();

            //Assert
            Assert.Empty(posts);
        }

        /// <summary>
        /// Verify that function Get All with specification has been called.
        /// </summary>
        [Theory]
        [InlineData("Created from ServicesTests ")]
        public void Verify_FunctionGetAll_WithSpecification_HasBeenCalled(string titleSearch)
        {
            //Arrange
            var random = new Random();
            var postslist = new List<Post>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var postId = i;
                postslist.Add(new Post
                {
                    Id = postId,
                    Title = $"Created from ServicesTests {postId}",
                    Description = $"Created from ServicesTests {postId}",
                    Content = $"Created from ServicesTests {postId}",
                    ImageUrl = $"Created from ServicesTests {postId}",
                });
            }

            var specification = new PostSpecification(x => x.Title.Contains(titleSearch));
            _postsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => postslist.Where(x => x.Title.Contains(titleSearch)).AsQueryable());

            //Act
            var posts = _postsService.GetAll(specification);

            //Assert
            _postsRepositoryMock.Verify(x => x.GetAll(specification), Times.Once);
        }

        /// <summary>
        /// Verify that function Find has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionFind_HasBeenCalled()
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
                .Returns(() => newPost);

            //Act
            var post = _postsService.Find(postId);

            //Assert
            _postsRepositoryMock.Verify(x => x.GetById(postId), Times.Once);
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
                .Returns(() => newPost);

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
        /// Verify that function Find Async has been called.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task Verify_FunctionFindAsync_HasBeenCalled()
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
                .ReturnsAsync(() => newPost);

            //Act
            var post = await _postsService.FindAsync(postId);

            //Assert
            _postsRepositoryMock.Verify(x => x.GetByIdAsync(postId), Times.Once);
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
                .ReturnsAsync(() => newPost);

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
        /// Verify that function Insert has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionInsert_HasBeenCalled()
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
            _postsRepositoryMock.Verify(x => x.Insert(newPost), Times.Once);
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
        /// Verify that function Insert Async has been called.
        /// </summary>
        [Fact]
        public async Task Verify_FunctionInsertAsync_HasBeenCalled()
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
            _postsRepositoryMock.Verify(x => x.InsertAsync(newPost), Times.Once);
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

        /// <summary>
        /// Verify that function Update has been called.
        /// </summary>
        /// <param name="newTitle">The new title.</param>
        [Theory]
        [InlineData("New title")]
        public void Verify_FunctionUpdate_HasBeenCalled(string newTitle)
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
        public void Update_ShouldReturnPost_WhenPostExists(string newTitle)
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
        public async Task UpdateAsync_ShouldReturnPost_WhenPostExists(string newTitle)
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

        /// <summary>
        /// Verify that function Delete has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionDelete_HasBeenCalled()
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
                .Returns(() => newPost);

            //Act
            _postsService.Insert(newPost);
            var post = _postsService.Find(postId);
            _postsService.Delete(newPost);
            _postsRepositoryMock.Setup(x => x.GetById(postId))
                .Returns(() => null);
            var deletedPost = _postsService.Find(postId);

            //Assert
            _postsRepositoryMock.Verify(x => x.Delete(post), Times.Once);
        }

        /// <summary>
        /// Delete post.
        /// Should return nothing when post is deleted.
        /// </summary>
        [Fact]
        public void Delete_ShouldReturnNothing_WhenPostIsDeleted()
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
            _postsRepositoryMock.Setup(x => x.GetById(postId))
                .Returns(() => newPost);

            //Act
            _postsService.Insert(newPost);
            var post = _postsService.Find(postId);
            _postsService.Delete(newPost);
            _postsRepositoryMock.Setup(x => x.GetById(postId))
                .Returns(() => null);
            var deletedPost = _postsService.Find(postId);

            //Assert
            Assert.Null(deletedPost);
        }

        /// <summary>
        /// Verify that function Delete Async has been called.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task Verify_FunctionDeleteAsync_HasBeenCalled()
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
                .ReturnsAsync(() => newPost);

            //Act
            await _postsService.InsertAsync(newPost);
            var post = await _postsService.FindAsync(postId);
            await _postsService.DeleteAsync(newPost);

            //Assert
            _postsRepositoryMock.Verify(x => x.DeleteAsync(post), Times.Once);
        }

        /// <summary>
        /// Async delete post.
        /// Should return nothing when post is deleted.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task DeleteAsync_ShouldReturnNothing_WhenPostIsDeleted()
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
            _postsRepositoryMock.Setup(x => x.GetByIdAsync(postId))
                .ReturnsAsync(() => newPost);

            //Act
            await _postsService.InsertAsync(newPost);
            var post = await _postsService.FindAsync(postId);
            await _postsService.DeleteAsync(newPost);
            _postsRepositoryMock.Setup(x => x.GetByIdAsync(postId))
                .Returns(() => null);
            var deletedPost = _postsService.Find(postId);

            //Assert
            Assert.Null(deletedPost);
        }
    }
}
