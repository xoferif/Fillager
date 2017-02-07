using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;

namespace Fillager.Services
{
    public class MinioService : IMinioService
    {
        private const string MinioUrl = "http://minio:9000"; //= "http://192.168.1.20:9000/";//todo env var from docker
        private const string AccessKey = "6b4535c9d0545e036d5b"; //todo env var from docker
        private const string SecretAccessKey = "f50a73124f5699570beb9ad44cd941";//todo env var from docker

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


        private static AmazonS3Client GetClient()
        {
            AWSCredentials creds = new BasicAWSCredentials(AccessKey, SecretAccessKey);

            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.EUWest1,
                SignatureVersion = "v4",
                ForcePathStyle = true, //required for minio
                ServiceURL = MinioUrl
            };

            var client = new AmazonS3Client(creds, config);
            return client;
        }
    }
}