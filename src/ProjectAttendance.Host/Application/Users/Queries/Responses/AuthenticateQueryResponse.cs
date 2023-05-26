using System.Text.Json.Serialization;

namespace ProjectAttendance.Host.Application.Users.Queries.Responses
{
    public class AuthenticateQueryResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("user")]
        public AuthenticationUserResponse User { get; set; }
    }

    public class AuthenticationUserResponse
    {
        [JsonPropertyName("name")]
        public string Username { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
