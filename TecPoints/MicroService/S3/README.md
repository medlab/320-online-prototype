# S3 存储

Amazon S3，全名为亚马逊简易存储服务（Amazon Simple Storage Service），是亚马逊公司利用其亚马逊网络服务系统所提供的网络在线存储服务。经由Web服务界面，包括REST、SOAP与BitTorrent，用户能够轻易把文件存储到网络服务器上。

由于S3基于Web标准，所以有很多兼容实现，本文使用MinIO作为测试S3服务器

## 安装
使用 Docker Compose 部署简易测试环境

### 前置条件
本文假设机器已经安装了Docker和Docker Compose

### 安装方案： 使用docker-compose构建集群（此处预定义访问信息注入失败，需要手动登录9001配置)
```bash
#如需要，替换成自己的目录
mkdir minio_test
wget -O docker-compose.yaml 'https://github.com/minio/minio/blob/master/docs/orchestration/docker-compose/docker-compose.yaml?raw=true' 
wget -O nginx.conf 'https://github.com/minio/minio/blob/master/docs/orchestration/docker-compose/nginx.conf?raw=true'

# Tips
# 1. 为了使用AWS的SDK， 需要在docker-compose.yaml中设置MINIO_REGION环境变量， 参考 https://docs.min.io/docs/how-to-use-aws-sdk-for-net-with-minio-server.html
#   如，MINIO_REGION: "us-east-1"
# 2. 预定义 MINIO_ROOT_USER、MINIO_ROOT_PASSWORD也可以当作accessKey、secretKey使用
docker-compose pull

# 如果需要，清理旧实例， 会删除数据卷，小心， 确保你知道你在干什么
# docker-compose down -v

# 注意，终端这是不是服务模式，Ctrl C 后，服务会销毁。
# 如需要持续运行， 使用docker-compose up -d
docker-compose up

#运行后访问 http://127.0.0.1:9001/login ， 登录后就可以管理了
#密码可以打开docker-compose.yaml查看，写这个文档时是minio:minio123

#另外，预定义 MINIO_ROOT_USER、MINIO_ROOT_PASSWORD也可以当作accessKey、secretKey使用
```

## .Net S3 API 介绍

MinIO Client SDK for .NET

快速示意
```c#
using Minio;

// Initialize the client with access credentials.
private static MinioClient minio = new MinioClient("play.min.io",
                "Q3AM3UQ867SPQQA43P2F",
                "zuf+tfteSlswRu7BJ86wekitnifILbZam1KYY3TG"
                ).WithSSL();

// Create an async task for listing buckets.
var getListBucketsTask = minio.ListBucketsAsync();

// Iterate over the list of buckets.
foreach (Bucket bucket in getListBucketsTask.Result.Buckets)
{
    Console.WriteLine(bucket.Name + " " + bucket.CreationDateDateTime);
}

```

复杂示意
```C#
using System;
using Minio;
using Minio.Exceptions;
using Minio.DataModel;
using System.Threading.Tasks;

namespace FileUploader
{
    class FileUpload
    {
        static void Main(string[] args)
        {
            var endpoint  = "play.min.io";
            var accessKey = "Q3AM3UQ867SPQQA43P2F";
            var secretKey = "zuf+tfteSlswRu7BJ86wekitnifILbZam1KYY3TG";
            try
            {
                var minio = new MinioClient(endpoint, accessKey, secretKey).WithSSL();
                FileUpload.Run(minio).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        // File uploader task.
        private async static Task Run(MinioClient minio)
        {
            var bucketName = "mymusic";
            var location   = "us-east-1";
            var objectName = "golden-oldies.zip";
            var filePath = "C:\\Users\\username\\Downloads\\golden_oldies.mp3";
            var contentType = "application/zip";

            try
            {
                // Make a bucket on the server, if not already present.
                bool found = await minio.BucketExistsAsync(bucketName);
                if (!found)
                {
                    await minio.MakeBucketAsync(bucketName, location);
                }
                // Upload a file to bucket.
                await minio.PutObjectAsync(bucketName, objectName, filePath, contentType);
                Console.WriteLine("Successfully uploaded " + objectName );
            }
            catch (MinioException e)
            {
                Console.WriteLine("File Upload Error: {0}", e.Message);
            }
        }
    }
}
```

How to use AWS SDK for .NET with MinIO Server 
```C#
using Amazon.S3;
using System;
using System.Threading.Tasks;
using Amazon;

class Program
{
    private const string accessKey = "PLACE YOUR ACCESS KEY HERE";
    private const string secretKey = "PLACE YOUR SECRET KEY HERE"; // do not store secret key hardcoded in your production source code!

    static void Main(string[] args)
    {
        Task.Run(MainAsync).GetAwaiter().GetResult();
    }

    private static async Task MainAsync()
    {
        var config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.USEast1, // MUST set this before setting ServiceURL and it should match the `MINIO_REGION` environment variable.
            ServiceURL = "http://localhost:9000", // replace http://localhost:9000 with URL of your MinIO server
            ForcePathStyle = true // MUST be true to work correctly with MinIO server
        };
        var amazonS3Client = new AmazonS3Client(accessKey, secretKey, config);

        // uncomment the following line if you like to troubleshoot communication with S3 storage and implement private void OnAmazonS3Exception(object sender, Amazon.Runtime.ExceptionEventArgs e)
        // amazonS3Client.ExceptionEvent += OnAmazonS3Exception;

        var listBucketResponse = await amazonS3Client.ListBucketsAsync();

        foreach (var bucket in listBucketResponse.Buckets)
        {
            Console.Out.WriteLine("bucket '" + bucket.BucketName + "' created at " + bucket.CreationDate);
        }
        if (listBucketResponse.Buckets.Count > 0)
        {
            var bucketName = listBucketResponse.Buckets[0].BucketName;

            var listObjectsResponse = await amazonS3Client.ListObjectsAsync(bucketName);

            foreach (var obj in listObjectsResponse.S3Objects)
            {
                Console.Out.WriteLine("key = '" + obj.Key + "' | size = " + obj.Size + " | tags = '" + obj.ETag + "' | modified = " + obj.LastModified);
            }
        }
    }
}
```

## 完整示例程序
### Initial
    此程序基于标准的 Server Side Balazor 模板
```bash
    dotnet new  blazorserver -o Initial
```
### Finally(TODO)
    调整首页功能，可以新增文件和列出文件

### QuickStart
    A Full Quick Api Demo

## 参考：
0. [CEPH C# S3 EXAMPLES](https://docs.ceph.com/en/latest/radosgw/s3/csharp/)
1. [How to use AWS SDK for .NET with MinIO Server](https://docs.min.io/docs/how-to-use-aws-sdk-for-net-with-minio-server.html)
2. [Amazon S3 Wikipedia](https://zh.wikipedia.org/wiki/Amazon_S3)
3. [Amazon S3官网](https://aws.amazon.com/cn/s3/?nc1=h_ls)
4. [Amazon S3介绍视频](https://www.youtube.com/watch?v=_I14_sXHO8U&ab_channel=AmazonWebServices)
5. [MINIO](https://min.io/)
6. [MINIO安装 Docker Compose 模式](https://docs.min.io/docs/deploy-minio-on-docker-compose.html)
7. [MinIO Client SDK for .NET](https://docs.min.io/docs/dotnet-client-quickstart-guide.html)