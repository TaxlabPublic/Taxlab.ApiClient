using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class FirstHomeSuperSaverRepository : RepositoryBase
    {
        public FirstHomeSuperSaverRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfFirstHomeSuperSaverWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            decimal grossIncome = 0,
            decimal taxPaid = 0
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetFirstHomeSuperSaverWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    Guid.Empty,
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.GrossIncome = grossIncome.ToNumericCell();
            workpaper.TaxPaid = taxPaid.ToNumericCell();

            var command = new UpsertFirstHomeSuperSaverWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertFirstHomeSuperSaverWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
