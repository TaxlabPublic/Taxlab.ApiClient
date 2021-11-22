using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class ForeignEmploymentIncomeRepository : RepositoryBase
    {
        public ForeignEmploymentIncomeRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfForeignEmploymentIncomeWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            TaxTreatment taxTreatment = TaxTreatment.Taxable,
            string countryCode = "",
            decimal grossIncome = 0m,
            decimal deductions = 0m,
            decimal taxPaid = 0m,
            string employerName = "",
            Payments[] paymentsInArrears = null
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetForeignEmploymentIncomeWorkpaperAsync(
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
            workpaper.CountryCode = countryCode;
            workpaper.GrossIncome = grossIncome.ToNumericCell();
            workpaper.Deductions = deductions.ToNumericCell();
            workpaper.TaxPaid = taxPaid.ToNumericCell();
            workpaper.EmployerName = employerName;
            workpaper.PaymentsInArrears = paymentsInArrears;

            var command = new UpsertForeignEmploymentIncomeWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_PostForeignEmploymentIncomeWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
