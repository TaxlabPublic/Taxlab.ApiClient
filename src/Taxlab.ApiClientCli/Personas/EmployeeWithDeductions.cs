using NodaTime;
using System;
using System.Threading.Tasks;
using Taxlab.ApiClientLibrary;
using Taxlab.ApiClientCli.Repositories.Taxpayer;
using Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers;
using System.Collections.Generic;

namespace Taxlab.ApiClientCli.Personas
{
    public class EmployeeWithDeductions
    {
        public async Task<TaxpayerDto> CreateAsync(TaxlabApiClient client)
        {
            const string firstName = "John";
            const string lastName = "Citizen";
            const string taxFileNumber = "32989432";
            const int taxYear = 2021;
            var balanceDate = new LocalDate(2021, 6, 30);
            var startDate = balanceDate.PlusYears(-1).PlusDays(-1);

            Console.WriteLine("== Step: Creating taxpayer ==========================================================");
            var taxpayerService = new TaxpayerRepository(client);
            var taxpayerResponse = await taxpayerService.CreateAsync(taxYear,
                firstName,
                lastName,
                taxFileNumber);

            var taxpayer = taxpayerResponse.Content;
            client.TaxpayerId = taxpayer.Id;
            client.Taxyear = taxYear;

            Console.WriteLine("== Step: Creating tax return ==========================================================");
            var taxReturnRepository = new TaxReturnRepository(client);
            var taxReturnResponse = await taxReturnRepository.CreateAsync(taxpayer.Id,
                 taxYear,
                 balanceDate,
                 startDate);

            Console.WriteLine("== Step: Creating Uniform workpaper ==========================================================");
            var deductionWorkpaper = new OtherDeductionRepository(client);
            await deductionWorkpaper.CreateAsync(taxpayer.Id,
                taxYear,
                -180m,
                ReturnDisclosureTypes.AUIndividualWorkRelatedClothingProtective,
                "Nice looking uniform"
            );

            if (taxReturnResponse.Success == false)
            {
                throw new Exception(taxReturnResponse.Message);
            }

            var listOfDeductionTypes = new Dictionary<ReturnDisclosureTypes, string>();
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualCostOfManagingTaxAffairsATOInterest, "Interest charged by the ATO");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualCostOfManagingTaxAffairsLitigation, "Litigation costs");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualCostOfManagingTaxAffairsOther, "Other expenses incurred in managing your tax affairs");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.DepreciationPool, "Deduction for project pool");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualDividendDeductions, "Dividend deductions");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.FarmingIncomeRepaymentsDeposits, "Deductible deposits");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualForestryManagement, "Forestry managed investment");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualGiftsOrDonations, "Gifts or donations");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualInterestDeductions, "Interest deductions");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualLowValuePoolDeductionOther, "Other low value pool deductions");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualLowValuePoolDeductionFinancialInvestment, "Relating to financial Investment");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualLowValuePoolDeductionRentalPool, "Relating to rental property");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUInvestmentIncomeDeduction, "Deduction relating to financial investment");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualElectionExpenses, "Election expenses");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.InsurancePremiumDeduction, "Income protection, sickness and accident insurance premiums");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.OtherDeductibleExpenses, "Other deductible expenses");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualWorkRelatedClothingUniformCompulsory, "Compulsory uniform");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualWorkRelatedClothingUniformNonCompulsory, "Non-compulsory uniform");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualWorkRelatedClothingOccupationSpecific, "Occupation-specific clothing");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualWorkRelatedClothingProtective, "Protective clothing");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualWorkRelatedTravelExpenses, "Work-related travel expenses");
            listOfDeductionTypes.Add(ReturnDisclosureTypes.AUIndividualOtherWorkRelatedExpenses, "Other work-related expenses");

            foreach (var item in listOfDeductionTypes)
            {
                await AddWorkpaper(client, taxYear, taxpayer.Id, item.Key, item.Value);
                await Task.Delay(2000);
            }

            return taxpayer;
        }

        public async Task AddWorkpaper(TaxlabApiClient client, int taxYear, Guid taxpayerId, ReturnDisclosureTypes returnDisclosureType, string workpaperDescription)
        {
            Console.WriteLine("== Step: Creating " + workpaperDescription + " workpaper ==========================================================");
            var deductionWorkpaper = new OtherDeductionRepository(client);
            await deductionWorkpaper.CreateAsync(taxpayerId,
                taxYear,
                -10000m,
                returnDisclosureType,
                workpaperDescription
            );
        }
    }
}