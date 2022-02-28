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

        #region Insert Async function

        /// <summary>
        /// Verify that function Insert Async has been called.
        /// </summary>
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
    }
}
