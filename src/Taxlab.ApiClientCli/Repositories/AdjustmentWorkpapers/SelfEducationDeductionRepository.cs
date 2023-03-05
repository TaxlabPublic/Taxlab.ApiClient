using System;
using System.Threading;
using System.Threading.Tasks;
using NodaTime;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;
using Xunit;
using System.Collections.Generic;

namespace Taxlab.ApiClientCli.Repositories.AdjustmentWorkpapers
{
    public class SelfEducationDeductionRepository : RepositoryBase
    {
        public SelfEducationDeductionRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfSelfEducationDeductionWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string description = "",
            string category = "",
            string reason = "",
            decimal expenses = 0m,
            decimal taxAdjustment = 0m,
            decimal deductibleExpense = 0m,
            decimal permanentDifference = 0m)
        {
            var workpaperResponse = await Client
                .Workpapers_GetSelfEducationDeductionWorkpaperAsync(
                    taxpayerId,
                    taxYear, Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.Description = description;
            workpaper.Category = category;
            workpaper.Reason = reason;
            workpaper.Expenses = expenses;
            workpaper.TaxAdjustment = taxAdjustment;
            workpaper.DeductibleExpense = deductibleExpense;
            workpaper.PermanentDifference = permanentDifference;


            var command = new UpsertSelfEducationDeductionWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_PostSelfEducationDeductionWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
