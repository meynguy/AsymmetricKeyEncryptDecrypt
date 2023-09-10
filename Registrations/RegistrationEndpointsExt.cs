namespace KeyEncryptDecrypt.Registrations
{
    public static class RegistrationEndpointsExt
    {
        public static void MapRegistrationEndpoints(this WebApplication app)
        {
            // Separate your endpoints into different classes or functions for better organization
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost("/storePublicKey", RegistrationEndpoints.StorePublicKeyEndpoint);
            });
        }
    }
}
