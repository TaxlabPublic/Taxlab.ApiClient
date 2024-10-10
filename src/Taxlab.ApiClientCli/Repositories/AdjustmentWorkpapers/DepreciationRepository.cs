using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;
using Xunit;

namespace Taxlab.ApiClientCli.Repositories.AdjustmentWorkpapers
{
    public class DepreciationRepository : RepositoryBase
    {
        public DepreciationRepository(TaxlabApiClient client) : base(client)
        {

        }

        public async Task<WorkpaperResponseOfDepreciationWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string taxDepreciationNarration = "",
            int taxDepreciationReturnDisclosureTypeId = 0,
            decimal taxDepreciation = 0m,
            bool simplifiedDepreciationIndicator = false
        )
        {
            var workpaperResponse = await Client
                .Workpapers_GetDepreciationWorkpaperAsync(
                    taxpayerId,
                    taxYear, 
                    Guid.Empty,
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.TaxDepreciationNarration = taxDepreciationNarration;
            workpaper.TaxDepreciationReturnDisclosureTypeId = taxDepreciationReturnDisclosureTypeId;
            workpaper.TaxDepreciation = taxDepreciation;
            workpaper.SimplifiedDepreciationIndicator = simplifiedDepreciationIndicator;


            var command = new UpsertDepreciationWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertDepreciationWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
