using ProjectAttendance.Core.Validators;

namespace ProjectAttendance.Host.Application.Users.Queries.Requests
{
    public class GetUserQueryRequest : ICanBeValidated
    {
        public long UserId { get; set; }
    }
}