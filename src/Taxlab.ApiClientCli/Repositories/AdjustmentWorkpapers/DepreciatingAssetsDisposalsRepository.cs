using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.AdjustmentWorkpapers
{
    public class DepreciatingAssetsDisposalsRepository : RepositoryBase
    {
        public DepreciatingAssetsDisposalsRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfDepreciatingAssetsDisposalsWorkpaper> CreateAsync(
        Guid taxpayerId,
        int taxYear,
        decimal costAccounting = 0m,
        decimal costTax = 0m,
        decimal accumulatedDepreciationAccounting = 0m,
        decimal accumulatedDepreciationTax = 0m,
        decimal netBookValueAccounting = 0m,
        decimal netBookValueTax = 0m,
        decimal proceedsAccounting = 0m,
        decimal proceedsTax = 0m,
        decimal gainLossOnDisposalAccounting = 0m,
        decimal gainLossOnDisposalTax = 0m,
        decimal differenceFromAccount = 0m,
        string accoutingCapitalGainOnDisposalDescription = "",
        decimal accoutingCapitalGainOnDisposal = 0m,
        string accountingRevenueGainLossOnDisposalDescription = "",
        decimal accountingRevenueGainLossOnDisposal = 0m,
        string assesableDeductibleBalancingEventDescription = "",
        decimal assesableDeductibleBalancingEvent = 0m,
        int taxReturnDisclosureTypeId = 0,
        CapitalAssetType assetCategory = CapitalAssetType.Intangible,
        bool simplifiedDepreciationIndicator = false)
        {
            var workpaperResponse = await Client
                .Workpapers_GetDepreciatingAssetsDisposalsWorkpaperAsync(
                    taxpayerId,
                    taxYear, Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.CostAccounting = costAccounting;
            workpaper.CostTax = costTax;
            workpaper.AccumulatedDepreciationAccounting = accumulatedDepreciationAccounting;
            workpaper.AccumulatedDepreciationTax = netBookValueAccounting;
            workpaper.NetBookValueAccounting = netBookValueTax;
            workpaper.NetBookValueTax = proceedsAccounting;
            workpaper.ProceedsAccounting = proceedsTax;
            workpaper.ProceedsTax = gainLossOnDisposalAccounting;
            workpaper.GainLossOnDisposalAccounting = gainLossOnDisposalTax;
            workpaper.GainLossOnDisposalTax = gainLossOnDisposalTax;
            workpaper.DifferenceFromAccount = differenceFromAccount;
            workpaper.AccoutingCapitalGainOnDisposalDescription = accoutingCapitalGainOnDisposalDescription;
            workpaper.AccoutingCapitalGainOnDisposal = accoutingCapitalGainOnDisposal;
            workpaper.AccountingRevenueGainLossOnDisposalDescription = accountingRevenueGainLossOnDisposalDescription;
            workpaper.AccountingRevenueGainLossOnDisposal = accountingRevenueGainLossOnDisposal;
            workpaper.AssesableDeductibleBalancingEventDescription = assesableDeductibleBalancingEventDescription;
            workpaper.AssesableDeductibleBalancingEvent = assesableDeductibleBalancingEvent;
            workpaper.TaxReturnDisclosureTypeId = taxReturnDisclosureTypeId;
            workpaper.AssetCategory = assetCategory;
            workpaper.SimplifiedDepreciationIndicator = simplifiedDepreciationIndicator;


            var command = new UpsertDepreciatingAssetsDisposalsWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_PostDepreciatingAssetsDisposalsWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
