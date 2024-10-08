using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class ForeignPensionOrAnnuityRepository : RepositoryBase
    {
        public ForeignPensionOrAnnuityRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfForeignPensionOrAnnuityWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            TaxTreatment taxTreatment = TaxTreatment.Taxable,
            decimal grossIncome = 0m,
            decimal deductions = 0m,
            decimal taxPaid = 0m,
            decimal undeductedPurchasePrice = 0m,
            string description = "",
            Payments[] paymentsInArrears = null
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetForeignPensionOrAnnuityWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.TaxTreatment = taxTreatment;
            workpaper.GrossIncome = grossIncome.ToNumericCell();
            workpaper.Deductions = deductions.ToNumericCell();
            workpaper.TaxPaid = taxPaid.ToNumericCell();
            workpaper.UndeductedPurchasePrice = undeductedPurchasePrice.ToNumericCell();
            workpaper.Description = description;
            workpaper.PaymentsInArrears = paymentsInArrears;

            var command = new UpsertForeignPensionOrAnnuityWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertForeignPensionOrAnnuityWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
