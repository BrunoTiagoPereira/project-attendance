using ProjectAttendance.Core.Validators;

namespace ProjectAttendance.Host.Application.Projects.Queries.Requests
{
    public class GetProjectQueryRequest : ICanBeValidated
    {
        public long ProjectId { get; set; }
    }
}