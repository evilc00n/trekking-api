
namespace TrekkingApi.Domain.DTO.Pin
{
    public class PinRequestDTO
    {
        public MetaDTO Meta { get; set; }
        public byte[] File { get; set; }
    }

    public class MetaDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? CollectionId { get; set; }
        public string? Link { get; set; }
        public string[] Tags { get; set; }
        public string? Location { get; set; }
        public FileInfoDTO FileMeta { get; set; }
    }

    public class FileInfoDTO
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string RawFilename { get; set; }
        public string Type { get; set; }
    }
}
