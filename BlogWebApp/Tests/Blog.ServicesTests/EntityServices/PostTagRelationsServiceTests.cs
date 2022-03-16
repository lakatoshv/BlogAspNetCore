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
        /// <param name="equalCount">The equal count.</param>
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
        /// Get all post tag relations.
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
        /// Get all post tag relations.
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

        #region Find function

        /// <summary>
        /// Verify that function Find has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionFind_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                Id = id,
                PostId = id,
                TagId = id,
                Tag = tag
            };
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            _postsTagsRelationsService.Find(id);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.GetById(id), Times.Once);
        }

        /// <summary>
        /// Find post tag relation.
        /// Should return post when post exists.
        /// </summary>
        [Fact]
        public void Find_ShouldReturnPostTagRelation_WhenPostTagRelationExists()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                Id = id,
                PostId = id,
                TagId = id,
                Tag = tag
            };
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            var postsTagsRelations = _postsTagsRelationsService.Find(id);

            //Assert
            Assert.Equal(id, postsTagsRelations.Id);
        }

        /// <summary>
        /// Find post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">THe tag title.</param>
        [Theory]
        [InlineData("Post", "Tag")]
        public void Find_ShouldReturnPostTagRelationWithExistingPostAndTags_WhenPostTagRelationExists(string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var postEntity = new Post
            {
                Id = id,
                Title = $"{postTitle} {id}",
                Description = $"{postTitle} {id}",
                Content = $"{postTitle} {id}",
                ImageUrl = $"{postTitle} {id}",
            };
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                Id = id,
                PostId = id,
                Post = postEntity,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            var postsTagsRelations = _postsTagsRelationsService.Find(id);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.Equal(id, postsTagsRelations.Id);
            Assert.NotNull(postsTagsRelations.Post);
            Assert.Contains(postTitle, postsTagsRelations.Post.Title);

            Assert.NotNull(postsTagsRelations.Tag);
            Assert.Contains(tagTitle, postsTagsRelations.Tag.Title);
        }

        /// <summary>
        /// Find post tag relation.
        /// Should return nothing when post does not exists.
        /// </summary>
        [Fact]
        public void Find_ShouldReturnNothing_WhenPostTagRelationDoesNotExists()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(() => null);

            //Act
            var postsTagsRelations = _postsTagsRelationsService.Find(id);

            //Assert
            Assert.Null(postsTagsRelations);
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
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                Id = id,
                PostId = id,
                TagId = id,
                Tag = tag
            };
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => newPostsTagsRelation);

            //Act
            await _postsTagsRelationsService.FindAsync(id);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.GetByIdAsync(id), Times.Once);
        }

        /// <summary>
        /// Async find post tag relation.
        /// Should return post when post exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task FindAsync_ShouldReturnPostTagRelation_WhenPostTagRelationExists()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                Id = id,
                PostId = id,
                TagId = id,
                Tag = tag
            };
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => newPostsTagsRelation);

            //Act
            var post = await _postsTagsRelationsService.FindAsync(id);

            //Assert
            Assert.Equal(id, post.Id);
        }

        /// <summary>
        /// Find async post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">THe tag title.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Post", "Tag")]
        public async Task FindAsync_ShouldReturnPostTagRelationWithExistingPostAndTags_WhenPostTagRelationExists(string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var postEntity = new Post
            {
                Id = id,
                Title = $"{postTitle} {id}",
                Description = $"{postTitle} {id}",
                Content = $"{postTitle} {id}",
                ImageUrl = $"{postTitle} {id}",
            };
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                Id = id,
                PostId = id,
                Post = postEntity,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => newPostsTagsRelation);

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.FindAsync(id);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.Equal(id, postsTagsRelations.Id);
            Assert.NotNull(postsTagsRelations.Post);
            Assert.Contains(postTitle, postsTagsRelations.Post.Title);

            Assert.NotNull(postsTagsRelations.Tag);
            Assert.Contains(tagTitle, postsTagsRelations.Tag.Title);
        }

        /// <summary>
        /// Async find post tag relation.
        /// Should return nothing when post does not exists.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task FindAsync_ShouldReturnNothing_WhenPostTagRelationsDoesNotExists()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.FindAsync(id);

            //Assert
            Assert.Null(postsTagsRelations);
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
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.Insert(newPostsTagsRelation), Times.Once);
        }

        /// <summary>
        /// Insert post tag relation.
        /// Should return post when post created.
        /// </summary>
        [Fact]
        public void Insert_ShouldReturnPostTagRelation_WhenPostExists()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);

            //Assert
            Assert.NotEqual(0, newPostsTagsRelation.Id);
        }

        /// <summary>
        /// Insert post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">THe tag title.</param>
        [Theory]
        [InlineData("Post", "Tag")]
        public void Insert_ShouldReturnPostTagRelationWithExistingPostAndTags_WhenPostTagRelationExists(string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var postEntity = new Post
            {
                Title = $"{postTitle} {id}",
                Description = $"{postTitle} {id}",
                Content = $"{postTitle} {id}",
                ImageUrl = $"{postTitle} {id}",
            };
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                Id = id,
                PostId = id,
                Post = postEntity,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);

            //Assert
            Assert.NotEqual(0, newPostsTagsRelation.Id);
            Assert.NotNull(newPostsTagsRelation.Post);
            Assert.Contains(postTitle, newPostsTagsRelation.Post.Title);

            Assert.NotNull(newPostsTagsRelation.Tag);
            Assert.Contains(tagTitle, newPostsTagsRelation.Tag.Title);
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
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelation);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.InsertAsync(newPostsTagsRelation), Times.Once);
        }

        /// <summary>
        /// Async insert post tag relations.
        /// Should return post when post created.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task InsertAsync_ShouldReturnPost_WhenPostExists()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelation);

            //Assert
            Assert.NotEqual(0, newPostsTagsRelation.Id);
        }

        /// <summary>
        /// Async insert post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">THe tag title.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Post", "Tag")]
        public async Task InsertAsync_ShouldReturnPostTagRelationWithExistingPostAndTags_WhenPostTagRelationExists(string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var postEntity = new Post
            {
                Id = id,
                Title = $"{postTitle} {id}",
                Description = $"{postTitle} {id}",
                Content = $"{postTitle} {id}",
                ImageUrl = $"{postTitle} {id}",
            };
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                Id = id,
                PostId = id,
                Post = postEntity,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelation);

            //Assert
            Assert.NotEqual(0, newPostsTagsRelation.Id);
            Assert.NotNull(newPostsTagsRelation.Post);
            Assert.Contains(postTitle, newPostsTagsRelation.Post.Title);

            Assert.NotNull(newPostsTagsRelation.Tag);
            Assert.Contains(tagTitle, newPostsTagsRelation.Tag.Title);
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
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };

            var newTag = new Tag
            {
                Id = id + 1,
                Title = $"Tag {id + 1}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);
            var postsTagsRelations = _postsTagsRelationsService.Find(id);
            postsTagsRelations.TagId = newTag.Id;
            postsTagsRelations.Tag = newTag;
            _postsTagsRelationsService.Update(postsTagsRelations);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.Update(postsTagsRelations), Times.Once);
        }

        /// <summary>
        /// Update  post tag relation.
        /// Should return post tag relation when post tag relation updated.
        /// </summary>
        [Fact]
        public void Update_ShouldReturnPostTagRelation_WhenPostTagRelationsExists()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };

            var newTag = new Tag
            {
                Id = id + 1,
                Title = $"Tag {id + 1}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);
            var postsTagsRelations = _postsTagsRelationsService.Find(id);
            postsTagsRelations.TagId = newTag.Id;
            postsTagsRelations.Tag = newTag;
            _postsTagsRelationsService.Update(newPostsTagsRelation);

            //Assert
            Assert.Equal(postsTagsRelations.TagId, newTag.Id);
            Assert.Equal(postsTagsRelations.Tag.Title, newTag.Title);
        }

        /// <summary>
        /// Insert post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">THe tag title.</param>
        [Theory]
        [InlineData("Post", "Tag")]
        public void Update_ShouldReturnPostTagRelationWithExistingPostAndTags_WhenPostTagRelationExists(string postTitle, string tagTitle)
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var postEntity = new Post
            {
                Title = $"{postTitle} {id}",
                Description = $"{postTitle} {id}",
                Content = $"{postTitle} {id}",
                ImageUrl = $"{postTitle} {id}",
            };
            var tag = new Tag
            {
                Id = id,
                Title = $"{tagTitle} {id}"
            };
            var newTag = new Tag
            {
                Id = id + 1,
                Title = $"{tagTitle} {id + 1}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                Id = id,
                PostId = id,
                Post = postEntity,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);
            var postsTagsRelations = _postsTagsRelationsService.Find(id);
            postsTagsRelations.TagId = newTag.Id;
            postsTagsRelations.Tag = newTag;
            _postsTagsRelationsService.Update(newPostsTagsRelation);

            //Assert
            Assert.NotEqual(0, postsTagsRelations.Id);
            Assert.NotNull(newPostsTagsRelation.Post);
            Assert.Contains(postTitle, newPostsTagsRelation.Post.Title);

            Assert.NotNull(newPostsTagsRelation.Tag);
            Assert.Contains(tagTitle, newPostsTagsRelation.Tag.Title);
            Assert.Equal(postsTagsRelations.TagId, newTag.Id);
            Assert.Equal(postsTagsRelations.Tag.Title, newTag.Title);
        }

        #endregion

        #region Update Async function

        /// <summary>
        /// Verify that function Update Async has been called.
        /// Should return post tag relation when post tag relation updated.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task Verify_FunctionUpdateAsync_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var tag = new Tag
            {
                Id = id,
                Title = $"Tag {id}"
            };

            var newTag = new Tag
            {
                Id = id + 1,
                Title = $"Tag {id + 1}"
            };
            var newPostsTagsRelation = new PostsTagsRelations
            {
                PostId = id,
                TagId = id,
                Tag = tag
            };

            _postsTagsRelationsRepositoryMock.Setup(x => x.InsertAsync(newPostsTagsRelation))
                .Callback(() =>
                {
                    newPostsTagsRelation.Id = id;
                });
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => newPostsTagsRelation);

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelation);
            var postsTagsRelations = await _postsTagsRelationsService.FindAsync(id);
            postsTagsRelations.TagId = newTag.Id;
            postsTagsRelations.Tag = newTag;
            await _postsTagsRelationsService.UpdateAsync(postsTagsRelations);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.UpdateAsync(postsTagsRelations), Times.Once);
        }

        #endregion
    }
}
