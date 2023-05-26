using Bogus;
using Microsoft.EntityFrameworkCore;
using ProjectAttendance.Infra;

namespace ProjectAttendance.UnitTests.Core.Fakers
{
    public class DatabaseContextFaker : Faker<DatabaseContext>
    {
        public DatabaseContextFaker()
        {
            CustomInstantiator((f) =>
            {
                var builder = new DbContextOptionsBuilder<DatabaseContext>();
                builder.UseInMemoryDatabase(f.Random.Guid().ToString());

                return new DatabaseContext(builder.Options);
            });
        }
    }
}