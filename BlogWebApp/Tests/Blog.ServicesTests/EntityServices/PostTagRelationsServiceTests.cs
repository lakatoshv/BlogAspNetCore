﻿using Blog.Data.Models;
using Blog.Data.Repository;
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

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="PostTagRelationsServiceTests"/> class.
        /// </summary>
        public PostTagRelationsServiceTests()
        {
            var tagsServiceMock = new Mock<ITagsService>();
            _postsTagsRelationsRepositoryMock = new Mock<IRepository<PostsTagsRelations>>();
            _postsTagsRelationsService = new PostsTagsRelationsService(_postsTagsRelationsRepositoryMock.Object, tagsServiceMock.Object);
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
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">The tag title.</param>
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

            void Action(PostsTagsRelations postsTagsRelation)
            {
                Assert.NotNull(postsTagsRelation.Post);
                Assert.Contains(postTitle, postsTagsRelation.Post.Title);

                Assert.NotNull(postsTagsRelation.Tag);
                Assert.Contains(tagTitle, postsTagsRelation.Tag.Title);
            }

            postsTagsRelations.ForEach(Action);
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

            void Action(PostsTagsRelations postsTagsRelation)
            {
                Assert.NotNull(postsTagsRelation.Post);
                Assert.Contains(postTitle, postsTagsRelation.Post.Title);

                Assert.NotNull(postsTagsRelation.Tag);
                Assert.Contains(tagTitle, postsTagsRelation.Tag.Title);
            }

            postsTagsRelations.ForEach(Action);
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

        #region Insert Enumerable function

        /// <summary>
        /// Verify that function Insert Enumerable has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionInsertEnumerable_HasBeenCalled()
        {
            //Arrange
            var random = new Random();
            var id = random.Next(52);
            var itemsCount = random.Next(10);
            var newPostsTagsRelations = new List<PostsTagsRelations>();


            for (int i = 0; i < itemsCount; i++)
            {
                var tag = new Tag
                {
                    Id = i,
                    Title = $"Tag {i}"
                };
                newPostsTagsRelations.Add(new PostsTagsRelations
                {
                    PostId = id,
                    TagId = tag.Id,
                    Tag = tag
                });
            }

            _postsTagsRelationsRepositoryMock.Setup(x => x.Insert(newPostsTagsRelations))
                .Callback(() =>
                {
                    for (var i = 0; i < itemsCount; i++)
                    {
                        newPostsTagsRelations[i].Id = id + i;
                    }
                });

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelations);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.Insert(newPostsTagsRelations), Times.Once);
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

        /// <summary>
        /// Async update post tag relation.
        /// Should return post tag relation when post tag relation updated.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task UpdateAsync_ShouldReturnPostTagRelation_WhenPostTagRelationExists()
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
            await _postsTagsRelationsService.UpdateAsync(newPostsTagsRelation);

            //Assert
            Assert.Equal(postsTagsRelations.TagId, newTag.Id);
            Assert.Equal(postsTagsRelations.Tag.Title, newTag.Title);
        }

        /// <summary>
        /// Update async post tag relations.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="postTitle">The post title.</param>
        /// <param name="tagTitle">THe tag title.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Post", "Tag")]
        public async Task UpdateAsync_ShouldReturnPostTagRelationWithExistingPostAndTags_WhenPostTagRelationExists(string postTitle, string tagTitle)
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
            await _postsTagsRelationsService.UpdateAsync(newPostsTagsRelation);

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

        #region Delete function

        /// <summary>
        /// Verify that function Delete has been called.
        /// </summary>
        [Fact]
        public void Verify_FunctionDelete_HasBeenCalled()
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);
            var postsTagsRelations = _postsTagsRelationsService.Find(id);
            _postsTagsRelationsService.Delete(postsTagsRelations);
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => null);
            _postsTagsRelationsService.Find(id);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.Delete(postsTagsRelations), Times.Once);
        }

        /// <summary>
        /// Delete post tag relation.
        /// Should return nothing when post tag relation is deleted.
        /// </summary>
        [Fact]
        public void Delete_ShouldReturnNothing_WhenPostTagRelationIsDeleted()
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => newPostsTagsRelation);

            //Act
            _postsTagsRelationsService.Insert(newPostsTagsRelation);
            var postsTagsRelations = _postsTagsRelationsService.Find(id);
            _postsTagsRelationsService.Delete(postsTagsRelations);
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetById(id))
                .Returns(() => null);
            var postsTagsRelation = _postsTagsRelationsService.Find(id);

            //Assert
            Assert.Null(postsTagsRelation);
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => newPostsTagsRelation);

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelation);
            var postsTagsRelation = await _postsTagsRelationsService.FindAsync(id);
            await _postsTagsRelationsService.DeleteAsync(postsTagsRelation);
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => null);
            await _postsTagsRelationsService.FindAsync(id);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.DeleteAsync(postsTagsRelation), Times.Once);
        }

        /// <summary>
        /// Async delete post tag relation.
        /// Should return nothing when post tag relation is deleted.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task DeleteAsync_ShouldReturnNothing_WhenPostTagRelationIsDeleted()
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => newPostsTagsRelation);

            //Act
            await _postsTagsRelationsService.InsertAsync(newPostsTagsRelation);
            var postsTagsRelations = await _postsTagsRelationsService.FindAsync(id);
            await _postsTagsRelationsService.DeleteAsync(postsTagsRelations);
            _postsTagsRelationsRepositoryMock.Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(() => null);
            var postsTagsRelation = await _postsTagsRelationsService.FindAsync(id);

            //Assert
            Assert.Null(postsTagsRelation);
        }

        #endregion

        #region Any function With Specification

        /// <summary>
        /// Verify that function Any with specification has been called.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests ")]
        public void Verify_FunctionAny_WithSpecification_HasBeenCalled(string titleSearch)
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => postsTagsRelationsList.Any(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            _postsTagsRelationsService.Any(specification);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.Any(specification), Times.Once);
        }

        /// <summary>
        /// Check if there are any post tag relations with specification.
        /// Should return true with contains specification when post tag relations exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests ")]
        public void Any_ShouldReturnTrue_WithContainsSpecification_WhenPostTagRelationsExists(string titleSearch)
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => postsTagsRelationsList.Any(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var areAnyPosts = _postsTagsRelationsService.Any(specification);

            //Assert
            Assert.True(areAnyPosts);
        }

        /// <summary>
        /// Check if there are any post tag relations with specification.
        /// Should return post with equal specification when post tag relations exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests 0")]
        public void Any_ShouldReturnTrue_WithEqualsSpecification_WhenPostTagRelationsExists(string titleSearch)
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


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => postsTagsRelationsList.Any(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var areAnyPosts = _postsTagsRelationsService.Any(specification);

            //Assert
            Assert.True(areAnyPosts);
        }

        /// <summary>
        /// Check if there are any post tag relations with specification.
        /// Should return false with when post tag relations does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests -1")]
        public void Any_ShouldReturnFalse_WithEqualSpecification_WhenPostTagRelationsExists(string titleSearch)
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => postsTagsRelationsList.Any(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var areAnyPosts = _postsTagsRelationsService.Any(specification);

            //Assert
            Assert.False(areAnyPosts);
        }

        /// <summary>
        /// Check if there are any post tag relations with specification.
        /// Should return false with when post tag relations does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests 0")]
        public void Any_ShouldReturnNothing_WithEqualSpecification_WhenPostTagRelationDoesNotExists(string titleSearch)
        {
            //Arrange
            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.Any(specification))
                .Returns(() => false);

            //Act
            var areAnyPosts = _postsTagsRelationsService.Any(specification);

            //Assert
            Assert.False(areAnyPosts);
        }

        #endregion

        #region Any Async function With Specification

        /// <summary>
        /// Verify that function Any Async with specification has been called.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Created from ServicesTests ")]
        public async Task Verify_FunctionAnyAsync_WithSpecification_HasBeenCalled(string titleSearch)
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => postsTagsRelationsList.Any(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            await _postsTagsRelationsService.AnyAsync(specification);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.AnyAsync(specification), Times.Once);
        }

        /// <summary>
        /// Async check if there are any post tag relations with specification.
        /// Should return true with contains specification when post tag relations exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Created from ServicesTests ")]
        public async Task AnyAsync_ShouldReturnTrue_WithContainsSpecification_WhenPostTagRelationsExists(string titleSearch)
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => postsTagsRelationsList.Any(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var areAnyPosts = await _postsTagsRelationsService.AnyAsync(specification);

            //Assert
            Assert.True(areAnyPosts);
        }

        /// <summary>
        /// Async check if there are any post tag relations with specification.
        /// Should return post with equal specification when post tag relations exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Created from ServicesTests 0")]
        public async Task AnyAsync_ShouldReturnTrue_WithEqualsSpecification_WhenPostTagRelationsExists(string titleSearch)
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


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => postsTagsRelationsList.Any(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var areAnyPosts = await _postsTagsRelationsService.AnyAsync(specification);

            //Assert
            Assert.True(areAnyPosts);
        }

        /// <summary>
        /// Async check if there are any post tag relations with specification.
        /// Should return false with when post tag relations does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Created from ServicesTests -1")]
        public async Task AnyAsync_ShouldReturnFalse_WithEqualSpecification_WhenPostTagRelationsExists(string titleSearch)
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => postsTagsRelationsList.Any(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var areAnyPosts = await _postsTagsRelationsService.AnyAsync(specification);

            //Assert
            Assert.False(areAnyPosts);
        }

        /// <summary>
        /// Async check if there are any post tag relations with specification.
        /// Should return false with when post tag relations does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData("Created from ServicesTests 0")]
        public async Task AnyAsync_ShouldReturnNothing_WithEqualSpecification_WhenPostTagRelationDoesNotExists(string titleSearch)
        {
            //Arrange
            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.AnyAsync(specification))
                .ReturnsAsync(() => false);

            //Act
            var areAnyPosts = await _postsTagsRelationsService.AnyAsync(specification);

            //Assert
            Assert.False(areAnyPosts);
        }

        #endregion

        #region First Or Default function With Specification

        /// <summary>
        /// Verify that function First Or Default with specification has been called.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests ")]
        public void Verify_FunctionFirstOrDefault_WithSpecification_HasBeenCalled(string titleSearch)
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => postsTagsRelationsList.FirstOrDefault(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            _postsTagsRelationsService.FirstOrDefault(specification);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.FirstOrDefault(specification), Times.Once);
        }

        /// <summary>
        /// Get first or default post with specification.
        /// Should return post with contains specification when post tag relations exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests ")]
        public void FirstOrDefault_ShouldReturnTrue_WithContainsSpecification_WhenPostTagRelationsExists(string titleSearch)
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => postsTagsRelationsList.FirstOrDefault(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var postsTagsRelations = _postsTagsRelationsService.FirstOrDefault(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.IsType<PostsTagsRelations>(postsTagsRelations);
        }

        /// <summary>
        /// Get first or default post with specification.
        /// Should return post with equal specification when post tag relations exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Tag 0")]
        public void FirstOrDefault_ShouldReturnTrue_WithEqualsSpecification_WhenPostTagRelationsExists(string titleSearch)
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => postsTagsRelationsList.FirstOrDefault(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var postsTagsRelations = _postsTagsRelationsService.FirstOrDefault(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.IsType<PostsTagsRelations>(postsTagsRelations);
        }

        /// <summary>
        /// Get first or default post with specification.
        /// Should return nothing with when post does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Tag -1")]
        public void FirstOrDefault_ShouldReturnNothing_WithEqualSpecification_WhenPostTagRelationsExists(string titleSearch)
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => postsTagsRelationsList.FirstOrDefault(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var post = _postsTagsRelationsService.FirstOrDefault(specification);

            //Assert
            Assert.Null(post);
        }

        /// <summary>
        /// Get first or default post with specification.
        /// Should return nothing with when post tag relations does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Tag 0")]
        public void FirstOrDefault_ShouldReturnNothing_WithEqualSpecification_WhenPostTagRelationDoesNotExists(string titleSearch)
        {
            //Arrange
            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.FirstOrDefault(specification))
                .Returns(() => null);

            //Act
            var postsTagsRelations = _postsTagsRelationsService.FirstOrDefault(specification);

            //Assert
            Assert.Null(postsTagsRelations);
        }

        #endregion

        #region Last Or Default function With Specification

        /// <summary>
        /// Verify that function Last Or Default with specification has been called.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests ")]
        public void Verify_FunctionLastOrDefault_WithSpecification_HasBeenCalled(string titleSearch)
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => postsTagsRelationsList.LastOrDefault(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            _postsTagsRelationsService.LastOrDefault(specification);

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.LastOrDefault(specification), Times.Once);
        }

        /// <summary>
        /// Get last or default post with specification.
        /// Should return post with contains specification when post tag relations exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests ")]
        public void LastOrDefault_ShouldReturnTrue_WithContainsSpecification_WhenPostTagRelationsExists(string titleSearch)
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => postsTagsRelationsList.LastOrDefault(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var postsTagsRelations = _postsTagsRelationsService.LastOrDefault(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.IsType<PostsTagsRelations>(postsTagsRelations);
        }

        /// <summary>
        /// Get last or default post with specification.
        /// Should return post with equal specification when post tag relations exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests 0")]
        public void LastOrDefault_ShouldReturnTrue_WithEqualsSpecification_WhenPostTagRelationsExists(string titleSearch)
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


            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => postsTagsRelationsList.LastOrDefault(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var postsTagsRelations = _postsTagsRelationsService.LastOrDefault(specification);

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.IsType<PostsTagsRelations>(postsTagsRelations);
        }

        /// <summary>
        /// Get last or default post with specification.
        /// Should return nothing with when post does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests -1")]
        public void LastOrDefault_ShouldReturnNothing_WithEqualSpecification_WhenPostTagRelationsExists(string titleSearch)
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
            _postsTagsRelationsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => postsTagsRelationsList.LastOrDefault(x => x.Tag.Title.Contains(titleSearch)));

            //Act
            var postsTagsRelations = _postsTagsRelationsService.LastOrDefault(specification);

            //Assert
            Assert.Null(postsTagsRelations);
        }

        /// <summary>
        /// Get last or default post with specification.
        /// Should return nothing with when post tag relations does not exists.
        /// </summary>
        /// <param name="titleSearch">The title search.</param>
        [Theory]
        [InlineData("Created from ServicesTests 0")]
        public void LastOrDefault_ShouldReturnNothing_WithEqualSpecification_WhenPostTagRelationDoesNotExists(string titleSearch)
        {
            //Arrange
            var specification = new BaseSpecification<PostsTagsRelations>(x => x.Tag.Title.Equals(titleSearch));
            _postsTagsRelationsRepositoryMock.Setup(x => x.LastOrDefault(specification))
                .Returns(() => null);

            //Act
            var postsTagsRelations = _postsTagsRelationsService.LastOrDefault(specification);

            //Assert
            Assert.Null(postsTagsRelations);
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
            /*_generalServiceMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => postslist);*/

            //Act
            await _postsTagsRelationsService.GetAllAsync();

            //Assert
            _postsTagsRelationsRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        /// <summary>
        /// Async get all posts.
        /// Should return post tag relations when post tag relations exists.
        /// </summary>
        /// <param name="notEqualCount">The not equal count.</param>
        //[Theory]
        //[InlineData(0)]
        public async Task GetAllAsync_ShouldReturnPosts_WhenPostsExists(int notEqualCount)
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


            _postsTagsRelationsRepositoryMock.Setup(x => x.GetAll())
                .Returns(() => postsTagsRelationsList.AsQueryable());

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.GetAllAsync();

            //Assert
            Assert.NotNull(postsTagsRelations);
            Assert.NotEmpty(postsTagsRelations);
            Assert.NotEqual(notEqualCount, postsTagsRelations.ToList().Count);
        }

        /// <summary>
        /// Async get all posts.
        /// Should return nothing when post tag relations does not exists.
        /// </summary>
        //[Fact]
        public async Task GetAllAsync_ShouldReturnNothing_WhenPostDoesNotExists()
        {
            //Test failed
            //Arrange
            /*_generalServiceMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => new List<Post>());*/

            //Act
            var postsTagsRelations = await _postsTagsRelationsService.GetAllAsync();

            //Assert
            Assert.Empty(postsTagsRelations);
        }

        //SearchAsync(SearchQuery<T> searchQuery)
        //GetAllAsync(ISpecification<T> specification)
        //GenerateQuery(TableFilter tableFilter, string includeProperties = null)
        //GetMemberName<T, TValue>(Expression<Func<T, TValue>> memberAccess)
        #endregion
    }
}
