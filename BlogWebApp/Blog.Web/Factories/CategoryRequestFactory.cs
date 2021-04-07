using Blog.Data.Specifications;

namespace Blog.Web.Factories
{
    using AutoMapper;
    using Blog.Contracts.V1.Requests.CategoriesRequests;
    using Blog.Data;
    using Blog.Data.Models;
    using Calabonga.Microservices.Core.Exceptions;
    using Blog.Web.Factories.Base;
    using Blog.Data.UnitOfWork;

    /// <summary>
    /// Category request factory.
    /// </summary>
    /// <seealso cref="UpdateCategoryRequest" />
    public class CategoryRequestFactory : RequestFactory<int, Category, CreateCategoryRequest, UpdateCategoryRequest>
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
        /// Initializes a new instance of the <see cref="CategoryRequestFactory"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CategoryRequestFactory(
            IMapper mapper,
            IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc cref="RequestFactory{T,TEntity,TCreateRequest,TUpdateRequest}"/>
        public override CreateCategoryRequest GenerateForCreate() =>
            new()
            {
                Name = "Категорія за замовчуванням"
            };

        /// <inheritdoc cref="RequestFactory{T,TEntity,TCreateRequest,TUpdateRequest}"/>
        public override UpdateCategoryRequest GenerateForUpdate(int id)
        {
            var category = _unitOfWork.GetRepository<Category>().FirstOrDefault(new CategorySpecification(x => x.Id == id));
            if (category == null)
            {
                throw new MicroserviceArgumentNullException();
            }

            var mapped = _mapper.Map<UpdateCategoryRequest>(category);

            return mapped;
        }
    }
}