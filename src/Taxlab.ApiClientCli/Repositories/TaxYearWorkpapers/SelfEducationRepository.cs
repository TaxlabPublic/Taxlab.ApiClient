using System;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class SelfEducationRepository : RepositoryBase
    {
        public SelfEducationRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfSelfEducationWorkpaper> GetAndUpsertAsync(
            Guid taxpayerId,
            int taxYear)
        {
            var workpaperResponse = await GetTaxYearWorkpaperAsync(
                taxpayerId,
                taxYear,
                (taxpayer, year) => Client.Workpapers_GetSelfEducationWorkpaperAsync(taxpayer, year));

            ModifyWorkpaper(workpaperResponse);
            
            var upsertCommand =
                BuildUpsertCommand<WorkpaperResponseOfSelfEducationWorkpaper, UpsertSelfEducationWorkpaperCommand>(
                    taxpayerId,
                    taxYear,
                    workpaperResponse,
                    WorkpaperType.SelfEducationWorkpaper);

            upsertCommand.Workpaper = workpaperResponse.Workpaper;

            var response = await ExecuteCommandAsync(
                upsertCommand,
                cmd => Client.Workpapers_PostSelfEducationWorkpaperAsync(cmd));

            return response;
        }

        private static void ModifyWorkpaper(WorkpaperResponseOfSelfEducationWorkpaper workpaperResponse)
        {
            var workpaper = workpaperResponse.Workpaper;
            workpaper.ExpenseCategoryA += 100m;
        }
    }
}