namespace FormCraft.Application.Models.RequestModels
{
    public class LoginRequest
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
