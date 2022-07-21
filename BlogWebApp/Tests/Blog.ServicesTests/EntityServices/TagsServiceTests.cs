using AutoMapper;
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
    /// Tags service tests.
    /// </summary>
    public class TagsServiceTests
    {
        #region Fields

        /// <summary>
        /// The tags service.
        /// </summary>
        private readonly ITagsService _tagsService;

        /// <summary>
        /// The tags repository mock.
        /// </summary>
        private readonly Mock<IRepository<Tag>> _tagsRepositoryMock;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="TagsServiceTests"/> class.
        /// </summary>
        public TagsServiceTests()
        {
            _tagsRepositoryMock = new Mock<IRepository<Tag>>();
            _tagsService = new TagsService(_tagsRepositoryMock.Object);
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
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            _tagsRepositoryMock.Setup(x => x.GetAll())
                .Returns(tagsList.AsQueryable());

            //Act
            _tagsService.GetAll();

            //Assert
            _tagsRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        /// <summary>
        /// Get all tags.
        /// Should return tags when tags exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        [Theory]
        [InlineData(0)]
        public void GetAll_ShouldReturnTags_WhenTagsExists(int notEqualCount)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            _tagsRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => tagsList.AsQueryable());

            //Act
            var tags = _tagsService.GetAll();

            //Assert
            Assert.NotNull(tags);
            Assert.NotEmpty(tags);
            Assert.NotEqual(notEqualCount, tags.ToList().Count);
        }

        /// <summary>
        /// Get all tags.
        /// Should return nothing when tags does not exists.
        /// </summary>
        [Fact]
        public void GetAll_ShouldReturnNothing_WhenTagsDoesNotExists()
        {
            //Arrange
            _tagsRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => new List<Tag>().AsQueryable());

            //Act
            var tags = _tagsService.GetAll();

            //Assert
            Assert.Empty(tags);
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
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            _tagsRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(tagsList);

            //Act
            await _tagsService.GetAllAsync();

            //Assert
            _tagsRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        /// <summary>
        /// Get all async tags.
        /// Should return tags when tags exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        [Theory]
        [InlineData(0)]
        public async Task GetAllAsync_ShouldReturnTags_WhenTagsExists(int notEqualCount)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            _tagsRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => tagsList);

            //Act
            var tags = await _tagsService.GetAllAsync();

            //Assert
            Assert.NotNull(tags);
            Assert.NotEmpty(tags);
            Assert.NotEqual(notEqualCount, tags.ToList().Count);
        }

        /// <summary>
        /// Get all async tags.
        /// Should return nothing when tags does not exists.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_ShouldReturnNothing_WhenTagsDoesNotExists()
        {
            //Arrange
            _tagsRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => new List<Tag>());

            //Act
            var tags = await _tagsService.GetAllAsync();

            //Assert
            Assert.Empty(tags);
        }

        #endregion

        #region Get all function With Specification

        /// <summary>
        /// Verify that function Get All with specification has been called.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag ")]
        public void Verify_FunctionGetAll_WithSpecification_HasBeenCalled(string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }

            var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
            _tagsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => tagsList.Where(x => x.Title.Contains(tagSearch)).AsQueryable());

            //Act
            _tagsService.GetAll(specification);

            //Assert
            _tagsRepositoryMock.Verify(x => x.GetAll(specification), Times.Once);
        }

        /// <summary>
        /// Get all tags with specification.
        /// Should return tags with contains specification when tags exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData(0, "Tag ")]
        public void GetAll_ShouldReturnTags_WithContainsSpecification_WhenTagsExists(int notEqualCount, string tagSearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
            _tagsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => tagsList.Where(x => x.Title.Contains(tagSearch)).AsQueryable());

            //Act
            var tags = _tagsService.GetAll(specification);

            //Assert
            Assert.NotNull(tags);
            Assert.NotEmpty(tags);
            Assert.NotEqual(notEqualCount, tags.ToList().Count);
        }


        /// <summary>
        /// Get all tags with specification.
        /// Should return tag with equal specification when tags exists.
        /// </summary>
        /// <param name="equalCount">The equal count.</param>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData(1, "Tag 0")]
        public void GetAll_ShouldReturnTag_WithEqualsSpecification_WhenTagsExists(int equalCount, string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => tagsList.Where(x => x.Title.Contains(tagSearch)).AsQueryable());

            //Act
            var tags = _tagsService.GetAll(specification);

            //Assert
            Assert.NotNull(tags);
            Assert.NotEmpty(tags);
            Assert.Equal(equalCount, tags.ToList().Count);
        }

        /// <summary>
        /// Get all tags with specification.
        /// Should return nothing with  when tags does not exists.
        /// </summary>
        /// <param name="equalCount">The equal count.</param>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData(0, "Tag -1")]
        public void GetAll_ShouldReturnNothing_WithEqualSpecification_WhenTagsExists(int equalCount, string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => tagsList.Where(x => x.Title.Contains(tagSearch)).AsQueryable());

            //Act
            var tags = _tagsService.GetAll(specification);

            //Assert
            Assert.NotNull(tags);
            Assert.Empty(tags);
            Assert.Equal(equalCount, tags.ToList().Count);
        }

        /// <summary>
        /// Get all tags.
        /// Should return nothing with  when tags does not exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag 0")]
        public void GetAll_ShouldReturnNothing_WithEqualSpecification_WhenTagsDoesNotExists(string tagSearch)
        {
            //Arrange
            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => new List<Tag>().AsQueryable());

            //Act
            var tags = _tagsService.GetAll();

            //Assert
            Assert.Empty(tags);
        }

        #endregion

        #region Get all async function With Specification

        /// <summary>
        /// Verify that function Get All Async with specification has been called.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag ")]
        public async Task Verify_FunctionGetAllAsync_WithSpecification_HasBeenCalled(string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }

            var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
            _tagsRepositoryMock.Setup(x => x.GetAllAsync(specification))
                .ReturnsAsync(() => tagsList.Where(x => x.Title.Contains(tagSearch)).ToList());

            //Act
            await _tagsService.GetAllAsync(specification);

            //Assert
            _tagsRepositoryMock.Verify(x => x.GetAllAsync(specification), Times.Once);
        }

        /// <summary>
        /// Get all async tags with specification.
        /// Should return tags with contains specification when tags exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData(0, "Tag ")]
        public async Task GetAllAsync_ShouldReturnTags_WithContainsSpecification_WhenTagsExists(int notEqualCount, string tagSearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
            _tagsRepositoryMock.Setup(x => x.GetAllAsync(specification))
                .ReturnsAsync(() => tagsList.Where(x => x.Title.Contains(tagSearch)).ToList());

            //Act
            var tags = await _tagsService.GetAllAsync(specification);

            //Assert
            Assert.NotNull(tags);
            Assert.NotEmpty(tags);
            Assert.NotEqual(notEqualCount, tags.ToList().Count);
        }

        /// <summary>
        /// Get all async tags with specification.
        /// Should return tag with equal specification when tags exists.
        /// </summary>
        /// <param name="equalCount">The equal count.</param>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData(1, "Tag 0")]
        public async void GetAllAsync_ShouldReturnTag_WithEqualsSpecification_WhenTagsExists(int equalCount, string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.GetAllAsync(specification))
                .ReturnsAsync(() => tagsList.Where(x => x.Title.Contains(tagSearch)).ToList());

            //Act
            var tags = await _tagsService.GetAllAsync(specification);

            //Assert
            Assert.NotNull(tags);
            Assert.NotEmpty(tags);
            Assert.Equal(equalCount, tags.ToList().Count);
        }

        /// <summary>
        /// Get all async tags with specification.
        /// Should return nothing with  when tags does not exists.
        /// </summary>
        /// <param name="equalCount">The equal count.</param>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData(0, "Tag -1")]
        public async void GetAllAsync_ShouldReturnNothing_WithEqualSpecification_WhenTagsExists(int equalCount, string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.GetAllAsync(specification))
                .ReturnsAsync(() => tagsList.Where(x => x.Title.Contains(tagSearch)).ToList());

            //Act
            var tags = await _tagsService.GetAllAsync(specification);

            //Assert
            Assert.NotNull(tags);
            Assert.Empty(tags);
            Assert.Equal(equalCount, tags.ToList().Count);
        }

        /// <summary>
        /// Get all async tags.
        /// Should return nothing with  when tags does not exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag 0")]
        public async void GetAllAsync_ShouldReturnNothing_WithEqualSpecification_WhenTagsDoesNotExists(string tagSearch)
        {
            //Arrange
            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.GetAllAsync(specification))
                .ReturnsAsync(() => new List<Tag>());

            //Act
            var tags = await _tagsService.GetAllAsync();

            //Assert
            Assert.Null(tags);
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
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Id = tagId,
                Title = $"Tag {tagId}",
            };
            _tagsRepositoryMock.Setup(x => x.GetById(tagId))
                .Returns(() => newTag);

            //Act
            _tagsService.Find(tagId);

            //Assert
            _tagsRepositoryMock.Verify(x => x.GetById(tagId), Times.Once);
        }

        /// <summary>
        /// Find tag.
        /// Should return tag when tag exists.
        /// </summary>
        [Fact]
        public void Find_ShouldReturnTag_WhenTagExists()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Id = tagId,
                Title = $"Tag {tagId}",
            };
            _tagsRepositoryMock.Setup(x => x.GetById(tagId))
                .Returns(() => newTag);

            //Act
            var tag = _tagsService.Find(tagId);

            //Assert
            Assert.Equal(tagId, tag.Id);
        }

        /// <summary>
        /// Find tag.
        /// Should return nothing when tag does not exists.
        /// </summary>
        [Fact]
        public void Find_ShouldReturnNothing_WhenTagDoesNotExists()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            _tagsRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(() => null);

            //Act
            var tag = _tagsService.Find(tagId);

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
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Id = tagId,
                Title = $"Tag {tagId}",
            };
            _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
                .ReturnsAsync(() => newTag);

            //Act
            await _tagsService.FindAsync(tagId);

            //Assert
            _tagsRepositoryMock.Verify(x => x.GetByIdAsync(tagId), Times.Once);
        }

        /// <summary>
        /// Async find tag.
        /// Should return tag when tag exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task FindAsync_ShouldReturnTag_WhenTagExists()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Id = tagId,
                Title = $"Tag {tagId}",
            };
            _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
                .ReturnsAsync(() => newTag);

            //Act
            var tag = await _tagsService.FindAsync(tagId);

            //Assert
            Assert.Equal(tagId, tag.Id);
        }

        /// <summary>
        /// Async find tag.
        /// Should return nothing when tag does not exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task FindAsync_ShouldReturnNothing_WhenTagDoesNotExists()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            _tagsRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var tag = await _tagsService.FindAsync(tagId);

            //Assert
            Assert.Null(tag);
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
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Title = $"Tag {tagId}",
            };

            _tagsRepositoryMock.Setup(x => x.Insert(newTag))
                .Callback(() =>
                {
                    newTag.Id = tagId;
                });

            //Act
            _tagsService.Insert(newTag);

            //Assert
            _tagsRepositoryMock.Verify(x => x.Insert(newTag), Times.Once);
        }

        /// <summary>
        /// Insert tag.
        /// Should return tag when tag created.
        /// </summary>
        [Fact]
        public void Insert_ShouldReturnTag_WhenTagExists()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Title = $"Tag {tagId}",
            };

            _tagsRepositoryMock.Setup(x => x.Insert(newTag))
                .Callback(() =>
                {
                    newTag.Id = tagId;
                });

            //Act
            _tagsService.Insert(newTag);

            //Assert
            Assert.NotEqual(0, newTag.Id);
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
            var tagId = random.Next(52);
            var itemsCount = random.Next(10);
            var newTags = new List<Tag>();

            for (int i = 0; i < itemsCount; i++)
            {
                newTags.Add(new Tag
                {
                    Title = $"Tag {i}",
                });
            }

            _tagsRepositoryMock.Setup(x => x.Insert(newTags))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newTags[i].Id = tagId + i;
                    }
                });

            //Act
            _tagsService.Insert(newTags);

            //Assert
            _tagsRepositoryMock.Verify(x => x.Insert(newTags), Times.Once);
        }

        /// <summary>
        /// Insert Enumerable tags.
        /// Should return tags when tags created.
        /// </summary>
        [Fact]
        public void InsertEnumerable_ShouldReturnTags_WhenTagsExists()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var itemsCount = random.Next(10);
            var newTags = new List<Tag>();

            for (int i = 0; i < itemsCount; i++)
            {
                newTags.Add(new Tag
                {
                    Title = $"Tag {i}",
                });
            }

            _tagsRepositoryMock.Setup(x => x.Insert(newTags))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newTags[i].Id = tagId + i;
                    }
                });

            //Act
            _tagsService.Insert(newTags);

            //Assert
            newTags.ForEach(x =>
            {
                Assert.NotEqual(0, x.Id);
            });
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
            var tagId = random.Next(52);
            var itemsCount = random.Next(10);
            var newTags = new List<Tag>();

            for (int i = 0; i < itemsCount; i++)
            {
                newTags.Add(new Tag
                {
                    Title = $"Tag {i}",
                });
            }

            _tagsRepositoryMock.Setup(x => x.InsertAsync(newTags))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newTags[i].Id = tagId + i;
                    }
                });

            //Act
            await _tagsService.InsertAsync(newTags);

            //Assert
            _tagsRepositoryMock.Verify(x => x.InsertAsync(newTags), Times.Once);
        }

        /// <summary>
        /// Insert Async Enumerable tags.
        /// Should return tags when tags created.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task InsertAsyncEnumerable_ShouldReturnTags_WhenTagsExists()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var itemsCount = random.Next(10);
            var newTags = new List<Tag>();

            for (int i = 0; i < itemsCount; i++)
            {
                newTags.Add(new Tag
                {
                    Title = $"Tag {i}",
                });
            }

            _tagsRepositoryMock.Setup(x => x.InsertAsync(newTags))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newTags[i].Id = tagId + i;
                    }
                });

            //Act
            await _tagsService.InsertAsync(newTags);

            //Assert
            newTags.ForEach(x =>
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
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Title = $"Tag {tagId}",
            };

            _tagsRepositoryMock.Setup(x => x.InsertAsync(newTag))
                .Callback(() =>
                {
                    newTag.Id = tagId;
                });

            //Act
            await _tagsService.InsertAsync(newTag);

            //Assert
            _tagsRepositoryMock.Verify(x => x.InsertAsync(newTag), Times.Once);
        }

        /// <summary>
        /// Async insert tag.
        /// Should return comment when comment created.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task InsertAsync_ShouldReturnTag_WhenTagExists()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Title = $"Tag {tagId}",
            };

            _tagsRepositoryMock.Setup(x => x.InsertAsync(newTag))
                .Callback(() =>
                {
                    newTag.Id = tagId;
                });

            //Act
            await _tagsService.InsertAsync(newTag);

            //Assert
            Assert.NotEqual(0, newTag.Id);
        }

        #endregion

        #region Upadate function

        /// <summary>
        /// Verify that function Update has been called.
        /// </summary>
        /// <param name="newTagTitle">The new tag title.</param>
        [Theory]
        [InlineData("New Tag")]
        public void Verify_FunctionUpdate_HasBeenCalled(string newTagTitle)
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Title = $"Ефп {tagId}",
            };

            _tagsRepositoryMock.Setup(x => x.Insert(newTag))
                .Callback(() =>
                {
                    newTag.Id = tagId;
                });
            _tagsRepositoryMock.Setup(x => x.GetById(tagId))
                .Returns(() => newTag);

            //Act
            _tagsService.Insert(newTag);
            var tag = _tagsService.Find(tagId);
            tag.Title = newTagTitle;
            _tagsService.Update(tag);

            //Assert
            _tagsRepositoryMock.Verify(x => x.Update(tag), Times.Once);
        }

        /// <summary>
        /// Update tag.
        /// Should return tag when tag updated.
        /// </summary>
        /// <param name="newTagTitle">The new tag title.</param>
        [Theory]
        [InlineData("New tag title")]
        public void Update_ShouldReturnComment_WhenCommentExists(string newTagTitle)
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Title = $"Tag {tagId}",
            };

            _tagsRepositoryMock.Setup(x => x.Insert(newTag))
                .Callback(() =>
                {
                    newTag.Id = tagId;
                });
            _tagsRepositoryMock.Setup(x => x.GetById(tagId))
                .Returns(() => newTag);

            //Act
            _tagsService.Insert(newTag);
            var tag = _tagsService.Find(tagId);
            tag.Title = newTagTitle;
            _tagsService.Update(tag);

            //Assert
            Assert.Equal(newTagTitle, tag.Title);
        }

        #endregion

        #region Upadate Enumerable function

        /// <summary>
        /// Verify that function Update Enumerable has been called.
        /// </summary>
        /// <param name="newTagTitle">The new tag title.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("New Tag")]
        public void Verify_FunctionUpdateEnumerable_HasBeenCalled(string newTagTitle)
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var itemsCount = random.Next(10);
            var newTags = new List<Tag>();

            for (int i = 0; i < itemsCount; i++)
            {
                newTags.Add(new Tag
                {
                    Title = $"Tag {i}",
                });
            }

            _tagsRepositoryMock.Setup(x => x.Insert(newTags))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newTags[i].Id = tagId + i;
                    }
                });

            //Act
            _tagsService.Insert(newTags);
            newTags.ForEach(tag =>
            {
                tag.Title = newTagTitle;
            });
            for (var i = 0; i < itemsCount; i++)
            {
                newTags[i].Title = $"{newTagTitle} {i}";
            }
            _tagsService.Update(newTags);

            //Assert
            _tagsRepositoryMock.Verify(x => x.Update(newTags), Times.Once);
        }

        /// <summary>
        /// Update Enumerable tag.
        /// Should return tag when tag updated.
        /// </summary>
        /// <param name="newTagTitle">The new tag title.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("New tag title")]
        public void UpdateEnumerable_ShouldReturnComment_WhenCommentExists(string newTagTitle)
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var itemsCount = random.Next(10);
            var newTags = new List<Tag>();

            for (int i = 0; i < itemsCount; i++)
            {
                newTags.Add(new Tag
                {
                    Title = $"Tag {i}",
                });
            }

            _tagsRepositoryMock.Setup(x => x.Insert(newTags))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newTags[i].Id = tagId + i;
                    }
                });

            //Act
            _tagsService.Insert(newTags);
            for (var i = 0; i < itemsCount; i++)
            {
                newTags[i].Title = $"{newTagTitle} {i}";
            }
            _tagsService.Update(newTags);

            //Assert
            for (var i = 0; i < itemsCount; i++)
            {
                Assert.Equal($"{newTagTitle} {i}", newTags[i].Title);
            }
        }

        #endregion

        #region Update Async function

        /// <summary>
        /// Verify that function Update Async has been called.
        /// Should return tag when tag updated.
        /// </summary>
        /// <param name="newTagTitle">The new tag title.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("New Tag")]
        public async Task Verify_FunctionUpdateAsync_HasBeenCalled(string newTagTitle)
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Title = $"Tag {tagId}",
            };

            _tagsRepositoryMock.Setup(x => x.InsertAsync(newTag))
                .Callback(() =>
                {
                    newTag.Id = tagId;
                });
            _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
                .ReturnsAsync(() => newTag);

            //Act
            await _tagsService.InsertAsync(newTag);
            var tag = await _tagsService.FindAsync(tagId);
            tag.Title = newTagTitle;
            await _tagsService.UpdateAsync(tag);

            //Assert
            _tagsRepositoryMock.Verify(x => x.UpdateAsync(tag), Times.Once);
        }

        /// <summary>
        /// Async update tag.
        /// Should return tag when tag updated.
        /// </summary>
        /// <param name="newTagTitle">The new tag title.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("New Tag")]
        public async Task UpdateAsync_ShouldReturnTag_WhenCommentExists(string newTagTitle)
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Title = $"Tag {tagId}",
            };

            _tagsRepositoryMock.Setup(x => x.InsertAsync(newTag))
                .Callback(() =>
                {
                    newTag.Id = tagId;
                });
            _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
                .ReturnsAsync(() => newTag);

            //Act
            await _tagsService.InsertAsync(newTag);
            var tag = await _tagsService.FindAsync(tagId);
            tag.Title = newTagTitle;
            await _tagsService.UpdateAsync(tag);

            //Assert
            Assert.Equal(newTagTitle, tag.Title);
        }

        #endregion

        #region Upadate Async Enumerable function

        /// <summary>
        /// Verify that function Update Async Enumerable has been called.
        /// </summary>
        /// <param name="newTagTitle">The new tag title.</param>
        [Theory]
        [InlineData("New Tag")]
        public async Task Verify_FunctionUpdateAsyncEnumerable_HasBeenCalled(string newTagTitle)
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var itemsCount = random.Next(10);
            var newTags = new List<Tag>();

            for (int i = 0; i < itemsCount; i++)
            {
                newTags.Add(new Tag
                {
                    Title = $"Tag {i}",
                });
            }

            _tagsRepositoryMock.Setup(x => x.InsertAsync(newTags))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newTags[i].Id = tagId + i;
                    }
                });

            //Act
            await _tagsService.InsertAsync(newTags);
            newTags.ForEach(tag =>
            {
                tag.Title = newTagTitle;
            });
            for (var i = 0; i < itemsCount; i++)
            {
                newTags[i].Title = $"{newTagTitle} {i}";
            }
            await _tagsService.UpdateAsync(newTags);

            //Assert
            _tagsRepositoryMock.Verify(x => x.UpdateAsync(newTags), Times.Once);
        }

        /// <summary>
        /// Update Async Enumerable tag.
        /// Should return tag when tag updated.
        /// </summary>
        /// <param name="newTagTitle">The new tag title.</param>
        [Theory]
        [InlineData("New tag title")]
        public async Task UpdateAsyncEnumerable_ShouldReturnComment_WhenCommentExists(string newTagTitle)
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var itemsCount = random.Next(10);
            var newTags = new List<Tag>();

            for (int i = 0; i < itemsCount; i++)
            {
                newTags.Add(new Tag
                {
                    Title = $"Tag {i}",
                });
            }

            _tagsRepositoryMock.Setup(x => x.InsertAsync(newTags))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newTags[i].Id = tagId + i;
                    }
                });

            //Act
            await _tagsService.InsertAsync(newTags);
            for (var i = 0; i < itemsCount; i++)
            {
                newTags[i].Title = $"{newTagTitle} {i}";
            }
            await _tagsService.UpdateAsync(newTags);

            //Assert
            for (var i = 0; i < itemsCount; i++)
            {
                Assert.Equal($"{newTagTitle} {i}", newTags[i].Title);
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
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Id = tagId,
                Title = $"Tag {tagId}",
            };
            _tagsRepositoryMock.Setup(x => x.GetById(tagId))
                .Returns(() => newTag);

            //Act
            _tagsService.Insert(newTag);
            var tag = _tagsService.Find(tagId);
            _tagsService.Delete(tagId);
            _tagsRepositoryMock.Setup(x => x.GetById(tagId))
                .Returns(() => null);
            _tagsService.Find(tagId);

            //Assert
            _tagsRepositoryMock.Verify(x => x.Delete(newTag), Times.Once);
        }

        /// <summary>
        /// Delete By Id tag.
        /// Should return nothing when tag is deleted.
        /// </summary>
        [Fact]
        public void DeleteById_ShouldReturnNothing_WhenTagsDeleted()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Title = $"Tag {tagId}",
            };

            _tagsRepositoryMock.Setup(x => x.Insert(newTag))
                .Callback(() =>
                {
                    newTag.Id = tagId;
                });
            _tagsRepositoryMock.Setup(x => x.GetById(tagId))
                .Returns(() => newTag);

            //Act
            _tagsService.Insert(newTag);
            var tag = _tagsService.Find(tagId);
            _tagsService.Delete(tagId);
            _tagsRepositoryMock.Setup(x => x.GetById(tagId))
                .Returns(() => null);
            var deletedTag = _tagsService.Find(tagId);

            //Assert
            Assert.Null(deletedTag);
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
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Id = tagId,
                Title = $"Tag {tagId}",
            };
            _tagsRepositoryMock.Setup(x => x.GetById(tagId))
                .Returns(() => newTag);

            //Act
            _tagsService.Insert(newTag);
            var tag = _tagsService.Find(tagId);
            _tagsService.Delete(tag);
            _tagsRepositoryMock.Setup(x => x.GetById(tagId))
                .Returns(() => null);
            _tagsService.Find(tagId);

            //Assert
            _tagsRepositoryMock.Verify(x => x.Delete(tag), Times.Once);
        }

        /// <summary>
        /// Delete By Object tag.
        /// Should return nothing when tag is deleted.
        /// </summary>
        [Fact]
        public void DeleteByObject_ShouldReturnNothing_WhenTagsDeleted()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Title = $"Tag {tagId}",
            };

            _tagsRepositoryMock.Setup(x => x.Insert(newTag))
                .Callback(() =>
                {
                    newTag.Id = tagId;
                });
            _tagsRepositoryMock.Setup(x => x.GetById(tagId))
                .Returns(() => newTag);

            //Act
            _tagsService.Insert(newTag);
            var tag = _tagsService.Find(tagId);
            _tagsService.Delete(tag);
            _tagsRepositoryMock.Setup(x => x.GetById(tagId))
                .Returns(() => null);
            var deletedTag = _tagsService.Find(tagId);

            //Assert
            Assert.Null(deletedTag);
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
            var tagId = random.Next(52);
            var itemsCount = random.Next(10);
            var newTags = new List<Tag>();

            for (int i = 0; i < itemsCount; i++)
            {
                newTags.Add(new Tag
                {
                    Title = $"Tag {i}",
                });
            }

            _tagsRepositoryMock.Setup(x => x.Insert(newTags))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newTags[i].Id = tagId + i;
                    }
                });

            //Act
            _tagsService.Insert(newTags);
            _tagsService.Delete(newTags);

            //Assert
            _tagsRepositoryMock.Verify(x => x.Delete(newTags), Times.Once);
        }

        /// <summary>
        /// Delete By Enumerable tag.
        /// Should return nothing when tag is deleted.
        /// </summary>
        [Fact]
        public void DeleteByEnumerable_ShouldReturnNothing_WhenTagsDeleted()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var itemsCount = random.Next(10);
            var newTags = new List<Tag>();

            for (int i = 0; i < itemsCount; i++)
            {
                newTags.Add(new Tag
                {
                    Title = $"Tag {i}",
                });
            }

            _tagsRepositoryMock.Setup(x => x.Insert(newTags))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newTags[i].Id = tagId + i;
                    }
                });
            _tagsRepositoryMock.Setup(x => x.Delete(newTags))
                .Callback(() =>
                {
                    newTags = null;
                });

            //Act
            _tagsService.Insert(newTags);
            _tagsService.Delete(newTags);

            //Assert
            Assert.Null(newTags);
        }

        #endregion

        #region Delete By Id Async function

        /// <summary>
        /// Verify that function Delete Async By Id has been called.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task Verify_FunctionDeleteAsyncById_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Id = tagId,
                Title = $"Tag {tagId}",
            };
            _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
                .ReturnsAsync(() => newTag);

            //Act
            await _tagsService.InsertAsync(newTag);
            var comment = await _tagsService.FindAsync(tagId);
            await _tagsService.DeleteAsync(tagId);

            //Assert
            _tagsRepositoryMock.Verify(x => x.DeleteAsync(newTag), Times.Once);
        }

        /// <summary>
        /// Async delete by id comment.
        /// Should return nothing when tag is deleted.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task DeleteAsyncById_ShouldReturnNothing_WhenTagIsDeleted()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Title = $"Tag {tagId}",
            };

            _tagsRepositoryMock.Setup(x => x.InsertAsync(newTag))
                .Callback(() =>
                {
                    newTag.Id = tagId;
                });
            _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
                .ReturnsAsync(() => newTag);

            //Act
            await _tagsService.InsertAsync(newTag);
            var tag = await _tagsService.FindAsync(tagId);
            await _tagsService.DeleteAsync(tagId);
            _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
                .ReturnsAsync(() => null);
            var deletedTag = await _tagsService.FindAsync(tagId);

            //Assert
            Assert.Null(deletedTag);
        }

        #endregion

        #region Delete By Object Async function

        /// <summary>
        /// Verify that function Delete Async By Object has been called.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task Verify_FunctionDeleteAsyncByObject_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Id = tagId,
                Title = $"Tag {tagId}",
            };
            _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
                .ReturnsAsync(() => newTag);

            //Act
            await _tagsService.InsertAsync(newTag);
            var comment = await _tagsService.FindAsync(tagId);
            await _tagsService.DeleteAsync(comment);

            //Assert
            _tagsRepositoryMock.Verify(x => x.DeleteAsync(comment), Times.Once);
        }

        /// <summary>
        /// Async delete by object comment.
        /// Should return nothing when tag is deleted.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task DeleteAsyncByObject_ShouldReturnNothing_WhenTagIsDeleted()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var newTag = new Tag
            {
                Title = $"Tag {tagId}",
            };

            _tagsRepositoryMock.Setup(x => x.InsertAsync(newTag))
                .Callback(() =>
                {
                    newTag.Id = tagId;
                });
            _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
                .ReturnsAsync(() => newTag);

            //Act
            await _tagsService.InsertAsync(newTag);
            var tag = await _tagsService.FindAsync(tagId);
            await _tagsService.DeleteAsync(tag);
            _tagsRepositoryMock.Setup(x => x.GetByIdAsync(tagId))
                .ReturnsAsync(() => null);
            var deletedTag = await _tagsService.FindAsync(tagId);

            //Assert
            Assert.Null(deletedTag);
        }

        #endregion

        #region Delete Async By Enumerable function

        /// <summary>
        /// Verify that function Delete Async By Enumerable has been called.
        /// </summary>
        [Fact]
        public async Task Verify_FunctionDeleteAsyncByEnumerable_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var itemsCount = random.Next(10);
            var newTags = new List<Tag>();

            for (int i = 0; i < itemsCount; i++)
            {
                newTags.Add(new Tag
                {
                    Title = $"Tag {i}",
                });
            }

            _tagsRepositoryMock.Setup(x => x.InsertAsync(newTags))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newTags[i].Id = tagId + i;
                    }
                });

            //Act
            await _tagsService.InsertAsync(newTags);
            await _tagsService.DeleteAsync(newTags);

            //Assert
            _tagsRepositoryMock.Verify(x => x.DeleteAsync(newTags), Times.Once);
        }

        /// <summary>
        /// Delete Async By Enumerable tag.
        /// Should return nothing when tag is deleted.
        /// </summary>
        [Fact]
        public async Task DeleteAsyncByEnumerable_ShouldReturnNothing_WhenTagsDeleted()
        {
            //Arrange
            var random = new Random();
            var tagId = random.Next(52);
            var itemsCount = random.Next(10);
            var newTags = new List<Tag>();

            for (int i = 0; i < itemsCount; i++)
            {
                newTags.Add(new Tag
                {
                    Title = $"Tag {i}",
                });
            }

            _tagsRepositoryMock.Setup(x => x.InsertAsync(newTags))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newTags[i].Id = tagId + i;
                    }
                });
            _tagsRepositoryMock.Setup(x => x.DeleteAsync(newTags))
                .Callback(() =>
                {
                    newTags = null;
                });

            //Act
            await _tagsService.InsertAsync(newTags);
            await _tagsService.DeleteAsync(newTags);

            //Assert
            Assert.Null(newTags);
        }

        #endregion

        #region Any function With Specification

        /// <summary>
        /// Verify that function Any with specification has been called.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag ")]
        public void Verify_FunctionAny_WithSpecification_HasBeenCalled(string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }

            var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
            _tagsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => tagsList.Any(x => x.Title.Contains(tagSearch)));

            //Act
            _tagsService.Any(specification);

            //Assert
            _tagsRepositoryMock.Verify(x => x.Any(specification), Times.Once);
        }

        /// <summary>
        /// Check if there are any tags with specification.
        /// Should return true with contains specification when tags exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag ")]
        public void Any_ShouldReturnTrue_WithContainsSpecification_WhenTagsExists(string tagSearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
            _tagsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => tagsList.Any(x => x.Title.Contains(tagSearch)));

            //Act
            var areAnyTags = _tagsService.Any(specification);

            //Assert
            Assert.True(areAnyTags);
        }

        /// <summary>
        /// Check if there are any tags with specification.
        /// Should return true with equal specification when tags exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag 0")]
        public void Any_ShouldReturnTrue_WithEqualsSpecification_WhenCommentsExists(string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => tagsList.Any(x => x.Title.Contains(tagSearch)));

            //Act
            var areAnyTags = _tagsService.Any(specification);

            //Assert
            Assert.True(areAnyTags);
        }

        /// <summary>
        /// Check if there are any tags with specification.
        /// Should return false with when tags does not exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag -1")]
        public void Any_ShouldReturnFalse_WithEqualSpecification_WhenCommentsExists(string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => tagsList.Any(x => x.Title.Contains(tagSearch)));

            //Act
            var areAnyTags = _tagsService.Any(specification);

            //Assert
            Assert.False(areAnyTags);
        }

        /// <summary>
        /// Check if there are any tags with specification.
        /// Should return false with when tags does not exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag 0")]
        public void Any_ShouldReturnNothing_WithEqualSpecification_WhenCommentDoesNotExists(string tagSearch)
        {
            //Arrange
            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => false);

            //Act
            var areAnyTags = _tagsService.Any(specification);

            //Assert
            Assert.False(areAnyTags);
        }

        #endregion

        #region Any Async function With Specification

        /// <summary>
        /// Verify that function Any Async with specification has been called.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Tag ")]
        public async Task Verify_FunctionAnyAsync_WithSpecification_HasBeenCalled(string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }

            var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
            _tagsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => tagsList.Any(x => x.Title.Contains(tagSearch)));

            //Act
            await _tagsService.AnyAsync(specification);

            //Assert
            _tagsRepositoryMock.Verify(x => x.AnyAsync(specification), Times.Once);
        }

        /// <summary>
        /// Async check if there are any tags with specification.
        /// Should return true with contains specification when tags exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Tag ")]
        public async Task AnyAsync_ShouldReturnTrue_WithContainsSpecification_WhenTagsExists(string tagSearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
            _tagsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => tagsList.Any(x => x.Title.Contains(tagSearch)));

            //Act
            var areAnyTags = await _tagsService.AnyAsync(specification);

            //Assert
            Assert.True(areAnyTags);
        }

        /// <summary>
        /// Async check if there are any tags with specification.
        /// Should return true with equal specification when tags exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Tag 0")]
        public async Task AnyAsync_ShouldReturnTrue_WithEqualsSpecification_WhenTagsExists(string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => tagsList.Any(x => x.Title.Contains(tagSearch)));

            //Act
            var areAnyTags = await _tagsService.AnyAsync(specification);

            //Assert
            Assert.True(areAnyTags);
        }

        /// <summary>
        /// Async check if there are any tags with specification.
        /// Should return false with when tags does not exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Tag -1")]
        public async Task AnyAsync_ShouldReturnFalse_WithEqualSpecification_WhenTagsExists(string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => tagsList.Any(x => x.Title.Contains(tagSearch)));

            //Act
            var areAnyTags = await _tagsService.AnyAsync(specification);

            //Assert
            Assert.False(areAnyTags);
        }

        /// <summary>
        /// Async check if there are any tags with specification.
        /// Should return false with when tags does not exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Tag 0")]
        public async Task AnyAsync_ShouldReturnNothing_WithEqualSpecification_WhenTagDoesNotExists(string tagSearch)
        {
            //Arrange
            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => false);

            //Act
            var areAnyTags = await _tagsService.AnyAsync(specification);

            //Assert
            Assert.False(areAnyTags);
        }

        #endregion

        #region First Or Default function With Specification

        /// <summary>
        /// Verify that function First Or Default with specification has been called.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag ")]
        public void Verify_FunctionFirstOrDefault_WithSpecification_HasBeenCalled(string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Comment {i}",
                });
            }

            var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
            _tagsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => tagsList.FirstOrDefault(x => x.Title.Contains(tagSearch)));

            //Act
            _tagsService.FirstOrDefault(specification);

            //Assert
            _tagsRepositoryMock.Verify(x => x.FirstOrDefault(specification), Times.Once);
        }

        /// <summary>
        /// Get first or default tag with specification.
        /// Should return tag with contains specification when tags exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag ")]
        public void FirstOrDefault_ShouldReturnTag_WithContainsSpecification_WhenTagsExists(string tagSearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
            _tagsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => tagsList.FirstOrDefault(x => x.Title.Contains(tagSearch)));

            //Act
            var tag = _tagsService.FirstOrDefault(specification);

            //Assert
            Assert.NotNull(tag);
            Assert.IsType<Tag>(tag);
        }

        /// <summary>
        /// Get first or default tag with specification.
        /// Should return tag with equal specification when tags exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag 0")]
        public void FirstOrDefault_ShouldReturnTag_WithEqualsSpecification_WhenTagsExists(string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => tagsList.FirstOrDefault(x => x.Title.Contains(tagSearch)));

            //Act
            var tag = _tagsService.FirstOrDefault(specification);

            //Assert
            Assert.NotNull(tag);
            Assert.IsType<Tag>(tag);
        }

        /// <summary>
        /// Get first or default tag with specification.
        /// Should return nothing with when tags does not exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag -1")]
        public void FirstOrDefault_ShouldReturnNothing_WithEqualSpecification_WhenTagsExists(string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => tagsList.FirstOrDefault(x => x.Title.Contains(tagSearch)));

            //Act
            var tag = _tagsService.FirstOrDefault(specification);

            //Assert
            Assert.Null(tag);
        }

        /// <summary>
        /// Get first or default tag with specification.
        /// Should return nothing with when tags does not exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag 0")]
        public void FirstOrDefault_ShouldReturnNothing_WithEqualSpecification_WhenTagsDoesNotExists(string tagSearch)
        {
            //Arrange
            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => null);

            //Act
            var tag = _tagsService.FirstOrDefault(specification);

            //Assert
            Assert.Null(tag);
        }

        #endregion

        #region Last Or Default function With Specification

        /// <summary>
        /// Verify that function Last Or Default with specification has been called.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag ")]
        public void Verify_FunctionLastOrDefault_WithSpecification_HasBeenCalled(string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Comment {i}",
                });
            }

            var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
            _tagsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => tagsList.LastOrDefault(x => x.Title.Contains(tagSearch)));

            //Act
            _tagsService.LastOrDefault(specification);

            //Assert
            _tagsRepositoryMock.Verify(x => x.LastOrDefault(specification), Times.Once);
        }

        /// <summary>
        /// Get last or default tag with specification.
        /// Should return tag with contains specification when tags exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag ")]
        public void LastOrDefault_ShouldReturnTag_WithContainsSpecification_WhenTagsExists(string tagSearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Contains(tagSearch));
            _tagsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => tagsList.LastOrDefault(x => x.Title.Contains(tagSearch)));

            //Act
            var tag = _tagsService.LastOrDefault(specification);

            //Assert
            Assert.NotNull(tag);
            Assert.IsType<Tag>(tag);
        }

        /// <summary>
        /// Get last or default tag with specification.
        /// Should return tag with equal specification when tags exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag 0")]
        public void LastOrDefault_ShouldReturnTag_WithEqualsSpecification_WhenTagsExists(string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => tagsList.LastOrDefault(x => x.Title.Contains(tagSearch)));

            //Act
            var tag = _tagsService.LastOrDefault(specification);

            //Assert
            Assert.NotNull(tag);
            Assert.IsType<Tag>(tag);
        }

        /// <summary>
        /// Get last or default tag with specification.
        /// Should return nothing with when tags does not exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag -1")]
        public void LastOrDefault_ShouldReturnNothing_WithEqualSpecification_WhenTagsExists(string tagSearch)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }


            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => tagsList.LastOrDefault(x => x.Title.Contains(tagSearch)));

            //Act
            var tag = _tagsService.LastOrDefault(specification);

            //Assert
            Assert.Null(tag);
        }

        /// <summary>
        /// Get last or default tag with specification.
        /// Should return nothing with when tags does not exists.
        /// </summary>
        /// <param name="tagSearch">The tag search.</param>
        [Theory]
        [InlineData("Tag 0")]
        public void LastOrDefault_ShouldReturnNothing_WithEqualSpecification_WhenTagsDoesNotExists(string tagSearch)
        {
            //Arrange
            var specification = new TagSpecification(x => x.Title.Equals(tagSearch));
            _tagsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => null);

            //Act
            var tag = _tagsService.LastOrDefault(specification);

            //Assert
            Assert.Null(tag);
        }

        #endregion

        #region Search async function

        /// <summary>
        /// Search the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commentsList">The comments list.</param>
        /// <returns>PagedListResult.</returns>
        protected PagedListResult<Tag> Search(SearchQuery<Tag> query, List<Tag> tagsList)
        {
            var sequence = tagsList.AsQueryable();

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
                sequence = ((IOrderedQueryable<Tag>)sequence).OrderBy(x => true);
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
            return new PagedListResult<Tag>()
            {
                Entities = result,
                HasNext = hasNext,
                HasPrevious = query.Skip > 0,
                Count = resultCount,
            };
        }

        /// <summary>
        /// Verify that function Search async has been called.
        /// </summary>
        [Theory]
        [InlineData("Tag ", 0, 10, "Title", OrderType.Ascending)]
        [InlineData("Tag ", 10, 10, "Title", OrderType.Ascending)]
        [InlineData("Tag ", 10, 20, "Title", OrderType.Ascending)]
        [InlineData("Tag ", 0, 100, "Title", OrderType.Ascending)]
        public async Task Verify_FunctionSearchAsync_HasBeenCalled(string search, int start, int take, string fieldName, OrderType orderType)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }

            var query = new SearchQuery<Tag>
            {
                Skip = start,
                Take = take
            };

            query.AddSortCriteria(new FieldSortOrder<Tag>(fieldName, orderType));

            query.AddFilter(x => x.Title.ToUpper().Contains($"{search}".ToUpper()));

            _tagsRepositoryMock.Setup(x => x.SearchAsync(query))
                .ReturnsAsync(() =>
                {
                    return Search(query, tagsList);
                });

            //Act
            await _tagsService.SearchAsync(query);

            //Assert
            _tagsRepositoryMock.Verify(x => x.SearchAsync(query), Times.Once);
        }

        /// <summary>
        /// Search async tags.
        /// Should return tags when tags exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        [Theory]
        [InlineData("Tag ", 0, 10, "Title", OrderType.Ascending)]
        [InlineData("Tag ", 10, 10, "Title", OrderType.Ascending)]
        [InlineData("Tag ", 10, 20, "Title", OrderType.Ascending)]
        [InlineData("Tag ", 0, 100, "Title", OrderType.Ascending)]
        public async Task SearchAsync_ShouldReturnTags_WhenTagsExists(string search, int start, int take, string fieldName, OrderType orderType)
        {
            //Arrange
            var random = new Random();
            var tagsList = new List<Tag>();

            for (var i = 0; i < random.Next(100); i++)
            {
                tagsList.Add(new Tag
                {
                    Id = i,
                    Title = $"Tag {i}",
                });
            }

            var query = new SearchQuery<Tag>
            {
                Skip = start,
                Take = take
            };

            query.AddSortCriteria(new FieldSortOrder<Tag>(fieldName, orderType));

            query.AddFilter(x => x.Title.ToUpper().Contains($"{search}".ToUpper()));

            _tagsRepositoryMock.Setup(x => x.SearchAsync(query))
                .ReturnsAsync(() =>
                {
                    return Search(query, tagsList);
                });

            //Act
            var tags = await _tagsService.SearchAsync(query);

            //Assert
            Assert.NotNull(tags);
            Assert.NotEmpty(tags.Entities);
        }

        #endregion

        #region NotTestedYet
        //GenerateQuery(TableFilter tableFilter, string includeProperties = null)
        //GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
        #endregion
    }
}
