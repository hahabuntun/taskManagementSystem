using Minio.DataModel.Args;
using Minio;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Vkr.Application.Interfaces.FileStorage;

namespace Vkr.FileStorage
{
    public class MinioService(IMinioClient minioClient, IOptions<MinioOptions> options) : IFileStorage
    {
        private readonly MinioOptions _options = options.Value;

        public async Task<(string, long)> UploadFileAsync(IFormFile file, string fileName)
        {
            var bucketName = _options.BucketName;
            var uniqueGuid = Guid.NewGuid().ToString();
            var ext = Path.GetExtension(file.FileName);
            var newFileName = $"{uniqueGuid}_{fileName}{ext}";

            var bucketExists = await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
            if (!bucketExists)
            {
                await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
            }

            await using var stream = file.OpenReadStream();
            var contentType = file.ContentType;

            var response = await minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(newFileName)
                .WithStreamData(stream)
                .WithObjectSize(file.Length)
                .WithContentType(contentType));

            return (response.ObjectName, response.Size);
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var bucketName = _options.BucketName;

            var stream = new MemoryStream();
            await minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName)
                .WithCallbackStream(s => s.CopyTo(stream)));

            stream.Position = 0;
            return stream;
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var bucketName = _options.BucketName;

            try
            {
                await minioClient.StatObjectAsync(new StatObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName));

                await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName));

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

}
