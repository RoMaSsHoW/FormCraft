namespace FormCraft.Application.Intefaces
{
    public interface ICurrentUserService
    {
        Guid? GetUserId();
        string? GetRole();
    }
}
