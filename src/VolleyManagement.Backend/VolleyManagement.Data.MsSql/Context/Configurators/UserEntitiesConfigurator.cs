namespace VolleyManagement.Data.MsSql.Context.Configurators
{
    using System.Data.Entity;
    using VolleyManagement.Data.MsSql.Entities;

    class UserEntitiesConfigurator : Interfaces.IUserEntitiesConfigurator
    {
        public void ConfigureRoles(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleEntity>()
                .ToTable(VolleyDatabaseMetadata.ROLES_TABLE_NAME)
                .HasKey(r => r.Id);

            modelBuilder.Entity<RoleEntity>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(ValidationConstants.Role.MAX_NAME_LENGTH)
                .IsUnicode()
                .IsVariableLength();
        }

        public void ConfigureUsers(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .ToTable(VolleyDatabaseMetadata.USERS_TABLE_NAME)
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserEntity>()
                .Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(ValidationConstants.User.MAX_USER_NAME_LENGTH)
                .IsUnicode()
                .IsVariableLength();

            modelBuilder.Entity<UserEntity>()
                .Property(u => u.Email)
                .IsOptional()
                .HasMaxLength(ValidationConstants.User.MAX_EMAIL_LENGTH)
                .IsUnicode()
                .IsVariableLength();

            modelBuilder.Entity<UserEntity>()
                .Property(u => u.FullName)
                .IsOptional()
                .HasMaxLength(ValidationConstants.User.MAX_FULL_NAME_LENGTH)
                .IsUnicode()
                .IsVariableLength();

            modelBuilder.Entity<UserEntity>()
                .Property(u => u.CellPhone)
                .IsOptional()
                .HasMaxLength(ValidationConstants.User.MAX_PHONE_LENGTH)
                .IsUnicode()
                .IsVariableLength();

            modelBuilder.Entity<UserEntity>()
                .HasMany(u => u.LoginProviders)
                .WithRequired(l => l.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserEntity>()
                .Property(u => u.IsBlocked);
        }
    }
}
