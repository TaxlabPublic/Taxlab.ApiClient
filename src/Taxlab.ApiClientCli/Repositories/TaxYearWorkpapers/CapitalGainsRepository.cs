using System;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class CapitalGainsRepository : RepositoryBase
    {
        public CapitalGainsRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfCapitalGainsWorkpaper> GetAndUpsertAsync(
            Guid taxpayerId,
            int taxYear)
        {
            var workpaperResponse = await GetTaxYearWorkpaperAsync(
                taxpayerId,
                taxYear,
                (taxpayer, year) => Client.Workpapers_GetCapitalGainsWorkpaperAsync(taxpayer, year));

            ModifyWorkpaper(workpaperResponse);
            
            var upsertCommand =
                BuildUpsertCommand<WorkpaperResponseOfCapitalGainsWorkpaper, UpsertCapitalGainsWorkpaperCommand>(
                    taxpayerId,
                    taxYear,
                    workpaperResponse,
                    WorkpaperType.CapitalGainsWorkpaper);

            upsertCommand.Workpaper = workpaperResponse.Workpaper;

            var response = await ExecuteCommandAsync(
                upsertCommand,
                cmd => Client.Workpapers_PostCapitalGainsWorkpaperAsync(cmd));

            return response;
        }

        private static void ModifyWorkpaper(WorkpaperResponseOfCapitalGainsWorkpaper workpaperResponse)
        {
            var workpaper = workpaperResponse.Workpaper;
            workpaper.ActiveAssetReductionAmount += 100m;
        }
    }
}