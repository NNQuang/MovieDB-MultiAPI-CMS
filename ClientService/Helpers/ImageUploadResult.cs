namespace ClientService.Helpers
{
    public class ImageUploadResult
    {
        public bool Success { get; set; }
        public string FullName { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public string FolderName { get; set; }
        public long Size { get; set; }
    }
}