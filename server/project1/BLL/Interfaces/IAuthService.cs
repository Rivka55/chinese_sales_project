using project1.DTOs.Auth;

namespace project1.BLL.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDTO dto);
        Task<string> LoginAsync(LoginDTO dto);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> NameExistsAsync(string name);

        //void Register(RegisterDTO dto);
    }
}
