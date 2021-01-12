namespace AuthService.Data.Repository
{
    using System.Linq;
    using System.Threading.Tasks;
    using AuthService.Core;
    using AuthService.Data.Core.Models.Interfaces;

    public interface IDeletableEntityRepository<TEntity> : IRepository<TEntity>
        where TEntity : IEntity, IDeletableEntity
    {
        IQueryable<TEntity> AllWithDeleted();

        IQueryable<TEntity> AllAsNoTrackingWithDeleted();

        Task<TEntity> GetByIdWithDeletedAsync(params object[] id);

        void HardDelete(TEntity entity);

        void Undelete(TEntity entity);
    }
}
