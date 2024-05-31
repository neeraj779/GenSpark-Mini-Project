using StudentManagementAPI.Models.DTOs;

namespace StudentManagementAPI.Interfaces
{
    public interface IUserService
    {
        public Task<LoginReturnDTO> Login(UserLoginDTO user);
        public Task<RegisteredUserDTO> Register(UserRegisterDTO user);
        public Task<IEnumerable<RegisteredUserDTO>> GetAllUsers();
        public Task<RegisteredUserDTO> ActivateUser(int id);
        public Task<RegisteredUserDTO> DeactivateUser(int id);

    }
}
