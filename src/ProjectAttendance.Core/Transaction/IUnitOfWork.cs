namespace ProjectAttendance.Core.Transaction
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}