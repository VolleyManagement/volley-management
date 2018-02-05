namespace VolleyManagement.Services
{
    using System;
    using System.IO;
    using System.Web;
    using Contracts;

    /// <summary>
    /// Represents an implementation of IFileService contract.
    /// </summary>
    public class FileService : IFileService
    {
        private const int MAX_FILE_SIZE = 1048576;

        /// <summary>
        /// Is file exists on the server
        /// </summary>
        /// <param name="filePath">path of the essence</param>
        /// <returns>is file exists</returns>
        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// Delete file from the server
        /// </summary>
        /// <param name="filePath">Path of file on server to delete</param>
        public void Delete(string filePath)
        {
            if (FileExists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                throw new FileNotFoundException("File not found");
            }
        }

        /// <summary>
        /// Upload file to the server
        /// </summary>
        /// <param name="file">file to upload</param>
        /// <param name="filePath">Path of file on server to upload</param>
        public void Upload(HttpPostedFileBase file, string filePath)
        {
            if (file != null && file.ContentLength > 0 && file.ContentLength < MAX_FILE_SIZE)
            {
                file.SaveAs(filePath);
            }
            else
            {
                throw new ArgumentException("File size must be less then 1 MB and greater then 0 MB");
            }
        }
    }
}
