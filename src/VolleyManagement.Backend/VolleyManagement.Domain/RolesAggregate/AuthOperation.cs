namespace VolleyManagement.Domain.RolesAggregate
{
    using System;

    /// <summary>
    /// Contains information about particular operation within application
    /// </summary>
    public sealed class AuthOperation : IEquatable<AuthOperation>
    {
        #region Constants

        private const byte BYTE_SIZE_SHIFT = 8;
        private readonly short _id;

        #endregion

        #region Constructor

        private AuthOperation(short id)
        {
            _id = id;
        }

        #endregion

        #region Operators overload

        /// <summary>
        /// Implementing of "implicit" operator from <see cref="short"/>
        /// </summary>
        /// <param name="id">Identifier of operation</param>
        /// <returns>instance of <see cref="AuthOperation"/> class</returns>
        public static implicit operator AuthOperation(short id)
        {
            return new AuthOperation(id);
        }

        /// <summary>
        /// Implementing of "implicit" operator from <see cref="AuthOperation"/> class
        /// </summary>
        /// <param name="operation">The operation</param>
        /// <returns>Identifier of the operation</returns>
        public static implicit operator short(AuthOperation operation)
        {
            return operation._id;
        }

        /// <summary>
        /// Implementing of "implicit" operator from tuple of <see cref="byte"/> parameters
        /// </summary>
        /// <param name="parameters">Parameters of the instance of <see cref="AuthOperation"/> class</param>
        /// <returns>instance of <see cref="AuthOperation"/> class</returns>
        public static implicit operator AuthOperation(Tuple<byte, int> parameters)
        {
            checked
            {
                return new AuthOperation(GetAuthOperationId(parameters.Item1, (byte)parameters.Item2));
            }
        }

        /// <summary>
        /// Implementing of "==" operator
        /// </summary>
        /// <param name="x">Left operation</param>
        /// <param name="y">Right operation</param>
        /// <returns>Flag if specified objects equals</returns>
        public static bool operator ==(AuthOperation x, AuthOperation y)
        {
            return ReferenceEquals(x, null)
                 ? ReferenceEquals(y, null)
                 : x.Equals(y);
        }

        /// <summary>
        /// Implementing of "!=" operator
        /// </summary>
        /// <param name="x">Left operation</param>
        /// <param name="y">Right operation</param>
        /// <returns>Flag if specified objects equals</returns>
        public static bool operator !=(AuthOperation x, AuthOperation y)
        {
            return !(x == y);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Implementing of "Equals" method
        /// </summary>
        /// <param name="obj">object to check equality</param>
        /// <returns>Flag if specified object equals to current</returns>
        public override bool Equals(object obj)
        {
            return !(obj is null) && obj is AuthOperation && Equals((AuthOperation)obj);
        }

        /// <summary>
        /// Implementing of "Equals" method
        /// </summary>
        /// <param name="other">>object to check equality</param>
        /// <returns>Flag if specified object equals to current</returns>
        public bool Equals(AuthOperation other)
        {
            return !(other is null) && _id == other._id;
        }

        /// <summary>
        /// Implementing of "GetHashCode" method
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        #endregion

        #region Private

        private static short GetAuthOperationId(byte areaId, byte operationId)
        {
            return (short)(areaId << BYTE_SIZE_SHIFT | operationId);
        }

        #endregion
    }
}
