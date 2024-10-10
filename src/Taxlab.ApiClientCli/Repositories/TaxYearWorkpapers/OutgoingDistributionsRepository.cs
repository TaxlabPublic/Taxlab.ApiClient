using System;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers;

public class OutgoingDistributionsRepository(TaxlabApiClient client) : RepositoryBase(client)
{
    public async Task<WorkpaperResponseOfOutgoingDistributionsWorkpaper> GetAsync(
        Guid taxpayerId,
        int taxYear)
    {
        var workpaperResponse = await GetTaxYearWorkpaperAsync(
            taxpayerId,
            taxYear,
            (taxpayer, year) => Client.Workpapers_GetOutgoingDistributionsWorkpaperAsync(taxpayer, year));

        return workpaperResponse;
    }

    public async Task<WorkpaperResponseOfOutgoingDistributionsWorkpaper> UpdateAndUpsertWorkpaper(
        OutgoingDistributionsWorkpaper workpaper,
        Guid linkedRecipientTaxpayerId)
    {
        workpaper.Distributions = [
            new OutgoingDistribution
            {
                Id = Guid.NewGuid(),
                HasRecipient = true,
                LinkedRecipientTaxpayerId = linkedRecipientTaxpayerId,
                EntityType = string.Empty,
                AssessmentCalculationCode = string.Empty,
                AustralianBusinessNumber = string.Empty,
                EntityTypeCode = "123",
                FirstName = string.Empty,
                LastName = string.Empty,
                Name = string.Empty,
                NonIndividualName = string.Empty,
                OtherGivenName = string.Empty,
                NameCurrencyCode = string.Empty,
                NameTypeCode = "071",
                NameUsageCode = "567"
            }
        ];

        var outgoingWorkpaperResponse = await Client.Workpapers_UpsertOutgoingDistributionsWorkpaperAsync(
        new UpsertOutgoingDistributionsWorkpaperCommand
        {
            Workpaper = workpaper
        });

        return outgoingWorkpaperResponse;
    }
}