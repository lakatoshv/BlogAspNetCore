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
    }
}
