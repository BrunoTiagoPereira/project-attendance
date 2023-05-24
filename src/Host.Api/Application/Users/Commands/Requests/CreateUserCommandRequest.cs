using Host.Api.Core.Validators;
using System.Text.Json.Serialization;

namespace Host.Api.Application.Users.Commands.Requests
{
    public class CreateUserCommandRequest : ICanBeValidated
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
