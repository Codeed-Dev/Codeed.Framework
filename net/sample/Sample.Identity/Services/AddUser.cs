using AutoMapper;
using Codeed.Framework.Commons.Exceptions;
using Codeed.Framework.Identity.Domain;
using Codeed.Framework.Services;
using Microsoft.EntityFrameworkCore;
using Sample.Identity.Models;
using Sample.Identity.Repositories;
using System.Security.Claims;

namespace Sample.Identity.Services
{
    public class AddUser : Service
        .WithParameters<UserDto>
        .WithResponse<UserDto>
    {
        private readonly ClaimsPrincipal _principal;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AddUser(ClaimsPrincipal principal, IUserRepository userRepository, IMapper mapper)
        {
            _principal = principal;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public override async Task<UserDto> ExecuteAsync(UserDto userDto, CancellationToken cancellationToken)
        {
            var userUid = _principal.GetUserId();
            var existingUser = await _userRepository.FindByUid(userUid).FirstOrDefaultAsync(cancellationToken);
            if (existingUser != null)
                throw new ServiceException("User already exists");

            var name = userDto.Email.Split("@").First();
            var user = new User(userUid, userDto.Email, name);
            _userRepository.Add(user);
            await _userRepository.UnitOfWork.Commit(cancellationToken);
            return _mapper.Map<UserDto>(user);
        }
    }
}
