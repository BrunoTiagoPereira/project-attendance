namespace Host.Api.Core.Transaction
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}