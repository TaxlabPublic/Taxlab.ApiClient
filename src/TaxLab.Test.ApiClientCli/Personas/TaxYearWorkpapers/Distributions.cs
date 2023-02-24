using System;
using System.Net.Http;
using Taxlab.ApiClientCli.Implementations;
using Taxlab.ApiClientCli.Personas;
using Taxlab.ApiClientLibrary;
using Xunit;

namespace TaxLab.Test.ApiClientCli.Workpapers.TaxYearWorkpapers
{

    public class Distributions
    {
        [Fact]
        public async void CreateTaxpayerWithDistributionsWorkpapers()
        {
            TaxlabApiClient client = TestSetup.GetTaxlabApiClient();

            var repo = new IndividualWithTrustDistribution();
            var taxYear = 2021;

           var taxpayer = await repo.CreateAsync(client, "John", "IndividualWithTrustDistributions", "32989432", taxYear);

            Assert.Equal("John", taxpayer.TaxpayerName);
        }
    }
}
