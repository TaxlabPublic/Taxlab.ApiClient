using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class EmploymentTerminationPaymentRepository : RepositoryBase
    {
        public EmploymentTerminationPaymentRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfEmploymentTerminationPaymentWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string countryCode = "",
            string employerName = "",
            string abn = "",
            string paymentDate = "",
            decimal nonTaxableAmount = 0m,
            decimal taxableAmount = 0m,
            decimal taxPaid = 0m
            )
        {
            DateTime? paymentDateTime = null;
            if (DateTime.TryParse(paymentDate, out var d))
            {
                paymentDateTime = d;
            }

            var workpaperResponse = await Client
                .Workpapers_GetEmploymentTerminationPaymentWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    Guid.Empty,
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.CountryCode = countryCode;
            workpaper.EmployerName = employerName;
            workpaper.Abn = abn;
            workpaper.PaymentDate = paymentDateTime;
            workpaper.NonTaxableAmount = nonTaxableAmount.ToNumericCell();
            workpaper.TaxableAmount = taxableAmount.ToNumericCell();
            workpaper.TaxPaid = taxPaid.ToNumericCell();

            var command = new UpsertEmploymentTerminationPaymentWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertEmploymentTerminationPaymentWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
