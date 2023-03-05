using System;
using System.Threading;
using System.Threading.Tasks;
using NodaTime;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;
using Xunit;

namespace Taxlab.ApiClientCli.Repositories.AdjustmentWorkpapers
{
    public class ExpensesCapitalisedForTaxRepository : RepositoryBase
    {
        public ExpensesCapitalisedForTaxRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfExpensesCapitalisedForTaxWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            decimal expensePerAccounts = 0m,
            decimal capitalisedForTax = 0m,
            decimal expenseDeductibleForTax = 0m,
            decimal taxDepreciation = 0m,
            decimal taxCarryingAmount = 0m,
            string taxDepreciationNarration = "",
            int taxDepreciationReturnDisclosureTypeId = 0,
            bool simplifiedDepreciationIndicator = false)
        {
            var workpaperResponse = await Client
                .Workpapers_GetExpensesCapitalisedForTaxWorkpaperAsync(
                    taxpayerId,
                    taxYear, Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.ExpensePerAccounts = expensePerAccounts.ToNumericCell();
            workpaper.CapitalisedForTax = capitalisedForTax;
            workpaper.ExpenseDeductibleForTax = expenseDeductibleForTax;
            workpaper.TaxDepreciation = taxDepreciation;
            workpaper.TaxCarryingAmount = taxCarryingAmount;
            workpaper.TaxDepreciationNarration = taxDepreciationNarration;
            workpaper.TaxDepreciationReturnDisclosureTypeId = taxDepreciationReturnDisclosureTypeId;
            workpaper.SimplifiedDepreciationIndicator = simplifiedDepreciationIndicator;


            var command = new UpsertExpensesCapitalisedForTaxWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_PostExpensesCapitalisedForTaxWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }


    }
}
