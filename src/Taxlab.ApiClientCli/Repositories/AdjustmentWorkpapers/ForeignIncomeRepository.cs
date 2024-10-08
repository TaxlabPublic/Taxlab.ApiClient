using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class ForeignIncomeRepository : RepositoryBase
    {
        public ForeignIncomeRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfForeignIncomeWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            TaxTreatment taxTreatment = TaxTreatment.Taxable,
            decimal grossIncome = 0m,
            decimal deductions = 0m,
            decimal taxPaid = 0m,
            decimal frankingCreditsFromNewZealandCompany = 0m,
            string descriptionOfIncome = "",
            string foreignEntityTrustName = "",
            string foreignEntityTrusteeName = ""
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetForeignIncomeWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.TaxTreatment = taxTreatment;
            workpaper.GrossIncome = grossIncome.ToNumericCell();
            workpaper.Deductions = deductions.ToNumericCell();
            workpaper.TaxPaid = taxPaid.ToNumericCell();
            workpaper.FrankingCreditsFromNewZealandCompany = frankingCreditsFromNewZealandCompany.ToNumericCell();
            workpaper.DescriptionOfIncome = descriptionOfIncome;
            workpaper.ForeignEntityTrustName = foreignEntityTrustName;
            workpaper.ForeignEntityTrusteeName = foreignEntityTrusteeName;

            var command = new UpsertForeignIncomeWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertForeignIncomeWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
