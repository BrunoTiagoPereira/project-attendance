using System.Text.Json.Serialization;

namespace ProjectAttendance.Host.Application.Users.Commands.Requests
{
    public class AuthenticateCommandRequest
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
