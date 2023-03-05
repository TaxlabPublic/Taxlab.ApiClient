using System;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class GovernmentSuperContributionsRepository : RepositoryBase
    {
        public GovernmentSuperContributionsRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfGovernmentSuperContributionsWorkpaper> GetAndUpsertAsync(
            Guid taxpayerId,
            int taxYear)
        {
            var workpaperResponse = await GetTaxYearWorkpaperAsync(
                taxpayerId, 
                taxYear,
                (taxpayer, year) => Client.Workpapers_GetGovernmentSuperContributionsWorkpaperAsync(taxpayer, year));

            ModifyWorkpaper(workpaperResponse);
            
            var upsertCommand =
                BuildUpsertCommand<WorkpaperResponseOfGovernmentSuperContributionsWorkpaper, UpsertGovernmentSuperContributionsWorkpaperCommand>(
                    taxpayerId,
                    taxYear,
                    workpaperResponse,
                    WorkpaperType.GovernmentSuperContributionsWorkpaper);

            upsertCommand.Workpaper = workpaperResponse.Workpaper;

            var response = await ExecuteCommandAsync(
                upsertCommand,
                cmd => Client.Workpapers_PostGovernmentSuperContributionsWorkpaperAsync(cmd));

            return response;
        }
        
        private static void ModifyWorkpaper(WorkpaperResponseOfGovernmentSuperContributionsWorkpaper workpaperResponse)
        {
            var workpaper = workpaperResponse.Workpaper;
            workpaper.MadeAnEligiblePersonalSuperContribution = !workpaper.MadeAnEligiblePersonalSuperContribution;
        }
    }
}