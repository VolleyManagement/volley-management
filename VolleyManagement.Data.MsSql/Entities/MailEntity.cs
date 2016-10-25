namespace VolleyManagement.Data.MsSql.Entities
{
    /// <summary>
    ///  DAL mail model
    /// </summary>
    public class MailEntity
    {
        /// <summary>
        /// Gets or sets mail id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets mail's receiver
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Gets or sets mail's sender
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets mail's body
        /// </summary>
        public string Body { get; set; }
    }
}