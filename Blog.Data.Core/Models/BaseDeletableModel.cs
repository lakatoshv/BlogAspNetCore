namespace Blog.Data.Core.Models
{
    using System;
    using Blog.Core;
    using Blog.Data.Core.Models.Interfaces;

    public abstract class BaseDeletableModel<TKey> : BaseModel<TKey>, IDeletableEntity, IEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
        private static bool IsTransient(BaseDeletableModel<TKey> obj)
        {
            return obj != null && Equals(obj.Id, default(int));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseDeletableModel<TKey>);
        }

        public virtual bool Equals(BaseDeletableModel<TKey> other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (IsTransient(this) || IsTransient(other) || !Equals(Id, other.Id))
                return false;

            var otherType = other.GetUnproxiedType();
            var thisType = GetUnproxiedType();

            return thisType.IsAssignableFrom(otherType) || otherType.IsAssignableFrom(thisType);
        }

        public override int GetHashCode()
        {
            return Equals(Id, default(int)) ? base.GetHashCode() : Id.GetHashCode();
        }

        public static bool operator ==(BaseDeletableModel<TKey> x, BaseDeletableModel<TKey> y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseDeletableModel<TKey> x, BaseDeletableModel<TKey> y)
        {
            return !(x == y);
        }
    }
}
