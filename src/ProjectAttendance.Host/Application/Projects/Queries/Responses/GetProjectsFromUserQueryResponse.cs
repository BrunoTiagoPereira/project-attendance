using System.Text.Json.Serialization;

namespace ProjectAttendance.Host.Application.Projects.Queries.Responses
{
    public class GetProjectsFromUserQueryResponse
    {
        [JsonPropertyName("projects")]
        public List<GetProjectsFromUserProjectQueryResponse> Projects { get; set; }    
    }

    public class GetProjectsFromUserProjectQueryResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("users")]
        public List<GetProjectsFromUserUserQueryResponse> Users { get; set; }

        [JsonPropertyName("times")]
        public List<GetProjectsFromUserWorkTimeQueryResponse> WorkTimes { get; set; }
    }

    public class GetProjectsFromUserUserQueryResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

    }

    public class GetProjectsFromUserWorkTimeQueryResponse
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
