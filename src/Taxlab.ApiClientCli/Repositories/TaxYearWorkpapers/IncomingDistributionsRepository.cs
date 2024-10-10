using System;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers
{
    public class IncomingDistributionsRepository : RepositoryBase
    {
        public IncomingDistributionsRepository(TaxlabApiClient client) : base(client)
        {

        }

        public async Task<WorkpaperResponseOfIncomingDistributionsWorkpaper> GetAsync(
            Guid taxpayerId,
            int taxYear)
        {
            var workpaperResponse = await Client
                .Workpapers_GetIncomingDistributionsWorkpaperAsync(taxpayerId, taxYear)
                .ConfigureAwait(false);

            return workpaperResponse;
        }

        public async Task<WorkpaperResponseOfIncomingDistributionsWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            decimal shareOfIncomePrimaryProduction = 0m,
            decimal shareOfNonPrimaryProductionIncome = 0m,
            decimal frankedDistributions = 0m,            
            decimal attributedForeignIncomeListedCountry = 0m,
            decimal attributedForeignIncomeUnlistedCountry = 0m,
            decimal otherAssessableForeignIncome = 0m,
            decimal frankingCredit = 0m,
            decimal australianFrankingCreditsFromNewZealandCompany = 0m,
            decimal netCapitalGain = 0m,
            decimal foreignIncomeTaxOffset = 0m,                        
            decimal shareOfNationalRentalAffordabilitySchemeTaxOffset = 0m,
            decimal shareOfCreditForTaxWithheldForeignResidentWithholdingExcludingCapitalGains = 0m,
            decimal tfnAmountWithheld = 0m,
            decimal shareOfCreditForTfnAmountsWithheldFromPaymentsFromCloselyHeldTrusts = 0m,
            decimal creditForTaxWithheldWhereAbnNotQuoted = 0m,
            decimal shareOfCreditForTaxPaidByTrustee = 0m,
            decimal shareOfNetSmallBusinessIncome = 0m,
            decimal shareOfNetFinancialInvestmentIncomeOrLoss = 0m,
            decimal shareOfNetRentalPropertyIncomeOrLoss = 0m,
            decimal s983AssessableAmount = 0m,
            decimal s984AssessableAmount = 0m,
            decimal amountOnWhichFamilyTrustDistributionTaxHasBeenPaid = 0m,
            decimal deductionsAgainstFrankedDistributionsFromTrust = 0m,
            decimal deductionsAgainstPrimaryProductionFromTrust = 0m,
            decimal deductionsAgainstNonPrimaryProductionFromTrust = 0m,
            decimal landcareOperationsWaterFacilityFencingAssetAndFodderStorageDeductions = 0m,
            decimal deferredNonCommercialLossDeductionPrimaryProduction = 0m,
            decimal otherDeductionsPrimaryProduction = 0m,
            decimal deductionsRelatingToFinancialInvestmentAmounts = 0m,
            decimal deductionsRelatingToRentalProperties = 0m,
            decimal deferredNonCommercialLossDeductionNonPrimaryProduction = 0m,
            decimal otherDeductionsNonPrimaryProduction = 0m            
        )
        {
            var workpaperResponse = await Client
                .Workpapers_GetIncomingDistributionsWorkpaperAsync(taxpayerId, taxYear)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.Distributions.Add(new IncomingDistribution()
            {           
                ShareOfIncomePrimaryProduction = shareOfIncomePrimaryProduction.ToNumericCell(),
                ShareOfIncomeNonPrimaryProduction = shareOfNonPrimaryProductionIncome.ToNumericCell(),
                FrankedDistributions = frankedDistributions.ToNumericCell(),             
                AttributedForeignIncomeListedCountry = attributedForeignIncomeListedCountry.ToNumericCell(),
                AttributedForeignIncomeUnlistedCountry = attributedForeignIncomeUnlistedCountry.ToNumericCell(),
                OtherAssessableForeignSourceIncome = otherAssessableForeignIncome.ToNumericCell(),
                FrankingCredit = frankingCredit.ToNumericCell(),
                AustralianFrankingCreditsFromNewZealandCompany = australianFrankingCreditsFromNewZealandCompany.ToNumericCell(),
                NetCapitalGain = netCapitalGain.ToNumericCell(),
                ForeignIncomeTaxOffset = foreignIncomeTaxOffset.ToNumericCell(),          
                ShareOfNationalRentalAffordabilitySchemeTaxOffset = shareOfNationalRentalAffordabilitySchemeTaxOffset.ToNumericCell(),               
                ShareOfCreditForTaxWithheldForeignResidentWithholdingExcludingCapitalGains = shareOfCreditForTaxWithheldForeignResidentWithholdingExcludingCapitalGains.ToNumericCell(),
                TfnAmountsWithheld = tfnAmountWithheld.ToNumericCell(),
                ShareOfCreditForTfnAmountsWithheldFromPaymentsFromCloselyHeldTrusts = shareOfCreditForTfnAmountsWithheldFromPaymentsFromCloselyHeldTrusts.ToNumericCell(),
                CreditForTaxWithheldWhereAbnNotQuoted = creditForTaxWithheldWhereAbnNotQuoted.ToNumericCell(),     
                ShareOfCreditForTaxPaidByTrustee = shareOfCreditForTaxPaidByTrustee.ToNumericCell(),
                ShareOfNetSmallBusinessIncome = shareOfNetSmallBusinessIncome.ToNumericCell(),
                ShareOfNetFinancialInvestmentIncomeOrLoss = shareOfNetFinancialInvestmentIncomeOrLoss.ToNumericCell(),
                ShareOfNetRentalPropertyIncomeOrLoss = shareOfNetRentalPropertyIncomeOrLoss.ToNumericCell(),
                S983AssessableAmount = s983AssessableAmount.ToNumericCell(),
                S984AssessableAmount = s984AssessableAmount.ToNumericCell(),
                AmountOnWhichFamilyTrustDistributionTaxHasBeenPaid = amountOnWhichFamilyTrustDistributionTaxHasBeenPaid.ToNumericCell(),
                DeductionsAgainstFrankedDistributionsFromTrust = deductionsAgainstFrankedDistributionsFromTrust.ToNumericCell(),
                DeductionsAgainstPrimaryProductionIncomeFromTrust = deductionsAgainstPrimaryProductionFromTrust.ToNumericCell(),
                DeductionsAgainstNonPrimaryProductionIncomeFromTrust = deductionsAgainstNonPrimaryProductionFromTrust.ToNumericCell(),
                LandcareOperationsWaterFacilityFencingAssetAndFodderStorageDeductions = landcareOperationsWaterFacilityFencingAssetAndFodderStorageDeductions.ToNumericCell(),
                DeferredNonCommercialLossDeductionPrimaryProduction = deferredNonCommercialLossDeductionPrimaryProduction.ToNumericCell(),
                OtherDeductionsPrimaryProduction = otherDeductionsPrimaryProduction.ToNumericCell(),
                DeductionsRelatingToFinancialInvestmentAmounts = deductionsRelatingToFinancialInvestmentAmounts.ToNumericCell(),
                DeductionsRelatingToRentalProperties = deductionsRelatingToRentalProperties.ToNumericCell(),
                DeferredNonCommercialLossDeductionNonPrimaryProduction = deferredNonCommercialLossDeductionNonPrimaryProduction.ToNumericCell(),
                OtherDeductionsNonPrimaryProduction = otherDeductionsNonPrimaryProduction.ToNumericCell()
            });

            // Update command for our new workpaper
            var upsertCommand = new UpsertIncomingDistributionsWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = workpaperResponse.DocumentIndexId,
                CompositeRequest = true,
                Workpaper = workpaperResponse.Workpaper
            };

            var upsertResponse = await Client.Workpapers_UpsertIncomingDistributionsWorkpaperAsync(upsertCommand)
                .ConfigureAwait(false);

            return upsertResponse;
        }
    }
}
