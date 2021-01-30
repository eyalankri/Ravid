using Ravid.Dto;
using Ravid.Dtos;
using Ravid.Models;

namespace Ravid
{
    public class ApplicationProfile : AutoMapper.Profile
    {
        public ApplicationProfile()
        {
            CreateMap<User, UserDto>()
                .ReverseMap();

            CreateMap<TrasportRequest, TrasportRequestDto>()
                .ReverseMap();
        }
    }
}
