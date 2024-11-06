
namespace SyncfusionSemanticKernelBlazor.Helper
{
    public class HttpLogger : DelegatingHandler
    {
        public static HttpClient GetHttpClient(bool log = false)
        {
            return log
                ? new HttpClient(new Helper.HttpLogger(new HttpClientHandler()))
                : new HttpClient();
        }

        public HttpLogger(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Request:");
            Console.WriteLine(request.ToString());
            if (request.Content != null)
            {
                Console.WriteLine(await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));
            }

            Console.WriteLine();

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            Console.WriteLine("Response:");
            Console.WriteLine(response.ToString());
            if (response.Content != null)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));
            }

            Console.WriteLine();

            return response;
        }
    }
}
