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

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagesServiceTests"/> class.
        /// </summary>
        public MessagesServiceTests()
        {
            _messagesRepositoryMock = new Mock<IRepository<Message>>();
            _messagesService = new MessagesService(_messagesRepositoryMock.Object);
        }

        #endregion
    }
}
