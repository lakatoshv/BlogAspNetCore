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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            _commentsRepositoryMock.Setup(x => x.GetAll())
                .Returns(commentslist.AsQueryable());

            //Act
            var comments = _commentsService.GetAll();

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
                    CommentBody = $"Commen {commentId}",
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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }

            var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => commentslist.Where(x => x.CommentBody.Contains(commentBodySearch)).AsQueryable());

            //Act
            var comments = _commentsService.GetAll(specification);

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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => commentslist.Where(x => x.CommentBody.Contains(commentBodySearch)).AsQueryable());

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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => commentslist.Where(x => x.CommentBody.Contains(commentBodySearch)).AsQueryable());

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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => commentslist.Where(x => x.CommentBody.Contains(commentBodySearch)).AsQueryable());

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
            var newcomment = new Comment
            {
                Id = commentId,
                CommentBody = $"Comment {commentId}",
            };
            _commentsRepositoryMock.Setup(x => x.GetById(commentId))
                .Returns(() => newcomment);

            //Act
            var comment = _commentsService.Find(commentId);

            //Assert
            _commentsRepositoryMock.Verify(x => x.GetById(commentId), Times.Once);
        }

        /// <summary>
        /// Find comment.
        /// Should return comment when comment exists.
        /// </summary>
        [Fact]
        public void Find_ShouldReturnComment_WhencommentExists()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var newcomment = new Comment
            {
                Id = commentId,
                CommentBody = $"Comment {commentId}",
            };
            _commentsRepositoryMock.Setup(x => x.GetById(commentId))
                .Returns(() => newcomment);

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
            var newcomment = new Comment
            {
                Id = commentId,
                CommentBody = $"Comment {commentId}",
            };
            _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
                .ReturnsAsync(() => newcomment);

            //Act
            var comment = await _commentsService.FindAsync(commentId);

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
            var newcomment = new Comment
            {
                Id = commentId,
                CommentBody = $"Comment {commentId}",
            };
            _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
                .ReturnsAsync(() => newcomment);

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
            var newcomment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

            _commentsRepositoryMock.Setup(x => x.Insert(newcomment))
                .Callback(() =>
                {
                    newcomment.Id = commentId;
                });

            //Act
            _commentsService.Insert(newcomment);

            //Assert
            _commentsRepositoryMock.Verify(x => x.Insert(newcomment), Times.Once);
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
            var newcomment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

            _commentsRepositoryMock.Setup(x => x.Insert(newcomment))
                .Callback(() =>
                {
                    newcomment.Id = commentId;
                });

            //Act
            _commentsService.Insert(newcomment);

            //Assert
            Assert.NotEqual(0, newcomment.Id);
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
            var newcomment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

            _commentsRepositoryMock.Setup(x => x.InsertAsync(newcomment))
                .Callback(() =>
                {
                    newcomment.Id = commentId;
                });

            //Act
            await _commentsService.InsertAsync(newcomment);

            //Assert
            _commentsRepositoryMock.Verify(x => x.InsertAsync(newcomment), Times.Once);
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
            var newcomment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

            _commentsRepositoryMock.Setup(x => x.InsertAsync(newcomment))
                .Callback(() =>
                {
                    newcomment.Id = commentId;
                });

            //Act
            await _commentsService.InsertAsync(newcomment);

            //Assert
            Assert.NotEqual(0, newcomment.Id);
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
            var newcomment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

            _commentsRepositoryMock.Setup(x => x.Insert(newcomment))
                .Callback(() =>
                {
                    newcomment.Id = commentId;
                });
            _commentsRepositoryMock.Setup(x => x.GetById(commentId))
                .Returns(() => newcomment);

            //Act
            _commentsService.Insert(newcomment);
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
            var newcomment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

            _commentsRepositoryMock.Setup(x => x.Insert(newcomment))
                .Callback(() =>
                {
                    newcomment.Id = commentId;
                });
            _commentsRepositoryMock.Setup(x => x.GetById(commentId))
                .Returns(() => newcomment);

            //Act
            _commentsService.Insert(newcomment);
            var comment = _commentsService.Find(commentId);
            comment.CommentBody = newCommentBody;
            _commentsService.Update(comment);

            //Assert
            Assert.Equal(newCommentBody, comment.CommentBody);
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
            var newcomment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

            _commentsRepositoryMock.Setup(x => x.InsertAsync(newcomment))
                .Callback(() =>
                {
                    newcomment.Id = commentId;
                });
            _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
                .ReturnsAsync(() => newcomment);

            //Act
            await _commentsService.InsertAsync(newcomment);
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
            var newcomment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

            _commentsRepositoryMock.Setup(x => x.InsertAsync(newcomment))
                .Callback(() =>
                {
                    newcomment.Id = commentId;
                });
            _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
                .ReturnsAsync(() => newcomment);

            //Act
            await _commentsService.InsertAsync(newcomment);
            var comment = await _commentsService.FindAsync(commentId);
            comment.CommentBody = newCommentBody;
            await _commentsService.UpdateAsync(comment);

            //Assert
            Assert.Equal(newCommentBody, comment.CommentBody);
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
            var commentId = random.Next(52);
            var newcomment = new Comment
            {
                Id = commentId,
                CommentBody = $"Comment {commentId}",
            };
            _commentsRepositoryMock.Setup(x => x.GetById(commentId))
                .Returns(() => newcomment);

            //Act
            _commentsService.Insert(newcomment);
            var comment = _commentsService.Find(commentId);
            _commentsService.Delete(newcomment);
            _commentsRepositoryMock.Setup(x => x.GetById(commentId))
                .Returns(() => null);
            var deletedcomment = _commentsService.Find(commentId);

            //Assert
            _commentsRepositoryMock.Verify(x => x.Delete(comment), Times.Once);
        }

        /// <summary>
        /// Delete comment.
        /// Should return nothing when comment is deleted.
        /// </summary>
        [Fact]
        public void Delete_ShouldReturnNothing_WhenCommentIsDeleted()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var newcomment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

            _commentsRepositoryMock.Setup(x => x.Insert(newcomment))
                .Callback(() =>
                {
                    newcomment.Id = commentId;
                });
            _commentsRepositoryMock.Setup(x => x.GetById(commentId))
                .Returns(() => newcomment);

            //Act
            _commentsService.Insert(newcomment);
            var comment = _commentsService.Find(commentId);
            _commentsService.Delete(newcomment);
            _commentsRepositoryMock.Setup(x => x.GetById(commentId))
                .Returns(() => null);
            var deletedcomment = _commentsService.Find(commentId);

            //Assert
            Assert.Null(deletedcomment);
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
            var commentId = random.Next(52);
            var newcomment = new Comment
            {
                Id = commentId,
                CommentBody = $"Comment {commentId}",
            };
            _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
                .ReturnsAsync(() => newcomment);

            //Act
            await _commentsService.InsertAsync(newcomment);
            var comment = await _commentsService.FindAsync(commentId);
            await _commentsService.DeleteAsync(newcomment);

            //Assert
            _commentsRepositoryMock.Verify(x => x.DeleteAsync(comment), Times.Once);
        }

        /// <summary>
        /// Async delete comment.
        /// Should return nothing when comment is deleted.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task DeleteAsync_ShouldReturnNothing_WhenCommentIsDeleted()
        {
            //Arrange
            var random = new Random();
            var commentId = random.Next(52);
            var newcomment = new Comment
            {
                CommentBody = $"Comment {commentId}",
            };

            _commentsRepositoryMock.Setup(x => x.InsertAsync(newcomment))
                .Callback(() =>
                {
                    newcomment.Id = commentId;
                });
            _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
                .ReturnsAsync(() => newcomment);

            //Act
            await _commentsService.InsertAsync(newcomment);
            var comment = await _commentsService.FindAsync(commentId);
            await _commentsService.DeleteAsync(newcomment);
            _commentsRepositoryMock.Setup(x => x.GetByIdAsync(commentId))
                .Returns(() => null);
            var deletedcomment = _commentsService.Find(commentId);

            //Assert
            Assert.Null(deletedcomment);
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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }

            var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => commentslist.Any(x => x.CommentBody.Contains(commentBodySearch)));

            //Act
            var areAnycomments = _commentsService.Any(specification);

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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => commentslist.Any(x => x.CommentBody.Contains(commentBodySearch)));

            //Act
            var areAnycomments = _commentsService.Any(specification);

            //Assert
            Assert.True(areAnycomments);
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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => commentslist.Any(x => x.CommentBody.Contains(commentBodySearch)));

            //Act
            var areAnycomments = _commentsService.Any(specification);

            //Assert
            Assert.True(areAnycomments);
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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => commentslist.Any(x => x.CommentBody.Contains(commentBodySearch)));

            //Act
            var areAnycomments = _commentsService.Any(specification);

            //Assert
            Assert.False(areAnycomments);
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
            var areAnycomments = _commentsService.Any(specification);

            //Assert
            Assert.False(areAnycomments);
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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }

            var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => commentslist.Any(x => x.CommentBody.Contains(commentBodySearch)));

            //Act
            var areAnycomments = await _commentsService.AnyAsync(specification);

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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => commentslist.Any(x => x.CommentBody.Contains(commentBodySearch)));

            //Act
            var areAnycomments = await _commentsService.AnyAsync(specification);

            //Assert
            Assert.True(areAnycomments);
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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => commentslist.Any(x => x.CommentBody.Contains(commentBodySearch)));

            //Act
            var areAnycomments = await _commentsService.AnyAsync(specification);

            //Assert
            Assert.True(areAnycomments);
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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => commentslist.Any(x => x.CommentBody.Contains(commentBodySearch)));

            //Act
            var areAnycomments = await _commentsService.AnyAsync(specification);

            //Assert
            Assert.False(areAnycomments);
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
            var areAnycomments = await _commentsService.AnyAsync(specification);

            //Assert
            Assert.False(areAnycomments);
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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }

            var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => commentslist.FirstOrDefault(x => x.CommentBody.Contains(commentBodySearch)));

            //Act
            var comment = _commentsService.FirstOrDefault(specification);

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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => commentslist.FirstOrDefault(x => x.CommentBody.Contains(commentBodySearch)));

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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => commentslist.FirstOrDefault(x => x.CommentBody.Contains(commentBodySearch)));

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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => commentslist.FirstOrDefault(x => x.CommentBody.Contains(commentBodySearch)));

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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }

            var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => commentslist.LastOrDefault(x => x.CommentBody.Contains(commentBodySearch)));

            //Act
            var comment = _commentsService.LastOrDefault(specification);

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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Contains(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => commentslist.LastOrDefault(x => x.CommentBody.Contains(commentBodySearch)));

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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => commentslist.LastOrDefault(x => x.CommentBody.Contains(commentBodySearch)));

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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            var specification = new CommentSpecification(x => x.CommentBody.Equals(commentBodySearch));
            _commentsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => commentslist.LastOrDefault(x => x.CommentBody.Contains(commentBodySearch)));

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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
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
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            /*_generalServiceMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => commentslist);*/

            //Act
            var comments = await _commentsService.GetAllAsync();

            //Assert
            _commentsRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        /// <summary>
        /// Async get all comments.
        /// Should return comments when comments exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        //[Theory]
        //[InlineData(0)]
        public async Task GetAllAsync_ShouldReturncomments_WhenCommentsExists(int notEqualCount)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var commentslist = new List<Comment>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var commentId = i;
                commentslist.Add(new Comment
                {
                    Id = commentId,
                    CommentBody = $"Comment {commentId}",
                });
            }


            _commentsRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => commentslist.AsQueryable());

            //Act
            var comments = await _commentsService.GetAllAsync();

            //Assert
            Assert.NotNull(comments);
            Assert.NotEmpty(comments);
            Assert.NotEqual(notEqualCount, comments.ToList().Count);
        }

        /// <summary>
        /// Async get all comments.
        /// Should return nothing when comments does not exists.
        /// </summary>
        //[Fact]
        public async Task GetAllAsync_ShouldReturnNothing_WhenCommentDoesNotExists()
        {
            //Test failed
            //Arrange
            /*_generalServiceMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => new List<Comment>());*/

            //Act
            var comments = await _commentsService.GetAllAsync();

            //Assert
            Assert.Empty(comments);
        }

        //SearchAsync(SearchQuery<T> searchQuery)
        //GetAllAsync(ISpecification<T> specification)
        //GenerateQuery(TableFilter tableFilter, string includeProperties = null)
        //GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
        #endregion
    }
}
