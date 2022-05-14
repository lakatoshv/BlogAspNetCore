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
            var profile = await _profileService.FindAsync(profileId);

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

        #region Insert function

        /// <summary>
        /// Verify that function Insert has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionInsert_HasBeenCalled()
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
                UserId = userId,
                User = user,
                ProfileImg = $"img{profileId}.jpg"
            };

            _profileRepositoryMock.Setup(x => x.Insert(newProfile))
                .Callback(() =>
                {
                    newProfile.Id = profileId;
                });

            //Act
            _profileService.Insert(newProfile);

            //Assert
            _profileRepositoryMock.Verify(x => x.Insert(newProfile), Times.Once);
        }

        /// <summary>
        /// Insert profile.
        /// Should return profile when profile created.
        /// </summary>
        [Fact]
        public void Insert_ShouldReturnProfile_WhenProfileExists()
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
                UserId = userId,
                User = user,
                ProfileImg = $"img{profileId}.jpg"
            };

            _profileRepositoryMock.Setup(x => x.Insert(newProfile))
                .Callback(() =>
                {
                    newProfile.Id = profileId;
                });

            //Act
            _profileService.Insert(newProfile);

            //Assert
            Assert.NotEqual(0, newProfile.Id);
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
                UserId = userId,
                User = user,
                ProfileImg = $"img{profileId}.jpg"
            };

            _profileRepositoryMock.Setup(x => x.InsertAsync(newProfile))
                .Callback(() =>
                {
                    newProfile.Id = profileId;
                });

            //Act
            await _profileService.InsertAsync(newProfile);

            //Assert
            _profileRepositoryMock.Verify(x => x.InsertAsync(newProfile), Times.Once);
        }

        /// <summary>
        /// Async insert profile.
        /// Should return profile when profile created.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task InsertAsync_ShouldReturnProfile_WhenProfileExists()
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
                UserId = userId,
                User = user,
                ProfileImg = $"img{profileId}.jpg"
            };

            _profileRepositoryMock.Setup(x => x.InsertAsync(newProfile))
                .Callback(() =>
                {
                    newProfile.Id = profileId;
                });

            //Act
            await _profileService.InsertAsync(newProfile);

            //Assert
            Assert.NotEqual(0, newProfile.Id);
        }

        #endregion

        #region Upadate function

        /// <summary>
        /// Verify that function Update has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionUpdate_HasBeenCalled()
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
                UserId = userId,
                User = user,
                ProfileImg = $"img{profileId}.jpg"
            };

            _profileRepositoryMock.Setup(x => x.Insert(newProfile))
                .Callback(() =>
                {
                    newProfile.Id = profileId;
                });
            _profileRepositoryMock.Setup(x => x.GetById(profileId))
                .Returns(() => newProfile);

            //Act
            _profileService.Insert(newProfile);
            var profile = _profileService.Find(profileId);
            profile.UserId = new Guid().ToString();
            _profileService.Update(profile);

            //Assert
            _profileRepositoryMock.Verify(x => x.Update(profile), Times.Once);
        }

        /// <summary>
        /// Update profile.
        /// Should return profile when profile updated.
        /// </summary>
        [Fact]
        public void Update_ShouldReturnProfile_WhenProfileExists()
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
                UserId = userId,
                User = user,
                ProfileImg = $"img{profileId}.jpg"
            };

            _profileRepositoryMock.Setup(x => x.Insert(newProfile))
                .Callback(() =>
                {
                    newProfile.Id = profileId;
                });
            _profileRepositoryMock.Setup(x => x.GetById(profileId))
                .Returns(() => newProfile);

            //Act
            _profileService.Insert(newProfile);
            var profile = _profileService.Find(profileId);
            var newUserId = new Guid().ToString();
            profile.UserId = newUserId;
            _profileService.Update(profile);

            //Assert
            Assert.Equal(newUserId, profile.UserId);
        }

        #endregion

        #region Update Async function

        /// <summary>
        /// Verify that function Update Async has been called.
        /// Should return tag when tag updated.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task Verify_FunctionUpdateAsync_HasBeenCalled()
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
                UserId = userId,
                User = user,
                ProfileImg = $"img{profileId}.jpg"
            };

            _profileRepositoryMock.Setup(x => x.InsertAsync(newProfile))
                .Callback(() =>
                {
                    newProfile.Id = profileId;
                });
            _profileRepositoryMock.Setup(x => x.GetByIdAsync(profileId))
                .ReturnsAsync(() => newProfile);

            //Act
            await _profileService.InsertAsync(newProfile);
            var profile = await _profileService.FindAsync(profileId);
            profile.UserId = new Guid().ToString();
            await _profileService.UpdateAsync(profile);

            //Assert
            _profileRepositoryMock.Verify(x => x.UpdateAsync(profile), Times.Once);
        }

        /// <summary>
        /// Async update profile.
        /// Should return profile when profile updated.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task UpdateAsync_ShouldReturnMessage_WhenMessageExists()
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
                UserId = userId,
                User = user,
                ProfileImg = $"img{profileId}.jpg"
            };

            _profileRepositoryMock.Setup(x => x.InsertAsync(newProfile))
                .Callback(() =>
                {
                    newProfile.Id = profileId;
                });
            _profileRepositoryMock.Setup(x => x.GetByIdAsync(profileId))
                .ReturnsAsync(() => newProfile);

            //Act
            await _profileService.InsertAsync(newProfile);
            var profile = await _profileService.FindAsync(profileId);
            var newUserId = new Guid().ToString();
            profile.UserId = newUserId;
            await _profileService.UpdateAsync(profile);

            //Assert
            Assert.Equal(newUserId, profile.UserId);
        }

        #endregion

        #region Delete function

        /// <summary>
        /// Verify that function Delete has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionDelete_HasBeenCalled()
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
                UserId = userId,
                User = user,
                ProfileImg = $"img{profileId}.jpg"
            };

            _profileRepositoryMock.Setup(x => x.Insert(newProfile))
                .Callback(() =>
                {
                    newProfile.Id = profileId;
                });
            _profileRepositoryMock.Setup(x => x.GetById(profileId))
                .Returns(() => newProfile);

            //Act
            _profileService.Insert(newProfile);
            var profile = _profileService.Find(profileId);
            _profileService.Delete(profile);
            _profileRepositoryMock.Setup(x => x.GetById(profileId))
                .Returns(() => null);
            _profileService.Find(profileId);

            //Assert
            _profileRepositoryMock.Verify(x => x.Delete(profile), Times.Once);
        }

        /// <summary>
        /// Delete profile.
        /// Should return nothing when profile is deleted.
        /// </summary>
        [Fact]
        public void Delete_ShouldReturnNothing_WhenProfileDeleted()
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
                UserId = userId,
                User = user,
                ProfileImg = $"img{profileId}.jpg"
            };

            _profileRepositoryMock.Setup(x => x.Insert(newProfile))
                .Callback(() =>
                {
                    newProfile.Id = profileId;
                });
            _profileRepositoryMock.Setup(x => x.GetById(profileId))
                .Returns(() => newProfile);

            //Act
            _profileService.Insert(newProfile);
            var profile = _profileService.Find(profileId);
            _profileService.Delete(profile);
            _profileRepositoryMock.Setup(x => x.GetById(profileId))
                .Returns(() => null);
            var deletedProfile = _profileService.Find(profileId);

            //Assert
            Assert.Null(deletedProfile);
        }

        #endregion

        #region Delete Async function

        /// <summary>
        /// Verify that function Delete Async has been called.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task Verify_FunctionDeleteAsync_HasBeenCalled()
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
                UserId = userId,
                User = user,
                ProfileImg = $"img{profileId}.jpg"
            };

            _profileRepositoryMock.Setup(x => x.InsertAsync(newProfile))
                .Callback(() =>
                {
                    newProfile.Id = profileId;
                });
            _profileRepositoryMock.Setup(x => x.GetByIdAsync(profileId))
                .ReturnsAsync(() => newProfile);

            //Act
            await _profileService.InsertAsync(newProfile);
            var profile = await _profileService.FindAsync(profileId);
            await _profileService.DeleteAsync(profile);

            //Assert
            _profileRepositoryMock.Verify(x => x.DeleteAsync(profile), Times.Once);
        }

        /// <summary>
        /// Async delete message.
        /// Should return nothing when profile is deleted.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task DeleteAsync_ShouldReturnNothing_WhenProfileIsDeleted()
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
                UserId = userId,
                User = user,
                ProfileImg = $"img{profileId}.jpg"
            };

            _profileRepositoryMock.Setup(x => x.InsertAsync(newProfile))
                .Callback(() =>
                {
                    newProfile.Id = profileId;
                });
            _profileRepositoryMock.Setup(x => x.GetByIdAsync(profileId))
                .ReturnsAsync(() => newProfile);

            //Act
            await _profileService.InsertAsync(newProfile);
            var profile = await _profileService.FindAsync(profileId);
            await _profileService.DeleteAsync(profile);
            _profileRepositoryMock.Setup(x => x.GetByIdAsync(profileId))
                .ReturnsAsync(() => null);
            var deletedProfile = await _profileService.FindAsync(profileId);

            //Assert
            Assert.Null(deletedProfile);
        }

        #endregion

        #region Any function With Specification

        /// <summary>
        /// Verify that function Any with specification has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionAny_WithSpecification_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<Data.Models.Profile>();
            var searchUserId = new Guid().ToString();

            for (var i = 0; i < random.Next(100); i++)
            {
                var userId = i == 0
                    ? searchUserId
                    : new Guid().ToString();
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
            _profileRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => profilesList.Any(x => x.UserId.Equals(searchUserId)));

            //Act
            _profileService.Any(specification);

            //Assert
            _profileRepositoryMock.Verify(x => x.Any(specification), Times.Once);
        }

        /// <summary>
        /// Check if there are any profiles with specification.
        /// Should return true with equals specification when profiles exists.
        /// </summary>
        [Fact]
        public void Any_ShouldReturnTrue_WithEqualsSpecification_WhenProfilesExists()
        {
            //Test failed
            //Arrange
            var random = new Random();
            var profilesList = new List<Data.Models.Profile>();
            var searchUserId = new Guid().ToString();

            for (var i = 0; i < random.Next(100); i++)
            {
                var userId = i == 0
                    ? searchUserId
                    : new Guid().ToString();
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
            _profileRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => profilesList.Any(x => x.UserId.Equals(searchUserId)));

            //Act
            var areAnyProfiles = _profileService.Any(specification);

            //Assert
            Assert.True(areAnyProfiles);
        }

        /// <summary>
        /// Check if there are any messages with specification.
        /// Should return false with when messages does not exists.
        /// </summary>
        [Fact]
        public void Any_ShouldReturnFalse_WithEqualSpecification_WhenProfilesExists()
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<Data.Models.Profile>();
            var searchUserId = $"{new Guid().ToString()}1";

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
            _profileRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => profilesList.Any(x => x.UserId.Equals(searchUserId)));

            //Act
            var areAnyProfiles = _profileService.Any(specification);

            //Assert
            Assert.False(areAnyProfiles);
        }

        /// <summary>
        /// Check if there are any profiles with specification.
        /// Should return false with when profiles does not exists.
        /// </summary>
        [Fact]
        public void Any_ShouldReturnNothing_WithEqualSpecification_WhenProfilesDoesNotExists()
        {
            //Arrange
            var searchUserId = new Guid().ToString();
            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => false);

            //Act
            var areAnyProfiles = _profileService.Any(specification);

            //Assert
            Assert.False(areAnyProfiles);
        }

        #endregion

        #region Any Async function With Specification

        /// <summary>
        /// Verify that function Any Async with specification has been called.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task Verify_FunctionAnyAsync_WithSpecification_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<Data.Models.Profile>();
            var searchUserId = new Guid().ToString();

            for (var i = 0; i < random.Next(100); i++)
            {
                var userId = i == 0
                    ? searchUserId
                    : new Guid().ToString();
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
            _profileRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => profilesList.Any(x => x.UserId.Equals(searchUserId)));

            //Act
            await _profileService.AnyAsync(specification);

            //Assert
            _profileRepositoryMock.Verify(x => x.AnyAsync(specification), Times.Once);
        }

        /// <summary>
        /// Async check if there are any profiles with specification.
        /// Should return true with equals specification when profiles exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task AnyAsync_ShouldReturnTrue_WithEqualsSpecification_WhenProfilesExists()
        {
            //Test failed
            //Arrange
            var random = new Random();
            var profilesList = new List<Data.Models.Profile>();
            var searchUserId = new Guid().ToString();

            for (var i = 0; i < random.Next(100); i++)
            {
                var userId = i == 0
                    ? searchUserId
                    : new Guid().ToString();
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
            _profileRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => profilesList.Any(x => x.UserId.Equals(searchUserId)));

            //Act
            var areAnyProfiles = await _profileService.AnyAsync(specification);

            //Assert
            Assert.True(areAnyProfiles);
        }

        /// <summary>
        /// Async check if there are any profiles with specification.
        /// Should return false with when profiles does not exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task AnyAsync_ShouldReturnFalse_WithEqualSpecification_WhenProfilesExists()
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<Data.Models.Profile>();
            var searchUserId = $"{new Guid().ToString()}1";

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
            _profileRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => profilesList.Any(x => x.UserId.Equals(searchUserId)));

            //Act
            var areAnyProfiles = await _profileService.AnyAsync(specification);

            //Assert
            Assert.False(areAnyProfiles);
        }

        #endregion
        /*
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
                .ReturnsAsync(() => commentslist);* /

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
                .ReturnsAsync(() => new List<Comment>());* /

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
        */
    }
}
