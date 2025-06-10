

using AutoMapper;
using TrekkingApi.Domain.DTO.User;
using TrekkingApi.Domain.Entity;

namespace TrekkingApi.Application.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<UserEntity, UserDTO>()
                .ForCtorParam(ctorParamName: "Id", m => m.MapFrom(s => s.Id))
                .ForCtorParam(ctorParamName: "Login", m => m.MapFrom(s => s.Login))
                .ForCtorParam(ctorParamName: "Name", m => m.MapFrom(s => s.Name))
                .ForCtorParam(ctorParamName: "AvatarUrl", m => m.MapFrom(s => s.AvatarUrl))
                .ForCtorParam(ctorParamName: "Description", m => m.MapFrom(s => s.Description));




            CreateMap<UserDTO, UserEntity>()
                .ForCtorParam(ctorParamName: "Id", m => m.MapFrom(s => s.Id))
                .ForCtorParam(ctorParamName: "Login", m => m.MapFrom(s => s.Login))
                .ForCtorParam(ctorParamName: "Name", m => m.MapFrom(s => s.Name))
                .ForCtorParam(ctorParamName: "AvatarUrl", m => m.MapFrom(s => s.AvatarUrl))
                .ForCtorParam(ctorParamName: "Description", m => m.MapFrom(s => s.Description));

        }
    }
}
