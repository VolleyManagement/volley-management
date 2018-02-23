namespace VolleyManagement.Data.MsSql.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// DAL Division model
    /// </summary>
    public class DivisionEntity
    {
        private ICollection<GroupEntity> _groups;

        /// <summary>
        /// Initializes a new instance of the <see cref="DivisionEntity"/> class.
        /// </summary>
        public DivisionEntity()
        {
            _groups = new List<GroupEntity>();
        }

        /// <summary>
        /// Gets or sets division Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets division name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets division's tournament id
        /// </summary>
        public int TournamentId { get; set; }

        /// <summary>
        /// Gets or sets division's tournament
        /// </summary>
        public virtual TournamentEntity Tournament { get; set; }

        /// <summary>
        /// Gets or sets division's groups
        /// </summary>
        public virtual ICollection<GroupEntity> Groups
        {
            get => _groups;
            set => _groups = value;
        }
    }
}
