using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class OtherDeductionRepository : RepositoryBase
    {
        public OtherDeductionRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfOtherDeductionsWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            decimal amount = 0m,
            ReturnDisclosureTypes classification = ReturnDisclosureTypes.AUIndividualElectionExpenses,
            string workpaperDescription = null
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetOtherDeductionsWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    Guid.NewGuid(),
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.TaxAdjustment = amount.ToNumericCell();

            if (!string.IsNullOrEmpty(workpaperDescription))
            {
                workpaper.Classification.TaxNarration = workpaperDescription;
                workpaper.Slug.AccountDescription = workpaperDescription;
            }

            var classificationId = GetDeductionClassificationId(classification);
            workpaper.Classification.ReturnDisclosureTypeId = classificationId;

            var command = new UpsertOtherDeductionsWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_PostOtherDeductionsWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }

        private int GetDeductionClassificationId(ReturnDisclosureTypes value)
        {

            var items = new Dictionary<ReturnDisclosureTypes, int>();
            items.Add(ReturnDisclosureTypes.AUIndividualCostOfManagingTaxAffairsATOInterest, 191);
            items.Add(ReturnDisclosureTypes.AUIndividualCostOfManagingTaxAffairsLitigation, 192);
            items.Add(ReturnDisclosureTypes.AUIndividualCostOfManagingTaxAffairsOther, 193);
            items.Add(ReturnDisclosureTypes.DepreciationPool, 35);
            items.Add(ReturnDisclosureTypes.AUIndividualDividendDeductions, 83);
            items.Add(ReturnDisclosureTypes.FarmingIncomeRepaymentsDeposits, 136);
            items.Add(ReturnDisclosureTypes.AUIndividualForestryManagement, 93);
            items.Add(ReturnDisclosureTypes.AUIndividualGiftsOrDonations, 84);
            items.Add(ReturnDisclosureTypes.AUIndividualInterestDeductions, 82);
            items.Add(ReturnDisclosureTypes.AUIndividualLowValuePoolDeductionOther, 194);
            items.Add(ReturnDisclosureTypes.AUIndividualLowValuePoolDeductionFinancialInvestment, 195);
            items.Add(ReturnDisclosureTypes.AUIndividualLowValuePoolDeductionRentalPool, 196);
            items.Add(ReturnDisclosureTypes.AUInvestmentIncomeDeduction, 111);
            items.Add(ReturnDisclosureTypes.AUIndividualElectionExpenses, 92);
            items.Add(ReturnDisclosureTypes.InsurancePremiumDeduction, 134);
            items.Add(ReturnDisclosureTypes.OtherDeductibleExpenses, 40);
            items.Add(ReturnDisclosureTypes.AUIndividualWorkRelatedClothingUniformCompulsory, 197);
            items.Add(ReturnDisclosureTypes.AUIndividualWorkRelatedClothingUniformNonCompulsory, 198);
            items.Add(ReturnDisclosureTypes.AUIndividualWorkRelatedClothingOccupationSpecific, 199);
            items.Add(ReturnDisclosureTypes.AUIndividualWorkRelatedClothingProtective, 200);
            items.Add(ReturnDisclosureTypes.AUIndividualWorkRelatedTravelExpenses, 77);
            items.Add(ReturnDisclosureTypes.AUIndividualOtherWorkRelatedExpenses, 80);

            items.TryGetValue(value, out int result);

            if (result == 0)
            {
                throw new Exception("Unable to map the return disclosure type");
            }

            return result;
        }
    }
}
