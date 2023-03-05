using System;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class ForeignIncomeTaxOffsetsRepository : RepositoryBase
    {
        public ForeignIncomeTaxOffsetsRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfForeignIncomeTaxOffsetsWorkpaper> GetAndUpsertAsync(
            Guid taxpayerId,
            int taxYear)
        {
            var workpaperResponse = await GetTaxYearWorkpaperAsync(
                taxpayerId,
                taxYear,
                (taxpayer, year) => Client.Workpapers_GetForeignIncomeTaxOffsetsWorkpaperAsync(taxpayer, year));

            ModifyWorkpaper(workpaperResponse);
            
            var upsertCommand =
                BuildUpsertCommand<WorkpaperResponseOfForeignIncomeTaxOffsetsWorkpaper, UpsertForeignIncomeTaxOffsetsWorkpaperCommand>(
                    taxpayerId,
                    taxYear,
                    workpaperResponse,
                    WorkpaperType.ForeignIncomeTaxOffsetsWorkpaper);

            upsertCommand.Workpaper = workpaperResponse.Workpaper;

            var response = await ExecuteCommandAsync(
                upsertCommand,
                cmd => Client.Workpapers_PostForeignIncomeTaxOffsetsWorkpaperAsync(cmd));

            return response;
        }
        
        private static void ModifyWorkpaper(WorkpaperResponseOfForeignIncomeTaxOffsetsWorkpaper workpaperResponse)
        {
            var workpaper = workpaperResponse.Workpaper;
            workpaper.TaxableIncome += 100m;
        }
    }
}