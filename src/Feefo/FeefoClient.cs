using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Feefo.Requests;
using Feefo.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Feefo
{
    public class FeefoClient : IFeefoClient, IDisposable
    {
        private readonly HttpMessageHandler _handler;
        private readonly IQueryStringFactory _queryStringFactory;
        private readonly IFeefoSettings _feefoSettings;

        public FeefoClient(HttpMessageHandler handler, IQueryStringFactory queryStringFactory, IFeefoSettings feefoSettings)
        {
            _handler = handler;
            _queryStringFactory = queryStringFactory;
            _feefoSettings = feefoSettings;
        }

        public FeefoClient(IFeefoSettings feefoSettings)
            : this(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            }, new QueryStringFactory(), feefoSettings)
        {
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = HttpClientFactory.Create(_handler);

            httpClient.BaseAddress = _feefoSettings.BaseUri;

            return httpClient;
        }

        public Task<FeefoClientResponse> GetFeedbackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetFeedbackAsync(new FeedbackRequest(), cancellationToken);
        }

        public async Task<FeefoClientResponse> GetFeedbackAsync(FeedbackRequest feedbackRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var httpClient = CreateHttpClient();
            var queryString = _queryStringFactory.Create(_feefoSettings, feedbackRequest);

            var response = await httpClient.GetAsync(queryString, cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };


            var content = await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            //content = Convert.ToBase64String(Encoding.UTF8.GetBytes(content));

            var content2 = JsonConvert.DeserializeObject<Rootobject>(content, jsonSettings);
            
            return new FeefoClientResponse(content2?.FeedbackList);
        }

        public void Dispose()
        {
            _handler.Dispose();
        }
    }
}
