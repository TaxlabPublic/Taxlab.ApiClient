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
    public class ForeignIncomeNonIndividualRepository : RepositoryBase
    {
        public ForeignIncomeNonIndividualRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfForeignIncomeNonIndividualWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            DateOnly datePaid = default,
            int taxTreatment = 0,
            decimal grossIncome = 0m,
            decimal frankingCreditsFromNewZealandCompany = 0m,
            decimal totalForeignIncome = 0m,
            decimal taxPaid = 0m,
            decimal permanentDifference = 0m,
            decimal permanentDifferenceLessFrankingCredits = 0m)
        {
            var workpaperResponse = await Client
                .Workpapers_GetForeignIncomeNonIndividualWorkpaperAsync(
                    taxpayerId,
                    taxYear, 
                    Guid.Empty,
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.DatePaid = new DateTime(datePaid.Year, datePaid.Month, datePaid.Day);
            workpaper.TaxTreatment = taxTreatment;
            workpaper.GrossIncome = grossIncome.ToNumericCell();
            workpaper.FrankingCreditsFromNewZealandCompany = frankingCreditsFromNewZealandCompany.ToNumericCell();
            workpaper.TotalForeignIncome = totalForeignIncome;
            workpaper.TaxPaid = taxPaid.ToNumericCell();
            workpaper.PermanentDifference = permanentDifference;
            workpaper.PermanentDifferenceLessFrankingCredits = permanentDifferenceLessFrankingCredits;


            var command = new UpsertForeignIncomeNonIndividualWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertForeignIncomeNonIndividualWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
