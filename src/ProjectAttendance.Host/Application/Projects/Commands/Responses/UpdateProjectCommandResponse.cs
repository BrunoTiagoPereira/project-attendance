using System.Text.Json.Serialization;

namespace ProjectAttendance.Host.Application.Projects.Commands.Requests
{
    public class UpdateProjectCommandResponse
    {
        [JsonPropertyName("project")]
        public UpdateProjectProjectCommandResponse Project { get; set; }
    }

    public class UpdateProjectProjectCommandResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("users")]
        public List<UpdateProjectProjectUserCommandResponse> Users { get; set; }

        [JsonPropertyName("times")]
        public List<UpdateProjectProjectWorkTimeCommandResponse> WorkTimes { get; set; }
    }

    public class UpdateProjectProjectUserCommandResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }
    }

    public class UpdateProjectProjectWorkTimeCommandResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("startedAt")]
        public DateTime StartedAt { get; set; }

        [JsonPropertyName("endedAt")]
        public DateTime EndedAt { get; set; }
    }
}