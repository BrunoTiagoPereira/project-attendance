using Microsoft.EntityFrameworkCore;

namespace ProjectAttendance.Core.Transaction
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual async Task<bool> CommitAsync()
        {
            var hasChanges = await _context.SaveChangesAsync() > 0;

            return hasChanges;
        }
    }
}