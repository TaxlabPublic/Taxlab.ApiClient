using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class PersonalSuperannuationContributionRepository : RepositoryBase
    {

        public PersonalSuperannuationContributionRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfPersonalSuperannuationContributionWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string organisationName = "",
            string accountNumber = "",
            string fundABN = "",
            string fundTFN = "",
            DateOnly lastEligibleDate = default,
            decimal contribution = 0m,
            bool didYouReceiveAnAcknowledgement = false
        )
        {
            var workpaperResponse = await Client
                .Workpapers_GetPersonalSuperannuationContributionWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    Guid.Empty,
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.OrganisationName = organisationName;
            workpaper.PersonalSuperannuationContribution = contribution.ToNumericCell();
            workpaper.PersonalSuperannuationAccountNumber = accountNumber;
            workpaper.FundABN = fundABN;
            workpaper.FundTFN = fundTFN;
            workpaper.LastEligibleDate = new DateTime(lastEligibleDate.Year, lastEligibleDate.Month, lastEligibleDate.Day);
            workpaper.ReceiveAnAcknowledgement = didYouReceiveAnAcknowledgement;

            var command = new UpsertPersonalSuperannuationContributionWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertPersonalSuperannuationContributionWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
