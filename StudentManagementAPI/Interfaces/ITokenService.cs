using StudentManagementAPI.Models.DBModels;

namespace StudentManagementAPI.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}
