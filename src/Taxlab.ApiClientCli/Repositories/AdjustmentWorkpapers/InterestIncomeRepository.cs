using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class InterestIncomeRepository : RepositoryBase
    {
        public InterestIncomeRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfInterestIncomeWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string financialInstitutionName,
            string accountNumber,
            int howManyPeopleHoldThisAccount,
            decimal income,
            decimal taxPaid
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetInterestIncomeWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.FinancialInstitutionName = financialInstitutionName;
            workpaper.AccountNumber = accountNumber;
            workpaper.AccountHoldersNumbers = howManyPeopleHoldThisAccount;
            workpaper.GrossIncome = income.ToNumericCell();
            workpaper.TaxPaid = taxPaid.ToNumericCell();

            var command = new UpsertInterestIncomeWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_PostInterestIncomeWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
