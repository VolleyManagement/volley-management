namespace VolleyManagement.Services
{
    using System;
    using System.IO;
    using System.Web;
    using VolleyManagement.Contracts;

    /// <summary>
    /// Represents an implementation of IFileService contract.
    /// </summary>
    public class FileService : IFileService
    {
        private const int MAX_FILE_SIZE = 1048576;

        /// <summary>
        /// Delete file from the server
        /// </summary>
        /// <param name="fileId">id of the essence</param>
        /// <param name="fileDir">Directory of file on server to delete</param>
        public void Delete(int fileId, string fileDir)
        {
            string fullPath = Path.Combine(HttpContext.Current.Request.MapPath(fileDir), fileId + ".jpg");
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            else
            {
                throw new FileNotFoundException("File not found");
            }
        }

        /// <summary>
        /// Upload file to the server
        /// </summary>
        /// <param name="fileId">id of the essence</param>
        /// <param name="file">file to upload</param>
        /// <param name="fileDir">Directory of file on server to upload</param>
        public void Upload(int fileId, HttpPostedFileBase file, string fileDir)
        {
            if (file != null && file.ContentLength > 0 && file.ContentLength < MAX_FILE_SIZE)
            {
                var path = Path.Combine(HttpContext.Current.Server.MapPath(fileDir), fileId + ".jpg");
                file.SaveAs(path);
            }
            else
            {
                throw new NullReferenceException("File size must be less then 1 MB and greater then 0 MB");
            }
        }
    }
}
