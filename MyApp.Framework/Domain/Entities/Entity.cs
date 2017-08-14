using System.Diagnostics.CodeAnalysis;

namespace MyApp.Framework.Domain.Entities
{
    public abstract class Entity
    {
        #region Properties

        public long Id { get; set; }
        public byte[] RowVersion { get; set; }
        public EntityChangeState State { get; set; }

        #endregion

        #region Public Methods

        [SuppressMessage("ReSharper", "BaseObjectGetHashCodeCallInGetHashCode")]
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            if (IsTransient())
                return base.GetHashCode();

            unchecked
            {
                var hash = this.GetRealType().GetHashCode();
                return (hash * 31) ^ Id.GetHashCode();
            }
        }

        public virtual bool IsTransient()
        {
            return Id == 0;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Entity;
            if (ReferenceEquals(other, null)) return false;

            if (ReferenceEquals(this, other)) return true;

            var typeOfThis = this.GetRealType();
            var typeOfOther = other.GetRealType();

            if (typeOfThis != typeOfOther) return false;

            if (IsTransient() || other.IsTransient()) return false;

            return Id.Equals(other.Id);
        }

        public override string ToString()
        {
            return $"[{this.GetRealType().Name} : {Id}]";
        }

        #endregion

        #region Operators

        public static bool operator ==(Entity left, Entity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        #endregion
    }
}