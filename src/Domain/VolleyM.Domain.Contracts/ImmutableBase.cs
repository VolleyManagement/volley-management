using System;
using System.Collections.Generic;

namespace VolleyM.Domain.Contracts
{
    public abstract class ImmutableBase<TUnderlyingType> : IEquatable<ImmutableBase<TUnderlyingType>>
    {
        private readonly TUnderlyingType _value;

        protected ImmutableBase(TUnderlyingType value)
        {
            _value = value;
        }

        public override string ToString() => _value.ToString();


        public bool Equals(ImmutableBase<TUnderlyingType> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return EqualityComparer<TUnderlyingType>.Default.Equals(_value, other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((ImmutableBase<TUnderlyingType>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TUnderlyingType>.Default.GetHashCode(_value);
        }

        public static bool operator ==(ImmutableBase<TUnderlyingType> left, ImmutableBase<TUnderlyingType> right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right))
                return false;

            if (!ReferenceEquals(null, left))
                return left.Equals(right);

            return false;
        }

        public static bool operator !=(ImmutableBase<TUnderlyingType> left, ImmutableBase<TUnderlyingType> right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right))
                return false;

            if (!ReferenceEquals(null, left))
                return !left.Equals(right);

            return true;
        }
    }
}