using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class RentalPropertyRepository : RepositoryBase
    {
        public RentalPropertyRepository(TaxlabApiClient client) : base(client)
        {

        }

        public async Task<WorkpaperResponseOfRentalPropertyWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string taxpayerName,
            RentalPropertyInformation rentalPropertyInformation,
            RentalTransactionCollectionOfNumericCellAndDecimal rentalIncome,
            RentalTransactionCollectionOfNumericCellAndDecimal otherIncome,
            RentalTransactionOfDecimalAndDecimal grossIncome,
            RentalTransactionCollectionOfNumericCellAndDecimal advertising,
            RentalTransactionCollectionOfNumericCellAndDecimal bodyCorporate,
            RentalTransactionCollectionOfNumericCellAndDecimal borrowingExpenses,
            RentalTransactionCollectionOfNumericCellAndDecimal cleaning,
            RentalTransactionCollectionOfNumericCellAndDecimal councilRates,
            RentalTransactionCollectionOfNumericCellAndDecimal capitalAllowances,
            RentalTransactionCollectionOfNumericCellAndDecimal capitalWorks,
            RentalTransactionCollectionOfNumericCellAndDecimal gardening,
            RentalTransactionCollectionOfNumericCellAndDecimal insurance,
            RentalTransactionCollectionOfNumericCellAndNumericCell interestOnLoans,
            RentalTransactionCollectionOfNumericCellAndDecimal landTax,
            RentalTransactionCollectionOfNumericCellAndDecimal legalFees,
            RentalTransactionCollectionOfNumericCellAndDecimal pestControl,
            RentalTransactionCollectionOfNumericCellAndDecimal agentFees,
            RentalTransactionCollectionOfNumericCellAndDecimal repairsAndMaintenance,
            RentalTransactionCollectionOfNumericCellAndDecimal stationeryPhonePostage,
            RentalTransactionCollectionOfNumericCellAndDecimal travelExpenses,
            RentalTransactionCollectionOfNumericCellAndDecimal waterCharges,
            RentalTransactionCollectionOfNumericCellAndDecimal sundryExpenses,
            int weeksRented = 0,
            int weeksAvailableForRent = 0,
            decimal ownershipPercentageMine = 0m,
            decimal ownershipPercentageTotal = 0m)
        {
            var workpaperResponse = await Client
                .Workpapers_GetRentalPropertyWorkpaperAsync(taxpayerId, taxYear, WorkpaperType.RentalPropertyWorkpaper, Guid.Empty, false, false)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.RentalPropertyInformation = rentalPropertyInformation;
            workpaper.RentalPropertyIncomeExpenses = new RentalPropertyIncomeExpenses
            {
                TaxpayerId =  taxpayerId,
                TaxpayerName = taxpayerName,
                WeeksRented = weeksRented,
                WeeksAvailableForRent = weeksAvailableForRent,
                OwnershipPercentageMine = ownershipPercentageMine,
                OwnershipPercentageTotal = ownershipPercentageTotal,
                RentalIncome = rentalIncome ?? workpaper.RentalPropertyIncomeExpenses.RentalIncome,
                OtherIncome = otherIncome ?? workpaper.RentalPropertyIncomeExpenses.OtherIncome,
                GrossIncome = grossIncome ?? workpaper.RentalPropertyIncomeExpenses.GrossIncome,
                Advertising = advertising ?? workpaper.RentalPropertyIncomeExpenses.Advertising,
                BodyCorporate = bodyCorporate ?? workpaper.RentalPropertyIncomeExpenses.BodyCorporate,
                BorrowingExpenses = borrowingExpenses ?? workpaper.RentalPropertyIncomeExpenses.BorrowingExpenses,
                Cleaning = cleaning ?? workpaper.RentalPropertyIncomeExpenses.Cleaning,
                CouncilRates = councilRates ?? workpaper.RentalPropertyIncomeExpenses.CouncilRates,
                CapitalAllowances = capitalAllowances ?? workpaper.RentalPropertyIncomeExpenses.CapitalAllowances,
                CapitalWorks = capitalWorks ?? workpaper.RentalPropertyIncomeExpenses.CapitalWorks,
                Gardening = gardening ?? workpaper.RentalPropertyIncomeExpenses.Gardening,
                Insurance = insurance ?? workpaper.RentalPropertyIncomeExpenses.Insurance,
                InterestOnLoans = interestOnLoans ?? workpaper.RentalPropertyIncomeExpenses.InterestOnLoans,
                LandTax = landTax ?? workpaper.RentalPropertyIncomeExpenses.LandTax,
                LegalFees = legalFees ?? workpaper.RentalPropertyIncomeExpenses.LegalFees,
                PestControl = pestControl ?? workpaper.RentalPropertyIncomeExpenses.PestControl,
                AgentFees = agentFees ?? workpaper.RentalPropertyIncomeExpenses.AgentFees,
                RepairsAndMaintenance = repairsAndMaintenance ?? workpaper.RentalPropertyIncomeExpenses.RepairsAndMaintenance,
                StationeryPhonePostage = stationeryPhonePostage ?? workpaper.RentalPropertyIncomeExpenses.StationeryPhonePostage,
                TravelExpenses = travelExpenses ?? workpaper.RentalPropertyIncomeExpenses.TravelExpenses,
                WaterCharges = waterCharges ?? workpaper.RentalPropertyIncomeExpenses.WaterCharges,
                SundryExpenses = sundryExpenses ?? workpaper.RentalPropertyIncomeExpenses.SundryExpenses,
                NetRent = workpaper.RentalPropertyIncomeExpenses.NetRent,
                OtherDeductions = workpaper.RentalPropertyIncomeExpenses.OtherDeductions,
                TotalDeductions = workpaper.RentalPropertyIncomeExpenses.TotalDeductions
            };       

            // Update command for our new workpaper
            var upsertCommand = new UpsertRentalPropertyWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = workpaperResponse.DocumentIndexId,
                CompositeRequest = true,
                WorkpaperType = WorkpaperType.DeclarationsWorkpaper,
                Workpaper = workpaperResponse.Workpaper
            };

            var upsertResponse = await Client.Workpapers_PostRentalPropertyWorkpaperAsync(upsertCommand)
                .ConfigureAwait(false);

            return upsertResponse;
        }

        public async Task<WorkpaperResponseOfRentalPropertiesSummaryWorkpaper> GetRentalSummaryWorkpaperAsync(Guid taxpayerId, int taxYear)
        {
            var workpaperResponse = await Client
              .Workpapers_GetRentalPropertiesSummaryWorkpaperAsync(taxpayerId, taxYear, WorkpaperType.RentalPropertiesSummaryWorkpaper, Guid.Empty, false, false)
              .ConfigureAwait(false);

            return workpaperResponse;
        }
    }
}