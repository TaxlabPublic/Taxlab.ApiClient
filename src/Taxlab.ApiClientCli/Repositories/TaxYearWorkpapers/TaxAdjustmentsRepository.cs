using System;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class TaxAdjustmentsRepository : RepositoryBase
    {
        public TaxAdjustmentsRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfTaxAdjustmentsWorkpaper> GetAsync(
            Guid taxpayerId,
            int taxYear)
        {
            var workpaperResponse = await GetTaxYearWorkpaperAsync(
                taxpayerId,
                taxYear,
                (taxpayer, year) => Client.Workpapers_GetTaxAdjustmentsWorkpaperAsync(taxpayer, year));
            
            return workpaperResponse;
        }
    }
}