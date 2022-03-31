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

        #region Get All function

        /// <summary>
        /// Verify that function Get All has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionGetAll_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var messagesList = new List<Message>();

            var sender = new ApplicationUser
            {
                Id = new Guid().ToString(),
                FirstName = "Test fn",
                LastName = "Test ln",
                Email = "test@test.test",
                UserName = "test@test.test"
            };

            for (var i = 0; i < random.Next(100); i++)
            {
                var recipient = new ApplicationUser
                {
                    Id = new Guid().ToString(),
                    FirstName = $"Test fn{i}",
                    LastName = $"Test ln{i}",
                    Email = $"test{i}@test.test",
                    UserName = $"test{i}@test.test"
                };
                messagesList.Add(new Message
                {
                    Id = i,
                    SenderId = sender.Id,
                    Sender = sender,
                    RecipientId = recipient.Id,
                    Recipient = recipient,
                    Subject = $"Test subject{i}",
                    Body = $"Test body{i}"
                });
            }

            _messagesRepositoryMock.Setup(x => x.GetAll())
                .Returns(messagesList.AsQueryable());

            //Act
            _messagesService.GetAll();

            //Assert
            _messagesRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        /// <summary>
        /// Get all messages.
        /// Should return messages when messages exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        [Theory]
        [InlineData(0)]
        public void GetAll_ShouldReturnMessages_WhenMessagesExists(int notEqualCount)
        {
            //Arrange
            var random = new Random();
            var messagesList = new List<Message>();

            var sender = new ApplicationUser
            {
                Id = new Guid().ToString(),
                FirstName = "Test fn",
                LastName = "Test ln",
                Email = "test@test.test",
                UserName = "test@test.test"
            };

            for (var i = 0; i < random.Next(100); i++)
            {
                var recipient = new ApplicationUser
                {
                    Id = new Guid().ToString(),
                    FirstName = $"Test fn{i}",
                    LastName = $"Test ln{i}",
                    Email = $"test{i}@test.test",
                    UserName = $"test{i}@test.test"
                };
                messagesList.Add(new Message
                {
                    Id = i,
                    SenderId = sender.Id,
                    Sender = sender,
                    RecipientId = recipient.Id,
                    Recipient = recipient,
                    Subject = $"Test subject{i}",
                    Body = $"Test body{i}"
                });
            }


            _messagesRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => messagesList.AsQueryable());

            //Act
            var messages = _messagesService.GetAll();

            //Assert
            Assert.NotNull(messages);
            Assert.NotEmpty(messages);
            Assert.NotEqual(notEqualCount, messages.ToList().Count);
        }

        /// <summary>
        /// Get all messages.
        /// Should return nothing when messages does not exists.
        /// </summary>
        [Fact]
        public void GetAll_ShouldReturnNothing_WhenMessagesDoesNotExists()
        {
            //Arrange
            _messagesRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => new List<Message>().AsQueryable());

            //Act
            var messages = _messagesService.GetAll();

            //Assert
            Assert.Empty(messages);
        }

        #endregion

        #region Get all function With Specification

        /// <summary>
        /// Verify that function Get All with specification has been called.
        /// </summary>
        /// <param name="bodySearch">The body search.</param>
        [Theory]
        [InlineData("Test body")]
        public void Verify_FunctionGetAll_WithSpecification_HasBeenCalled(string bodySearch)
        {
            //Arrange
            var random = new Random();
            var messagesList = new List<Message>();

            var sender = new ApplicationUser
            {
                Id = new Guid().ToString(),
                FirstName = "Test fn",
                LastName = "Test ln",
                Email = "test@test.test",
                UserName = "test@test.test"
            };

            for (var i = 0; i < random.Next(100); i++)
            {
                var recipient = new ApplicationUser
                {
                    Id = new Guid().ToString(),
                    FirstName = $"Test fn{i}",
                    LastName = $"Test ln{i}",
                    Email = $"test{i}@test.test",
                    UserName = $"test{i}@test.test"
                };
                messagesList.Add(new Message
                {
                    Id = i,
                    SenderId = sender.Id,
                    Sender = sender,
                    RecipientId = recipient.Id,
                    Recipient = recipient,
                    Subject = $"Test subject{i}",
                    Body = $"Test body{i}"
                });
            }

            var specification = new MessageSpecification(x => x.Body.Contains(bodySearch));
            _messagesRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => messagesList.Where(x => x.Body.Contains(bodySearch)).AsQueryable());

            //Act
            _messagesService.GetAll(specification);

            //Assert
            _messagesRepositoryMock.Verify(x => x.GetAll(specification), Times.Once);
        }

        /// <summary>
        /// Get all messages with specification.
        /// Should return messages with contains specification when messages exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        /// <param name="bodySearch">The body search.</param>
        [Theory]
        [InlineData(0, "Test body")]
        public void GetAll_ShouldReturnMessages_WithContainsSpecification_WhenMessagesExists(int notEqualCount, string bodySearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var messagesList = new List<Message>();

            var sender = new ApplicationUser
            {
                Id = new Guid().ToString(),
                FirstName = "Test fn",
                LastName = "Test ln",
                Email = "test@test.test",
                UserName = "test@test.test"
            };

            for (var i = 0; i < random.Next(100); i++)
            {
                var recipient = new ApplicationUser
                {
                    Id = new Guid().ToString(),
                    FirstName = $"Test fn{i}",
                    LastName = $"Test ln{i}",
                    Email = $"test{i}@test.test",
                    UserName = $"test{i}@test.test"
                };
                messagesList.Add(new Message
                {
                    Id = i,
                    SenderId = sender.Id,
                    Sender = sender,
                    RecipientId = recipient.Id,
                    Recipient = recipient,
                    Subject = $"Test subject{i}",
                    Body = $"Test body{i}"
                });
            }

            var specification = new MessageSpecification(x => x.Body.Contains(bodySearch));
            _messagesRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => messagesList.Where(x => x.Body.Contains(bodySearch)).AsQueryable());

            //Act
            var messages = _messagesService.GetAll(specification);

            //Assert
            Assert.NotNull(messages);
            Assert.NotEmpty(messages);
            Assert.NotEqual(notEqualCount, messages.ToList().Count);
        }


        /// <summary>
        /// Get all messages with specification.
        /// Should return message with equal specification when messages exists.
        /// </summary>
        /// <param name="equalCount">The equal count.</param>
        /// <param name="bodySearch">The body search.</param>
        [Theory]
        [InlineData(1, "Test body0")]
        public void GetAll_ShouldReturnMessage_WithEqualsSpecification_WhenMessagesExists(int equalCount, string bodySearch)
        {
            //Arrange
            var random = new Random();
            var messagesList = new List<Message>();

            var sender = new ApplicationUser
            {
                Id = new Guid().ToString(),
                FirstName = "Test fn",
                LastName = "Test ln",
                Email = "test@test.test",
                UserName = "test@test.test"
            };

            for (var i = 0; i < random.Next(100); i++)
            {
                var recipient = new ApplicationUser
                {
                    Id = new Guid().ToString(),
                    FirstName = $"Test fn{i}",
                    LastName = $"Test ln{i}",
                    Email = $"test{i}@test.test",
                    UserName = $"test{i}@test.test"
                };
                messagesList.Add(new Message
                {
                    Id = i,
                    SenderId = sender.Id,
                    Sender = sender,
                    RecipientId = recipient.Id,
                    Recipient = recipient,
                    Subject = $"Test subject{i}",
                    Body = $"Test body{i}"
                });
            }

            var specification = new MessageSpecification(x => x.Body.Contains(bodySearch));
            _messagesRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => messagesList.Where(x => x.Body.Contains(bodySearch)).AsQueryable());

            //Act
            var messages = _messagesService.GetAll(specification);

            //Assert
            Assert.NotNull(messages);
            Assert.NotEmpty(messages);
            Assert.Equal(equalCount, messages.ToList().Count);
        }

        /// <summary>
        /// Get all messages with specification.
        /// Should return nothing with  when messages does not exists.
        /// </summary>
        /// <param name="equalCount">The equal count.</param>
        /// <param name="bodySearch">The body search.</param>
        [Theory]
        [InlineData(0, "Test body-1")]
        public void GetAll_ShouldReturnNothing_WithEqualSpecification_WhenMessagesExists(int equalCount, string bodySearch)
        {
            //Arrange
            var random = new Random();
            var messagesList = new List<Message>();

            var sender = new ApplicationUser
            {
                Id = new Guid().ToString(),
                FirstName = "Test fn",
                LastName = "Test ln",
                Email = "test@test.test",
                UserName = "test@test.test"
            };

            for (var i = 0; i < random.Next(100); i++)
            {
                var recipient = new ApplicationUser
                {
                    Id = new Guid().ToString(),
                    FirstName = $"Test fn{i}",
                    LastName = $"Test ln{i}",
                    Email = $"test{i}@test.test",
                    UserName = $"test{i}@test.test"
                };
                messagesList.Add(new Message
                {
                    Id = i,
                    SenderId = sender.Id,
                    Sender = sender,
                    RecipientId = recipient.Id,
                    Recipient = recipient,
                    Subject = $"Test subject{i}",
                    Body = $"Test body{i}"
                });
            }

            var specification = new MessageSpecification(x => x.Body.Contains(bodySearch));
            _messagesRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => messagesList.Where(x => x.Body.Contains(bodySearch)).AsQueryable());

            //Act
            var messages = _messagesService.GetAll(specification);

            //Assert
            Assert.NotNull(messages);
            Assert.Empty(messages);
            Assert.Equal(equalCount, messages.ToList().Count);
        }

        /// <summary>
        /// Get all messages.
        /// Should return nothing with  when messages does not exists.
        /// </summary>
        /// <param name="bodySearch">The message search.</param>
        [Theory]
        [InlineData("Tag 0")]
        public void GetAll_ShouldReturnNothing_WithEqualSpecification_WhenMessagesDoesNotExists(string bodySearch)
        {
            //Arrange
            var specification = new MessageSpecification(x => x.Body.Equals(bodySearch));
            _messagesRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => new List<Message>().AsQueryable());

            //Act
            var messages = _messagesService.GetAll();

            //Assert
            Assert.Empty(messages);
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
            var messageId = random.Next(52);

            var sender = new ApplicationUser
            {
                Id = new Guid().ToString(),
                FirstName = "Test fn",
                LastName = "Test ln",
                Email = "test@test.test",
                UserName = "test@test.test"
            };

            var recipient = new ApplicationUser
            {
                Id = new Guid().ToString(),
                FirstName = $"Test fn{messageId}",
                LastName = $"Test ln{messageId}",
                Email = $"test{messageId}@test.test",
                UserName = $"test{messageId}@test.test"
            };
            var newMessage = new Message
            {
                Id = messageId,
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = recipient.Id,
                Recipient = recipient,
                Subject = $"Test subject{messageId}",
                Body = $"Test body{messageId}"
            };
            _messagesRepositoryMock.Setup(x => x.GetById(messageId))
                .Returns(() => newMessage);

            //Act
            _messagesService.Find(messageId);

            //Assert
            _messagesRepositoryMock.Verify(x => x.GetById(messageId), Times.Once);
        }

        /// <summary>
        /// Find message.
        /// Should return message when message exists.
        /// </summary>
        [Fact]
        public void Find_ShouldReturnMessage_WhenMessageExists()
        {
            //Arrange
            var random = new Random();
            var messageId = random.Next(52);

            var sender = new ApplicationUser
            {
                Id = new Guid().ToString(),
                FirstName = "Test fn",
                LastName = "Test ln",
                Email = "test@test.test",
                UserName = "test@test.test"
            };

            var recipient = new ApplicationUser
            {
                Id = new Guid().ToString(),
                FirstName = $"Test fn{messageId}",
                LastName = $"Test ln{messageId}",
                Email = $"test{messageId}@test.test",
                UserName = $"test{messageId}@test.test"
            };
            var newMessage = new Message
            {
                Id = messageId,
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = recipient.Id,
                Recipient = recipient,
                Subject = $"Test subject{messageId}",
                Body = $"Test body{messageId}"
            };
            _messagesRepositoryMock.Setup(x => x.GetById(messageId))
                .Returns(() => newMessage);

            //Act
            var message = _messagesService.Find(messageId);

            //Assert
            Assert.Equal(messageId, message.Id);
        }

        #endregion
    }
}
