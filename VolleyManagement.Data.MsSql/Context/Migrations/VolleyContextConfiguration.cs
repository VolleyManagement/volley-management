namespace VolleyManagement.Data.MsSql.Context.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;

    using VolleyManagement.Data.MsSql.Entities;

    /// <summary>
    /// The volley context configuration.
    /// </summary>
    internal sealed class VolleyContextConfiguration : DbMigrationsConfiguration<VolleyManagementEntities>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VolleyContextConfiguration"/> class.
        /// </summary>
        public VolleyContextConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Context\Migrations";
            ContextKey = "VolleyManagement.Data.MsSql.Context.VolleyManagementEntities";
        }

        /// <summary>
        /// This method will be called after migrating to the latest version.
        /// </summary>
        /// <param name="context"> Volley Management context</param>
        protected override void Seed(VolleyManagementEntities context)
        {
            var defaultRoles = new List<RoleEntity>();

            defaultRoles.Add(CreateRole("Administrator"));
            defaultRoles.Add(CreateRole("TournamentAdministrator"));
            defaultRoles.Add(CreateRole("User"));

            context.Roles.AddOrUpdate(
                r => r.Name,
                defaultRoles.ToArray());

            var contributorTeams = new[]
            {
                new ContributorTeamEntity
                {
                    Name = "Project Management",
                    CourseDirection = "All",
                    Contributors = new List<ContributorEntity> 
                    {
                        new ContributorEntity { Name = "Sergii Diachenko" },
                        new ContributorEntity { Name = "Iurii Osipchuk" },
                        new ContributorEntity { Name = "Oleksii Konovalenko" },
                        new ContributorEntity { Name = "Pavlo Ignatenko" },
                        new ContributorEntity { Name = "Dmytro Petin" },
                        new ContributorEntity { Name = "Oleg Shvets" }
                     }
                },
                new ContributorTeamEntity
                {
                    Name = "Dp-042",
                    CourseDirection = ".NET",
                    Contributors = new List<ContributorEntity> 
                    {
                        new ContributorEntity { Name = "Igor Leta" },
                        new ContributorEntity { Name = "Dmytro Balayev"},
                        new ContributorEntity { Name = "Ielyzaveta Kalinchuk"},
                        new ContributorEntity { Name = "Oleg Petrushov" },
                        new ContributorEntity { Name = "Evgenij Kozhan" }
                    }
                },
                new ContributorTeamEntity
                {
                    Name = "Dp-052",
                    CourseDirection = ".NET",
                    Contributors = new List<ContributorEntity> 
                    {
                        new ContributorEntity { Name = "Julia Bukova" },
                        new ContributorEntity { Name = "Alexandr Marchotko" },
                        new ContributorEntity { Name = "Pavlo Dragoetskij" },
                        new ContributorEntity { Name = "Anastacia Necheporenko" },
                        new ContributorEntity { Name = "Egor Meshcheriakov" },
                        new ContributorEntity { Name = "Pavel Goncharenko" }
                    }
                },
                new ContributorTeamEntity
                {
                    Name = "Dp-061",
                    CourseDirection = ".NET",
                    Contributors = new List<ContributorEntity>
                    {
                        new ContributorEntity { Name = "Nikita Gordienko" },
                        new ContributorEntity { Name = "Marina Gaponova" },
                        new ContributorEntity { Name = "Sofia Shaposhnik" },
                        new ContributorEntity { Name = "Pavlo Pokhylenko"},
                        new ContributorEntity { Name = "Sergii Kuzmin" }
                    }
                },
                new ContributorTeamEntity
                {
                    Name = "Dp-064",
                    CourseDirection = ".NET",
                    Contributors = new List<ContributorEntity>
                    { 
                        new ContributorEntity { Name = "Andrii Zelyk" },
                        new ContributorEntity { Name = "Viktoriia Ryndina" },
                        new ContributorEntity { Name = "Alex Lapin" },
                        new ContributorEntity { Name = "Dmytro Chernyshov"},
                        new ContributorEntity { Name = "Maria Kochetkova" },
                        new ContributorEntity { Name = "Oleh Vovkodav" },
                        new ContributorEntity { Name = "Alexandr Maha" }
                    }
                },
                new ContributorTeamEntity
                {
                    Name = "Dp-064",
                    CourseDirection = "ATQC",
                    Contributors = new List<ContributorEntity> 
                    {
                        new ContributorEntity { Name = "Mykola Kolisnyk" },
                        new ContributorEntity { Name = "Maria Tsymbaliuk" },
                        new ContributorEntity { Name = "Ipatov Sergii" },
                        new ContributorEntity { Name = "Dmitriy Otrashevskiy" },
                        new ContributorEntity { Name = "Ruslan Borisenko" },
                        new ContributorEntity { Name = "Zavizion Stanislav" },
                        new ContributorEntity { Name = "Oleksandr Rudyi"},
                        new ContributorEntity { Name = "Sergey Bondarenko" }
                    }
                },
                new ContributorTeamEntity
                {
                    Name = "Dp-065",
                    CourseDirection = "UI",
                    Contributors = new List<ContributorEntity> 
                    {
                        new ContributorEntity { Name = "Denis Chernysh" },
                        new ContributorEntity { Name = "Oleh Man'kov" },
                        new ContributorEntity { Name = "Kateryna Nikolaieva" },
                        new ContributorEntity { Name = "Stanislav Makhnyts'kyi" },
                        new ContributorEntity { Name = "Yevhen Alf'orov"}
                    }
                },
                new ContributorTeamEntity
                {
                    Name = "Dp-070",
                    CourseDirection = "UI",
                    Contributors = new List<ContributorEntity>
                    {
                        new ContributorEntity { Name = "Dykhov Egor" },
                        new ContributorEntity { Name = "Shafarenko Ol'ha" },
                        new ContributorEntity { Name = "Ovcharenko Ekateryna" },
                        new ContributorEntity { Name = "Korostienko Daniil" }
                    }
                },
                new ContributorTeamEntity
                {
                    Name = "Dp-072",
                    CourseDirection = ".NET",
                    Contributors = new List<ContributorEntity>
                    {
                        new ContributorEntity { Name = "Denis Rudenko" },
                        new ContributorEntity { Name = "Mykyta Kurchenkov" },
                        new ContributorEntity { Name = "Elyzaveta Rudakova" },
                        new ContributorEntity { Name = "Mihail Zazharskiy" },
                        new ContributorEntity { Name = "Dmitriy Ryndin" },
                        new ContributorEntity { Name = "Dmitriy Shapoval" }
                    }
                },
                new ContributorTeamEntity
                {
                    Name = "Dp-076",
                    CourseDirection = "ATQC",
                    Contributors = new List<ContributorEntity>
                    {
                        new ContributorEntity { Name = "Alla Prykhodchenko" },
                        new ContributorEntity { Name = "Andriy Lantukh" },
                        new ContributorEntity { Name = "Oleksandr Zaitsev" },
                        new ContributorEntity { Name = "Artem Pychenko" },
                        new ContributorEntity { Name = "Dmytro Maslov" },
                        new ContributorEntity { Name = "Artem  Pozdeev" }
                    }
                },
                new ContributorTeamEntity
                {
                    Name = "Dp-085",
                    CourseDirection = ".NET",
                    Contributors = new List<ContributorEntity>
                    {
                        new ContributorEntity { Name = "Ievgen Oparyshev" },
                        new ContributorEntity { Name = "Mariia Shvets" },
                        new ContributorEntity { Name = "Eugene Gerbut" },
                        new ContributorEntity { Name = "Pavel Novichkhin" },
                        new ContributorEntity { Name = "Artem Kolisnyk" },
                        new ContributorEntity { Name = "Oleksii Khloptsev" }
                    }
                }
            };
            context.ContributorTeam.AddOrUpdate(s => s.Name, contributorTeams);
        }

        private static RoleEntity CreateRole(string name)
        {
            return new RoleEntity { Name = name };
        }
    }
}
