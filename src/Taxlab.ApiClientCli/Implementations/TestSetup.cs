using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Implementations
{
    public static class TestSetup
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        //choose api location
        private static readonly string BaseUrl = "https://localhost:44359/";
        //private static readonly string BaseUrl = "https://preview.taxlab.online/api-internal/";

        //add B2C token or jwtToken based on scenario
        private static readonly string Token = "add token here";

        public static TaxlabApiClient GetTaxlabApiClient()
        {
            var authService = new AuthService(Token);
            return new TaxlabApiClient(BaseUrl, HttpClient, authService);

        }
    }
}
