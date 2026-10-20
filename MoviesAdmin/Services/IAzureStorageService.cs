namespace MoviesAdmin.Services
{
    public interface IAzureStorageService
    {
        // Service to upload file to azure blob storage 
        public Task<String> UploadFileAsync(IFormFile file, string fileName);

        // TO-DO Create function to delete files after a movie record was deletedS
    }
}
