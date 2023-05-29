using System.Text.Json.Serialization;

namespace ProjectAttendance.Host.Application.Projects.Queries.Responses
{
    public class GetProjectQueryResponse
    {
        [JsonPropertyName("project")]
        public GetProjectProjectQueryResponse Project { get; set; }
    }

    public class GetProjectProjectQueryResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("users")]
        public List<GetProjectProjectUserQueryResponse> Users { get; set; }

        [JsonPropertyName("times")]
        public List<GetProjectProjectWorkTimeQueryResponse> WorkTimes { get; set; }
    }

    public class GetProjectProjectUserQueryResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

    }

    public class GetProjectProjectWorkTimeQueryResponse
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
