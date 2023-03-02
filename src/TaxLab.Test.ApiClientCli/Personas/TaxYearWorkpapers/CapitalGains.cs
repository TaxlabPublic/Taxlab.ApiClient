using System;
using System.Net.Http;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Implementations;
using Taxlab.ApiClientCli.Personas;
using Taxlab.ApiClientLibrary;
using Xunit;

namespace TaxLab.Test.ApiClientCli.Workpapers.TaxYearWorkpapers
{

    public class CapitalGains
    {
        [Fact]
        public async void CreateTaxpayerWithCapitalGainsWorkpapers()
        {
            TaxlabApiClient client = TestSetup.GetTaxlabApiClient();

            var repo = new IndividualWithPropertyCapitalGain();
            var taxYear = 2021;
            var taxpayer = await repo.CreateAsync(client,
                "John",
                "IndividualWithPropertyCapitalGain",
                "32989432",
                taxYear)
                .ConfigureAwait(false);

            //allow for calculation to be run
            await Task.Delay(10000);
            
            // get the capital gains workpaper and check the contents
            var capitalGainsWorkpaper = await client
                .Workpapers_GetCapitalGainsWorkpaperAsync(taxpayer.Id, taxYear)
                .ConfigureAwait(false);

            Assert.Equal(203896.52m, capitalGainsWorkpaper.Workpaper.CurrentYearGains);
        }
    }
}
