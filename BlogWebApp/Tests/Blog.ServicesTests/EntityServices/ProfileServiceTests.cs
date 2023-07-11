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
using ProfileModel = Blog.Data.Models.Profile;

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
        private readonly Mock<IRepository<ProfileModel>> _profileRepositoryMock;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileServiceTests"/> class.
        /// </summary>
        public ProfileServiceTests()
        {
            _profileRepositoryMock = new Mock<IRepository<ProfileModel>>();
            var mapper = new Mock<IMapper>();
            _profileService = new ProfileService(_profileRepositoryMock.Object, mapper.Object);
        }

        #endregion

        #region Get All

        #region Get All function

        /// <summary>
        /// Verify that function Get All has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionGetAll_HasBeenCalled()
        {
            //Arrange  
            var random = new Random();
            var profilesList = new List<ProfileModel>();

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
                profilesList.Add(new ProfileModel
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
        public void GetAll_WhenProfilesExists_ShouldReturnProfiles(int notEqualCount)
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();

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
                profilesList.Add(new ProfileModel
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
        public void GetAll_WhenProfilesDoesNotExists_ShouldReturnNothing()
        {
            //Arrange
            _profileRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => new List<ProfileModel>().AsQueryable());

            //Act
            var profiles = _profileService.GetAll();

            //Assert
            Assert.Empty(profiles);
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
            var profilesList = new List<ProfileModel>();

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
                profilesList.Add(new ProfileModel
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            _profileRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(profilesList);

            //Act
            await _profileService.GetAllAsync();

            //Assert
            _profileRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        /// <summary>
        /// Get all async profiles.
        /// Should return profiles when profiles exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        [Theory]
        [InlineData(0)]
        public async Task GetAllAsync_WhenProfilesExists_ShouldReturnProfiles(int notEqualCount)
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();

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
                profilesList.Add(new ProfileModel
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            _profileRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(profilesList);

            //Act
            var profiles = await _profileService.GetAllAsync();

            //Assert
            Assert.NotNull(profiles);
            Assert.NotEmpty(profiles);
            Assert.NotEqual(notEqualCount, profiles.ToList().Count);
        }

        /// <summary>
        /// Get all async profiles.
        /// Should return nothing when profiles does not exists.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_WhenProfilesDoesNotExists_ShouldReturnNothing()
        {
            //Arrange
            _profileRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => new List<ProfileModel>());

            //Act
            var profiles = await _profileService.GetAllAsync();

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
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
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
        public void GetAll_WithEqualsSpecification_WhenProfilesExists_ShouldReturnProfiles(int notEqualCount)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
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
        public void GetAll_WithEqualSpecification_WhenProfilesExists_ShouldReturnNothing(int equalCount)
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
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
        public void GetAll_WithEqualSpecification_WhenMessagesDoesNotExists_ShouldReturnNothing()
        {
            //Arrange
            var searchUserId = new Guid().ToString();
            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => new List<ProfileModel>().AsQueryable());

            //Act
            var messages = _profileService.GetAll();

            //Assert
            Assert.Empty(messages);
        }

        #endregion

        #region Get all async function With Specification

        /// <summary>
        /// Verify that function Get All Async with specification has been called.
        /// </summary>
        [Fact]
        public async Task Verify_FunctionGetAllAsync_WithSpecification_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }
            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.GetAllAsync(specification))
                .ReturnsAsync(profilesList.Where(x => x.UserId.Contains(searchUserId)).ToList());

            //Act
            await _profileService.GetAllAsync(specification);

            //Assert
            _profileRepositoryMock.Verify(x => x.GetAllAsync(specification), Times.Once);
        }

        /// <summary>
        /// Get all async profiles with specification.
        /// Should return profiles with equals specification when messages exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        [Theory]
        [InlineData(0)]
        public async Task GetAllAsync_WithEqualsSpecification_WhenProfilesExists_ShouldReturnProfiles(int notEqualCount)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }
            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.GetAllAsync(specification))
                .ReturnsAsync(profilesList.Where(x => x.UserId.Contains(searchUserId)).ToList());

            //Act
            var profiles = await _profileService.GetAllAsync(specification);

            //Assert
            Assert.NotNull(profiles);
            Assert.NotEmpty(profiles);
            Assert.NotEqual(notEqualCount, profiles.ToList().Count);
        }

        /// <summary>
        /// Get all async messages with specification.
        /// Should return nothing with  when messages does not exists.
        /// </summary>
        /// <param name="equalCount">The equal count.</param>
        [Theory]
        [InlineData(0)]
        public async void GetAllAsync_WithEqualSpecification_WhenProfilesExists_ShouldReturnNothing(int equalCount)
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }
            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.GetAllAsync(specification))
                .ReturnsAsync(profilesList.Where(x => x.UserId.Contains(searchUserId)).ToList());

            //Act
            var profiles = await _profileService.GetAllAsync(specification);

            //Assert
            Assert.NotNull(profiles);
            Assert.Empty(profiles);
            Assert.Equal(equalCount, profiles.ToList().Count);
        }

        /// <summary>
        /// Get all async messages.
        /// Should return nothing with  when messages does not exists.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_WithEqualSpecification_WhenMessagesDoesNotExists_ShouldReturnNothing()
        {
            //Arrange
            var searchUserId = new Guid().ToString();
            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.GetAllAsync(specification))
                .ReturnsAsync(() => new List<ProfileModel>());

            //Act
            var messages = await _profileService.GetAllAsync();

            //Assert
            Assert.Null(messages);
        }

        #endregion

        #endregion

        #region Find

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
            var newProfile = new ProfileModel
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
        public void Find_WhenProfilesExists_ShouldReturnProfile()
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
            var newProfile = new ProfileModel
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
        public void Find_WhenProfilesDoesNotExists_ShouldReturnNothing()
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
            var newProfile = new ProfileModel
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
        public async Task FindAsync_WhenProfilesExists_ShouldReturnProfiles()
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
            var newProfile = new ProfileModel
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
        public async Task FindAsync_WhenProfilesDoesNotExists_ShouldReturnNothing()
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

        #endregion

        #region Insert

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
            var newProfile = new ProfileModel
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
        public void Insert_WhenProfileExists_ShouldReturnProfile()
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
            var newProfile = new ProfileModel
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

        #region Insert Enumerable function

        /// <summary>
        /// Verify that function Insert Enumerable has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionInsertEnumerable_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var profileId = random.Next(52);
            var itemsCount = random.Next(10);
            var newProfiles = new List<ProfileModel>();

            for (int i = 0; i < itemsCount; i++)
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
                newProfiles.Add(new ProfileModel
                {
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            _profileRepositoryMock.Setup(x => x.Insert(newProfiles))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newProfiles[i].Id = profileId + i;
                    }
                });

            //Act
            _profileService.Insert(newProfiles);

            //Assert
            _profileRepositoryMock.Verify(x => x.Insert(newProfiles), Times.Once);
        }

        /// <summary>
        /// Insert Enumerable profiles.
        /// Should return profiles when profiles created.
        /// </summary>
        [Fact]
        public void InsertEnumerable_WhenProfilesExists_ShouldReturnProfiles()
        {
            //Arrange
            var random = new Random();
            var profileId = random.Next(52);
            var itemsCount = random.Next(10);
            var newProfiles = new List<ProfileModel>();

            for (int i = 0; i < itemsCount; i++)
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
                newProfiles.Add(new ProfileModel
                {
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            _profileRepositoryMock.Setup(x => x.Insert(newProfiles))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newProfiles[i].Id = profileId + i;
                    }
                });

            //Act
            _profileService.Insert(newProfiles);

            //Assert
            newProfiles.ForEach(x =>
            {
                Assert.NotEqual(0, x.Id);
            });
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
            var newProfile = new ProfileModel
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
        public async Task InsertAsync_WhenProfileExists_ShouldReturnProfile()
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
            var newProfile = new ProfileModel
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
            var profileId = random.Next(52);
            var itemsCount = random.Next(10);
            var newProfiles = new List<ProfileModel>();

            for (int i = 0; i < itemsCount; i++)
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
                newProfiles.Add(new ProfileModel
                {
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            _profileRepositoryMock.Setup(x => x.InsertAsync(newProfiles))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newProfiles[i].Id = profileId + i;
                    }
                });

            //Act
            await _profileService.InsertAsync(newProfiles);

            //Assert
            _profileRepositoryMock.Verify(x => x.InsertAsync(newProfiles), Times.Once);
        }

        /// <summary>
        /// Insert Async Enumerable profiles.
        /// Should return profiles when profiles created.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task InsertAsyncEnumerable_WhenProfilesExists_ShouldReturnProfiles()
        {
            //Arrange
            var random = new Random();
            var profileId = random.Next(52);
            var itemsCount = random.Next(10);
            var newProfiles = new List<ProfileModel>();

            for (int i = 0; i < itemsCount; i++)
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
                newProfiles.Add(new ProfileModel
                {
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            _profileRepositoryMock.Setup(x => x.InsertAsync(newProfiles))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newProfiles[i].Id = profileId + i;
                    }
                });

            //Act
            await _profileService.InsertAsync(newProfiles);

            //Assert
            newProfiles.ForEach(x =>
            {
                Assert.NotEqual(0, x.Id);
            });
        }

        #endregion

        #endregion

        #region Update

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
            var newProfile = new ProfileModel
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
        public void Update_WhenProfileExists_ShouldReturnProfile()
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
            var newProfile = new ProfileModel
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

        #region Upadate Enumerable function

        /// <summary>
        /// Verify that function Update Enumerable has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionUpdateEnumerable_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var profileId = random.Next(52);
            var itemsCount = random.Next(10);
            var profileIds = new List<string>();
            var newProfiles = new List<ProfileModel>();

            for (int i = 0; i < itemsCount; i++)
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
                newProfiles.Add(new ProfileModel
                {
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
                profileIds.Add(new Guid().ToString());
            }

            _profileRepositoryMock.Setup(x => x.Insert(newProfiles))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newProfiles[i].Id = profileId + i;
                    }
                });

            //Act
            _profileService.Insert(newProfiles);
            for (var i = 0; i < itemsCount; i++)
            {
                newProfiles[i].UserId = profileIds[i];
            }
            _profileService.Update(newProfiles);

            //Assert
            _profileRepositoryMock.Verify(x => x.Update(newProfiles), Times.Once);
        }

        /// <summary>
        /// Update Enumerable profile.
        /// Should return profile when profile updated.
        /// </summary>
        [Fact]
        public void UpdateEnumerable_WhenProfileExists_ShouldReturnProfile()
        {
            //Arrange
            var random = new Random();
            var profileId = random.Next(52);
            var itemsCount = random.Next(10);
            var newProfiles = new List<ProfileModel>();
            var profileIds = new List<string>();

            for (int i = 0; i < itemsCount; i++)
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
                newProfiles.Add(new ProfileModel
                {
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
                profileIds.Add(new Guid().ToString());
            }

            _profileRepositoryMock.Setup(x => x.Insert(newProfiles))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newProfiles[i].Id = profileId + i;
                    }
                });

            //Act
            _profileService.Insert(newProfiles);

            for (var i = 0; i < itemsCount; i++)
            {
                newProfiles[i].UserId = profileIds[i];
            }
            _profileService.Update(newProfiles);

            //Assert

            for (var i = 0; i < itemsCount; i++)
            {
                Assert.Equal(profileIds[i], newProfiles[i].UserId);
            }
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
            var newProfile = new ProfileModel
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
        public async Task UpdateAsync_WhenMessageExists_ShouldReturnMessage()
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
            var newProfile = new ProfileModel
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

        #region Upadate Async Enumerable function

        /// <summary>
        /// Verify that function Update Async Enumerable has been called.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task Verify_FunctionUpdateAsyncEnumerable_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var profileId = random.Next(52);
            var itemsCount = random.Next(10);
            var profileIds = new List<string>();
            var newProfiles = new List<ProfileModel>();

            for (int i = 0; i < itemsCount; i++)
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
                newProfiles.Add(new ProfileModel
                {
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
                profileIds.Add(new Guid().ToString());
            }

            _profileRepositoryMock.Setup(x => x.InsertAsync(newProfiles))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newProfiles[i].Id = profileId + i;
                    }
                });

            //Act
            await _profileService.InsertAsync(newProfiles);
            for (var i = 0; i < itemsCount; i++)
            {
                newProfiles[i].UserId = profileIds[i];
            }
            await _profileService.UpdateAsync(newProfiles);

            //Assert
            _profileRepositoryMock.Verify(x => x.UpdateAsync(newProfiles), Times.Once);
        }

        /// <summary>
        /// Update Async Enumerable profile.
        /// Should return profile when profile updated.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task UpdateAsyncEnumerable_WhenProfileExists_ShouldReturnProfile()
        {
            //Arrange
            var random = new Random();
            var profileId = random.Next(52);
            var itemsCount = random.Next(10);
            var newProfiles = new List<ProfileModel>();
            var profileIds = new List<string>();

            for (int i = 0; i < itemsCount; i++)
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
                newProfiles.Add(new ProfileModel
                {
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
                profileIds.Add(new Guid().ToString());
            }

            _profileRepositoryMock.Setup(x => x.InsertAsync(newProfiles))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newProfiles[i].Id = profileId + i;
                    }
                });

            //Act
            await _profileService.InsertAsync(newProfiles);

            for (var i = 0; i < itemsCount; i++)
            {
                newProfiles[i].UserId = profileIds[i];
            }
            await _profileService.UpdateAsync(newProfiles);

            //Assert

            for (var i = 0; i < itemsCount; i++)
            {
                Assert.Equal(profileIds[i], newProfiles[i].UserId);
            }
        }

        #endregion

        #endregion

        #region Delete

        #region Delete By Id function

        /// <summary>
        /// Verify that function Delete By Id has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionDeleteById_HasBeenCalled()
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
            var newProfile = new ProfileModel
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
            _profileService.Delete(profileId);
            _profileRepositoryMock.Setup(x => x.GetById(profileId))
                .Returns(() => null);
            _profileService.Find(profileId);

            //Assert
            _profileRepositoryMock.Verify(x => x.Delete(newProfile), Times.Once);
        }

        /// <summary>
        /// Delete By Id profile.
        /// Should return nothing when profile is deleted.
        /// </summary>
        [Fact]
        public void DeleteById_WhenProfileDeleted_ShouldReturnNothing()
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
            var newProfile = new ProfileModel
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
            _profileService.Delete(profileId);
            _profileRepositoryMock.Setup(x => x.GetById(profileId))
                .Returns(() => null);
            var deletedProfile = _profileService.Find(profileId);

            //Assert
            Assert.Null(deletedProfile);
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
            var newProfile = new ProfileModel
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
        /// Delete By Object profile.
        /// Should return nothing when profile is deleted.
        /// </summary>
        [Fact]
        public void DeleteByObject_WhenProfileDeleted_ShouldReturnNothing()
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
            var newProfile = new ProfileModel
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

        #region Delete By Enumerable function

        /// <summary>
        /// Verify that function Delete By Enumerable has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionDeleteByEnumerable_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var profileId = random.Next(52);
            var itemsCount = random.Next(10);
            var newProfiles = new List<ProfileModel>();

            for (int i = 0; i < itemsCount; i++)
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
                newProfiles.Add(new ProfileModel
                {
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            _profileRepositoryMock.Setup(x => x.Insert(newProfiles))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newProfiles[i].Id = profileId + i;
                    }
                });

            //Act
            _profileService.Insert(newProfiles);
            _profileService.Delete(newProfiles);

            //Assert
            _profileRepositoryMock.Verify(x => x.Delete(newProfiles), Times.Once);
        }

        /// <summary>
        /// Delete By Enumerable profile.
        /// Should return nothing when profile is deleted.
        /// </summary>
        [Fact]
        public void DeleteByEnumerable_WhenProfileDeleted_ShouldReturnNothing()
        {
            //Arrange
            var random = new Random();
            var profileId = random.Next(52);
            var itemsCount = random.Next(10);
            var newProfiles = new List<ProfileModel>();

            for (int i = 0; i < itemsCount; i++)
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
                newProfiles.Add(new ProfileModel
                {
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            _profileRepositoryMock.Setup(x => x.Insert(newProfiles))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newProfiles[i].Id = profileId + i;
                    }
                });
            _profileRepositoryMock.Setup(x => x.Delete(newProfiles))
                .Callback(() =>
                {
                    newProfiles = null;
                });

            //Act
            _profileService.Insert(newProfiles);
            _profileService.Delete(newProfiles);

            //Assert
            Assert.Null(newProfiles);
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
            var newProfile = new ProfileModel
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
            await _profileService.DeleteAsync(profileId);

            //Assert
            _profileRepositoryMock.Verify(x => x.DeleteAsync(newProfile), Times.Once);
        }

        /// <summary>
        /// Async delete by id message.
        /// Should return nothing when profile is deleted.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task DeleteAsyncById_WhenProfileIsDeleted_ShouldReturnNothing()
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
            var newProfile = new ProfileModel
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
            await _profileService.DeleteAsync(profileId);
            _profileRepositoryMock.Setup(x => x.GetByIdAsync(profileId))
                .ReturnsAsync(() => null);
            var deletedProfile = await _profileService.FindAsync(profileId);

            //Assert
            Assert.Null(deletedProfile);
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
            var newProfile = new ProfileModel
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
        /// Async delete by object message.
        /// Should return nothing when profile is deleted.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task DeleteAsyncByObject_WhenProfileIsDeleted_ShouldReturnNothing()
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
            var newProfile = new ProfileModel
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

        #region Delete Async By Enumerable function

        /// <summary>
        /// Verify that function Delete Async By Enumerable has been called.
        /// </summary>
        [Fact]
        public async Task Verify_FunctionDeleteAsyncByEnumerable_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var profileId = random.Next(52);
            var itemsCount = random.Next(10);
            var newProfiles = new List<ProfileModel>();

            for (int i = 0; i < itemsCount; i++)
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
                newProfiles.Add(new ProfileModel
                {
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            _profileRepositoryMock.Setup(x => x.InsertAsync(newProfiles))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newProfiles[i].Id = profileId + i;
                    }
                });

            //Act
            await _profileService.InsertAsync(newProfiles);
            await _profileService.DeleteAsync(newProfiles);

            //Assert
            _profileRepositoryMock.Verify(x => x.DeleteAsync(newProfiles), Times.Once);
        }

        /// <summary>
        /// Delete Async By Enumerable profile.
        /// Should return nothing when profile is deleted.
        /// </summary>
        [Fact]
        public async Task DeleteAsyncByEnumerable_WhenProfileDeleted_ShouldReturnNothing()
        {
            //Arrange
            var random = new Random();
            var profileId = random.Next(52);
            var itemsCount = random.Next(10);
            var newProfiles = new List<ProfileModel>();

            for (int i = 0; i < itemsCount; i++)
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
                newProfiles.Add(new ProfileModel
                {
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            _profileRepositoryMock.Setup(x => x.InsertAsync(newProfiles))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newProfiles[i].Id = profileId + i;
                    }
                });
            _profileRepositoryMock.Setup(x => x.DeleteAsync(newProfiles))
                .Callback(() =>
                {
                    newProfiles = null;
                });

            //Act
            await _profileService.InsertAsync(newProfiles);
            await _profileService.DeleteAsync(newProfiles);

            //Assert
            Assert.Null(newProfiles);
        }

        #endregion

        #endregion

        #region Any

        #region Any function With Specification

        /// <summary>
        /// Verify that function Any with specification has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionAny_WithSpecification_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
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
        public void Any_WithEqualsSpecification_WhenProfilesExists_ShouldReturnTrue()
        {
            //Test failed
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
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
        public void Any_WithEqualSpecification_WhenProfilesExists_ShouldReturnFalse()
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
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
        public void Any_WithEqualSpecification_WhenProfilesDoesNotExists_ShouldReturnNothing()
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
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
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
        public async Task AnyAsync_WithEqualsSpecification_WhenProfilesExists_ShouldReturnTrue()
        {
            //Test failed
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
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
        public async Task AnyAsync_WithEqualSpecification_WhenProfilesExists_ShouldReturnFalse()
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
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

        /// <summary>
        /// Async check if there are any profiles with specification.
        /// Should return false with when profiles does not exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task AnyAsync_WithEqualSpecification_WhenProfilesDoesNotExists_ShouldReturnNothing()
        {
            //Arrange
            var searchUserId = new Guid().ToString();
            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => false);

            //Act
            var areAnyProfiles = await _profileService.AnyAsync(specification);

            //Assert
            Assert.False(areAnyProfiles);
        }

        #endregion

        #endregion

        #region First Or Default function With Specification

        /// <summary>
        /// Verify that function First Or Default with specification has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionFirstOrDefault_WithSpecification_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => profilesList.FirstOrDefault(x => x.UserId.Equals(searchUserId)));

            //Act
            _profileService.FirstOrDefault(specification);

            //Assert
            _profileRepositoryMock.Verify(x => x.FirstOrDefault(specification), Times.Once);
        }

        /// <summary>
        /// Get first or default profile with specification.
        /// Should return profile with equals specification when profiles exists.
        /// </summary>
        [Fact]
        public void FirstOrDefault_WithContainsSpecification_WhenProfilesExists_ShouldReturnProfile()
        {
            //Test failed
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => profilesList.FirstOrDefault(x => x.UserId.Equals(searchUserId)));

            //Act
            var profile = _profileService.FirstOrDefault(specification);

            //Assert
            Assert.NotNull(profile);
            Assert.IsType<ProfileModel>(profile);
        }

        /// <summary>
        /// Get first or default profile with specification.
        /// Should return nothing with when profiles exists.
        /// </summary>
        [Fact]
        public void FirstOrDefault_WithEqualSpecification_WhenProfilesExists_ShouldReturnNothing()
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => profilesList.FirstOrDefault(x => x.UserId.Equals(searchUserId)));

            //Act
            var profile = _profileService.FirstOrDefault(specification);

            //Assert
            Assert.Null(profile);
        }

        /// <summary>
        /// Get first or default profile with specification.
        /// Should return nothing with equal specification when profiles does not exists.
        /// </summary>
        [Fact]
        public void FirstOrDefault_WithEqualSpecification_WhenProfilesDoesNotExists_ShouldReturnNothing()
        {
            //Arrange
            var searchUserId = new Guid().ToString();
            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => null);

            //Act
            var profile = _profileService.FirstOrDefault(specification);

            //Assert
            Assert.Null(profile);
        }

        #endregion

        #region Last Or Default function With Specification

        /// <summary>
        /// Verify that function Last Or Default with specification has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionLastOrDefault_WithSpecification_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => profilesList.LastOrDefault(x => x.UserId.Equals(searchUserId)));

            //Act
            _profileService.LastOrDefault(specification);

            //Assert
            _profileRepositoryMock.Verify(x => x.LastOrDefault(specification), Times.Once);
        }

        /// <summary>
        /// Get last or default profile with specification.
        /// Should return profile with equals specification when profiles exists.
        /// </summary>
        [Fact]
        public void LastOrDefault_WithEqualsSpecification_WhenProfilesExists_ShouldReturnProfile()
        {
            //Test failed
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => profilesList.LastOrDefault(x => x.UserId.Equals(searchUserId)));

            //Act
            var profile = _profileService.LastOrDefault(specification);

            //Assert
            Assert.NotNull(profile);
            Assert.IsType<ProfileModel>(profile);
        }

        /// <summary>
        /// Get last or default profile with specification.
        /// Should return nothing with equals specification when profiles does not exists.
        /// </summary>
        [Fact]
        public void LastOrDefault_WithEqualSpecification_WhenProfilesExists_ShouldReturnNothing()
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => profilesList.LastOrDefault(x => x.UserId.Equals(searchUserId)));

            //Act
            var profile = _profileService.LastOrDefault(specification);

            //Assert
            Assert.Null(profile);
        }

        /// <summary>
        /// Get last or default profile with specification.
        /// Should return nothing with specification when profiles does not exists.
        /// </summary>
        [Fact]
        public void LastOrDefault_WithEqualSpecification_WhenProfilesDoesNotExists_ShouldReturnNothing()
        {
            //Arrange
            var searchUserId = new Guid().ToString();
            var specification = new ProfileSpecification(x => x.UserId.Equals(searchUserId));
            _profileRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => null);

            //Act
            var profile = _profileService.LastOrDefault(specification);

            //Assert
            Assert.Null(profile);
        }

        #endregion

        #region Search async function

        /// <summary>
        /// Search the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="postsList">The posts list.</param>
        /// <returns>PagedListResult.</returns>
        protected PagedListResult<ProfileModel> Search(SearchQuery<ProfileModel> query, List<ProfileModel> postsList)
        {
            var sequence = postsList.AsQueryable();

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
                sequence = ((IOrderedQueryable<ProfileModel>)sequence).OrderBy(x => true);
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
            return new PagedListResult<ProfileModel>()
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
        /// <param name="search">The search.</param>
        /// <param name="start">The start.</param>
        /// <param name="take">The take.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="orderType">The order type.</param>
        [Theory]
        [InlineData("test", 0, 10, "User.Email", OrderType.Ascending)]
        [InlineData("test", 10, 10, "User.Email", OrderType.Ascending)]
        [InlineData("test", 10, 20, "User.Email", OrderType.Ascending)]
        [InlineData("test", 0, 100, "User.Email", OrderType.Ascending)]
        public async Task Verify_FunctionSearchAsync_HasBeenCalled(string search, int start, int take, string fieldName, OrderType orderType)
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            var query = new SearchQuery<ProfileModel>
            {
                Skip = start,
                Take = take
            };

            query.AddSortCriteria(new FieldSortOrder<ProfileModel>(fieldName, orderType));

            query.AddFilter(x => x.User.Email.ToUpper().Contains($"{search}".ToUpper()));

            _profileRepositoryMock.Setup(x => x.SearchAsync(query))
                .ReturnsAsync(() =>
                {
                    return Search(query, profilesList);
                });

            //Act
            await _profileService.SearchAsync(query);

            //Assert
            _profileRepositoryMock.Verify(x => x.SearchAsync(query), Times.Once);
        }

        /// <summary>
        /// Search async profiles.
        /// Should return profiles when profiles exists.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="start">The start.</param>
        /// <param name="take">The take.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="orderType">The order type.</param>
        [Theory]
        [InlineData("test", 0, 10, "User.Email", OrderType.Ascending)]
        [InlineData("test", 10, 10, "User.Email", OrderType.Ascending)]
        [InlineData("test", 10, 20, "User.Email", OrderType.Ascending)]
        [InlineData("test", 0, 100, "User.Email", OrderType.Ascending)]
        public async Task SearchAsync_WhenProfilesExists_ShouldReturnProfiles(string search, int start, int take, string fieldName, OrderType orderType)
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            var query = new SearchQuery<ProfileModel>
            {
                Skip = start,
                Take = take
            };

            query.AddSortCriteria(new FieldSortOrder<ProfileModel>(fieldName, orderType));

            query.AddFilter(x => x.User.Email.ToUpper().Contains($"{search}".ToUpper()));

            _profileRepositoryMock.Setup(x => x.SearchAsync(query))
                .ReturnsAsync(() =>
                {
                    return Search(query, profilesList);
                });

            //Act
            var posts = await _profileService.SearchAsync(query);

            //Assert
            Assert.NotNull(posts);
            Assert.NotEmpty(posts.Entities);
        }

        /// <summary>
        /// Search async profiles with specification.
        /// Should return profile with equal specification when profiles exists.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="start">The start.</param>
        /// <param name="take">The take.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="orderType">The order type.</param>
        [Theory]
        [InlineData("test1", 0, 10, "User.Email", OrderType.Ascending)]
        [InlineData("test11", 10, 10, "User.Email", OrderType.Ascending)]
        [InlineData("test11", 10, 20, "User.Email", OrderType.Ascending)]
        [InlineData("test11", 0, 100, "User.Email", OrderType.Ascending)]
        public async Task SearchAsync_WithEqualsSpecification_WhenProfilesExists_ShouldReturnProfile(string search, int start, int take, string fieldName, OrderType orderType)
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            var query = new SearchQuery<ProfileModel>
            {
                Skip = start,
                Take = take
            };

            query.AddSortCriteria(new FieldSortOrder<ProfileModel>(fieldName, orderType));

            query.AddFilter(x => x.User.Email.ToUpper().Contains($"{search}".ToUpper()));

            _profileRepositoryMock.Setup(x => x.SearchAsync(query))
                .ReturnsAsync(() =>
                {
                    return Search(query, profilesList);
                });

            //Act
            var posts = await _profileService.SearchAsync(query);

            //Assert
            Assert.NotNull(posts);
            Assert.NotEmpty(posts.Entities);
            Assert.Single(posts.Entities);
        }

        /// <summary>
        /// Search async profiles with specification.
        /// Should return nothing with  when profiles exists.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="start">The start.</param>
        /// <param name="take">The take.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="orderType">The order type.</param>
        [Theory]
        [InlineData("test-1", 0, 10, "User.Email", OrderType.Ascending)]
        [InlineData("test-2", 10, 10, "User.Email", OrderType.Ascending)]
        [InlineData("test-11", 10, 20, "User.Email", OrderType.Ascending)]
        [InlineData("test-1", 0, 100, "User.Email", OrderType.Ascending)]
        public async Task SearchAsync_WithEqualSpecification_WhenProfilesExists_ShouldReturnNothing(string search, int start, int take, string fieldName, OrderType orderType)
        {
            //Arrange
            var random = new Random();
            var profilesList = new List<ProfileModel>();
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
                profilesList.Add(new ProfileModel
                {
                    Id = i,
                    UserId = userId,
                    User = user,
                    ProfileImg = $"img{i}.jpg"
                });
            }

            var query = new SearchQuery<ProfileModel>
            {
                Skip = start,
                Take = take
            };

            query.AddSortCriteria(new FieldSortOrder<ProfileModel>(fieldName, orderType));

            query.AddFilter(x => x.User.Email.ToUpper().Contains($"{search}".ToUpper()));

            _profileRepositoryMock.Setup(x => x.SearchAsync(query))
                .ReturnsAsync(() =>
                {
                    return Search(query, profilesList);
                });

            //Act
            var posts = await _profileService.SearchAsync(query);

            //Assert
            Assert.NotNull(posts);
            Assert.Empty(posts.Entities);
        }

        /// <summary>
        /// Search async profiles.
        /// Should return nothing when profiles does not exists.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <param name="start">The start.</param>
        /// <param name="take">The take.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="orderType">The order type.</param>
        [Theory]
        [InlineData("Created from ServicesTests 0", 0, 10, "User.Email", OrderType.Ascending)]
        [InlineData("Created from ServicesTests 11", 10, 10, "User.Email", OrderType.Ascending)]
        [InlineData("Created from ServicesTests 11", 10, 20, "User.Email", OrderType.Ascending)]
        [InlineData("Created from ServicesTests 11", 0, 100, "User.Email", OrderType.Ascending)]
        public async Task SearchAsync_WhenProfilesDoesNotExists_ShouldReturnNothing(string search, int start, int take, string fieldName, OrderType orderType)
        {
            //Arrange
            var query = new SearchQuery<ProfileModel>
            {
                Skip = start,
                Take = take
            };

            query.AddSortCriteria(new FieldSortOrder<ProfileModel>(fieldName, orderType));

            query.AddFilter(x => x.User.Email.ToUpper().Contains($"{search}".ToUpper()));

            _profileRepositoryMock.Setup(x => x.SearchAsync(query))
                .ReturnsAsync(() => new PagedListResult<ProfileModel>());

            //Act
            var posts = await _profileService.SearchAsync(query);

            //Assert
            Assert.Empty(posts.Entities);
        }

        #endregion

        #region NotTestedYet
        //GenerateQuery(TableFilter tableFilter, string includeProperties = null)
        //GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
        #endregion
    }
}
