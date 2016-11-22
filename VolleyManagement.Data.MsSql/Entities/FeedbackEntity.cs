namespace VolleyManagement.Data.MsSql.Entities
{
    using System;

    /// <summary>
    /// DAL feedback model
    /// </summary>
    public class FeedbackEntity
    {
        /// <summary>
        /// Gets or sets feedback Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets users email
        /// </summary>
        public string UsersEmail { get; set; }

        /// <summary>
        /// Gets or sets feedback content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets date of feedback
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets feedback status
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// Gets or sets user environment
        /// </summary>
        public string UserEnvironment { get; set; }

        /// <summary>
        /// Gets or sets update date of feedback
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets admin name
        /// </summary>
        public string AdminName { get; set; }
    }
}
