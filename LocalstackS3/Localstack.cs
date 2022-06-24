using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;

namespace LocalstackS3
{
    public class Localstack
    {
        private readonly IAmazonS3 _amazonS3;
        public Localstack(IAmazonS3 amazonS3)
        {
            _amazonS3 = amazonS3;
        }

        public void StartUp()
        {
            Console.WriteLine("Iniciado...");
            Console.WriteLine("Listando buckets...");
            ListBucketAsync().GetAwaiter().GetResult();
            Console.WriteLine("criando novo bucket...");
            CreateBucketAsync("teste").GetAwaiter().GetResult();
            Console.WriteLine("Excluindo bucket recem criado...");
            DeleteBucketsAsync("teste").GetAwaiter().GetResult();
            Console.WriteLine("Listando buckets novamente...");
            ListBucketAsync().GetAwaiter().GetResult();
            Console.WriteLine("Finalizado...");
            Console.ReadKey();
        }

        private async Task CreateBucketAsync(string bucketName)
        {
            await _amazonS3.PutBucketAsync(new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            });
        }

        private async Task ListBucketAsync()
        {
            var response = await _amazonS3.ListBucketsAsync();

            foreach (var bucket in response.Buckets)
            {
                Console.WriteLine($"Nome do bucket: {bucket.BucketName}");
            }
        }

        private async Task DeleteBucketsAsync(string bucketName)
        {
            await _amazonS3.DeleteBucketAsync(new DeleteBucketRequest { BucketName = bucketName });
        }
    }
}

