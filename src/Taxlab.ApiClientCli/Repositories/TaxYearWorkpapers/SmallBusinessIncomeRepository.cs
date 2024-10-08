using System;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class SmallBusinessIncomeRepository : RepositoryBase
    {
        public SmallBusinessIncomeRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfSmallBusinessIncomeWorkpaper> GetAndUpsertAsync(
            Guid taxpayerId,
            int taxYear)
        {
            var workpaperResponse = await GetTaxYearWorkpaperAsync(
                taxpayerId,
                taxYear,
                (taxpayer, year) => Client.Workpapers_GetSmallBusinessIncomeWorkpaperAsync(taxpayer, year));

            ModifyWorkpaper(workpaperResponse);
            
            var upsertCommand =
                BuildUpsertCommand<WorkpaperResponseOfSmallBusinessIncomeWorkpaper, UpsertSmallBusinessIncomeWorkpaperCommand>(
                    taxpayerId,
                    taxYear,
                    workpaperResponse,
                    WorkpaperType.SmallBusinessIncomeWorkpaper);

            upsertCommand.Workpaper = workpaperResponse.Workpaper;

            var response = await ExecuteCommandAsync(
                upsertCommand,
                cmd => Client.Workpapers_UpsertSmallBusinessIncomeWorkpaperAsync(cmd));

            return response;
        }

        private static void ModifyWorkpaper(WorkpaperResponseOfSmallBusinessIncomeWorkpaper workpaperResponse)
        {
            var workpaper = workpaperResponse.Workpaper;
            workpaper.TaxableIncome += 100m;
        }
    }
}