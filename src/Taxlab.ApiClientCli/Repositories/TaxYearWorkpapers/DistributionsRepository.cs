using System;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class DistributionsRepository : RepositoryBase
    {
        public DistributionsRepository(TaxlabApiClient client) : base(client)
        {

        }

        public async Task<WorkpaperResponseOfDistributionsWorkpaper> CreateAsync(
             Guid taxpayerId,
             int taxYear,
             string taxpayerName,
             string typeOfTrustCode,
             decimal shareOfIncomePrimaryProduction = 0m,
             decimal shareOfIncomeNonPrimaryProduction = 0m,
             decimal frankedDistributions = 0m,
             decimal attributedForeignIncomeListedCountry = 0m,
             decimal attributedForeignIncomeUnlistedCountry = 0m,
             decimal otherAssessableForeignSourceIncome = 0m,
             decimal capitalGains = 0m,
             decimal frankingCredit = 0m,
             decimal australianFrankingCreditsFromANewZealandCompanyCredit = 0m,
             decimal foreignIncomeTaxOffset = 0m,
             decimal shareOfNationalRentalAffordabilitySchemeTaxOffset = 0m,
             decimal shareOfCreditForTaxWithheldForeignResidentWithholdingExcludingCapitalGains = 0m,
             decimal tfnAmountsWithheld = 0m,
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
             decimal otherDeductionsNonPrimaryProduction = 0m,
             string reasonTrusteePaidTax = "",
             decimal shareOfIncomeOfTheTrustEstate = 0m,
             decimal earlyStageVentureCapitalLimitedPartnershipTaxOffset = 0m,
             decimal earlyStageInvestorTaxOffset = 0m,
             decimal explorationCreditsDistributed = 0m,
             decimal shareOfCreditForForeignResidentCapitalGainsWithholdingAmounts = 0m,
             decimal taxPreferredAmountsTbStatementInformation = 0m,
             decimal untaxedPartOfShareOfNetIncomeTbStatementInformation = 0m,
             decimal distributionFromOrdinaryOrStatutoryIncomeDuringIncomeYear = 0m,
             decimal totalTfnAmountsWithheldFromPayments = 0m,
             decimal div6AAEligibleIncome = 0m
            )
        {

            var createCommand = new CreateDistributionsWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear
            };

            var createResponse = await Client
                .Workpapers_CreateDistributionsWorkpaperAsync(createCommand)
                .ConfigureAwait(false);

            var workpaper = createResponse.Workpaper;
            workpaper.TypeOfTrustCode = typeOfTrustCode;
            workpaper.TaxpayerName = taxpayerName;
            workpaper.ShareOfIncomePrimaryProduction = shareOfIncomePrimaryProduction;
            workpaper.ShareOfIncomeNonPrimaryProduction = shareOfIncomeNonPrimaryProduction;
            workpaper.FrankedDistributions = frankedDistributions;
            workpaper.AttributedForeignIncomeListedCountry = attributedForeignIncomeListedCountry;
            workpaper.AttributedForeignIncomeUnlistedCountry = attributedForeignIncomeUnlistedCountry;
            workpaper.OtherAssessableForeignSourceIncome = otherAssessableForeignSourceIncome;
            workpaper.CapitalGains = capitalGains;
            workpaper.FrankingCredit = frankingCredit;
            workpaper.AustralianFrankingCreditsFromANewZealandCompanyCredit = australianFrankingCreditsFromANewZealandCompanyCredit;
            workpaper.ForeignIncomeTaxOffset = foreignIncomeTaxOffset;
            workpaper.ShareOfNationalRentalAffordabilitySchemeTaxOffset = shareOfNationalRentalAffordabilitySchemeTaxOffset;
            workpaper.ShareOfCreditForTaxWithheldForeignResidentWithholdingExcludingCapitalGains = shareOfCreditForTaxWithheldForeignResidentWithholdingExcludingCapitalGains;
            workpaper.TfnAmountsWithheld = tfnAmountsWithheld;
            workpaper.ShareOfCreditForTfnAmountsWithheldFromPaymentsFromCloselyHeldTrusts = shareOfCreditForTfnAmountsWithheldFromPaymentsFromCloselyHeldTrusts;
            workpaper.CreditForTaxWithheldWhereAbnNotQuoted = creditForTaxWithheldWhereAbnNotQuoted;
            workpaper.ShareOfCreditForTaxPaidByTrustee = shareOfCreditForTaxPaidByTrustee;
            workpaper.ShareOfNetSmallBusinessIncome = shareOfNetSmallBusinessIncome;
            workpaper.ShareOfNetFinancialInvestmentIncomeOrLoss = shareOfNetFinancialInvestmentIncomeOrLoss;
            workpaper.ShareOfNetRentalPropertyIncomeOrLoss = shareOfNetRentalPropertyIncomeOrLoss;
            workpaper.S983AssessableAmount = s983AssessableAmount;
            workpaper.S984AssessableAmount = s984AssessableAmount;
            workpaper.AmountOnWhichFamilyTrustDistributionTaxHasBeenPaid = amountOnWhichFamilyTrustDistributionTaxHasBeenPaid;
            workpaper.DeductionsAgainstFrankedDistributions = deductionsAgainstFrankedDistributions;
            workpaper.DeductionsAgainstPrimaryProduction = deductionsAgainstPrimaryProduction;
            workpaper.DeductionsAgainstNonPrimaryProduction = deductionsAgainstNonPrimaryProduction;
            workpaper.LandcareOperationsWaterFacilityFencingAssetAndFodderStorageDeductions = landcareOperationsWaterFacilityFencingAssetAndFodderStorageDeductions;
            workpaper.DeferredNonCommercialLossDeductionPrimaryProduction = deferredNonCommercialLossDeductionPrimaryProduction;
            workpaper.OtherDeductionsPrimaryProduction = otherDeductionsPrimaryProduction;
            workpaper.DeductionsRelatingToFinancialInvestmentAmounts = deductionsRelatingToFinancialInvestmentAmounts;
            workpaper.DeductionsRelatingToRentalProperties = deductionsRelatingToRentalProperties;
            workpaper.DeferredNonCommercialLossDeductionNonPrimaryProduction = deferredNonCommercialLossDeductionNonPrimaryProduction;
            workpaper.OtherDeductionsNonPrimaryProduction = otherDeductionsNonPrimaryProduction;
            workpaper.ReasonTrusteePaidTax = reasonTrusteePaidTax;
            workpaper.ShareOfIncomeOfTheTrustEstate = shareOfIncomeOfTheTrustEstate;
            workpaper.EarlyStageVentureCapitalLimitedPartnershipTaxOffset = earlyStageVentureCapitalLimitedPartnershipTaxOffset;
            workpaper.EarlyStageInvestorTaxOffset = earlyStageInvestorTaxOffset;
            workpaper.ExplorationCreditsDistributed = explorationCreditsDistributed;
            workpaper.ShareOfCreditForForeignResidentCapitalGainsWithholdingAmounts = shareOfCreditForForeignResidentCapitalGainsWithholdingAmounts;
            workpaper.TaxPreferredAmountsTbStatementInformation = taxPreferredAmountsTbStatementInformation;
            workpaper.UntaxedPartOfShareOfNetIncomeTbStatementInformation = untaxedPartOfShareOfNetIncomeTbStatementInformation;
            workpaper.DistributionFromOrdinaryOrStatutoryIncomeDuringIncomeYear = distributionFromOrdinaryOrStatutoryIncomeDuringIncomeYear;
            workpaper.TotalTfnAmountsWithheldFromPayments = totalTfnAmountsWithheldFromPayments;
            workpaper.Div6AAEligibleIncome = div6AAEligibleIncome;

            var command = new UpsertDistributionsWorkpaperCommand()
            {
                DocumentIndexId = createResponse.DocumentId,
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                WorkpaperType = WorkpaperType.DistributionsWorkpaper,
                Workpaper = workpaper
            };

            var upsertResponse = await Client
                .Workpapers_PostDistributionsWorkpaperAsync(command)
                .ConfigureAwait(false);

            return upsertResponse;
        }

        public async Task<WorkpaperResponseOfCapitalGainOrLossTransactionWorkpaper> GetCapitalGainOrLossTransactionWorkpaperAsync(Guid taxpayerId, int taxYear, CreateCapitalGainOrLossTransactionWorkpaperResponse createCapitalGainOrLossTransactionResponse)
        {
            var workpaperResponse = await Client
                .Workpapers_GetCapitalGainOrLossTransactionWorkpaperAsync(taxpayerId, taxYear, createCapitalGainOrLossTransactionResponse.DocumentId)
                .ConfigureAwait(false);

            return workpaperResponse;
        }
    }
}