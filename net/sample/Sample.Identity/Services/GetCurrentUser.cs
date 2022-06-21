using AutoMapper;
using Codeed.Framework.Services;
using Microsoft.EntityFrameworkCore;
using Sample.Identity.Repositories;
using System.Security.Claims;

namespace Sample.Identity.Services
{
    public class GetCurrentUser : Service
        .WithoutParameters
        .WithResponse<UserDto?>
    {
        private static IDictionary<string, UserDto> CACHE = new Dictionary<string, UserDto>();
        private readonly ClaimsPrincipal _principal;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetCurrentUser(ClaimsPrincipal principal, IUserRepository userRepository, IMapper mapper)
        {
            _principal = principal;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public override async Task<UserDto?> ExecuteAsync(CancellationToken cancellationToken)
        {
            var userUid = _principal.GetUserId();

            if (CACHE.TryGetValue(userUid, out UserDto userDto))
                return userDto;

            var user = await _userRepository.FindByUid(userUid).FirstOrDefaultAsync(cancellationToken);
            if (user == null)
                return null;

            userDto = _mapper.Map<UserDto>(user);
            CACHE.Add(userUid, userDto);
            return userDto;
        }
    }
}
