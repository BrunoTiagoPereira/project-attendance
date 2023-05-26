using Bogus;
using ProjectAttendance.Domain.Users.Entities;

namespace ProjectAttendance.UnitTests.Core.Fakers
{
    public class UserFaker : Faker<User>
    {
        public UserFaker()
        {
            CustomInstantiator((f) =>
            {
                return new User(f.Internet.UserName(), f.Internet.UserName(), f.Internet.Email(), f.Internet.Password());
            });
        }
    }
}