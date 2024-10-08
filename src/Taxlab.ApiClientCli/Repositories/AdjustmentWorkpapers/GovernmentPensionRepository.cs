using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class GovernmentPensionRepository : RepositoryBase
    {
        public GovernmentPensionRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfGovernmentPensionWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string description = "",
            decimal grossIncome = 0,
            decimal taxPaid = 0
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetGovernmentPensionWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.Description = description;
            workpaper.GrossIncome = grossIncome.ToNumericCell();
            workpaper.TaxPaid = taxPaid.ToNumericCell();

            var command = new UpsertGovernmentPensionWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertGovernmentPensionWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
