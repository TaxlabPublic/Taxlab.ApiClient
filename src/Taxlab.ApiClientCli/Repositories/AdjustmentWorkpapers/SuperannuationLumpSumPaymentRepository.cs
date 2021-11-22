using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class SuperannuationLumpSumPaymentRepository : RepositoryBase
    {
        public SuperannuationLumpSumPaymentRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfSuperannuationLumpSumPaymentWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string payersName = "",
            string abn = "",
            string paymentDate = "",
            decimal taxableAmount = 0m,
            decimal untaxedAmount = 0m,
            decimal nonTaxableAmount = 0m,
            bool isDeathBenefit = false
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetSuperannuationLumpSumPaymentWorkpaperAsync(
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
            workpaper.PaymentDate = paymentDate;
            workpaper.TaxableAmount = taxableAmount.ToNumericCell();
            workpaper.UntaxedAmount = untaxedAmount.ToNumericCell();
            workpaper.NonTaxableAmount = nonTaxableAmount.ToNumericCell();
            workpaper.IsDeathBenefit = isDeathBenefit;

            var command = new UpsertSuperannuationLumpSumPaymentWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_PostSuperannuationLumpSumPaymentWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
