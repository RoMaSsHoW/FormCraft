namespace FormCraft.Application.Models.DTO
{
    public class JWTSettings
    {
        public string Key { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int ExpireTime { get; init; }
    }
}
