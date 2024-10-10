using System;
using System.Linq;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Implementations;
using Taxlab.ApiClientCli.Repositories.Taxpayer;
using Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers;
using Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers;
using Taxlab.ApiClientLibrary;
using Xunit;
using Xunit.Abstractions;

namespace Taxlab.ApiClientCli.Personas;

public class DistributionsTest(ITestOutputHelper output) : BaseScopedTests(output)
{
    private TaxlabApiClient _client;

    protected override async Task SetupTest()
    {
        _client = TestSetup.GetTaxlabApiClient();
    }

    private async Task<TaxpayerDto> CreateTaxpayer(
        int taxYear,
        string firstName, 
        string lastName,
        string taxFileNumber,
        EntityType entityType)
    {
        var taxpayerService = new TaxpayerRepository(_client);

        var taxpayerResponse = await taxpayerService.CreateAsync(
            taxYear,
            firstName,
            lastName,
            taxFileNumber,
            entityType);

        var balanceDate = new DateOnly(taxYear, 6, 30);

        var taxpayer = taxpayerResponse.Content;
        _client.TaxpayerId = taxpayer.Id;
        _client.Taxyear = taxYear;
        _client.TaxpayerEntity = entityType;

        var taxReturnRepository = new TaxReturnRepository(_client);
        var taxReturnResponse = await taxReturnRepository.CreateAsync(
            taxpayer.Id,
            taxYear,
            balanceDate,
            balanceDate.AddYears(-1).AddDays(1));


        if (taxReturnResponse.Success == false)
        {
            throw new Exception(taxReturnResponse.Message);
        }

        return taxpayerResponse.Content;
    }

    [Fact]
    public async Task OutgoingAndIncomingDistributionsTest()
    {
        var taxYear = 2024;
        var providerType = EntityType.TrustAU;
        var recipientType = EntityType.IndividualAU;

        var recipientTaxpayer = await CreateTaxpayer(taxYear, "Individual", "Zeyb", "32989432", recipientType);
        var providerTaxpayer = await CreateTaxpayer(taxYear, "Notindividual", "ZeyCompany", "32989432", providerType);
        
        var outgoingRepository = new OutgoingDistributionsRepository(_client);
        var outgoingResponse = await outgoingRepository.GetAsync(providerTaxpayer.Id, taxYear);
        await outgoingRepository.UpdateAndUpsertWorkpaper(outgoingResponse.Workpaper, recipientTaxpayer.Id);

        var outgoingDistributions = outgoingResponse.Workpaper.Distributions.ToList();
        Assert.Equal(outgoingDistributions.Count, 1);

        // Wait for recipient taxpayer to calculate
        await Task.Delay(10_000);

        _client.TaxpayerId = recipientTaxpayer.Id;
        _client.TaxpayerEntity = recipientTaxpayer.EntityType;

        var incomingRepository = new IncomingDistributionsRepository(_client);
        var incomingResponse = await incomingRepository.GetAsync(recipientTaxpayer.Id, taxYear);

        var incomingDistributions = incomingResponse.Workpaper.Distributions.ToList();
        Assert.Equal(incomingDistributions.Count, 1);
        Assert.Equal(incomingDistributions[0].Id, outgoingDistributions[0].Id);
        Assert.Equal(incomingDistributions[0].LinkedProviderTaxpayerId, providerTaxpayer.Id);
    }
}