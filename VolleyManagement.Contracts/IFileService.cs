namespace VolleyManagement.Contracts
{
    using System.Web;

    /// <summary>
    /// Defines the contract for file service
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Is file exists on the server
        /// </summary>
        /// <param name="filePath">path of the essence</param>
        /// <returns>is file exists</returns>
        bool FileExists(string filePath);

        /// <summary>
        /// Upload file to the server
        /// </summary>
        /// <param name="file">file to upload</param>
        /// <param name="filePath">Path of file on server to upload</param>
        void Upload(HttpPostedFileBase file, string filePath);

        /// <summary>
        /// Delete file from the server
        /// </summary>
        /// <param name="filePath">Path of file on server to delete</param>
        void Delete(string filePath);
    }
}
