using AutoMapper;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.Domain.RepositoryContracts;
using Relaxinema.Core.DTO;
using Relaxinema.Core.ServiceContracts;
using Relaxinema.Core.Helpers.RepositoryParams;

namespace Relaxinema.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public RoleService(IRoleRepository roleRepository, IMapper mapper, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task AddToRoleAsync(Guid userId, string roleName)
        {
            var user = await _userRepository.GetByIdAsync(userId, new [] { nameof(User.Roles) });

            if(user == null) 
                throw new KeyNotFoundException("Doesn't contains user with such id");

            user.Roles ??= new List<Role>();

            var role = await _roleRepository.GetByNameAsync(roleName, new RoleParams { IncludeStrings = new []{ nameof(Role.Users) } });

            if (role == null)
            {
                role = new Role { Name = roleName, Users = new List<User>() };
                await CreateAsync(role);
            }  

            if (!await _roleRepository.AddToRoleAsync(user, role))
                throw new ArgumentException("This user already exits in these role");
        }

        public async Task CreateAsync(Role role)
        {
            await _roleRepository.CreateAsync(role);
        }

        public async Task DeleteAsync(Guid roleId)
        {
            if (!await _roleRepository.DeleteAsync(roleId))
                throw new KeyNotFoundException("Doesn't contains role with such Id");
        }

        public Task<IEnumerable<RoleResponse>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RoleResponse> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<RoleResponse> UpdateAsync(Role entity)
        {
            throw new NotImplementedException();
        }
    }
}
