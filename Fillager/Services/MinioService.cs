using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.Extensions.Configuration;

namespace Fillager.Services
{
    public class MinioService : IMinioService
    {
        private readonly string _minioUrl; //= "http://192.168.1.20:9000/";//todo env var from docker
        private readonly string _accessKey; //todo env var from docker
        private readonly string _secretAccessKey; //todo env var from docker

        public MinioService(IConfiguration configuration)
        {
            var hostname = configuration.GetValue<string>("MINIO_NAME");
            var port = configuration.GetValue<string>("MINIO_PORT");
            _minioUrl = $"http://{hostname}:{port}";
            _accessKey = configuration.GetValue<string>("MINIO_ACCESS_KEY");
            _secretAccessKey = configuration.GetValue<string>("MINIO_SECRET_KEY");
        }


        /// <summary>
        ///     Uploads a stream to minio
        /// </summary>
        /// <param name="bucketName">the bucket under which to save the datastream</param>
        /// <param name="dataStream">the object to save</param>
        /// <returns>the minio id on which the item can be retrieved</returns>
        public async Task<string> UploadFile(string bucketName, Stream dataStream)
        {
            var client = GetClient();

            var bucketExist = await AmazonS3Util.DoesS3BucketExistAsync(client, bucketName);


            if (!bucketExist)
                await client.PutBucketAsync(bucketName);
            var fileId = Guid.NewGuid().ToString();

            var putResult = await GetClient().PutObjectAsync(new PutObjectRequest
            {
                BucketName = bucketName,
                Key = fileId,
                InputStream = dataStream
            });

            var code = putResult.HttpStatusCode;

            if (code == HttpStatusCode.OK)
                return fileId;

            throw new AmazonS3Exception("Upload Error");
        }

        public Stream DownloadFile(string bucketName, string fileGuid)
        {
            var client = GetClient();
            var obj = client.GetObjectAsync(bucketName, fileGuid).Result;
            return obj.ResponseStream;
        }


        private AmazonS3Client GetClient()
        {
            AWSCredentials creds = new BasicAWSCredentials(_accessKey, _secretAccessKey);

            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.EUWest1,
                SignatureVersion = "v4",
                ForcePathStyle = true, //required for minio
                ServiceURL = _minioUrl
            };

            var client = new AmazonS3Client(creds, config);
            return client;
        }
    }
}