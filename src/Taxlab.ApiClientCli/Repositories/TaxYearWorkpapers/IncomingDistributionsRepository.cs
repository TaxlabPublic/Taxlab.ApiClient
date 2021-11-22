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
            decimal shareOfIncomeOfTheTrustEstate = 0m,
            decimal shareOfNonPrimaryProductionIncome = 0m,
            decimal frankedDistributions = 0m,
            decimal atrributedForeignIncome = 0m,
            decimal otherAssessableForeignIncome = 0m,
            decimal frankingCredit = 0m,
            decimal australianFrankingCreditsFromANewZealandCompany = 0m,
            decimal capitalGains = 0m,
            decimal foreignIncomeTaxOffset = 0m,
            decimal earlyStageVentureCapitalLimitedPartnershipTaxOffset = 0m,
            decimal earlyStageInvestorTaxOffset = 0m,
            decimal shareOfNationalRentalAffordabilitySchemeTaxOffset = 0m,
            decimal explorationCreditsDistributed = 0m,
            decimal shareOfCreditForTaxWithheldForeignResidentWithholdingExcludingCapitalGains = 0m,
            decimal tfnAmountWithheld = 0m,
            decimal shareOfCreditForTfnAmountsWithheldFromPaymentsFromCloselyHeldTrusts = 0m,
            decimal creditForTaxWithheldWhereAbnNotQuoted = 0m,
            decimal shareOfCreditForForeignResidentCapitalGainsWithholdingAmounts = 0m,
            decimal shareOfNetSmallBusinessIncome = 0m,
            decimal shareOfNetFinancialInvestmentIncomeOrLoss = 0m,
            decimal shareOfNetRentalPropertyIncomeOrLoss = 0m,
            decimal s983AssessableAmount = 0m,
            decimal s984AssessableAmount = 0m,
            decimal taxPreferredAmountsTbStatementInformation = 0m,
            decimal untaxedPartOfShareOfNetIncomeTbStatementInformation = 0m,
            decimal distributionFromOrdinaryOrStatutoryIncomeDuringIncomeYear = 0m,
            decimal totalTfnAmountsWithheldFromPayments = 0m
        )
        {
            var workpaperResponse = await Client
                .Workpapers_GetIncomingDistributionsWorkpaperAsync(taxpayerId, taxYear, WorkpaperType.TaxpayerDetailsWorkpaper, Guid.Empty, false, false)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.Distributions.Add(new IncomingDistribution()
            {
                ShareOfIncomeOfTheTrustEstate = shareOfIncomeOfTheTrustEstate,
                ShareOfIncomeNonPrimaryProduction = shareOfNonPrimaryProductionIncome,
                FrankedDistributions = frankedDistributions,
                AttributedForeignIncome = atrributedForeignIncome,
                OtherAssessableForeignSourceIncome = otherAssessableForeignIncome,
                FrankingCredit = frankingCredit,
                AustralianFrankingCreditsFromANewZealandCompany = australianFrankingCreditsFromANewZealandCompany,
                CapitalGains = capitalGains,
                ForeignIncomeTaxOffset = foreignIncomeTaxOffset,
                EarlyStageVentureCapitalLimitedPartnershipTaxOffset = earlyStageVentureCapitalLimitedPartnershipTaxOffset,
                EarlyStageInvestorTaxOffset = earlyStageInvestorTaxOffset,
                ShareOfNationalRentalAffordabilitySchemeTaxOffset = shareOfNationalRentalAffordabilitySchemeTaxOffset,
                ExplorationCreditsDistributed = explorationCreditsDistributed,
                ShareOfCreditForTaxWithheldForeignResidentWithholdingExcludingCapitalGains = shareOfCreditForTaxWithheldForeignResidentWithholdingExcludingCapitalGains,
                TfnAmountsWithheld = tfnAmountWithheld,
                ShareOfCreditForTfnAmountsWithheldFromPaymentsFromCloselyHeldTrusts = shareOfCreditForTfnAmountsWithheldFromPaymentsFromCloselyHeldTrusts,
                CreditForTaxWithheldWhereAbnNotQuoted = creditForTaxWithheldWhereAbnNotQuoted,
                ShareOfCreditForForeignResidentCapitalGainsWithholdingAmounts = shareOfCreditForForeignResidentCapitalGainsWithholdingAmounts,
                ShareOfNetSmallBusinessIncome = shareOfNetSmallBusinessIncome,
                ShareOfNetFinancialInvestmentIncomeOrLoss = shareOfNetFinancialInvestmentIncomeOrLoss,
                ShareOfNetRentalPropertyIncomeOrLoss = shareOfNetRentalPropertyIncomeOrLoss,
                S983AssessableAmount = s983AssessableAmount,
                S984AssessableAmount = s984AssessableAmount,
                TaxPreferredAmountsTbStatementInformation = taxPreferredAmountsTbStatementInformation,
                UntaxedPartOfShareOfNetIncomeTbStatementInformation = untaxedPartOfShareOfNetIncomeTbStatementInformation,
                DistributionFromOrdinaryOrStatutoryIncomeDuringIncomeYear = distributionFromOrdinaryOrStatutoryIncomeDuringIncomeYear,
                TotalTfnAmountsWithheldFromPayments = totalTfnAmountsWithheldFromPayments
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
