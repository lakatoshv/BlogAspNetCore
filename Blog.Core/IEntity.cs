namespace Blog.Core
{
    public interface IEntity
    {

    }

    public interface IEntityBase : IEntity
    {
    }


    public interface IEntity<T> : IEntity
    {
        T Id { get; set; }
    }

    public interface IEntityBase<T> : IEntityBase
    {
        T Id { get; set; }
    }
}
