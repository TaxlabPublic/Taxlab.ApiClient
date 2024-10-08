using System;
using System.Threading;
using System.Threading.Tasks;

using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;
using Xunit;

namespace Taxlab.ApiClientCli.Repositories.AdjustmentWorkpapers
{
    public class DividendIncomeNonIndividualRepository : RepositoryBase
    {
        public DividendIncomeNonIndividualRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfDividendIncomeNonIndividualWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            DateOnly datePaid = new DateOnly(),
            decimal unfrankedAmount = 0m,
            decimal frankedAmount = 0m,
            decimal totalDividends = 0m,
            decimal frankedDistributionAmount = 0m,
            decimal frankingCredits = 0m,
            decimal disallowedFrankingCredits = 0m,
            decimal totalFrankingCredits = 0m,
            decimal totalDividendIncome = 0m,
            decimal nonAssessableDividendIncome = 0m,
            decimal taxableIncome = 0m,
            decimal taxPaid = 0m,
            decimal permanentDifference = 0m,
            decimal permanentDifferenceLessFrankingCredits = 0m)
        {
            var workpaperResponse = await Client
                .Workpapers_GetDividendIncomeNonIndividualWorkpaperAsync(
                    taxpayerId,
                    taxYear, Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.DatePaid = new DateTimeOffset(new DateTime(datePaid.Year, datePaid.Month, datePaid.Day));
            workpaper.UnfrankedAmount = unfrankedAmount.ToNumericCell();
            workpaper.FrankedAmount= frankedAmount.ToNumericCell();
            workpaper.TotalDividends = totalDividends;
            workpaper.FrankedDistributionAmount = frankedDistributionAmount;
            workpaper.FrankingCredits = frankingCredits.ToNumericCell();
            workpaper.DisallowedFrankingCredits = disallowedFrankingCredits.ToNumericCell();
            workpaper.TotalFrankingCredits = totalFrankingCredits;
            workpaper.TotalDividendIncome = totalDividendIncome;
            workpaper.NonAssessableDividendIncome = nonAssessableDividendIncome.ToNumericCell();
            workpaper.TaxableIncome = taxableIncome;
            workpaper.TaxPaid = taxPaid.ToNumericCell();
            workpaper.PermanentDifference = permanentDifference;
            workpaper.PermanentDifferenceLessFrankingCredits = permanentDifferenceLessFrankingCredits;


            var command = new UpsertDividendIncomeNonIndividualWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertDividendIncomeNonIndividualWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }

    }
}
