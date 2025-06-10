

using System.Net;

namespace TrekkingApi.Domain.Entity
{
    public class UserEntity
    {
        
        public UserEntity()
        {

        }


        public UserEntity(long id, string login, string name, 
            string avatarUrl, string? description)
        {
            Id = id;
            Login = login;
            Name = name;
            AvatarUrl = avatarUrl;
            Description = description;
        }



        public long Id { get; set; }
        public string Login { get; set; }
        public string? Name { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<CollectionEntity> Collections { get; set; }
        public CredentialEntity Credential { get; set; }
        public ICollection<FollowEntity> Following { get; set; } // Подписки (где пользователь — follower)
        public ICollection<FollowEntity> Followers { get; set; } // Подписчики (где пользователь — following)
        public ICollection<PinEntity> Pins { get; set; }
        public UserSettingsEntity Settings { get; set; }
        public ICollection<PinViewEntity> Views { get; set; }




    }
}
