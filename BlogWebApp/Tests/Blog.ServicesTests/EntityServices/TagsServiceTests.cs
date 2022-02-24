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
            var tags = _tagsService.GetAll();

            //Assert
            _tagsRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        /// <summary>
        /// Get all comments.
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
        /// Get all comments.
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
            var tags = _tagsService.GetAll(specification);

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

        #endregion
    }
}
