
using WCNApi.Infrastructure.Helpers;

namespace WCNApi.Infrastructure.Middlewares
{
    public class EncryptionMiddleware
    {
        private readonly RequestDelegate _next;
        public EncryptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string controllerName = Convert.ToString(context.GetRouteData().Values["controller"]);
            if (!controllerName.Equals("External", StringComparison.OrdinalIgnoreCase))
            {
                var originalBodyStream = context.Response.Body;
                using (var memoryStream = new MemoryStream())
                {
                    context.Response.Body = memoryStream;

                    await _next(context);

                    // Only encrypt if the response is successful
                    if (context.Response.StatusCode == 200)
                    {
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        var plainText = await new StreamReader(memoryStream).ReadToEndAsync();
                        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Guid.NewGuid().ToString();
                        string key = CryptoHelper.GenerateSecretKey(timestamp);
                        var encryptedData = CryptoHelper.EncryptResponse(plainText, key);
                        var encryptedResponse = new { EncryptedData = encryptedData };
                        context.Response.Headers.Add("X-Timestamp", CryptoHelper.EncryptData(timestamp));
                        context.Response.Headers.Remove("Content-Length");
                        context.Response.Body = originalBodyStream;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(encryptedResponse);
                    }
                    else
                    {
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        await memoryStream.CopyToAsync(originalBodyStream);
                    }
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
