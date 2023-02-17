using System;
using System.Threading.Tasks;
using NodaTime;
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
            decimal australianFrankingCreditsFromANewZealandCompanyCredit = 0m,
            decimal capitalGains = 0m,
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
            decimal deductionsAgainstFrankedDistributions = 0m,
            decimal deductionsAgainstPrimaryProduction = 0m,
            decimal deductionsAgainstNonPrimaryProduction = 0m,
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
                ShareOfIncomePrimaryProduction = shareOfIncomePrimaryProduction,
                ShareOfIncomeNonPrimaryProduction = shareOfNonPrimaryProductionIncome,
                FrankedDistributions = frankedDistributions,             
                AttributedForeignIncomeListedCountry = attributedForeignIncomeListedCountry,
                AttributedForeignIncomeUnlistedCountry = attributedForeignIncomeUnlistedCountry,
                OtherAssessableForeignSourceIncome = otherAssessableForeignIncome,
                FrankingCredit = frankingCredit,
                AustralianFrankingCreditsFromANewZealandCompanyCredit = australianFrankingCreditsFromANewZealandCompanyCredit,
                CapitalGains = capitalGains,
                ForeignIncomeTaxOffset = foreignIncomeTaxOffset,          
                ShareOfNationalRentalAffordabilitySchemeTaxOffset = shareOfNationalRentalAffordabilitySchemeTaxOffset,               
                ShareOfCreditForTaxWithheldForeignResidentWithholdingExcludingCapitalGains = shareOfCreditForTaxWithheldForeignResidentWithholdingExcludingCapitalGains,
                TfnAmountsWithheld = tfnAmountWithheld,
                ShareOfCreditForTfnAmountsWithheldFromPaymentsFromCloselyHeldTrusts = shareOfCreditForTfnAmountsWithheldFromPaymentsFromCloselyHeldTrusts,
                CreditForTaxWithheldWhereAbnNotQuoted = creditForTaxWithheldWhereAbnNotQuoted,     
                ShareOfCreditForTaxPaidByTrustee = shareOfCreditForTaxPaidByTrustee,
                ShareOfNetSmallBusinessIncome = shareOfNetSmallBusinessIncome,
                ShareOfNetFinancialInvestmentIncomeOrLoss = shareOfNetFinancialInvestmentIncomeOrLoss,
                ShareOfNetRentalPropertyIncomeOrLoss = shareOfNetRentalPropertyIncomeOrLoss,
                S983AssessableAmount = s983AssessableAmount,
                S984AssessableAmount = s984AssessableAmount,
                AmountOnWhichFamilyTrustDistributionTaxHasBeenPaid = amountOnWhichFamilyTrustDistributionTaxHasBeenPaid,
                DeductionsAgainstFrankedDistributions = deductionsAgainstFrankedDistributions,
                DeductionsAgainstPrimaryProduction = deductionsAgainstPrimaryProduction,
                DeductionsAgainstNonPrimaryProduction = deductionsAgainstNonPrimaryProduction,
                LandcareOperationsWaterFacilityFencingAssetAndFodderStorageDeductions = landcareOperationsWaterFacilityFencingAssetAndFodderStorageDeductions,
                DeferredNonCommercialLossDeductionPrimaryProduction = deferredNonCommercialLossDeductionPrimaryProduction,
                OtherDeductionsPrimaryProduction = otherDeductionsPrimaryProduction,
                DeductionsRelatingToFinancialInvestmentAmounts = deductionsRelatingToFinancialInvestmentAmounts,
                DeductionsRelatingToRentalProperties = deductionsRelatingToRentalProperties,
                DeferredNonCommercialLossDeductionNonPrimaryProduction = deferredNonCommercialLossDeductionNonPrimaryProduction,
                OtherDeductionsNonPrimaryProduction = otherDeductionsNonPrimaryProduction
            });

            // Update command for our new workpaper
            var upsertCommand = new UpsertIncomingDistributionsWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = workpaperResponse.DocumentIndexId,
                CompositeRequest = true,
                WorkpaperType = WorkpaperType.DeclarationsWorkpaper,
                Workpaper = workpaperResponse.Workpaper
            };

            var upsertResponse = await Client.Workpapers_PostIncomingDistributionsWorkpaperAsync(upsertCommand)
                .ConfigureAwait(false);

            return upsertResponse;
        }
    }
}
