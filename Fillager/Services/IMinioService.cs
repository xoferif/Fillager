using System;
using System.IO;

namespace Fillager.Services
{
    public interface IMinioService
    {
        void UploadFile(string bucketName, Stream fileStream);
        Stream DownloadFile(string bucketName, string fileGuid);
    }
}