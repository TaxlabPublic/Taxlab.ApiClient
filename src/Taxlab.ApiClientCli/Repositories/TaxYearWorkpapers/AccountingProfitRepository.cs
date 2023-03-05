using System;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class AccountingProfitRepository : RepositoryBase
    {
        public AccountingProfitRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfAccountingProfitWorkpaper> GetAsync(
            Guid taxpayerId,
            int taxYear)
        {
            var getWorkpaperResponse = await GetTaxYearWorkpaperAsync(
                taxpayerId, 
                taxYear, 
                (taxpayer, year) => Client.Workpapers_GetAccountingProfitWorkpaperAsync(taxpayer, year));
            
            return getWorkpaperResponse;
        }
    }
}
