using System.Text.Json.Serialization;

namespace KeyEncryptDecrypt.Registrations
{
    public sealed class SignatureVerificationRequest
    {
        [JsonPropertyName("publicKeyAsBase64")]
        public string PublicKeyAsBase64 { get; set; }
    }
}
