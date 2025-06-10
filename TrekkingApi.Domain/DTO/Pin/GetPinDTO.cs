

namespace TrekkingApi.Domain.DTO.Pin
{
    public class GetPinDTO
    {
        public GetPinMetaDTO Meta { get; set; }
    }

    public class GetPinMetaDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? CollectionId { get; set; }
        public string? Link { get; set; }
        public string[] Tags { get; set; }
        public string? Location { get; set; }
        public FileInfoDTO FileMeta { get; set; }
    }

    public class GetFileInfoDTO
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
