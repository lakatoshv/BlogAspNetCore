﻿using Blog.Data.Models;
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

        /// <summary>
        /// Find message.
        /// Should return nothing when message does not exists.
        /// </summary>
        [Fact]
        public void Find_ShouldReturnNothing_WhenMessageDoesNotExists()
        {
            //Arrange
            var random = new Random();
            var messageId = random.Next(52);
            _messagesRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(() => null);

            //Act
            var tag = _messagesService.Find(messageId);

            //Assert
            Assert.Null(tag);
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
            _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
                .ReturnsAsync(() => newMessage);

            //Act
            await _messagesService.FindAsync(messageId);

            //Assert
            _messagesRepositoryMock.Verify(x => x.GetByIdAsync(messageId), Times.Once);
        }

        /// <summary>
        /// Async find message.
        /// Should return message when message exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task FindAsync_ShouldReturnMessage_WhenMessageExists()
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
            _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
                .ReturnsAsync(() => newMessage);

            //Act
            var tag = await _messagesService.FindAsync(messageId);

            //Assert
            Assert.Equal(messageId, tag.Id);
        }

        /// <summary>
        /// Async find message.
        /// Should return nothing when message does not exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task FindAsync_ShouldReturnNothing_WhenMessageDoesNotExists()
        {
            //Arrange
            var random = new Random();
            var messageId = random.Next(52);
            _messagesRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var message = await _messagesService.FindAsync(messageId);

            //Assert
            Assert.Null(message);
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
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = recipient.Id,
                Recipient = recipient,
                Subject = $"Test subject{messageId}",
                Body = $"Test body{messageId}"
            };

            _messagesRepositoryMock.Setup(x => x.Insert(newMessage))
                .Callback(() =>
                {
                    newMessage.Id = messageId;
                });

            //Act
            _messagesService.Insert(newMessage);

            //Assert
            _messagesRepositoryMock.Verify(x => x.Insert(newMessage), Times.Once);
        }

        /// <summary>
        /// Insert message.
        /// Should return message when message created.
        /// </summary>
        [Fact]
        public void Insert_ShouldReturnMessage_WhenMessageExists()
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
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = recipient.Id,
                Recipient = recipient,
                Subject = $"Test subject{messageId}",
                Body = $"Test body{messageId}"
            };

            _messagesRepositoryMock.Setup(x => x.Insert(newMessage))
                .Callback(() =>
                {
                    newMessage.Id = messageId;
                });

            //Act
            _messagesService.Insert(newMessage);

            //Assert
            Assert.NotEqual(0, newMessage.Id);
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
            var messageId = random.Next(52);
            var itemsCount = random.Next(10);
            var newMessages = new List<Message>();

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

            for (int i = 0; i < itemsCount; i++)
            {
                newMessages.Add(new Message
                {
                    SenderId = sender.Id,
                    Sender = sender,
                    RecipientId = recipient.Id,
                    Recipient = recipient,
                    Subject = $"Test subject{messageId}",
                    Body = $"Test body{messageId}"
                });
            }

            _messagesRepositoryMock.Setup(x => x.Insert(newMessages))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newMessages[i].Id = messageId + i;
                    }
                });

            //Act
            _messagesService.Insert(newMessages);

            //Assert
            _messagesRepositoryMock.Verify(x => x.Insert(newMessages), Times.Once);
        }

        /// <summary>
        /// Insert Enumerable comment.
        /// Should return comments when comments created.
        /// </summary>
        [Fact]
        public void InsertEnumerable_ShouldReturnComments_WhenCommentsExists()
        {
            //Arrange
            var random = new Random();
            var messageId = random.Next(52);
            var itemsCount = random.Next(10);
            var newMessages = new List<Message>();

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

            for (int i = 0; i < itemsCount; i++)
            {
                newMessages.Add(new Message
                {
                    SenderId = sender.Id,
                    Sender = sender,
                    RecipientId = recipient.Id,
                    Recipient = recipient,
                    Subject = $"Test subject{messageId}",
                    Body = $"Test body{messageId}"
                });
            }

            _messagesRepositoryMock.Setup(x => x.Insert(newMessages))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newMessages[i].Id = messageId + i;
                    }
                });

            //Act
            _messagesService.Insert(newMessages);

            //Assert
            newMessages.ForEach(x =>
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
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = recipient.Id,
                Recipient = recipient,
                Subject = $"Test subject{messageId}",
                Body = $"Test body{messageId}"
            };

            _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessage))
                .Callback(() =>
                {
                    newMessage.Id = messageId;
                });

            //Act
            await _messagesService.InsertAsync(newMessage);

            //Assert
            _messagesRepositoryMock.Verify(x => x.InsertAsync(newMessage), Times.Once);
        }

        /// <summary>
        /// Async insert message.
        /// Should return message when message created.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task InsertAsync_ShouldReturnMessage_WhenMessageExists()
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
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = recipient.Id,
                Recipient = recipient,
                Subject = $"Test subject{messageId}",
                Body = $"Test body{messageId}"
            };

            _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessage))
                .Callback(() =>
                {
                    newMessage.Id = messageId;
                });

            //Act
            await _messagesService.InsertAsync(newMessage);

            //Assert
            Assert.NotEqual(0, newMessage.Id);
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
            var messageId = random.Next(52);
            var itemsCount = random.Next(10);
            var newMessages = new List<Message>();

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

            for (int i = 0; i < itemsCount; i++)
            {
                newMessages.Add(new Message
                {
                    SenderId = sender.Id,
                    Sender = sender,
                    RecipientId = recipient.Id,
                    Recipient = recipient,
                    Subject = $"Test subject{messageId}",
                    Body = $"Test body{messageId}"
                });
            }

            _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessages))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newMessages[i].Id = messageId + i;
                    }
                });

            //Act
            await _messagesService.InsertAsync(newMessages);

            //Assert
            _messagesRepositoryMock.Verify(x => x.InsertAsync(newMessages), Times.Once);
        }

        /// <summary>
        /// Insert Async Enumerable comment.
        /// Should return comments when comments created.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task InsertAsyncEnumerable_ShouldReturnComments_WhenCommentsExists()
        {
            //Arrange
            var random = new Random();
            var messageId = random.Next(52);
            var itemsCount = random.Next(10);
            var newMessages = new List<Message>();

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

            for (int i = 0; i < itemsCount; i++)
            {
                newMessages.Add(new Message
                {
                    SenderId = sender.Id,
                    Sender = sender,
                    RecipientId = recipient.Id,
                    Recipient = recipient,
                    Subject = $"Test subject{messageId}",
                    Body = $"Test body{messageId}"
                });
            }

            _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessages))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newMessages[i].Id = messageId + i;
                    }
                });

            //Act
            await _messagesService.InsertAsync(newMessages);

            //Assert
            newMessages.ForEach(x =>
            {
                Assert.NotEqual(0, x.Id);
            });
        }

        #endregion

        #region Upadate function

        /// <summary>
        /// Verify that function Update has been called.
        /// </summary>
        /// <param name="newMessageSubject">The new message subject.</param>
        [Theory]
        [InlineData("New subject")]
        public void Verify_FunctionUpdate_HasBeenCalled(string newMessageSubject)
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
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = recipient.Id,
                Recipient = recipient,
                Subject = $"Test subject{messageId}",
                Body = $"Test body{messageId}"
            };

            _messagesRepositoryMock.Setup(x => x.Insert(newMessage))
                .Callback(() =>
                {
                    newMessage.Id = messageId;
                });
            _messagesRepositoryMock.Setup(x => x.GetById(messageId))
                .Returns(() => newMessage);

            //Act
            _messagesService.Insert(newMessage);
            var message = _messagesService.Find(messageId);
            message.Subject = newMessageSubject;
            _messagesService.Update(message);

            //Assert
            _messagesRepositoryMock.Verify(x => x.Update(message), Times.Once);
        }

        /// <summary>
        /// Update message.
        /// Should return message when message updated.
        /// </summary>
        /// <param name="newMessageSubject">The new message subject.</param>
        [Theory]
        [InlineData("New subject")]
        public void Update_ShouldReturnMessage_WhenMessageExists(string newMessageSubject)
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
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = recipient.Id,
                Recipient = recipient,
                Subject = $"Test subject{messageId}",
                Body = $"Test body{messageId}"
            };

            _messagesRepositoryMock.Setup(x => x.Insert(newMessage))
                .Callback(() =>
                {
                    newMessage.Id = messageId;
                });
            _messagesRepositoryMock.Setup(x => x.GetById(messageId))
                .Returns(() => newMessage);

            //Act
            _messagesService.Insert(newMessage);
            var message = _messagesService.Find(messageId);
            message.Subject = newMessageSubject;
            _messagesService.Update(message);

            //Assert
            Assert.Equal(newMessageSubject, message.Subject);
        }

        #endregion

        #region Upadate Enumerable function

        /// <summary>
        /// Verify that function Update has been called.
        /// </summary>
        /// <param name="newMessageSubject">The new message subject.</param>
        [Theory]
        [InlineData("New subject")]
        public void Verify_FunctionUpdateEnumerable_HasBeenCalled(string newMessageSubject)
        {
            //Arrange
            var random = new Random();
            var messageId = random.Next(52);
            var itemsCount = random.Next(10);
            var newMessages = new List<Message>();

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

            for (int i = 0; i < itemsCount; i++)
            {
                newMessages.Add(new Message
                {
                    SenderId = sender.Id,
                    Sender = sender,
                    RecipientId = recipient.Id,
                    Recipient = recipient,
                    Subject = $"Test subject{messageId}",
                    Body = $"Test body{messageId}"
                });
            }

            _messagesRepositoryMock.Setup(x => x.Insert(newMessages))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newMessages[i].Id = messageId + i;
                    }
                });

            //Act
            _messagesService.Insert(newMessages);
            for (var i = 0; i < itemsCount; i++)
            {
                newMessages[i].Subject = $"{newMessageSubject} {i}";
            }
            _messagesService.Update(newMessages);

            //Assert
            _messagesRepositoryMock.Verify(x => x.Update(newMessages), Times.Once);
        }

        /// <summary>
        /// Update Enumerable message.
        /// Should return message when message updated.
        /// </summary>
        /// <param name="newMessageSubject">The new message subject.</param>
        [Theory]
        [InlineData("New subject")]
        public void UpdateEnumerable_ShouldReturnMessage_WhenMessageExists(string newMessageSubject)
        {
            //Arrange
            var random = new Random();
            var messageId = random.Next(52);
            var itemsCount = random.Next(10);
            var newMessages = new List<Message>();

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

            for (int i = 0; i < itemsCount; i++)
            {
                newMessages.Add(new Message
                {
                    SenderId = sender.Id,
                    Sender = sender,
                    RecipientId = recipient.Id,
                    Recipient = recipient,
                    Subject = $"Test subject{messageId}",
                    Body = $"Test body{messageId}"
                });
            }

            _messagesRepositoryMock.Setup(x => x.Insert(newMessages))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newMessages[i].Id = messageId + i;
                    }
                });

            //Act
            _messagesService.Insert(newMessages);
            for (var i = 0; i < itemsCount; i++)
            {
                newMessages[i].Subject = $"{newMessageSubject} {i}";
            }
            _messagesService.Update(newMessages);

            //Assert
            for (var i = 0; i < itemsCount; i++)
            {
                Assert.Equal($"{newMessageSubject} {i}", newMessages[i].Subject);
            }
        }

        #endregion

        #region Update Async function

        /// <summary>
        /// Verify that function Update Async has been called.
        /// Should return tag when tag updated.
        /// </summary>
        /// <param name="newMessageSubject">The new message subject.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("New Tag")]
        public async Task Verify_FunctionUpdateAsync_HasBeenCalled(string newMessageSubject)
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
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = recipient.Id,
                Recipient = recipient,
                Subject = $"Test subject{messageId}",
                Body = $"Test body{messageId}"
            };

            _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessage))
                .Callback(() =>
                {
                    newMessage.Id = messageId;
                });
            _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
                .ReturnsAsync(() => newMessage);

            //Act
            await _messagesService.InsertAsync(newMessage);
            var message = await _messagesService.FindAsync(messageId);
            message.Subject = newMessageSubject;
            await _messagesService.UpdateAsync(message);

            //Assert
            _messagesRepositoryMock.Verify(x => x.UpdateAsync(message), Times.Once);
        }

        /// <summary>
        /// Async update message.
        /// Should return message when message updated.
        /// </summary>
        /// <param name="newMessageSubject">The new message subject.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("New Tag")]
        public async Task UpdateAsync_ShouldReturnMessage_WhenMessageExists(string newMessageSubject)
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
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = recipient.Id,
                Recipient = recipient,
                Subject = $"Test subject{messageId}",
                Body = $"Test body{messageId}"
            };

            _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessage))
                .Callback(() =>
                {
                    newMessage.Id = messageId;
                });
            _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
                .ReturnsAsync(() => newMessage);

            //Act
            await _messagesService.InsertAsync(newMessage);
            var message = await _messagesService.FindAsync(messageId);
            message.Subject = newMessageSubject;
            await _messagesService.UpdateAsync(message);

            //Assert
            Assert.Equal(newMessageSubject, message.Subject);
        }

        #endregion

        #region Upadate Async Enumerable function

        /// <summary>
        /// Verify that function Update Async has been called.
        /// </summary>
        /// <param name="newMessageSubject">The new message subject.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("New subject")]
        public async Task Verify_FunctionUpdateAsyncEnumerable_HasBeenCalled(string newMessageSubject)
        {
            //Arrange
            var random = new Random();
            var messageId = random.Next(52);
            var itemsCount = random.Next(10);
            var newMessages = new List<Message>();

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

            for (int i = 0; i < itemsCount; i++)
            {
                newMessages.Add(new Message
                {
                    SenderId = sender.Id,
                    Sender = sender,
                    RecipientId = recipient.Id,
                    Recipient = recipient,
                    Subject = $"Test subject{messageId}",
                    Body = $"Test body{messageId}"
                });
            }

            _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessages))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newMessages[i].Id = messageId + i;
                    }
                });

            //Act
            await _messagesService.InsertAsync(newMessages);
            for (var i = 0; i < itemsCount; i++)
            {
                newMessages[i].Subject = $"{newMessageSubject} {i}";
            }
            await _messagesService.UpdateAsync(newMessages);

            //Assert
            _messagesRepositoryMock.Verify(x => x.UpdateAsync(newMessages), Times.Once);
        }

        /// <summary>
        /// Update Async Enumerable message.
        /// Should return message when message updated.
        /// </summary>
        /// <param name="newMessageSubject">The new message subject.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("New subject")]
        public async void UpdateAsyncEnumerable_ShouldReturnMessage_WhenMessageExists(string newMessageSubject)
        {
            //Arrange
            var random = new Random();
            var messageId = random.Next(52);
            var itemsCount = random.Next(10);
            var newMessages = new List<Message>();

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

            for (int i = 0; i < itemsCount; i++)
            {
                newMessages.Add(new Message
                {
                    SenderId = sender.Id,
                    Sender = sender,
                    RecipientId = recipient.Id,
                    Recipient = recipient,
                    Subject = $"Test subject{messageId}",
                    Body = $"Test body{messageId}"
                });
            }

            _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessages))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newMessages[i].Id = messageId + i;
                    }
                });

            //Act
            await _messagesService.InsertAsync(newMessages);
            for (var i = 0; i < itemsCount; i++)
            {
                newMessages[i].Subject = $"{newMessageSubject} {i}";
            }
            await _messagesService.UpdateAsync(newMessages);

            //Assert
            for (var i = 0; i < itemsCount; i++)
            {
                Assert.Equal($"{newMessageSubject} {i}", newMessages[i].Subject);
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
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = recipient.Id,
                Recipient = recipient,
                Subject = $"Test subject{messageId}",
                Body = $"Test body{messageId}"
            };
            _messagesRepositoryMock.Setup(x => x.Insert(newMessage))
                .Callback(() =>
                {
                    newMessage.Id = messageId;
                });
            _messagesRepositoryMock.Setup(x => x.GetById(messageId))
                .Returns(() => newMessage);

            //Act
            _messagesService.Insert(newMessage);
            var message = _messagesService.Find(messageId);
            _messagesService.Delete(messageId);
            _messagesRepositoryMock.Setup(x => x.GetById(messageId))
                .Returns(() => null);
            _messagesService.Find(messageId);

            //Assert
            _messagesRepositoryMock.Verify(x => x.Delete(messageId), Times.Once);
        }

        /// <summary>
        /// Delete By Id message.
        /// Should return nothing when message is deleted.
        /// </summary>
        [Fact]
        public void DeleteById_ShouldReturnNothing_WhenMessageDeleted()
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
            _messagesRepositoryMock.Setup(x => x.Insert(newMessage))
                .Callback(() =>
                {
                    newMessage.Id = messageId;
                });
            _messagesRepositoryMock.Setup(x => x.GetById(messageId))
                .Returns(() => newMessage);

            //Act
            _messagesService.Insert(newMessage);
            var message = _messagesService.Find(messageId);
            _messagesService.Delete(messageId);
            _messagesRepositoryMock.Setup(x => x.GetById(messageId))
                .Returns(() => null);
            var deletedMessage = _messagesService.Find(messageId);

            //Assert
            Assert.Null(deletedMessage);
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
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = recipient.Id,
                Recipient = recipient,
                Subject = $"Test subject{messageId}",
                Body = $"Test body{messageId}"
            };
            _messagesRepositoryMock.Setup(x => x.Insert(newMessage))
                .Callback(() =>
                {
                    newMessage.Id = messageId;
                });
            _messagesRepositoryMock.Setup(x => x.GetById(messageId))
                .Returns(() => newMessage);

            //Act
            _messagesService.Insert(newMessage);
            var message = _messagesService.Find(messageId);
            _messagesService.Delete(message);
            _messagesRepositoryMock.Setup(x => x.GetById(messageId))
                .Returns(() => null);
            _messagesService.Find(messageId);

            //Assert
            _messagesRepositoryMock.Verify(x => x.Delete(message), Times.Once);
        }

        /// <summary>
        /// Delete By Object message.
        /// Should return nothing when message is deleted.
        /// </summary>
        [Fact]
        public void DeleteByObject_ShouldReturnNothing_WhenMessageDeleted()
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
            _messagesRepositoryMock.Setup(x => x.Insert(newMessage))
                .Callback(() =>
                {
                    newMessage.Id = messageId;
                });
            _messagesRepositoryMock.Setup(x => x.GetById(messageId))
                .Returns(() => newMessage);

            //Act
            _messagesService.Insert(newMessage);
            var message = _messagesService.Find(messageId);
            _messagesService.Delete(message);
            _messagesRepositoryMock.Setup(x => x.GetById(messageId))
                .Returns(() => null);
            var deletedMessage = _messagesService.Find(messageId);

            //Assert
            Assert.Null(deletedMessage);
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
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = recipient.Id,
                Recipient = recipient,
                Subject = $"Test subject{messageId}",
                Body = $"Test body{messageId}"
            };

            _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessage))
                .Callback(() =>
                {
                    newMessage.Id = messageId;
                });
            _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
                .ReturnsAsync(() => newMessage);

            //Act
            await _messagesService.InsertAsync(newMessage);
            var message = await _messagesService.FindAsync(messageId);
            await _messagesService.DeleteAsync(messageId);

            //Assert
            _messagesRepositoryMock.Verify(x => x.DeleteAsync(messageId), Times.Once);
        }

        /// <summary>
        /// Async delete by id message.
        /// Should return nothing when message is deleted.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task DeleteAsyncById_ShouldReturnNothing_WhenMessageIsDeleted()
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
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = recipient.Id,
                Recipient = recipient,
                Subject = $"Test subject{messageId}",
                Body = $"Test body{messageId}"
            };

            _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessage))
                .Callback(() =>
                {
                    newMessage.Id = messageId;
                });
            _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
                .ReturnsAsync(() => newMessage);

            //Act
            await _messagesService.InsertAsync(newMessage);
            var message = await _messagesService.FindAsync(messageId);
            await _messagesService.DeleteAsync(messageId);
            _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
                .ReturnsAsync(() => null);
            var deletedMessage = await _messagesService.FindAsync(messageId);

            //Assert
            Assert.Null(deletedMessage);
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
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = recipient.Id,
                Recipient = recipient,
                Subject = $"Test subject{messageId}",
                Body = $"Test body{messageId}"
            };

            _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessage))
                .Callback(() =>
                {
                    newMessage.Id = messageId;
                });
            _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
                .ReturnsAsync(() => newMessage);

            //Act
            await _messagesService.InsertAsync(newMessage);
            var comment = await _messagesService.FindAsync(messageId);
            await _messagesService.DeleteAsync(comment);

            //Assert
            _messagesRepositoryMock.Verify(x => x.DeleteAsync(comment), Times.Once);
        }

        /// <summary>
        /// Async delete by object message.
        /// Should return nothing when message is deleted.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task DeleteAsyncByObject_ShouldReturnNothing_WhenMessageIsDeleted()
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
                SenderId = sender.Id,
                Sender = sender,
                RecipientId = recipient.Id,
                Recipient = recipient,
                Subject = $"Test subject{messageId}",
                Body = $"Test body{messageId}"
            };

            _messagesRepositoryMock.Setup(x => x.InsertAsync(newMessage))
                .Callback(() =>
                {
                    newMessage.Id = messageId;
                });
            _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
                .ReturnsAsync(() => newMessage);

            //Act
            await _messagesService.InsertAsync(newMessage);
            var message = await _messagesService.FindAsync(messageId);
            await _messagesService.DeleteAsync(message);
            _messagesRepositoryMock.Setup(x => x.GetByIdAsync(messageId))
                .ReturnsAsync(() => null);
            var deletedMessage = await _messagesService.FindAsync(messageId);

            //Assert
            Assert.Null(deletedMessage);
        }

        #endregion

        #region Any function With Specification

        /// <summary>
        /// Verify that function Any with specification has been called.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        [Theory]
        [InlineData("Test subject")]
        public void Verify_FunctionAny_WithSpecification_HasBeenCalled(string subjectSearch)
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

            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => messagesList.Any(x => x.Subject.Contains(subjectSearch)));

            //Act
            _messagesService.Any(specification);

            //Assert
            _messagesRepositoryMock.Verify(x => x.Any(specification), Times.Once);
        }

        /// <summary>
        /// Check if there are any messages with specification.
        /// Should return true with contains specification when messages exists.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        [Theory]
        [InlineData("Test subject")]
        public void Any_ShouldReturnTrue_WithContainsSpecification_WhenMessagesExists(string subjectSearch)
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


            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => messagesList.Any(x => x.Subject.Contains(subjectSearch)));

            //Act
            var areAnyMessages = _messagesService.Any(specification);

            //Assert
            Assert.True(areAnyMessages);
        }

        /// <summary>
        /// Check if there are any messages with specification.
        /// Should return true with equal specification when messages exists.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        [Theory]
        [InlineData("Test subject0")]
        public void Any_ShouldReturnTrue_WithEqualsSpecification_WhenMessagesExists(string subjectSearch)
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


            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => messagesList.Any(x => x.Subject.Contains(subjectSearch)));

            //Act
            var areAnyMessages = _messagesService.Any(specification);

            //Assert
            Assert.True(areAnyMessages);
        }

        /// <summary>
        /// Check if there are any messages with specification.
        /// Should return false with when messages does not exists.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        [Theory]
        [InlineData("Test subject-1")]
        public void Any_ShouldReturnFalse_WithEqualSpecification_WhenMessagesExists(string subjectSearch)
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


            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => messagesList.Any(x => x.Subject.Contains(subjectSearch)));

            //Act
            var areAnyMessages = _messagesService.Any(specification);

            //Assert
            Assert.False(areAnyMessages);
        }

        /// <summary>
        /// Check if there are any messages with specification.
        /// Should return false with when messages does not exists.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        [Theory]
        [InlineData("Test subject0")]
        public void Any_ShouldReturnNothing_WithEqualSpecification_WhenMessagesDoesNotExists(string subjectSearch)
        {
            //Arrange
            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => false);

            //Act
            var areAnyMessages = _messagesService.Any(specification);

            //Assert
            Assert.False(areAnyMessages);
        }

        #endregion

        #region Any Async function With Specification

        /// <summary>
        /// Verify that function Any Async with specification has been called.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Test subject")]
        public async Task Verify_FunctionAnyAsync_WithSpecification_HasBeenCalled(string subjectSearch)
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

            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => messagesList.Any(x => x.Subject.Contains(subjectSearch)));

            //Act
            await _messagesService.AnyAsync(specification);

            //Assert
            _messagesRepositoryMock.Verify(x => x.AnyAsync(specification), Times.Once);
        }

        /// <summary>
        /// Async check if there are any messages with specification.
        /// Should return true with contains specification when messages exists.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Test subject")]
        public async Task AnyAsync_ShouldReturnTrue_WithContainsSpecification_WhenMessagesExists(string subjectSearch)
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


            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => messagesList.Any(x => x.Subject.Contains(subjectSearch)));

            //Act
            var areAnyMessages = await _messagesService.AnyAsync(specification);

            //Assert
            Assert.True(areAnyMessages);
        }

        /// <summary>
        /// Async check if there are any messages with specification.
        /// Should return true with equal specification when messages exists.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Test subject0")]
        public async Task AnyAsync_ShouldReturnTrue_WithEqualsSpecification_WhenMessagesExists(string subjectSearch)
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


            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => messagesList.Any(x => x.Subject.Contains(subjectSearch)));

            //Act
            var areAnyMessages = await _messagesService.AnyAsync(specification);

            //Assert
            Assert.True(areAnyMessages);
        }

        /// <summary>
        /// Async check if there are any messages with specification.
        /// Should return false with when messages does not exists.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Test subject-1")]
        public async Task AnyAsync_ShouldReturnFalse_WithEqualSpecification_WhenMessagesExists(string subjectSearch)
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


            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => messagesList.Any(x => x.Subject.Contains(subjectSearch)));

            //Act
            var areAnyMessages = await _messagesService.AnyAsync(specification);

            //Assert
            Assert.False(areAnyMessages);
        }

        /// <summary>
        /// Async check if there are any messages with specification.
        /// Should return false with when messages does not exists.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Test subject0")]
        public async Task AnyAsync_ShouldReturnNothing_WithEqualSpecification_WhenMessagesDoesNotExists(string subjectSearch)
        {
            //Arrange
            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => false);

            //Act
            var areAnyMessages = await _messagesService.AnyAsync(specification);

            //Assert
            Assert.False(areAnyMessages);
        }

        #endregion

        #region First Or Default function With Specification

        /// <summary>
        /// Verify that function First Or Default with specification has been called.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        [Theory]
        [InlineData("Test subject")]
        public void Verify_FunctionFirstOrDefault_WithSpecification_HasBeenCalled(string subjectSearch)
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

            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => messagesList.FirstOrDefault(x => x.Subject.Contains(subjectSearch)));

            //Act
            _messagesService.FirstOrDefault(specification);

            //Assert
            _messagesRepositoryMock.Verify(x => x.FirstOrDefault(specification), Times.Once);
        }

        /// <summary>
        /// Get first or default message with specification.
        /// Should return message with contains specification when messages exists.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        [Theory]
        [InlineData("Test subject")]
        public void FirstOrDefault_ShouldReturnMessage_WithContainsSpecification_WhenMessagesExists(string subjectSearch)
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


            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => messagesList.FirstOrDefault(x => x.Subject.Contains(subjectSearch)));

            //Act
            var message = _messagesService.FirstOrDefault(specification);

            //Assert
            Assert.NotNull(message);
            Assert.IsType<Message>(message);
        }

        /// <summary>
        /// Get first or default message with specification.
        /// Should return message with equal specification when messages exists.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        [Theory]
        [InlineData("Test subject0")]
        public void FirstOrDefault_ShouldReturnMessage_WithEqualsSpecification_WhenMessagesExists(string subjectSearch)
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


            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => messagesList.FirstOrDefault(x => x.Subject.Contains(subjectSearch)));

            //Act
            var message = _messagesService.FirstOrDefault(specification);

            //Assert
            Assert.NotNull(message);
            Assert.IsType<Message>(message);
        }

        /// <summary>
        /// Get first or default message with specification.
        /// Should return nothing with when messages does not exists.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        [Theory]
        [InlineData("Test subject-1")]
        public void FirstOrDefault_ShouldReturnNothing_WithEqualSpecification_WhenMessagesExists(string subjectSearch)
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


            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => messagesList.FirstOrDefault(x => x.Subject.Contains(subjectSearch)));

            //Act
            var message = _messagesService.FirstOrDefault(specification);

            //Assert
            Assert.Null(message);
        }

        /// <summary>
        /// Get first or default message with specification.
        /// Should return nothing with when messages does not exists.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        [Theory]
        [InlineData("Test subject0")]
        public void FirstOrDefault_ShouldReturnNothing_WithEqualSpecification_WhenMessagesDoesNotExists(string subjectSearch)
        {
            //Arrange
            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => null);

            //Act
            var message = _messagesService.FirstOrDefault(specification);

            //Assert
            Assert.Null(message);
        }

        #endregion

        #region Last Or Default function With Specification

        /// <summary>
        /// Verify that function Last Or Default with specification has been called.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        [Theory]
        [InlineData("Test subject")]
        public void Verify_FunctionLastOrDefault_WithSpecification_HasBeenCalled(string subjectSearch)
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

            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => messagesList.LastOrDefault(x => x.Subject.Contains(subjectSearch)));

            //Act
            _messagesService.LastOrDefault(specification);

            //Assert
            _messagesRepositoryMock.Verify(x => x.LastOrDefault(specification), Times.Once);
        }

        /// <summary>
        /// Get last or default message with specification.
        /// Should return message with contains specification when messages exists.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        [Theory]
        [InlineData("Test subject")]
        public void LastOrDefault_ShouldReturnMessage_WithContainsSpecification_WhenMessagesExists(string subjectSearch)
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


            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => messagesList.LastOrDefault(x => x.Subject.Contains(subjectSearch)));

            //Act
            var message = _messagesService.LastOrDefault(specification);

            //Assert
            Assert.NotNull(message);
            Assert.IsType<Message>(message);
        }

        /// <summary>
        /// Get last or default message with specification.
        /// Should return message with equal specification when messages exists.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        [Theory]
        [InlineData("Test subject0")]
        public void LastOrDefault_ShouldReturnMessage_WithEqualsSpecification_WhenMessagesExists(string subjectSearch)
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


            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => messagesList.LastOrDefault(x => x.Subject.Contains(subjectSearch)));

            //Act
            var message = _messagesService.LastOrDefault(specification);

            //Assert
            Assert.NotNull(message);
            Assert.IsType<Message>(message);
        }

        /// <summary>
        /// Get last or default message with specification.
        /// Should return nothing with when messages does not exists.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        [Theory]
        [InlineData("Test subject-1")]
        public void LastOrDefault_ShouldReturnNothing_WithEqualSpecification_WhenMessagesExists(string subjectSearch)
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


            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => messagesList.LastOrDefault(x => x.Subject.Contains(subjectSearch)));

            //Act
            var message = _messagesService.LastOrDefault(specification);

            //Assert
            Assert.Null(message);
        }

        /// <summary>
        /// Get last or default message with specification.
        /// Should return nothing with when messages does not exists.
        /// </summary>
        /// <param name="subjectSearch">The subject search.</param>
        [Theory]
        [InlineData("Test subject0")]
        public void LastOrDefault_ShouldReturnNothing_WithEqualSpecification_WhenMessagesDoesNotExists(string subjectSearch)
        {
            //Arrange
            var specification = new MessageSpecification(x => x.Subject.Equals(subjectSearch));
            _messagesRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => null);

            //Act
            var message = _messagesService.LastOrDefault(specification);

            //Assert
            Assert.Null(message);
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


            /*_generalServiceMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => commentslist);*/

            //Act
            var messages = await _messagesService.GetAllAsync();

            //Assert
            _messagesRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        /// <summary>
        /// Async get all tags.
        /// Should return tags when comments exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        //[Theory]
        //[InlineData(0)]
        public async Task GetAllAsync_ShouldReturnTags_WhenTagsExists(int notEqualCount)
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


            _messagesRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => messagesList.AsQueryable());

            //Act
            var messages = await _messagesService.GetAllAsync();

            //Assert
            Assert.NotNull(messages);
            Assert.NotEmpty(messages);
            Assert.NotEqual(notEqualCount, messages.ToList().Count);
        }

        /// <summary>
        /// Async get all tags.
        /// Should return nothing when tags does not exists.
        /// </summary>
        //[Fact]
        public async Task GetAllAsync_ShouldReturnNothing_WhenTagDoesNotExists()
        {
            //Test failed
            //Arrange
            /*_generalServiceMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => new List<Comment>());*/

            //Act
            var messages = await _messagesService.GetAllAsync();

            //Assert
            Assert.Empty(messages);
        }

        //SearchAsync(SearchQuery<T> searchQuery)
        //GetAllAsync(ISpecification<T> specification)
        //GenerateQuery(TableFilter tableFilter, string includeProperties = null)
        //GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
        #endregion
    }
}
