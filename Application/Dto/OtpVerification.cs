using System.Text.Json.Serialization;

namespace Application.Dto
{
    public class OtpVerificationDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("otpCode")]
        public string OtpCode { get; set; } = string.Empty;
    }
}