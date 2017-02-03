using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Microsoft.Extensions.Options;

namespace Fillager.Services
{
    public class MinioService : IMinioService
    {
        private const string MinioUrl = "http://minio:9000"; //= "http://192.168.1.20:9000/";
        private const string AccessKey = "6b4535c9d0545e036d5b";
        private const string SecretAccessKey = "f50a73124f5699570beb9ad44cd941";


        //uploadFile(string bucketName, stream file);
        //downloadFile(string bucketName);
        public async void UploadFile(string bucketName, Stream fileStream)
        {
            var client = GetClient();

            var listBucketsResponse = await client.ListBucketsAsync();
            var bucketExist = await AmazonS3Util.DoesS3BucketExistAsync(client, bucketName);


            if (! listBucketsResponse.Buckets.Any(bucket => bucket.BucketName.Equals(bucketName)))
            {
                await client.PutBucketAsync(bucketName);
            }
            var fileId = Guid.NewGuid().ToString();
            //var fileId = "test";

            var objectBucket = listBucketsResponse.Buckets.FirstOrDefault(bckt => bckt.BucketName.Equals(bucketName));
            if (objectBucket != null)
            {
                await GetClient().PutObjectAsync(new PutObjectRequest()
                {
                    BucketName = objectBucket.BucketName,
                    Key = fileId,
                    InputStream = fileStream
                });
            }
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
           
            var config = new AmazonS3Config()
            {
                RegionEndpoint = RegionEndpoint.EUWest1,
                SignatureVersion = "v4",
                ForcePathStyle = true,//required for minio
                ServiceURL = MinioUrl
            };

            var client = new AmazonS3Client(creds, config);
            return client;
        }

    }
}
