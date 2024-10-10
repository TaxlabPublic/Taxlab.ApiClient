using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class DeductibleCarExpenseRepository : RepositoryBase
    {
        public DeductibleCarExpenseRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfDeductibleCarExpenseWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string carMakeModel = "",
            DeductibleCarExpenseModes deductibleCarExpenseMode = DeductibleCarExpenseModes.CentsPerKilometre,
            int centsPerKilometerBusinessKilometersTravelled = 0,
            decimal logbookFuelAndOilAmount = 0m,
            decimal logbookRegistrationAmount = 0m,
            decimal logbookRepairAndMaintenanceAmount = 0m,
            decimal logbookInsuranceAmount = 0m,
            decimal logbookLoanInterestAmount = 0m,
            decimal logbookLeasePaymentsAmount = 0m,
            decimal logbookOtherAmount = 0m,
            decimal logbookDeclineInValue = 0m,
            decimal logbookPercentageOfBusinessUse = 0m
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetDeductibleCarExpenseWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    Guid.Empty,
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.CarMakeModel = carMakeModel;
            workpaper.DeductibleCarExpenseMode = deductibleCarExpenseMode;
            workpaper.CentsPerKilometerBusinessKilometersTravelled = centsPerKilometerBusinessKilometersTravelled;
            workpaper.LogbookFuelAndOilAmount = logbookFuelAndOilAmount.ToNumericCell();
            workpaper.LogbookRegistrationAmount = logbookRegistrationAmount.ToNumericCell();
            workpaper.LogbookRepairAndMaintenanceAmount = logbookRepairAndMaintenanceAmount.ToNumericCell();
            workpaper.LogbookInsuranceAmount = logbookInsuranceAmount.ToNumericCell();
            workpaper.LogbookLoanInterestAmount = logbookLoanInterestAmount.ToNumericCell();
            workpaper.LogbookLeasePaymentsAmount = logbookLeasePaymentsAmount.ToNumericCell();
            workpaper.LogbookOtherAmount = logbookOtherAmount.ToNumericCell();
            workpaper.LogbookDeclineInValue = logbookDeclineInValue.ToNumericCell();
            workpaper.LogbookPercentageOfBusinessUse = logbookPercentageOfBusinessUse;

            var command = new UpsertDeductibleCarExpenseWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertDeductibleCarExpenseWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
