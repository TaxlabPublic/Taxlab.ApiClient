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
    public class ForeignDeductionNonIndividualRepository : RepositoryBase
    {
        public ForeignDeductionNonIndividualRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfForeignDeductionNonIndividualWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            decimal amountPerAccounts = 0m,
            decimal taxAdjustment = 0m,
            decimal deductibleAmount = 0m,
            decimal permanentDifference = 0m)
        {
            var workpaperResponse = await Client
                .Workpapers_GetForeignDeductionNonIndividualWorkpaperAsync(
                    taxpayerId,
                    taxYear, Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.AmountPerAccounts = amountPerAccounts;
            workpaper.TaxAdjustment = taxAdjustment.ToNumericCell();
            workpaper.DeductibleAmount = deductibleAmount;
            workpaper.PermanentDifference = permanentDifference;


            var command = new UpsertForeignDeductionNonIndividualWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_PostForeignDeductionNonIndividualWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }

    }
}
