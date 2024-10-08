using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class AttributedPersonalServicesIncomeRepository : RepositoryBase
    {
        public AttributedPersonalServicesIncomeRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfAttributedPersonalServicesIncomeWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string payersName = "",
            string abn = "",
            decimal grossIncome = 0m,
            decimal taxPaid = 0m,
            decimal reportableSuperContributions = 0m
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetAttributedPersonalServicesIncomeWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.PayersName = payersName;
            workpaper.Abn = abn;
            workpaper.GrossIncome = grossIncome.ToNumericCell();
            workpaper.TaxPaid = taxPaid.ToNumericCell();
            workpaper.ReportableSuperContributions = reportableSuperContributions.ToNumericCell();

            var command = new UpsertAttributedPersonalServicesIncomeWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertAttributedPersonalServicesIncomeWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
