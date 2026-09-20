using System.Text.Json.Serialization;

namespace Application.Dto
{
    public class LoginPasswordDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;


    }
}
