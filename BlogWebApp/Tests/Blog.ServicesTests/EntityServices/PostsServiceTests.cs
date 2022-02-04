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

        #endregion

        #region Ctor

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

        #endregion

        #region Get All function

        /// <summary>
        /// Verify that function Get All has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionGetAll_HasBeenCalled()
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
        /// Get all posts with specification.
        /// Should return posts with contains specification when posts exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData(0, "Created from ServicesTests ")]
        public void GetAll_ShouldReturnPosts_WithContainsSpecification_WhenPostsExists(int notEqualCount, string titleSearch)
        {
            //Test failed
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
        public void GetAll_ShouldReturnPost_WithEqualsSpecification_WhenPostsExists(int equalCount, string titleSearch)
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


            var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
            _postsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => postslist.Where(x => x.Title.Contains(titleSearch)).AsQueryable());

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
        public void GetAll_ShouldReturnNothing_WithEqualSpecification_WhenPostsExists(int equalCount, string titleSearch)
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


            var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
            _postsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => postslist.Where(x => x.Title.Contains(titleSearch)).AsQueryable());

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
        public void GetAll_ShouldReturnNothing_WithEqualSpecification_WhenPostDoesNotExists(string titleSearch)
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

        #region Find function

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

        #endregion

        #region Insert function

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

        #endregion

        #region Insert Async function

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

        #endregion

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

        #endregion

        #region Delete function

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

        #endregion

        #region Delete Async function

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

        #endregion

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
            _postsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => postslist.Any(x => x.Title.Contains(titleSearch)));

            //Act
            var areAnyPosts = _postsService.Any(specification);

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
        public void Any_ShouldReturnTrue_WithContainsSpecification_WhenPostsExists(string titleSearch)
        {
            //Test failed
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
            _postsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => postslist.Any(x => x.Title.Contains(titleSearch)));

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
        public void Any_ShouldReturnTrue_WithEqualsSpecification_WhenPostsExists(string titleSearch)
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


            var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
            _postsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => postslist.Any(x => x.Title.Contains(titleSearch)));

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
        public void Any_ShouldReturnFalse_WithEqualSpecification_WhenPostsExists(string titleSearch)
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


            var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
            _postsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => postslist.Any(x => x.Title.Contains(titleSearch)));

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
        public void Any_ShouldReturnNothing_WithEqualSpecification_WhenPostDoesNotExists(string titleSearch)
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
            _postsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => postslist.Any(x => x.Title.Contains(titleSearch)));

            //Act
            var areAnyPosts = await _postsService.AnyAsync(specification);

            //Assert
            _postsRepositoryMock.Verify(x => x.Any(specification), Times.Once);
        }

        /// <summary>
        /// Async check if there are any posts with specification.
        /// Should return true with contains specification when posts exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Created from ServicesTests ")]
        public async Task AnyAsync_ShouldReturnTrue_WithContainsSpecification_WhenPostsExists(string titleSearch)
        {
            //Test failed
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
            _postsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => postslist.Any(x => x.Title.Contains(titleSearch)));

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
        public async Task AnyAsync_ShouldReturnTrue_WithEqualsSpecification_WhenPostsExists(string titleSearch)
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


            var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
            _postsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => postslist.Any(x => x.Title.Contains(titleSearch)));

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
        public async Task AnyAsync_ShouldReturnFalse_WithEqualSpecification_WhenPostsExists(string titleSearch)
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


            var specification = new PostSpecification(x => x.Title.Equals(titleSearch));
            _postsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => postslist.Any(x => x.Title.Contains(titleSearch)));

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
        public async Task AnyAsync_ShouldReturnNothing_WithEqualSpecification_WhenPostDoesNotExists(string titleSearch)
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
            _postsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => postslist.FirstOrDefault(x => x.Title.Contains(titleSearch)));

            //Act
            var post = _postsService.FirstOrDefault(specification);

            //Assert
            _postsRepositoryMock.Verify(x => x.FirstOrDefault(specification), Times.Once);
        }

        #endregion

        #region NotTestedYet
        /// <summary>
        /// Verify that function Get All Async has been called.
        /// </summary>
        //[Fact]
        public async Task Verify_FunctionGetAllAsync_HasBeenCalled()
        {
            //Test failed
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


            /*_generalServiceMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => postslist);*/

            //Act
            var posts = await _postsService.GetAllAsync();

            //Assert
            _postsRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        /// <summary>
        /// Async get all posts.
        /// Should return posts when posts exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        //[Theory]
        //[InlineData(0)]
        public async Task GetAllAsync_ShouldReturnPosts_WhenPostsExists(int notEqualCount)
        {
            //Test failed
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
            var posts = await _postsService.GetAllAsync();

            //Assert
            Assert.NotNull(posts);
            Assert.NotEmpty(posts);
            Assert.NotEqual(notEqualCount, posts.ToList().Count);
        }

        /// <summary>
        /// Async get all posts.
        /// Should return nothing when posts does not exists.
        /// </summary>
        //[Fact]
        public async Task GetAllAsync_ShouldReturnNothing_WhenPostDoesNotExists()
        {
            //Test failed
            //Arrange
            /*_generalServiceMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => new List<Post>());*/

            //Act
            var posts = await _postsService.GetAllAsync();

            //Assert
            Assert.Empty(posts);
        }

        //SearchAsync(SearchQuery<T> searchQuery)
        //GetAllAsync(ISpecification<T> specification)
        //GenerateQuery(TableFilter tableFilter, string includeProperties = null)
        //GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
        #endregion
    }
}
