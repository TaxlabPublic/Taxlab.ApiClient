using System;
using System.Net.Http;

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
            TaxlabApiClient client = TestSetup.GetTaxlabApiClient();

            var repo = new TaxPaymentRepository(client);
            var taxYear = 2021;

            var taxpayerId = Guid.NewGuid();

            var taxPayment = await repo.CreateAsync(taxpayerId,
                taxYear,
                "Pay as you go - Withholding",
                20130, // PAYG ID
                true,
                new DateOnly(2021, 1, 1),
                -1000);
        }
    }
}