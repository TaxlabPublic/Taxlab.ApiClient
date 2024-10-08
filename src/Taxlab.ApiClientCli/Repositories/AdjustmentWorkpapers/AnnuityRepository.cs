using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class AnnuityRepository : RepositoryBase
    {
        public AnnuityRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfAnnuityWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string payersName = "",
            string abn = "",
            decimal grossIncome = 0m,
            decimal deductibleAmount = 0m,
            decimal taxPaid = 0m
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetAnnuityWorkpaperAsync(
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
            workpaper.GrossIncome = grossIncome.ToNumericCell();
            workpaper.DeductibleAmount = deductibleAmount.ToNumericCell();
            workpaper.TaxPaid = taxPaid.ToNumericCell();

            var command = new UpsertAnnuityWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertAnnuityWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
