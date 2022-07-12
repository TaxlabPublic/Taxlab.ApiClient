using NodaTime;
using System;
using System.Net.Http;
using Taxlab.ApiClientCli.Implementations;
using Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers;
using Taxlab.ApiClientLibrary;
using Xunit;

namespace TaxLab.Test.ApiClientCli.Personas.TaxYearWorkpapers
{
    public class TaxpayerDetails
    {

        [Fact]
        public async void UpdateTaxpayerDetails()
        {
            string baseUrl = "https://localhost:44359/";
            HttpClient httpclient = new HttpClient();
            var authService = new AuthService();
            TaxlabApiClient client = new TaxlabApiClient(baseUrl, httpclient, authService);

            var repo = new TaxpayerDetailsRepository(client);
            var taxYear = 2021;
            var taxpayerId = new Guid("ff611f77-ebf9-438e-8933-15c600c5269b");


            var taxpayer = await repo.CreateAsync(taxpayerId, taxYear, new LocalDate());

        }
    }
}
