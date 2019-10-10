using System;
using System.Collections.Generic;

namespace VolleyM.Domain.Contracts
{
    public abstract class IdBase<TUnderlyingType>:IEquatable<IdBase<TUnderlyingType>>
    {
        private readonly TUnderlyingType _value;

        protected IdBase(TUnderlyingType value)
        {
            _value = value;
        }

        public override string ToString() => _value.ToString();


        public bool Equals(IdBase<TUnderlyingType> other)
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

            return Equals((IdBase<TUnderlyingType>) obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TUnderlyingType>.Default.GetHashCode(_value);
        }
    }
}