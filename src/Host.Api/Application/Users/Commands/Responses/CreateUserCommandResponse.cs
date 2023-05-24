using System.Text.Json.Serialization;

namespace Host.Api.Application.Users.Commands.Responses
{
    public class CreateUserCommandResponse
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
