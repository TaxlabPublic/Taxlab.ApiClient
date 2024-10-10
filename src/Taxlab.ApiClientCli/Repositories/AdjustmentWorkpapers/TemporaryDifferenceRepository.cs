using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;
using Xunit;
using System.Collections.Generic;

namespace Taxlab.ApiClientCli.Repositories.AdjustmentWorkpapers
{
    public class TemporaryDifferenceRepository : RepositoryBase
    {
        public TemporaryDifferenceRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfTemporaryDifferenceWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            AdvancedSwitch advancedSwitch = AdvancedSwitch.Standard,
            decimal totalCarryingAmountPriorPeriod = 0m,
            decimal totalFutureTaxablePriorPeriod = 0m,
            decimal totalFutureDeductiblePriorPeriod = 0m,
            decimal totalTaxBasePriorPeriod = 0m,
            decimal totalCarryingAmountCurrentPeriod = 0m,
            decimal totalFutureTaxableCurrentPeriod = 0m,
            decimal totalFutureDeductibleCurrentPeriod = 0m,
            decimal totalTaxBaseCurrentPeriod = 0m,
            decimal totalCarryingAmountMovement = 0m,
            decimal totalFutureTaxableMovement = 0m,
            decimal totalFutureDeductibleMovement = 0m,
            decimal totalTaxBaseMovement = 0m,
            decimal balanceSheetCarryingAmountPriorPeriod = 0m,
            decimal balanceSheetFutureTaxablePriorPeriod = 0m,
            decimal balanceSheetFutureDeductiblePriorPeriod = 0m,
            decimal balanceSheetTaxBasePriorPeriod = 0m,
            decimal balanceSheetCarryingAmountCurrentPeriod = 0m,
            decimal balanceSheetFutureTaxableCurrentPeriod = 0m,
            decimal balanceSheetFutureDeductibleCurrentPeriod = 0m,
            decimal balanceSheetTaxBaseCurrentPeriod = 0m,
            decimal balanceSheetCarryingAmountMovement = 0m,
            decimal balanceSheetFutureTaxableMovement = 0m,
            decimal balanceSheetFutureDeductibleMovement = 0m,
            decimal balanceSheetTaxBaseMovement = 0m,
            decimal incomeStatementCarryingAmountPriorPeriod = 0m,
            decimal incomeStatementFutureTaxablePriorPeriod = 0m,
            decimal incomeStatementFutureDeductiblePriorPeriod = 0m,
            decimal incomeStatementTaxReturnAdjustmentPriorYear = 0m,
            decimal incomeStatementTaxBasePriorPeriod = 0m,
            decimal incomeStatementCarryingAmountCurrentPeriod = 0m,
            decimal incomeStatementFutureTaxableCurrentPeriod = 0m,
            decimal incomeStatementFutureDeductibleCurrentPeriod = 0m,
            decimal incomeStatementTaxReturnAdjustmentCurrentYear = 0m,
            decimal incomeStatementTaxBaseCurrentPeriod = 0m,
            decimal incomeStatementCarryingAmountMovement = 0m,
            decimal incomeStatementFutureTaxableMovement = 0m,
            decimal incomeStatementFutureDeductibleMovement = 0m,
            decimal incomeStatementTaxBaseMovement = 0m,
            decimal incomeStatementFutureTaxableAdjustment = 0m,
            decimal incomeStatementFutureDeductibleAdjustment = 0m,
            decimal incomeStatementAdjustment = 0m,
            int returnDisclosureTypePriorPeriod = 0)
        {
            var workpaperResponse = await Client
                .Workpapers_GetTemporaryDifferenceWorkpaperAsync(
                    taxpayerId,
                    taxYear, 
                    Guid.Empty,
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.AdvancedSwitch = advancedSwitch;
            workpaper.TotalCarryingAmountPriorPeriod = totalCarryingAmountPriorPeriod.ToNumericCell();
            workpaper.TotalFutureTaxablePriorPeriod = totalFutureTaxablePriorPeriod;
            workpaper.TotalFutureDeductiblePriorPeriod = totalFutureDeductiblePriorPeriod;
            workpaper.TotalTaxBasePriorPeriod = totalTaxBasePriorPeriod;
            workpaper.TotalCarryingAmountCurrentPeriod = totalCarryingAmountCurrentPeriod.ToNumericCell();
            workpaper.TotalFutureTaxableCurrentPeriod = totalFutureDeductiblePriorPeriod;
            workpaper.TotalFutureDeductibleCurrentPeriod = totalFutureDeductiblePriorPeriod;
            workpaper.TotalTaxBaseCurrentPeriod = totalTaxBaseCurrentPeriod;
            workpaper.TotalCarryingAmountMovement = totalCarryingAmountMovement;
            workpaper.TotalFutureTaxableMovement = totalFutureTaxableMovement;
            workpaper.TotalFutureDeductibleMovement = totalFutureDeductibleMovement;
            workpaper.TotalTaxBaseMovement = totalTaxBaseMovement;
            workpaper.BalanceSheetCarryingAmountPriorPeriod = balanceSheetCarryingAmountPriorPeriod.ToNumericCell();
            workpaper.BalanceSheetFutureTaxablePriorPeriod = balanceSheetFutureTaxablePriorPeriod.ToNumericCell();
            workpaper.BalanceSheetFutureDeductiblePriorPeriod = balanceSheetFutureDeductiblePriorPeriod.ToNumericCell();
            workpaper.BalanceSheetTaxBasePriorPeriod = balanceSheetTaxBasePriorPeriod;
            workpaper.BalanceSheetCarryingAmountCurrentPeriod = balanceSheetCarryingAmountCurrentPeriod.ToNumericCell();
            workpaper.BalanceSheetFutureTaxableCurrentPeriod = balanceSheetFutureTaxableCurrentPeriod.ToNumericCell();
            workpaper.BalanceSheetFutureDeductibleCurrentPeriod = balanceSheetFutureDeductibleCurrentPeriod.ToNumericCell();
            workpaper.BalanceSheetTaxBaseCurrentPeriod = balanceSheetTaxBaseCurrentPeriod;
            workpaper.BalanceSheetCarryingAmountMovement = balanceSheetCarryingAmountMovement;
            workpaper.BalanceSheetFutureTaxableMovement = balanceSheetFutureTaxableMovement;
            workpaper.BalanceSheetFutureDeductibleMovement = balanceSheetFutureDeductibleMovement;
            workpaper.BalanceSheetTaxBaseMovement = balanceSheetTaxBaseMovement;
            workpaper.IncomeStatementCarryingAmountPriorPeriod = incomeStatementCarryingAmountPriorPeriod;
            workpaper.IncomeStatementFutureTaxablePriorPeriod = incomeStatementFutureTaxablePriorPeriod.ToNumericCell();
            workpaper.IncomeStatementFutureDeductiblePriorPeriod = incomeStatementFutureDeductiblePriorPeriod.ToNumericCell();
            workpaper.IncomeStatementTaxReturnAdjustmentPriorYear = incomeStatementTaxReturnAdjustmentPriorYear;
            workpaper.IncomeStatementTaxBasePriorPeriod = incomeStatementTaxBasePriorPeriod;
            workpaper.IncomeStatementCarryingAmountCurrentPeriod = incomeStatementCarryingAmountCurrentPeriod;
            workpaper.IncomeStatementFutureTaxableCurrentPeriod = incomeStatementFutureTaxableCurrentPeriod.ToNumericCell();
            workpaper.IncomeStatementFutureDeductibleCurrentPeriod = incomeStatementFutureDeductibleCurrentPeriod.ToNumericCell();
            workpaper.IncomeStatementTaxReturnAdjustmentCurrentYear = incomeStatementTaxReturnAdjustmentCurrentYear;
            workpaper.IncomeStatementTaxBaseCurrentPeriod = incomeStatementTaxBaseCurrentPeriod;
            workpaper.IncomeStatementCarryingAmountMovement = incomeStatementCarryingAmountMovement;
            workpaper.IncomeStatementFutureTaxableMovement = incomeStatementFutureTaxableMovement;
            workpaper.IncomeStatementFutureDeductibleMovement = incomeStatementFutureDeductibleMovement;
            workpaper.IncomeStatementTaxBaseMovement = incomeStatementTaxBaseMovement;
            workpaper.IncomeStatementFutureTaxableAdjustment = incomeStatementFutureTaxableAdjustment;
            workpaper.IncomeStatementFutureDeductibleAdjustment = incomeStatementFutureDeductibleAdjustment;
            workpaper.IncomeStatementAdjustment = incomeStatementAdjustment;
            workpaper.ReturnDisclosureTypePriorPeriod = returnDisclosureTypePriorPeriod;


            var command = new UpsertTemporaryDifferenceWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertTemporaryDifferenceWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
