namespace VolleyManagement.Contracts
{
    using System.Web;

    /// <summary>
    /// Defines the contract for file service
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Upload file to the server
        /// </summary>
        /// <param name="fileId">id of the essence</param>
        /// <param name="file">file to upload</param>
        /// <param name="fileDir">Directory of file on server to upload</param>
        void Upload(int fileId, HttpPostedFileBase file, string fileDir);

        /// <summary>
        /// Delete file from the server
        /// </summary>
        /// <param name="fileId">id of the essence</param>
        /// <param name="fileDir">Directory of file on server to delete</param>
        void Delete(int fileId, string fileDir);
    }
}
