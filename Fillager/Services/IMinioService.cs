using System.IO;
using System.Threading.Tasks;

namespace Fillager.Services
{
    public interface IMinioService
    {
        /// <summary>
        ///     Uploads a stream to minio
        /// </summary>
        /// <param name="bucketName">the bucket under which to save the datastream</param>
        /// <param name="dataStream">the object to save</param>
        /// <returns>the minio id on which the item can be retrieved</returns>
        Task<string> UploadFile(string bucketName, Stream dataStream);

        Stream DownloadFile(string bucketName, string fileGuid);
    }
}