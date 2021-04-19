using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Taxlab.ApiClientLibrary.Interfaces;

namespace Taxlab.ApiClientLibrary
{
    public partial class TaxlabApiClient
    {
        private readonly IAuthService _authService;

        public TaxlabApiClient(string baseUrl, HttpClient httpClient, IAuthService authService)
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
        }
    }
}