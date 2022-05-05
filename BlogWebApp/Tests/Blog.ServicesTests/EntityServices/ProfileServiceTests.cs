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
using Xunit;

namespace Blog.ServicesTests.EntityServices
{
    /// <summary>
    /// Profile service tests.
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
        /// Initializes a new instance of the <see cref="ProfileServiceTests"/> class.
        /// </summary>
        public ProfileServiceTests()
        {
            _profileRepositoryMock = new Mock<IRepository<Data.Models.Profile>>();
            var mapper = new Mock<IMapper>();
            _profileService = new ProfileService(_profileRepositoryMock.Object, mapper.Object);
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
            var profilesList = new List<Data.Models.Profile>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var userId = new Guid().ToString();
                var user = new ApplicationUser
                {
                    Id = userId,
                    FirstName = "Test fn",
                    LastName = "Test ln",
                    Email = "test@test.test",
                    UserName = "test@test.test"
                };
                profilesList.Add(new Data.Models.Profile
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            _profileRepositoryMock.Setup(x => x.GetAll())
                .Returns(profilesList.AsQueryable());

            //Act
            _profileService.GetAll();

            //Assert
            _profileRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        /// <summary>
        /// Get all profiles.
        /// Should return profiles when profiles exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        [Theory]
        [InlineData(0)]
        public void GetAll_ShouldReturnProfiles_WhenProfilesExists(int notEqualCount)
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<Data.Models.Profile>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var userId = new Guid().ToString();
                var user = new ApplicationUser
                {
                    Id = userId,
                    FirstName = "Test fn",
                    LastName = "Test ln",
                    Email = "test@test.test",
                    UserName = "test@test.test"
                };
                profilesList.Add(new Data.Models.Profile
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            _profileRepositoryMock.Setup(x => x.GetAll())
                .Returns(profilesList.AsQueryable());

            //Act
            var profiles = _profileService.GetAll();

            //Assert
            Assert.NotNull(profiles);
            Assert.NotEmpty(profiles);
            Assert.NotEqual(notEqualCount, profiles.ToList().Count);
        }

        /// <summary>
        /// Get all profiles.
        /// Should return nothing when profiles does not exists.
        /// </summary>
        [Fact]
        public void GetAll_ShouldReturnNothing_WhenProfilesDoesNotExists()
        {
            //Arrange
            _profileRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => new List<Data.Models.Profile>().AsQueryable());

            //Act
            var profiles = _profileService.GetAll();

            //Assert
            Assert.Empty(profiles);
        }

        #endregion

        #region Get all function With Specification

        /// <summary>
        /// Verify that function Get All with specification has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionGetAll_WithSpecification_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<Data.Models.Profile>();
            var searchUserId = new Guid().ToString();

            for (var i = 0; i < random.Next(100); i++)
            {
                var userId = i == 0 ? searchUserId : new Guid().ToString();
                var user = new ApplicationUser
                {
                    Id = userId,
                    FirstName = "Test fn",
                    LastName = "Test ln",
                    Email = "test@test.test",
                    UserName = "test@test.test"
                };
                profilesList.Add(new Data.Models.Profile
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }
            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(profilesList.Where(x => x.UserId.Contains(searchUserId)).AsQueryable());

            //Act
            _profileService.GetAll(specification);

            //Assert
            _profileRepositoryMock.Verify(x => x.GetAll(specification), Times.Once);
        }

        /// <summary>
        /// Get all profiles with specification.
        /// Should return profiles with equals specification when messages exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        [Theory]
        [InlineData(0)]
        public void GetAll_ShouldReturnProfiles_WithEqualsSpecification_WhenProfilesExists(int notEqualCount)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var profilesList = new List<Data.Models.Profile>();
            var searchUserId = new Guid().ToString();

            for (var i = 0; i < random.Next(100); i++)
            {
                var userId = i == 0 ? searchUserId : new Guid().ToString();
                var user = new ApplicationUser
                {
                    Id = userId,
                    FirstName = "Test fn",
                    LastName = "Test ln",
                    Email = "test@test.test",
                    UserName = "test@test.test"
                };
                profilesList.Add(new Data.Models.Profile
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }
            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(profilesList.Where(x => x.UserId.Contains(searchUserId)).AsQueryable());

            //Act
            var profiles = _profileService.GetAll(specification);

            //Assert
            Assert.NotNull(profiles);
            Assert.NotEmpty(profiles);
            Assert.NotEqual(notEqualCount, profiles.ToList().Count);
        }

        /// <summary>
        /// Get all messages with specification.
        /// Should return nothing with  when messages does not exists.
        /// </summary>
        /// <param name="equalCount">The equal count.</param>
        [Theory]
        [InlineData(0)]
        public void GetAll_ShouldReturnNothing_WithEqualSpecification_WhenProfilesExists(int equalCount)
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<Data.Models.Profile>();
            var searchUserId = $"{new Guid()}1";

            for (var i = 0; i < random.Next(100); i++)
            {
                var userId = new Guid().ToString();
                var user = new ApplicationUser
                {
                    Id = userId,
                    FirstName = "Test fn",
                    LastName = "Test ln",
                    Email = "test@test.test",
                    UserName = "test@test.test"
                };
                profilesList.Add(new Data.Models.Profile
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }
            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(profilesList.Where(x => x.UserId.Contains(searchUserId)).AsQueryable());

            //Act
            var profiles = _profileService.GetAll(specification);

            //Assert
            Assert.NotNull(profiles);
            Assert.Empty(profiles);
            Assert.Equal(equalCount, profiles.ToList().Count);
        }

        /// <summary>
        /// Get all messages.
        /// Should return nothing with  when messages does not exists.
        /// </summary>
        [Fact]
        public void GetAll_ShouldReturnNothing_WithEqualSpecification_WhenMessagesDoesNotExists()
        {
            //Arrange
            var searchUserId = new Guid().ToString();
            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => new List<Data.Models.Profile>().AsQueryable());

            //Act
            var messages = _profileService.GetAll();

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
            var profileId = random.Next(52);

            var userId = new Guid().ToString();
            var user = new ApplicationUser
            {
                Id = userId,
                FirstName = "Test fn",
                LastName = "Test ln",
                Email = "test@test.test",
                UserName = "test@test.test"
            };
            var newProfile = new Data.Models.Profile
            {
                Id = profileId,
                UserId = userId,
                User = user,
                ProfileImg = $"img{profileId}.jpg"
            };

            _profileRepositoryMock.Setup(x => x.GetById(profileId))
                .Returns(() => newProfile);

            //Act
            _profileService.Find(profileId);

            //Assert
            _profileRepositoryMock.Verify(x => x.GetById(profileId), Times.Once);
        }

