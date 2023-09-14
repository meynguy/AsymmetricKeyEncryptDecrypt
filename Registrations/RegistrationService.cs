using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KeyEncryptDecrypt.Registrations
{
    public class RegistrationService
    {
        public byte[] StorePublicKey(string publicKeyJsonAsBase64)
        {
            string plainText = "Hello World!# Awesome";
            try
            {
                // Convert the base64-encoded public key back to JSON format
                string publicKeyJson = Encoding.UTF8.GetString(Convert.FromBase64String(publicKeyJsonAsBase64));
                // Save the public key as needed
                // For example, you can store it in a database or cache

                // Deserialize the JWK
                PublicKey publicKey = JsonSerializer.Deserialize<PublicKey>(publicKeyJson);

                if (publicKey.IsEncryptionAllowed)
                {
                    RSA rsa = RSA.Create();
                    // Extract key components
                    rsa.ImportParameters(new()
                    {
                        Modulus = publicKey.Modulus,
                        Exponent = publicKey.Exponent
                    });
                    byte[] messageBytes = Encoding.UTF8.GetBytes(plainText);
                    return rsa.Encrypt(messageBytes, publicKey.RSAEncryptionPadding);
                }
            }
            catch (Exception ex)
            {
                // Handle any errors and return an error response
            }
            return Array.Empty<byte>();
        }
    }


    public class PublicKey
    {
        [JsonPropertyName("alg")]
        public string Algorithm { get; set; }

        [JsonPropertyName("e")]
        public string E { get; set; }

        [JsonPropertyName("ext")]
        public bool Ext { get; set; }

        [JsonPropertyName("key_ops")]
        public string[] KeyOps { get; set; }

        [JsonPropertyName("kty")]
        public string Kty { get; set; }

        [JsonPropertyName("n")]
        public string N { get; set; }

        public byte[] Modulus => Convert.FromBase64String(ToBase64Standard(N));
        public byte[] Exponent => Convert.FromBase64String(ToBase64Standard(E));
        private static string ToBase64Standard(string base64) => base64.Replace('-', '+').Replace('_', '/').PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');

        public RSAEncryptionPadding RSAEncryptionPadding
        {
            get
            {
                return Algorithm switch
                {
                    "RSA-OAEP-256" => RSAEncryptionPadding.OaepSHA256,
                    _ => throw new NotSupportedException($"Unsupported algorithm: {Algorithm}"),// Handle other cases or throw an exception if necessary
                };
            }
        }

        public bool IsEncryptionAllowed => KeyOps.Contains("encrypt");
    }


}
