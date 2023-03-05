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

        public async Task<WorkpaperResponseOfLossesScheduleWorkpaper> GetAndUpsertAsync(
            Guid taxpayerId,
            int taxYear)
        {
            var workpaperResponse = await GetTaxYearWorkpaperAsync(
                taxpayerId,
                taxYear,
                (taxpayer, year) => Client.Workpapers_GetLossesScheduleWorkpaperAsync(taxpayer, year));

            ModifyWorkpaper(workpaperResponse);
            
            var upsertCommand =
                BuildUpsertCommand<WorkpaperResponseOfLossesScheduleWorkpaper, UpsertLossesScheduleWorkpaperCommand>(
                    taxpayerId,
                    taxYear,
                    workpaperResponse,
                    WorkpaperType.LossesScheduleWorkpaper);

            upsertCommand.Workpaper = workpaperResponse.Workpaper;

            var response = await ExecuteCommandAsync(
                upsertCommand,
                cmd => Client.Workpapers_PostLossesScheduleWorkpaperAsync(cmd));

            return response;
        }

        private static void ModifyWorkpaper(WorkpaperResponseOfLossesScheduleWorkpaper workpaperResponse)
        {
            var workpaper = workpaperResponse.Workpaper;
            workpaper.BctCapitalLosses += 100m;
        }
    }
}