namespace MoviesApi.Services.Contracts
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(RegisterDto dto);
        Task<AuthDto> GetTokenAsync(TokenRequestDto dto);
        Task<string> AddUserToRoleAsync(AddUserToRoleDto dto);
        Task<string> AddRoleAsync(string roleName);
        Task SignOutAsync(string userId);

    }
}
