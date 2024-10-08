using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class GovernmentAllowanceRepository : RepositoryBase
    {
        public GovernmentAllowanceRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfGovernmentAllowanceWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string benefitType = "",
            string description = "",
            decimal grossIncome = 0m,
            decimal taxPaid = 0m
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetGovernmentAllowanceWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.BenefitType = benefitType;
            workpaper.Description = description;
            workpaper.GrossIncome = grossIncome.ToNumericCell();
            workpaper.TaxPaid = taxPaid.ToNumericCell();

            var command = new UpsertGovernmentAllowanceWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertGovernmentAllowanceWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
