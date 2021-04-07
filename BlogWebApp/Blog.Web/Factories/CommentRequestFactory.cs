using Blog.Data.Specifications;

namespace Blog.Web.Factories
{
    using AutoMapper;
    using Blog.Contracts.V1.Requests.CommentsRequests;
    using Blog.Data;
    using Blog.Data.Models;
    using Calabonga.Microservices.Core.Exceptions;
    using Blog.Web.Factories.Base;
    using Blog.Data.UnitOfWork;

    /// <summary>
    /// Comment request factory.
    /// </summary>
    /// <seealso cref="UpdateCommentRequest" />
    public class CommentRequestFactory : RequestFactory<int, Comment, CreateCommentRequest, UpdateCommentRequest>
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentRequestFactory"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CommentRequestFactory(
            IMapper mapper,
            IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc cref="RequestFactory{T,TEntity,TCreateRequest,TUpdateRequest}"/>
        public override CreateCommentRequest GenerateForCreate()
        {
            var post = _unitOfWork.GetRepository<Post>().FirstOrDefault(null);

            return new CreateCommentRequest()
            {
                PostId = post.Id,
                CommentBody = "Коментар",
                UserId = post.AuthorId,
            };
        }

        /// <inheritdoc cref="RequestFactory{T,TEntity,TCreateRequest,TUpdateRequest}"/>
        public override UpdateCommentRequest GenerateForUpdate(int id)
        {
            var category = _unitOfWork.GetRepository<Comment>().FirstOrDefault(new CommentSpecification(x => x.Id == id));
            if (category == null)
            {
                throw new MicroserviceArgumentNullException();
            }

            var mapped = _mapper.Map<UpdateCommentRequest>(category);

            return mapped;
        }
    }
}