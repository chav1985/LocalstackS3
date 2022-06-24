using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;

namespace LocalstackS3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var amazonS3 = new AmazonS3Client(new BasicAWSCredentials("abc", "def"), new AmazonS3Config
            {
                ServiceURL = "http://localhost:4566",
                ForcePathStyle = true,
                UseHttp = true
            });

            var serviceProvider = new ServiceCollection()
                .AddSingleton<Localstack>()
                .AddScoped(typeof(IAmazonS3), provider => amazonS3)
                .BuildServiceProvider();

            var localstack = serviceProvider.GetService<Localstack>();
            localstack.StartUp();
        }
    }
}
