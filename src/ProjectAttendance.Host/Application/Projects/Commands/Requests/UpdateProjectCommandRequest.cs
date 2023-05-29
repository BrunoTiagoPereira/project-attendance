using ProjectAttendance.Core.Validators;
using System.Text.Json.Serialization;

namespace ProjectAttendance.Host.Application.Projects.Commands.Requests
{
    public class UpdateProjectCommandRequest : ICanBeValidated
    {
        [JsonPropertyName("id")]
        public long ProjectId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("user_id")]
        public List<long> UsersIds { get; set; }
    }
}