namespace Blog.Web.Factories;

using AutoMapper;
using Calabonga.Microservices.Core.Exceptions;
using Contracts.V1.Requests.CategoriesRequests;
using Data;
using Data.Models;
using Data.Specifications;
using Data.UnitOfWork;
using Base;

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
        new ()
        {
            Name = "Категорія за замовчуванням"
        };

    /// <inheritdoc cref="RequestFactory{T,TEntity,TCreateRequest,TUpdateRequest}"/>
    public override UpdateCategoryRequest GenerateForUpdate(int id)
    {
        var category = _unitOfWork.GetRepository<Category>().FirstOrDefault(new CategorySpecification(x => x.Id == id))
            ?? throw new MicroserviceArgumentNullException();


        var mapped = _mapper.Map<UpdateCategoryRequest>(category);

        return mapped;
    }
}