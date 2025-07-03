namespace FormCraft.Application.Common.Persistance
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync(CancellationToken cancellationToken = default);
    }
}
