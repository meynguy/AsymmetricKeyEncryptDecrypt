using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KeyEncryptDecrypt.Registrations
{
    public class RegistrationService
    {

        public string StorePublicKey(string publicKeyJsonAsBase64)
        {
            string plainText = "Hello World!#";
            string cipherText = string.Empty;
            try
            {


                var publicKeyJsonBytes = Convert.FromBase64String(publicKeyJsonAsBase64);
                var publicKeyJson = Encoding.Default.GetString(publicKeyJsonBytes);
                // Deserialize the JSON representation of publicKey into an instance of ECDsaCngPublicKey
                var publicKey = JsonSerializer.Deserialize<PublicKey>(publicKeyJson);

            }
            catch
            {
                // An exception occurred during the signature verification or the publicKey is not in the correct format
                // Return false to indicate that the signature is not valid

            }
            return cipherText;
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
    }

}
