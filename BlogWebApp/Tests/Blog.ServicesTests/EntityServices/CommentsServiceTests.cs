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
    /// Comments service tests.
    /// </summary>
    public class CommentsServiceTests
    {
        #region Fields

        /// <summary>
        /// The commnts service.
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
        #endregion
    }
}
