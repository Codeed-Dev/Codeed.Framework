using AutoMapper;
using Codeed.Framework.Identity.Domain;
using Sample.Identity.Services;

namespace Sample.Identity.Maps
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
