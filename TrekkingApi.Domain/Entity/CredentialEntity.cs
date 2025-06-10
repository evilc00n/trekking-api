
namespace TrekkingApi.Domain.Entity
{
    public class CredentialEntity
    {
        public int Id { get; set; }
        public string Hash { get; set; }
        public long UserId { get; set; }

        public UserEntity User { get; set; }
    }
}
