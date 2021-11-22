using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class OtherDeductionRepository : RepositoryBase
    {
        public OtherDeductionRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfOtherDeductionsWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            decimal amount = 0m,
            ReturnDisclosureTypes classification = ReturnDisclosureTypes.OtherDeductibleExpenses
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetOtherDeductionsWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.TaxAdjustment = amount.ToNumericCell();
            workpaper.Classification.ReturnDisclosureTypeId = (int)classification;

            var command = new UpsertOtherDeductionsWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_PostOtherDeductionsWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
