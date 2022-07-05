using Blog.Core.Enums;
using Blog.Core.Infrastructure;
using Blog.Core.Infrastructure.Pagination;
using Blog.Data.Models;
using Blog.Data.Repository;
using Blog.Data.Specifications;
using Blog.Services;
using Blog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Blog.ServicesTests.EntityServices
{
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

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsServiceTests"/> class.
        /// </summary>
        public CommentsServiceTests()
        {
            _commentsRepositoryMock = new Mock<IRepository<Comment>>();
            _commentsService = new CommentsService(_commentsRepositoryMock.Object);
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
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


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
        public void GetAll_ShouldReturnComments_WhenCommentsExists(int notEqualCount)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentsList.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


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
        public void GetAll_ShouldReturnNothing_WhenCommentsDoesNotExists()
        {
            //Arrange
            _commentsRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => new List<Comment>().AsQueryable());

            //Act
            var comments = _commentsService.GetAll();

            //Assert
            Assert.Empty(comments);
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
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }

            _commentsRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(commentsList);

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
        public async Task GetAllAsync_ShouldReturnComments_WhenCommentsExists(int notEqualCount)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentsList.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            _commentsRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => commentsList);

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
        public async void GetAllAsync_ShouldReturnNothing_WhenCommentsDoesNotExists()
        {
            //Arrange
            _commentsRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => new List<Comment>());

            //Act
            var comments = await _commentsService.GetAllAsync();

            //Assert
            Assert.Empty(comments);
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
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }

            var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.GetAll(specification))
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
        public void GetAll_ShouldReturnComments_WithContainsSpecification_WhenCommentsExists(int notEqualCount, string commentBodySearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.GetAll(specification))
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
        public void GetAll_ShouldReturnComment_WithEqualsSpecification_WhenCommentsExists(int equalCount, string commentBodySearch)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.GetAll(specification))
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
        public void GetAll_ShouldReturnNothing_WithEqualSpecification_WhenCommentsExists(int equalCount, string commentBodySearch)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.GetAll(specification))
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
        public void GetAll_ShouldReturnNothing_WithEqualSpecification_WhenCommentDoesNotExists(string commentBodySearch)
        {
            //Arrange
            var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => new List<Comment>().AsQueryable());

            //Act
            var comments = _commentsService.GetAll();

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
        public async void Verify_FunctionGetAllAsync_WithSpecification_HasBeenCalled(string commentBodySearch)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }

            var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.GetAllAsync(specification))
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
        public async Task GetAllAsync_ShouldReturnComments_WithContainsSpecification_WhenCommentsExists(int notEqualCount, string commentBodySearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.GetAllAsync(specification))
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
        public async Task GetAllAsync_ShouldReturnComment_WithEqualsSpecification_WhenCommentsExists(int equalCount, string commentBodySearch)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }

            var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.GetAllAsync(specification))
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
        public async void GetAllAsync_ShouldReturnNothing_WithEqualSpecification_WhenCommentsExists(int equalCount, string commentBodySearch)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.GetAllAsync(specification))
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
        public async Task GetAllAsync_ShouldReturnNothing_WithEqualSpecification_WhenCommentDoesNotExists(string commentBodySearch)
        {
            //Arrange
            var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.GetAllAsync(specification))
                .ReturnsAsync(() => new List<Comment>());

            //Act
            var comments = await _commentsService.GetAllAsync();

            //Assert
            Assert.Null(comments);
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
            var commentId = random.Next(52);
            var commentsList = new Comment
            {
                Id = commentId,
                CommentBody = $"Comment {commentId}",
            };
            _commentsRepositoryMock.Setup(x => x.GetById(commentId))
                .Returns(() => commentsList);

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
        public void Find_ShouldReturnComment_WhenCommentExists()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                Id = commentId,
                CommentBody = $"Comment {commentId}",
            };
            _commentsRepositoryMock.Setup(x => x.GetById(commentId))
                .Returns(() => newComment);

            //Act
            var comment = _commentsService.Find(commentId);

            //Assert
            Assert.Equal(commentId, comment.Id);
        }

        /// <summary>
        /// Find comment.
        /// Should return nothing when comment does not exists.
        /// </summary>
        [Fact]
        public void Find_ShouldReturnNothing_WhenCommentDoesNotExists()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
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
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                Id = commentId,
                CommentBody = $"Comment {commentId}",
            };
            _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
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
        public async Task FindAsync_ShouldReturnComment_WhenCommentExists()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                Id = commentId,
                CommentBody = $"Comment {commentId}",
            };
            _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
                .ReturnsAsync(() => newComment);

            //Act
            var comment = await _commentsService.FindAsync(commentId);

            //Assert
            Assert.Equal(commentId, comment.Id);
        }

        /// <summary>
        /// Async find comment.
        /// Should return nothing when comment does not exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task FindAsync_ShouldReturnNothing_WhenCommentDoesNotExists()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            _commentsRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var comment = await _commentsService.FindAsync(commentId);

            //Assert
            Assert.Null(comment);
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
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

            _commentsRepositoryMock.Setup(x => x.Insert(newComment))
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
        public void Insert_ShouldReturnComment_WhenCommentExists()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

            _commentsRepositoryMock.Setup(x => x.Insert(newComment))
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
            var commentId = random.Next(52);
            var itemsCount = random.Next(10);
            var newComments = new List<Comment>();

            for (int i = 0; i < itemsCount; i++)
            {
                newComments.Add(new Comment
                {
                    CommentBody = $"Comment {itemsCount}",
                });
            }

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

            //Assert
            _commentsRepositoryMock.Verify(x => x.Insert(newComments), Times.Once);
        }

        /// <summary>
        /// Insert Enumerable comments.
        /// Should return comments when comments created.
        /// </summary>
        [Fact]
        public void InsertEnumerable_ShouldReturnComments_WhenCommentsExists()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var itemsCount = random.Next(10);
            var newComments = new List<Comment>();

            for (int i = 0; i < itemsCount; i++)
            {
                newComments.Add(new Comment
                {
                    CommentBody = $"Comment {itemsCount}",
                });
            }

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
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

            _commentsRepositoryMock.Setup(x => x.InsertAsync(newComment))
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
        public async Task InsertAsync_ShouldReturnComment_WhenCommentExists()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

            _commentsRepositoryMock.Setup(x => x.InsertAsync(newComment))
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
            var commentId = random.Next(52);
            var itemsCount = random.Next(10);
            var newComments = new List<Comment>();

            for (int i = 0; i < itemsCount; i++)
            {
                newComments.Add(new Comment
                {
                    CommentBody = $"Comment {itemsCount}",
                });
            }

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

            //Assert
            _commentsRepositoryMock.Verify(x => x.InsertAsync(newComments), Times.Once);
        }

        /// <summary>
        /// Insert Async Enumerable comments.
        /// Should return comments when comments created.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task InsertAsyncEnumerable_ShouldReturnComments_WhenCommentsExists()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var itemsCount = random.Next(10);
            var newComments = new List<Comment>();

            for (int i = 0; i < itemsCount; i++)
            {
                newComments.Add(new Comment
                {
                    CommentBody = $"Comment {itemsCount}",
                });
            }

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

            //Assert
            newComments.ForEach(x =>
            {
                Assert.NotEqual(0, x.Id);
            });
        }

        #endregion

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
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

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
        public void Update_ShouldReturnComment_WhenCommentExists(string newCommentBody)
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

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
            var commentId = random.Next(52);
            var itemsCount = random.Next(10);
            var newComments = new List<Comment>();

            for (int i = 0; i < itemsCount; i++)
            {
                newComments.Add(new Comment
                {
                    CommentBody = $"Comment {itemsCount}",
                });
            }

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
        public void UpdateEnumerable_ShouldReturnComment_WhenCommentExists(string newCommentBody)
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var itemsCount = random.Next(10);
            var newComments = new List<Comment>();

            for (int i = 0; i < itemsCount; i++)
            {
                newComments.Add(new Comment
                {
                    CommentBody = $"Comment {itemsCount}",
                });
            }

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
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

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
        public async Task UpdateAsync_ShouldReturnComment_WhenCommentExists(string newCommentBody)
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

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
            var commentId = random.Next(52);
            var itemsCount = random.Next(10);
            var newComments = new List<Comment>();

            for (int i = 0; i < itemsCount; i++)
            {
                newComments.Add(new Comment
                {
                    CommentBody = $"Comment {itemsCount}",
                });
            }

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
        public async Task UpdateAsyncEnumerable_ShouldReturnComment_WhenCommentExists(string newCommentBody)
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var itemsCount = random.Next(10);
            var newComments = new List<Comment>();

            for (int i = 0; i < itemsCount; i++)
            {
                newComments.Add(new Comment
                {
                    CommentBody = $"Comment {itemsCount}",
                });
            }

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

        #region Delete By Id function

        /// <summary>
        /// Verify that function Delete By Id has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionDeleteById_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                Id = commentId,
                CommentBody = $"Comment {commentId}",
            };
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
        public void DeleteById_ShouldReturnNothing_WhenCommentIsDeleted()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

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
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                Id = commentId,
                CommentBody = $"Comment {commentId}",
            };
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
        public void DeleteByObject_ShouldReturnNothing_WhenCommentIsDeleted()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

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
            var newComments = new List<Comment>();

            for (int i = 0; i < itemsCount; i++)
            {
                newComments.Add(new Comment
                {
                    CommentBody = $"Comment {itemsCount}",
                });
            }

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
        public void DeleteByEnumerable_ShouldReturnNothing_WhenCommentIsDeleted()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var itemsCount = random.Next(10);
            var newComments = new List<Comment>();

            for (int i = 0; i < itemsCount; i++)
            {
                newComments.Add(new Comment
                {
                    CommentBody = $"Comment {itemsCount}",
                });
            }

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
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                Id = commentId,
                CommentBody = $"Comment {commentId}",
            };
            _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
                .ReturnsAsync(() => newComment);

            //Act
            await _commentsService.InsertAsync(newComment);
            var comment = await _commentsService.FindAsync(commentId);
            await _commentsService.DeleteAsync(commentId);

            //Assert
            _commentsRepositoryMock.Verify(x => x.DeleteAsync(newComment), Times.Once);
        }

        /// <summary>
        /// Async delete by id comment.
        /// Should return nothing when comment is deleted.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task DeleteAsyncById_ShouldReturnNothing_WhenCommentIsDeleted()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

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
            await _commentsService.DeleteAsync(commentId);
            _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
                .ReturnsAsync(() => null);
            var deletedComment = await _commentsService.FindAsync(commentId);

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
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                Id = commentId,
                CommentBody = $"Comment {commentId}",
            };
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
        public async Task DeleteAsyncByObject_ShouldReturnNothing_WhenCommentIsDeleted()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var newComment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

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
            var newComments = new List<Comment>();

            for (int i = 0; i < itemsCount; i++)
            {
                newComments.Add(new Comment
                {
                    CommentBody = $"Comment {itemsCount}",
                });
            }

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
        public async Task DeleteAsyncByEnumerable_ShouldReturnNothing_WhenCommentIsDeleted()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var itemsCount = random.Next(10);
            var newComments = new List<Comment>();

            for (int i = 0; i < itemsCount; i++)
            {
                newComments.Add(new Comment
                {
                    CommentBody = $"Comment {itemsCount}",
                });
            }

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
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }

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
        public void Any_ShouldReturnTrue_WithContainsSpecification_WhenCommentsExists(string commentBodySearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


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
        public void Any_ShouldReturnTrue_WithEqualsSpecification_WhenCommentsExists(string commentBodySearch)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


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
        public void Any_ShouldReturnFalse_WithEqualSpecification_WhenCommentsExists(string commentBodySearch)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


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
        public void Any_ShouldReturnNothing_WithEqualSpecification_WhenCommentDoesNotExists(string commentBodySearch)
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
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }

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
        public async Task AnyAsync_ShouldReturnTrue_WithContainsSpecification_WhenCommentsExists(string commentBodySearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


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
        public async Task AnyAsync_ShouldReturnTrue_WithEqualsSpecification_WhenCommentsExists(string commentBodySearch)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


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
        public async Task AnyAsync_ShouldReturnFalse_WithEqualSpecification_WhenCommentsExists(string commentBodySearch)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


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
        public async Task AnyAsync_ShouldReturnNothing_WithEqualSpecification_WhenCommentDoesNotExists(string commentBodySearch)
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
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }

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
        public void FirstOrDefault_ShouldReturnTrue_WithContainsSpecification_WhenCommentsExists(string commentBodySearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


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
        public void FirstOrDefault_ShouldReturnTrue_WithEqualsSpecification_WhenCommentsExists(string commentBodySearch)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


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
        public void FirstOrDefault_ShouldReturnNothing_WithEqualSpecification_WhenCommentsExists(string commentBodySearch)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


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
        public void FirstOrDefault_ShouldReturnNothing_WithEqualSpecification_WhenCommentDoesNotExists(string commentBodySearch)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


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
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }

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
        public void LastOrDefault_ShouldReturnTrue_WithContainsSpecification_WhenCommentsExists(string commentBodySearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


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
        public void LastOrDefault_ShouldReturnTrue_WithEqualsSpecification_WhenCommentsExists(string commentBodySearch)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


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
        public void LastOrDefault_ShouldReturnNothing_WithEqualSpecification_WhenCommentsExists(string commentBodySearch)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


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
        public void LastOrDefault_ShouldReturnNothing_WithEqualSpecification_WhenCommentDoesNotExists(string commentBodySearch)
        {
            //Arrange
            var random = new Random();
            var commentsList = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                commentsList.Add(new Comment
                {
                    Id = i,
                    CommentBody = $"Comment {i}",
                });
            }


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
        protected PagedListResult<Comment> Search(SearchQuery<Comment> query, List<Comment> commentsList)
        {
            var sequence = commentsList.AsQueryable();

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
                sequence = ((IOrderedQueryable<Comment>)sequence).OrderBy(x => true);
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
            return new PagedListResult<Comment>()
            {
                Entities = result,
                HasNext = hasNext,
                HasPrevious = query.Skip > 0,
                Count = resultCount,
            };
        }

        #endregion

        #region NotTestedYet
        //GenerateQuery(TableFilter tableFilter, string includeProperties = null)
        //GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
        #endregion
    }
}
