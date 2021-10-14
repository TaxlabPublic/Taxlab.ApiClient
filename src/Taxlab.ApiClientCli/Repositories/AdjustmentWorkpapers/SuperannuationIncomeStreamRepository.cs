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
            string payersName,
            string abn,
            bool isDeathBenefit,
            string paymentStartDate,
            string paymentEndDate,
            decimal incomeStreamTaxFree,
            decimal lumpsumArrearsTaxFree,
            decimal incomeStreamTaxed,
            decimal incomeStreamUntaxed,
            decimal definedBenefitAmount,
            decimal lumpsumArrearsTaxed,
            decimal lumpsumArrearsUntaxed,
            decimal taxPaid,
            decimal sisOffsetAmount,
            Payments[] paymentsInArrears
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

            var workpaper = workpaperResponse.Workpaper;
            workpaper.PayersName = payersName;
            workpaper.Abn = abn;
            workpaper.IsDeathBenefit = isDeathBenefit;
            workpaper.PaymentStartDate = paymentStartDate;
            workpaper.PaymentEndDate = paymentEndDate;
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

            var commandResponse = await Client.Workpapers_PostSuperannuationIncomeStreamWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
