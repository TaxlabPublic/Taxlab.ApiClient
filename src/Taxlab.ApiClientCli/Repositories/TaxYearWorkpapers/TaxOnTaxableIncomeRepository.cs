using System;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class TaxOnTaxableIncomeRepository : RepositoryBase
    {
        public TaxOnTaxableIncomeRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfTaxOnTaxableIncomeWorkpaper> GetAsync(
            Guid taxpayerId,
            int taxYear)
        {
            var workpaperResponse = await GetTaxYearWorkpaperAsync(
                taxpayerId,
                taxYear,
                (taxpayer, year) => Client.Workpapers_GetTaxOnTaxableIncomeWorkpaperAsync(taxpayer, year));
            
            return workpaperResponse;
        }
    }
}