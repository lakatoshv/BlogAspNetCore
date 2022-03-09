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
    /// Post tag relations service tests.
    /// </summary>
    public class PostTagRelationsServiceTests
    {
        #region Fields

        /// <summary>
        /// The posts tags relations service.
        /// </summary>
        private readonly IPostsTagsRelationsService _postsTagsRelationsService;

        /// <summary>
        /// The posts tags relations repository mock.
        /// </summary>
        private readonly Mock<IRepository<PostsTagsRelations>> _postsTagsRelationsRepositoryMock;

        /// <summary>
        /// The posts repository mock.
        /// </summary>
        private readonly Mock<IRepository<Post>> _postsRepositoryMock;

        /// <summary>
        /// The tags repository mock.
        /// </summary>
        private readonly Mock<IRepository<Tag>> _tagsRepositoryMock;

        /// <summary>
        /// The mapper mock.
        /// </summary>
        private readonly Mock<IMapper> _mapper;

        /// <summary>
        /// The posts service mock.
        /// </summary>
        private readonly Mock<IPostsService> _postsServiceMock;

        /// <summary>
        /// The tags service mock.
        /// </summary>
        private readonly Mock<ITagsService> _tagsServiceMock;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="PostTagRelationsServiceTests"/> class.
        /// </summary>
        public PostTagRelationsServiceTests()
        {
            _postsServiceMock = new Mock<IPostsService>();
            _tagsServiceMock = new Mock<ITagsService>();
            _postsTagsRelationsRepositoryMock = new Mock<IRepository<PostsTagsRelations>>();
            _postsRepositoryMock = new Mock<IRepository<Post>>();
            _tagsRepositoryMock = new Mock<IRepository<Tag>>();
            _mapper = new Mock<IMapper>();
            _postsTagsRelationsService = new PostsTagsRelationsService(_postsTagsRelationsRepositoryMock.Object, _tagsServiceMock.Object);
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
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                });
            }


            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll())
                .Returns(postsTagsRelationsList.AsQueryable());

            //Act
            _postsTagsRelationsService.GetAll();

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        /// <summary>
        /// Get all post tag relations.
        /// Should return posts when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        [Theory]
        [InlineData(0)]
        public void GetAll_ShouldReturnPostTagRelations_WhenPostTagRelationExists(int notEqualCount)
        {
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                postsTagsRelationsList.Add(new PostsTagsRelations()
                {
                    Id = i,
                    PostId = i,
                    TagId = i
                });
            }


            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => postsTagsRelationsList.AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll();

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);
        }

        /// <summary>
        /// Get all post tag relations.
        /// Should return posts when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        [Theory]
        [InlineData(0, "Post", "Tag")]
        public void GetAll_ShouldReturnPostTagRelationsWithExistingPostAndTags_WhenPostTagRelationExists(int notEqualCount, string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
            var postId = random.Next(100);
            var postEntity = new Post
            {
                Id = postId,
                Title = $"{postTitle} {postId}",
                Description = $"{postTitle} {postId}",
                Content = $"{postTitle} {postId}",
                ImageUrl = $"{postTitle} {postId}",
            };

            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{tagTitle} {i}",
                };

                postsTagsRelationsList.Add(new PostsTagsRelations()
                {
                    Id = i,
                    PostId = postId,
                    Post = postEntity,
                    TagId = i,
                    Tag = tag
                });
            }


            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => postsTagsRelationsList.AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll().ToList();

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);
            postsTagsRelations.ForEach(postsTagsRelation =>
            {
                Assert.NotNull(postsTagsRelation.Post);
                Assert.Contains(postTitle, postsTagsRelation.Post.Title);

                Assert.NotNull(postsTagsRelation.Tag);
                Assert.Contains(tagTitle, postsTagsRelation.Tag.Title);
            });
        }

        #endregion
    }
}
