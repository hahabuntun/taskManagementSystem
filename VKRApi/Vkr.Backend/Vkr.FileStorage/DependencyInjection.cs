using Microsoft.Extensions.DependencyInjection;
using Minio;
using Vkr.Application.Interfaces.FileStorage;

namespace Vkr.FileStorage
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFileStorage(this IServiceCollection services, MinioOptions minioOptions) 
        {
            var minioClient = new MinioClient()
                .WithEndpoint(minioOptions.Endpoint)
                .WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey)
                .Build();


            services.AddSingleton(minioClient);
            services.AddScoped<IFileStorage, MinioService>();

            return services;
        }
    }
}
