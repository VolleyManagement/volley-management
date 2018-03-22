namespace VolleyManagement.Domain.UsersAggregate
{
    using Data.Contracts;

    /// <summary>
    /// Defines specific contract for UserRepository
    /// </summary>
    public interface IUserRepository : IGenericRepository<User>, IRepository
    {
    }
}
