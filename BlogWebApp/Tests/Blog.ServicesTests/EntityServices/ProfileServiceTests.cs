using AutoMapper;
using Blog.Data.Repository;
using Blog.Services;
using Blog.Services.Interfaces;
using Moq;

namespace Blog.ServicesTests.EntityServices
{
    /// <summary>
    /// Messages service tests.
    /// </summary>
    public class ProfileServiceTests
    {
        #region Fields

        /// <summary>
        /// The profile service.
        /// </summary>
        private readonly IProfileService _profileService;

        /// <summary>
        /// The profile repository mock.
        /// </summary>
        private readonly Mock<IRepository<Data.Models.Profile>> _profileRepositoryMock;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagesServiceTests"/> class.
        /// </summary>
        public ProfileServiceTests()
        {
            _profileRepositoryMock = new Mock<IRepository<Data.Models.Profile>>();
            var mapper = new Mock<IMapper>();
            _profileService = new ProfileService(_profileRepositoryMock.Object, mapper.Object);
        }

        #endregion
    }
}
