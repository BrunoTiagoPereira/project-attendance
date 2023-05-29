using ProjectAttendance.Core.Validators;
using System.Text.Json.Serialization;

namespace ProjectAttendance.Host.Application.Projects.Commands.Requests
{
    public class CreateProjectCommandRequest : ICanBeValidated
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("user_id")]
        public List<long> UsersIds { get; set; }
    }
}