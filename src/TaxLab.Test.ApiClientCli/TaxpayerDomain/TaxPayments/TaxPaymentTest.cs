using System;
using System.Net.Http;
using NodaTime;
using Taxlab.ApiClientCli.Implementations;
using Taxlab.ApiClientCli.Repositories.Taxpayer;
using Taxlab.ApiClientLibrary;
using Xunit;

namespace TaxLab.Test.ApiClientCli.TaxpayerDomain.TaxPayments
{
    public class TaxPaymentTest
    {
        [Fact]
        public async void CreateTaxPayment()
        {
            string baseUrl = "https://preview.taxlab.online/api-internal/";
            HttpClient httpclient = new HttpClient();
            var authService = new AuthService();
            TaxlabApiClient client = new TaxlabApiClient(baseUrl, httpclient, authService);

            var repo = new TaxPaymentRepository(client);
            var taxYear = 2021;

            var taxpayerId = Guid.NewGuid();

            var taxPayment = await repo.CreateAsync(taxpayerId,
                taxYear,
                "PAYG Installment",
                5250, // PAYG ID
                true,
                new LocalDate(2021, 1, 1),
                -1000);
        }
    }
}