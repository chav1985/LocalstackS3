using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
            try
            {
                Console.WriteLine("Iniciado...");
                Console.WriteLine("Listando buckets...");
                ListBucketAsync().GetAwaiter().GetResult();

                Console.WriteLine("criando novo bucket...");
                CreateBucketAsync("teste").GetAwaiter().GetResult();

                PutObjectAsync("..\\..\\..\\..\\teste_0.txt", "teste").GetAwaiter().GetResult();
                PutObjectAsync("..\\..\\..\\..\\teste_1.txt", "teste").GetAwaiter().GetResult();
                PutObjectAsync("..\\..\\..\\..\\teste_2.txt", "teste").GetAwaiter().GetResult();
                PutObjectAsync("..\\..\\..\\..\\teste_3.txt", "teste").GetAwaiter().GetResult();
                PutObjectAsync("..\\..\\..\\..\\teste_4.txt", "teste").GetAwaiter().GetResult();

                var lstKeys = ListObjectsAsync("teste").GetAwaiter().GetResult();

                DeleteObjectsAsync("teste", lstKeys).GetAwaiter().GetResult();

                ListObjectsAsync("teste").GetAwaiter().GetResult();

                Console.WriteLine("Excluindo bucket recem criado...");
                DeleteBucketsAsync("teste").GetAwaiter().GetResult();

                Console.WriteLine("Listando buckets novamente...");
                ListBucketAsync().GetAwaiter().GetResult();

                Console.WriteLine("Finalizado...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao acessar bucket: {ex.Message}");
                throw;
            }
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

        private async Task PutObjectAsync(string path, string bucketName)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine("teste");
            sw.WriteLine("outro teste");
            sw.WriteLine("mais um teste");
            sw.Close();

            await _amazonS3.PutObjectAsync(new PutObjectRequest { BucketName = bucketName, FilePath = path });
        }

        private async Task<List<string>> ListObjectsAsync(string bucketName)
        {
            List<string> lstKeys = new List<string>();
            var response = await _amazonS3.ListObjectsAsync(bucketName);

            foreach (var item in response.S3Objects)
            {
                Console.WriteLine($"objeto: {item.Key}");
                lstKeys.Add(item.Key);
            }
            return lstKeys;
        }

        private async Task DeleteObjectsAsync(string bucketName, List<string> lstKeys)
        {
            foreach (var item in lstKeys)
            {
                await _amazonS3.DeleteObjectAsync(new DeleteObjectRequest { BucketName = bucketName , Key = item});
            }
        }
    }
}

