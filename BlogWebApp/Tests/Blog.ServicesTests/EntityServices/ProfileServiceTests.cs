using AutoMapper;
using Blog.Data.Models;
using Blog.Data.Repository;
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
                var user = new ApplicationUser
                {
                    Id = new Guid().ToString(),
                    FirstName = "Test fn",
                    LastName = "Test ln",
                    Email = "test@test.test",
                    UserName = "test@test.test"
                };
                profilesList.Add(new Data.Models.Profile
                {
                    Id = i,
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

        #endregion
    }
}
