using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class BusinessIncomeExpensesRepository : RepositoryBase
    {
        public BusinessIncomeExpensesRepository(TaxlabApiClient client) : base(client)
        {

        }

        public async Task<WorkpaperResponseOfBusinessIncomeExpensesWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string businessName = "",
            BusinessIncomeTypes businessIncomeType = BusinessIncomeTypes.Business,
            BusinessProductionTypes productionType = BusinessProductionTypes.NonPrimary,
            BusinessNonPrimaryProductionTypes nonPrimaryProductionType = BusinessNonPrimaryProductionTypes.None
            )
        {
            var workpaperResponse = await GetBusinessIncomeExpensesWorkpaperAsync(taxpayerId, taxYear);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.BusinessName = businessName;
            workpaper.BusinessIncomeType = businessIncomeType;
            workpaper.ProductionType = productionType;
            workpaper.NonPrimaryProductionType = nonPrimaryProductionType;

            // Update command for our new workpaper
            var upsertCommand = new UpsertBusinessIncomeExpensesWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = workpaperResponse.DocumentIndexId,
                CompositeRequest = true,
                WorkpaperType = WorkpaperType.BusinessIncomeExpensesWorkpaper,
                Workpaper = workpaperResponse.Workpaper
            };

            var upsertResponse = await Client.Workpapers_PostBusinessIncomeExpensesWorkpaperAsync(upsertCommand)
                .ConfigureAwait(false);

            return upsertResponse;
        }

        public async Task<WorkpaperResponseOfBusinessIncomeExpensesWorkpaper> GetBusinessIncomeExpensesWorkpaperAsync(Guid taxpayerId, int taxYear)
        {
            var workpaperResponse = await Client
                .Workpapers_GetBusinessIncomeExpensesWorkpaperAsync(taxpayerId, taxYear)
                .ConfigureAwait(false);

            return workpaperResponse;
        }
    }
}