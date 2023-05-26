using System.Text.Json.Serialization;

namespace ProjectAttendance.Host.Application.Users.Queries.Responses
{
    public class GetUserQueryResponse
    {
        [JsonPropertyName("user")]
        public GetUserQueryUserResponse User { get; set; }
    }

    public class GetUserQueryUserResponse 
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Username { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("login")]
        public string Login { get; set; }
    }

}
