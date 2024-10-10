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
    public class PermanentDifferenceRepository : RepositoryBase
    {
        public PermanentDifferenceRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfPermanentDifferenceWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            decimal amountPerAccounts = 0m,
            decimal taxAdjustment = 0m,
            decimal taxableAmount = 0m,
            decimal permanentDifference = 0m)
        {
            var workpaperResponse = await Client
                .Workpapers_GetPermanentDifferenceWorkpaperAsync(
                    taxpayerId,
                    taxYear, 
                    Guid.Empty,
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.AmountPerAccounts = amountPerAccounts;
            workpaper.TaxAdjustment = taxAdjustment.ToNumericCell();
            workpaper.TaxableAmount = taxableAmount;
            workpaper.PermanentDifference = permanentDifference;


            var command = new UpsertPermanentDifferenceWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertPermanentDifferenceWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
