using System.Text.Json;

namespace KeyEncryptDecrypt.Registrations
{
    public static class RegistrationEndpoints
    {
        public static async Task StorePublicKeyEndpoint(HttpContext context, RegistrationService registrationService)
        {
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            var requestData = JsonSerializer.Deserialize<SignatureVerificationRequest>(requestBody);

            // You'll need to implement the following `VerifySignature` method to verify the signature
            var encryptedToken = registrationService.StorePublicKey(requestData.PublicKeyAsBase64);

            var response = new { EncryptedMessage = Convert.ToBase64String(encryptedToken) };
            var jsonResponse = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
