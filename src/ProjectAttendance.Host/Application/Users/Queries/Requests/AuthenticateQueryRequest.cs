using ProjectAttendance.Core.Validators;
using System.Text.Json.Serialization;

namespace ProjectAttendance.Host.Application.Users.Queries.Requests
{
    public class AuthenticateQueryRequest : ICanBeValidated
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
