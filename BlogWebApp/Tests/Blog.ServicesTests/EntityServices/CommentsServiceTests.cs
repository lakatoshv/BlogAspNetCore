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

        #endregion
    }
}