        /// <summary>
        /// Find profile.
        /// Should return profile when profiles exists.
        /// </summary>
        [Fact]
        public void Find_ShouldReturnProfile_WhenProfilesExists()
        {
            //Arrange
            var random = new Random();
            var profileId = random.Next(52);

            var userId = new Guid().ToString();
            var user = new ApplicationUser
            {
                Id = userId,
                FirstName = "Test fn",
                LastName = "Test ln",
                Email = "test@test.test",
                UserName = "test@test.test"
            };
            var newProfile = new Data.Models.Profile
            {
                Id = profileId,
                UserId = userId,
                User = user,
                ProfileImg = $"img{profileId}.jpg"
            };

            _profileRepositoryMock.Setup(x => x.GetById(profileId))
                .Returns(() => newProfile);

            //Act
            var profile = _profileService.Find(profileId);

            //Assert
            Assert.Equal(profileId, profile.Id);
        }

        /// <summary>
        /// Find profile.
        /// Should return nothing when profiles does not exists.
        /// </summary>
        [Fact]
        public void Find_ShouldReturnNothing_WhenProfilesDoesNotExists()
        {
            //Arrange
            var random = new Random();
            var profileId = random.Next(52);
            _profileRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(() => null);

            //Act
            var profile = _profileService.Find(profileId);

            //Assert
            Assert.Null(profile);
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
            var profileId = random.Next(52);

            var userId = new Guid().ToString();
            var user = new ApplicationUser
            {
                Id = userId,
                FirstName = "Test fn",
                LastName = "Test ln",
                Email = "test@test.test",
                UserName = "test@test.test"
            };
            var newProfile = new Data.Models.Profile
            {
                Id = profileId,
                UserId = userId,
                User = user,
                ProfileImg = $"img{profileId}.jpg"
            };

            _profileRepositoryMock.Setup(x => x.GetByIdAsync(profileId))
                .ReturnsAsync(() => newProfile);

            //Act
            await _profileService.FindAsync(profileId);

            //Assert
            _profileRepositoryMock.Verify(x => x.GetByIdAsync(profileId), Times.Once);
        }

        /// <summary>
        /// Async find profile.
        /// Should return profile when profiles exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task FindAsync_ShouldReturnProfiles_WhenProfilesExists()
        {
            //Arrange
            var random = new Random();
            var profileId = random.Next(52);

            var userId = new Guid().ToString();
            var user = new ApplicationUser
            {
                Id = userId,
                FirstName = "Test fn",
                LastName = "Test ln",
                Email = "test@test.test",
                UserName = "test@test.test"
            };
            var newProfile = new Data.Models.Profile
            {
                Id = profileId,
                UserId = userId,
                User = user,
                ProfileImg = $"img{profileId}.jpg"
            };

            _profileRepositoryMock.Setup(x => x.GetByIdAsync(profileId))
                .ReturnsAsync(() => newProfile);

            //Act
            var profile = await _profileRepositoryMock.FindAsync(profileId);

            //Assert
            Assert.Equal(profileId, profile.Id);
        }

        /// <summary>
        /// Async find profile.
        /// Should return nothing when profiles does not exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task FindAsync_ShouldReturnNothing_WhenProfilesDoesNotExists()
        {
            //Arrange
            var random = new Random();
            var profileId = random.Next(52);
            _profileRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var profile = await _profileService.FindAsync(profileId);

            //Assert
            Assert.Null(profile);
        }

        #endregion
    }
}
