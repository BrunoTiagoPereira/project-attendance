using ProjectAttendance.Core.Validators;
using System.Text.Json.Serialization;

namespace ProjectAttendance.Host.Application.Users.Commands.Requests
{
    public class UpdateUserCommandRequest : ICanBeValidated
    {
        [JsonPropertyName("id")]
        public long UserId { get; set; }

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
