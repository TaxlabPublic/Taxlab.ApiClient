using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class SuperannuationIncomeStreamRepository : RepositoryBase
    {
        public SuperannuationIncomeStreamRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfSuperannuationIncomeStreamWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string payersName = "",
            string abn = "",
            bool isDeathBenefit = false,
            string paymentStartDate = "",
            string paymentEndDate = "",
            decimal incomeStreamTaxFree = 0m,
            decimal lumpsumArrearsTaxFree = 0m,
            decimal incomeStreamTaxed = 0m,
            decimal incomeStreamUntaxed = 0m,
            decimal definedBenefitAmount = 0m,
            decimal lumpsumArrearsTaxed = 0m,
            decimal lumpsumArrearsUntaxed = 0m,
            decimal taxPaid = 0m,
            decimal sisOffsetAmount = 0m,
            Payments[] paymentsInArrears = null
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetSuperannuationIncomeStreamWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            DateTime? start = null;
            DateTime? end = null;
            if (DateTime.TryParse(paymentStartDate, out var s))
            {
                start = s;
            }
            if (DateTime.TryParse(paymentStartDate, out var e))
            {
                end = e;
            }

            var workpaper = workpaperResponse.Workpaper;
            workpaper.PayersName = payersName;
            workpaper.Abn = abn;
            workpaper.IsDeathBenefit = isDeathBenefit;
            workpaper.PaymentStartDate = start;
            workpaper.PaymentEndDate = end;
            workpaper.IncomeStreamTaxFree = incomeStreamTaxFree.ToNumericCell();
            workpaper.LumpsumArrearsTaxFree = lumpsumArrearsTaxFree.ToNumericCell();
            workpaper.IncomeStreamTaxed = incomeStreamTaxed.ToNumericCell();
            workpaper.IncomeStreamUntaxed = incomeStreamUntaxed.ToNumericCell();
            workpaper.DefinedBenefitAmount = definedBenefitAmount.ToNumericCell();
            workpaper.LumpsumArrearsTaxed = lumpsumArrearsTaxed.ToNumericCell();
            workpaper.LumpsumArrearsUntaxed = lumpsumArrearsUntaxed.ToNumericCell();
            workpaper.TaxPaid = taxPaid.ToNumericCell();
            workpaper.SisOffsetAmount = sisOffsetAmount.ToNumericCell();
            workpaper.PaymentsInArrears = paymentsInArrears;

            var command = new UpsertSuperannuationIncomeStreamWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertSuperannuationIncomeStreamWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
