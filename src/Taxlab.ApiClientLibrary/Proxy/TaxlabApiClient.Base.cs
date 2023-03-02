using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Taxlab.ApiClientLibrary.Interfaces;

namespace Taxlab.ApiClientLibrary
{
    public partial class TaxlabApiClient
    {
        private readonly IAuthService _authService;
        public Guid TaxpayerId { get; set; }
        public int Taxyear { get; set; }

        public EntityType TaxpayerEntity { get; set; } = EntityType.IndividualAU;

        public TaxlabApiClient(string baseUrl,
            HttpClient httpClient,
            IAuthService authService)
        {
            _authService = authService;
            _baseUrl = baseUrl;
            _httpClient = httpClient;
            _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(CreateSerializerSettings);
        }

        partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
        {
            var bearerToken = _authService.GetBearerToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            request.Headers.Add("TaxpayerId", TaxpayerId.ToString());
            request.Headers.Add("TaxYear", Taxyear.ToString());
            request.Headers.Add("EntityType",((int)TaxpayerEntity).ToString());
        }
    }
}