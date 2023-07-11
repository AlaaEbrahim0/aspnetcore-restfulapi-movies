namespace MoviesApi.Services.Contracts
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(RegisterDto dto);
        Task<AuthDto> GetTokenAsync(TokenRequestDto dto);
        Task<string> AddUserToRole(AddUserToRoleDto dto);

    }
}
