namespace VolleyManagement.Data.MsSql.Context.Interfaces
{
    using System.Data.Entity;
    
    interface IUserEntitiesConfigurator
    {
        void ConfigureRoles(DbModelBuilder modelBuilder);
        void ConfigureUsers(DbModelBuilder modelBuilder);
    }
}
