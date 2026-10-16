using Azure.Storage.Blobs;

namespace MoviesAdmin.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string containerName;

        public AzureStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
            containerName = _configuration["AzureStorage:ContainerName"] ?? string.Empty;
            string connectionString = _configuration["AzureStorage:ConnectionString"] ?? string.Empty;
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string filename)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(filename);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream);
            }
            return blobClient.Uri.ToString();
        }
    }
}
