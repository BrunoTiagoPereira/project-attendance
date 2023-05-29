using ProjectAttendance.Core.Validators;
using System.Text.Json.Serialization;

namespace ProjectAttendance.Host.Application.Projects.Commands.Requests
{
    public class CreateProjectCommandResponse : ICanBeValidated
    {
        [JsonPropertyName("project")]
        public CreateProjectProjectCommandResponse Project { get; set; }
    }

    public class CreateProjectProjectCommandResponse
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