using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.AdjustmentWorkpapers
{
    public class DepreciatingAssetsFirstDeductedRepository : RepositoryBase
    {
        public DepreciatingAssetsFirstDeductedRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfDepreciatingAssetsFirstDeductedWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            decimal accountingAdditions = 0m,
            decimal taxAdditions = 0m,
            CapitalAssetType assetCategory = CapitalAssetType.Other,
            bool simplifiedDepreciationIndicator = false
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetDepreciatingAssetsFirstDeductedWorkpaperAsync(
                    taxpayerId,
                    taxYear, 
                    Guid.Empty,
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.AccountingAdditions = accountingAdditions;
            workpaper.TaxAdditions = taxAdditions;
            workpaper.AssetCategory = assetCategory;
            workpaper.SimplifiedDepreciationIndicator = simplifiedDepreciationIndicator;


            var command = new UpsertDepreciatingAssetsFirstDeductedWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertDepreciatingAssetsFirstDeductedWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
