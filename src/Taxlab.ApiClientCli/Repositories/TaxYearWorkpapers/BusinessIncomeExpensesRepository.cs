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
            var createCommand = new CreateBusinessIncomeExpensesWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                InitialValue = new BusinessIncomeExpensesWorkpaperInitialValue
                {
                    BusinessName = businessName,
                    BusinessIncomeType = businessIncomeType,
                    ProductionType = productionType,
                    NonPrimaryProductionType = nonPrimaryProductionType
                }
            };

            var createResponse = await Client.Workpapers_CreateBusinessIncomeExpensesWorkpaperAsync(createCommand)
                .ConfigureAwait(false);

            return createResponse;
        }

        public async Task<WorkpaperResponseOfBusinessIncomeExpensesWorkpaper> GetBusinessIncomeExpensesWorkpaperAsync(Guid taxpayerId, int taxYear)
        {
            var workpaperResponse = await Client
                .Workpapers_GetBusinessIncomeExpensesWorkpaperAsync(taxpayerId, taxYear, Guid.Empty)
                .ConfigureAwait(false);

            return workpaperResponse;
        }
    }
}