using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class DividendIncomeRepository : RepositoryBase
    {
        public DividendIncomeRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfDividendIncomeWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string companyName,
            string referenceNumber,
            int howManyPeopleHoldThisAccount,
            decimal unfrankedAmount,
            decimal frankedAmount,
            decimal frankingCredits,
            decimal taxPaid
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetDividendIncomeWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.CompanyName = companyName;
            workpaper.ReferenceNumber = referenceNumber;
            workpaper.AccountHoldersNumbers = howManyPeopleHoldThisAccount;
            workpaper.UnfrankedAmount = unfrankedAmount.ToNumericCell();
            workpaper.FrankedAmount = frankedAmount.ToNumericCell();
            workpaper.FrankingCredits = frankingCredits.ToNumericCell();
            workpaper.TaxPaid = taxPaid.ToNumericCell();

            var command = new UpsertDividendIncomeWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_PostDividendIncomeWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
