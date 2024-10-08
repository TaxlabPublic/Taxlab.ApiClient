using System;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class LossesScheduleRepository : RepositoryBase
    {
        public LossesScheduleRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfLossDisclosuresWorkpaper> GetAndUpsertAsync(
            Guid taxpayerId,
            int taxYear)
        {
            var workpaperResponse = await GetTaxYearWorkpaperAsync(
                taxpayerId,
                taxYear,
                (taxpayer, year) => Client.Workpapers_GetLossDisclosuresWorkpaperAsync(taxpayer, year));

            ModifyWorkpaper(workpaperResponse);
            
            var upsertCommand =
                BuildUpsertCommand<WorkpaperResponseOfLossDisclosuresWorkpaper, UpsertLossDisclosuresWorkpaperCommand>(
                    taxpayerId,
                    taxYear,
                    workpaperResponse,
                    WorkpaperType.LossesScheduleWorkpaper);

            upsertCommand.Workpaper = workpaperResponse.Workpaper;

            var response = await ExecuteCommandAsync(
                upsertCommand,
                cmd => Client.Workpapers_UpsertLossDisclosuresWorkpaperAsync(cmd));

            return response;
        }

        private static void ModifyWorkpaper(WorkpaperResponseOfLossDisclosuresWorkpaper workpaperResponse)
        {
            var workpaper = workpaperResponse.Workpaper;
            workpaper.BctCapitalLosses += 100m;
        }
    }
}