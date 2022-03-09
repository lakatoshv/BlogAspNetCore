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
using Blog.Data.Specifications.Base;
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
        /// The post tag relations tags relations service.
        /// </summary>
        private readonly IPostsTagsRelationsService _postsTagsRelationsService;

        /// <summary>
        /// The post tag relations tags relations repository mock.
        /// </summary>
        private readonly Mock<IRepository<PostsTagsRelations>> _postsTagsRelationsRepositoryMock;

        /// <summary>
        /// The post tag relations repository mock.
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
        /// The post tag relations service mock.
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
        /// Should return post tag relations when post tag relations exists.
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
        /// Should return post tag relations when post tag relations exists.
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

        /// <summary>
        /// Get all post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        [Theory]
        [InlineData(0, "Post", "Tag")]
        public void GetAll_ShouldReturnPostTagRelationsWithExistingPostAndTagsAndShouldContainsTheSameTagsCount_WhenPostTagRelationExists(int notEqualCount, string postTitle, string tagTitle)
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
            var postsTagsRelationsCount = random.Next(100);

            for (var i = 0; i < postsTagsRelationsCount; i++)
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
            Assert.Equal(postsTagsRelationsCount, postsTagsRelations.Count);
        }

        /// <summary>
        /// Get all post tag relations.
        /// Should return nothing when post tag relations does not exists.
        /// </summary>
        [Fact]
        public void GetAll_ShouldReturnNothing_WhenPostTagRelationsDoesNotExists()
        {
            //Arrange
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => new List<PostsTagsRelations>().AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll();

            //Assert
            Assert.Empty(postsTagsRelations);
        }

        #endregion

        #region Get all function With Specification

        /// <summary>
        /// Verify that function Get All with specification has been called.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Tag ")]
        public void Verify_FunctionGetAll_WithSpecification_HasBeenCalled(string titleSearch)
        {
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{titleSearch} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(titleSearch)).AsQueryable());

            //Act
            _postsTagsRelationsService.GetAll(specification);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.GetAll(specification), Times.Once);
        }

        /// <summary>
        /// Get all post tag relations with specification.
        /// Should return post tag relations with contains specification when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData(0, "Created from ServicesTests ")]
        public void GetAll_ShouldReturnPostTagRelations_WithContainsSpecification_WhenPostTagRelationsExists(int notEqualCount, string titleSearch)
        {
            //Test failed
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"{titleSearch} {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(titleSearch)).AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);
        }

        /// <summary>
        /// Get all post tag relations with specification.
        /// Should return post with equal specification when post tag relations exists.
        /// </summary>
        /// <param name="equalCount">The equal cou nt.</param>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData(1, "Tag 0")]
        public void GetAll_ShouldReturnPost_WithEqualsSpecification_WhenPostsExists(int equalCount, string titleSearch)
        {
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(titleSearch)).AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.Equal(equalCount, postsTagsRelations.ToList().Count);
        }

        /// <summary>
        /// Get all post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">The tag title.</param>
        [Theory]
        [InlineData(0, "Post", "Tag")]
        public void GetAll_ShouldReturnPostTagRelationsWithExistingPostAndTags_WithContainsSpecification_WhenPostTagRelationExists(int notEqualCount, string postTitle, string tagTitle)
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

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(tagTitle));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(tagTitle)).AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll(specification).ToList();

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

        /// <summary>
        /// Get all post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        [Theory]
        [InlineData(0, "Post", "Tag")]
        public void GetAll_ShouldReturnPostTagRelationsWithExistingPostAndTagsAndShouldContainsTheSameTagsCount_WithContainsSpecification_WhenPostTagRelationsExists(int notEqualCount, string postTitle, string tagTitle)
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
            var postsTagsRelationsCount = random.Next(100);

            for (var i = 0; i < postsTagsRelationsCount; i++)
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

            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Contains(tagTitle));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(tagTitle)).AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll(specification).ToList();

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);
            Assert.Equal(postsTagsRelationsCount, postsTagsRelations.Count);
        }

        /// <summary>
        /// Get all posts.
        /// Should return nothing with  when post tag relations does not exists.
        /// </summary>
        /// <param name="equalCount">The equal count.</param>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData(0, "Tag -1")]
        public void GetAll_ShouldReturnNothing_WithEqualSpecification_WhenPostTagRelationsExists(int equalCount, string titleSearch)
        {
            //Arrange
            var random = new Random();
            var postsTagsRelationsList = new List<PostsTagsRelations>();

            for (var i = 0; i < random.Next(100); i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                postsTagsRelationsList.Add(new PostsTagsRelations
                {
                    Id = i,
                    PostId = i,
                    TagId = i,
                    Tag = tag
                });
            }


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => postsTagsRelationsList.Where(x => x.Tag.Title.Contains(titleSearch)).AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.Empty(postsTagsRelations);
            Assert.Equal(equalCount, postsTagsRelations.ToList().Count);
        }

        /// <summary>
        /// Get all posts.
        /// Should return nothing with  when post tag relations does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests 0")]
        public void GetAll_ShouldReturnNothing_WithEqualSpecification_WhenPostTagRelationsDoesNotExists(string titleSearch)
        {
            //Arrange
            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll(specification))
                .Returns(() => new List<PostsTagsRelations>().AsQueryable());

            //Act
            var postsTagsRelations = _postsTagsRelationsService.GetAll();

            //Assert
            Assert.Empty(postsTagsRelations);
        }

        #endregion
    }
}
