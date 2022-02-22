using System.Text.Json.Serialization;

namespace NewsPlus.Data.ViewModel
{
    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
