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
            string taxpayerName = "",
            RentalPropertyInformation rentalPropertyInformation = null,
            RentalTransactionCollectionOfNumericCellAndDecimal rentalIncome = null,
            RentalTransactionCollectionOfNumericCellAndDecimal otherIncome = null,
            RentalTransactionOfDecimalAndDecimal grossIncome = null,
            RentalTransactionCollectionOfNumericCellAndDecimal advertising = null,
            RentalTransactionCollectionOfNumericCellAndDecimal bodyCorporate = null,
            RentalTransactionCollectionOfNumericCellAndDecimal borrowingExpenses = null,
            RentalTransactionCollectionOfNumericCellAndDecimal cleaning = null,
            RentalTransactionCollectionOfNumericCellAndDecimal councilRates = null,
            RentalTransactionCollectionOfNumericCellAndDecimal capitalAllowances = null,
            RentalTransactionCollectionOfNumericCellAndDecimal capitalWorks = null,
            RentalTransactionCollectionOfNumericCellAndDecimal gardening = null,
            RentalTransactionCollectionOfNumericCellAndDecimal insurance = null,
            RentalTransactionCollectionOfNumericCellAndNumericCell interestOnLoans = null,
            RentalTransactionCollectionOfNumericCellAndDecimal landTax = null,
            RentalTransactionCollectionOfNumericCellAndDecimal legalFees = null,
            RentalTransactionCollectionOfNumericCellAndDecimal pestControl = null,
            RentalTransactionCollectionOfNumericCellAndDecimal agentFees = null,
            RentalTransactionCollectionOfNumericCellAndDecimal repairsAndMaintenance = null,
            RentalTransactionCollectionOfNumericCellAndDecimal stationeryPhonePostage = null,
            RentalTransactionCollectionOfNumericCellAndDecimal travelExpenses = null,
            RentalTransactionCollectionOfNumericCellAndDecimal waterCharges = null,
            RentalTransactionCollectionOfNumericCellAndDecimal sundryExpenses = null,
            int weeksRented = 0,
            int weeksAvailableForRent = 0,
            decimal ownershipPercentageMine = 0m,
            decimal ownershipPercentageTotal = 0m)
        {
            var createRentalPropertyCommand = new CreateRentalPropertyWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
            };
            
            var createRentalPropertyResponse = await Client.Workpapers_CreateRentalPropertyWorkpaperAsync(createRentalPropertyCommand)
                .ConfigureAwait(false);

            var getWorkpaperResponse = await GetRentalPropertyWorkpaperAsync(taxpayerId, taxYear, createRentalPropertyResponse);

            var workpaper = getWorkpaperResponse.Workpaper;
            workpaper.RentalPropertyInformation = rentalPropertyInformation;
            workpaper.RentalPropertyIncomeExpenses = new RentalPropertyIncomeExpenses
            {
                TaxpayerId = taxpayerId,
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
                DocumentIndexId = getWorkpaperResponse.DocumentIndexId,
                CompositeRequest = true,
                WorkpaperType = WorkpaperType.RentalPropertyWorkpaper,
                Workpaper = getWorkpaperResponse.Workpaper
            };

            var upsertResponse = await Client.Workpapers_PostRentalPropertyWorkpaperAsync(upsertCommand)
                .ConfigureAwait(false);

            return upsertResponse;
        }

        public async Task<DeleteRentalPropertyWorkpaperResponse> DeleteAsync(Guid taxpayerId, int taxYear, Guid documentId)
        {
            // Delete command for our workpaper
            var deleteCommand = new DeleteRentalPropertyWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentId = documentId
            };

            var deleteResponse = await Client
              .Workpapers_DeleteRentalPropertyWorkpaperAsync(deleteCommand)
              .ConfigureAwait(false);

            return deleteResponse;
        }

        public async Task<WorkpaperResponseOfRentalPropertyWorkpaper> GetRentalPropertyWorkpaperAsync(Guid taxpayerId, int taxYear, CreateRentalPropertyWorkpaperResponse createRentalPropertyResponse)
        {
            var workpaperResponse = await Client
                .Workpapers_GetRentalPropertyWorkpaperAsync(taxpayerId, taxYear, createRentalPropertyResponse.DocumentId)
                .ConfigureAwait(false);

            return workpaperResponse;
        }
    }
}