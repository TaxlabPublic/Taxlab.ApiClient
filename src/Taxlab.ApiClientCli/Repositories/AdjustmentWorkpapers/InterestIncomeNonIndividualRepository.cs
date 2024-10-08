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
    public class InterestIncomeNonIndividualRepository : RepositoryBase
    {
        public InterestIncomeNonIndividualRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfInterestIncomeNonIndividualWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            DateOnly datePaid = new DateOnly(),
            decimal grossIncome = 0m,
            decimal taxPaid = 0m,
            decimal netIncome = 0m,
            decimal permanentDifference = 0m)
        {
            var workpaperResponse = await Client
                .Workpapers_GetInterestIncomeNonIndividualWorkpaperAsync(
                    taxpayerId,
                    taxYear, Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.DatePaid = new DateTime(datePaid.Year, datePaid.Month, datePaid.Day);
            workpaper.GrossIncome = grossIncome.ToNumericCell();
            workpaper.TaxPaid = taxPaid.ToNumericCell();
            workpaper.NetIncome = netIncome;
            workpaper.PermanentDifference = permanentDifference;


            var command = new UpsertInterestIncomeNonIndividualWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertInterestIncomeNonIndividualWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
