
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Implementations;

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Taxlab.ApiClientCli.Implementations;
using Taxlab.ApiClientCli.Repositories.Taxpayer;
using Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers;
using Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers;
using Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers;
using Taxlab.ApiClientLibrary;
using Xunit;
using Xunit.Abstractions;
using Taxlab.ApiClientCli.Repositories.AdjustmentWorkpapers;

namespace Taxlab.ApiClientCli.Personas
{

    public class TaxpayerAllWorkpapersNITR : BaseScopedTests
    {
        private TaxlabApiClient Client;

        //set your test taxpayer values here
        private const string FirstName = "Notindividual";
        private const string LastName = "ZeyCompany";
        private const string TaxFileNumber = "32989432";
        private const EntityType TaxpayerEntity = EntityType.CompanyAU;
        private const int TaxYear = 2021;
        private  DateOnly _balanceDate = new DateOnly(2021, 6, 30);
        private TaxpayerDto _taxpayer;

        public TaxpayerAllWorkpapersNITR(ITestOutputHelper output) : base(output)
        {

        }

        private async Task<TaxpayerResponse> CreateTaxpayer(TaxlabApiClient Client)
        {
            var taxpayerService = new TaxpayerRepository(Client);

            var taxpayerResponse = await taxpayerService.CreateAsync(TaxYear,
                FirstName,
                LastName,
                TaxFileNumber,
                TaxpayerEntity);

            var taxpayer = taxpayerResponse.Content;
            Client.TaxpayerId = taxpayer.Id;
            Client.Taxyear = TaxYear;
            Client.TaxpayerEntity = TaxpayerEntity;

            var taxReturnRepository = new TaxReturnRepository(Client);
            var taxReturnResponse = await taxReturnRepository.CreateAsync(taxpayer.Id,
                TaxYear,
                _balanceDate,
                _balanceDate.AddYears(-1).AddDays(1));


            if (taxReturnResponse.Success == false)
            {
                throw new Exception(taxReturnResponse.Message);
            }

            var details = new TaxpayerDetailsRepository(Client);
            await details.CreateAsync(taxpayer.Id,
                TaxYear,
                dateOfBirth: new DateOnly(1975, 4, 12),
                dateOfDeath: new DateOnly(2020, 12, 31),
                finalReturn: true,
                mobilePhoneNumber: "0402698741",
                daytimeAreaPhoneCode: "613",
                daytimePhoneNumber: "54835123",
                emailAddress: "JohnC12@hotmail.com",
                bsbNumber: "553026",
                bankAccountName: "Bank of Melbourne",
                bankAccountNumber: "15987456",
                higherEducationLoanProgramBalance: 100,
                vetStudentLoanBalance: 200,
                studentFinancialSupplementSchemeBalance: 300,
                studentStartupLoanBalance: 400,
                abstudyStudentStartupLoanBalance: 500,
                tradeSupportLoanBalance: 600,
                smallBusinessIndicator: false
            );

            return taxpayerResponse;
        }


        protected override async Task SetupTest()
        {
            Client = TestSetup.GetTaxlabApiClient();
            var taxpayerResponse = await CreateTaxpayer(Client);
            _taxpayer = taxpayerResponse.Content;
        }

        [Fact]
        public async void NITRWorkpapersInRepositoriesTest()
        {
            CreateTaxpayerTest();
            await DepreciatingAssetsDisposalsWorkpaperTest();
            await DepreciatingAssetsFirstDeductedWorkpaperTest();
            await DepreciationWorkpaperTest();
            await DividendIncomeNonIndividualWorkpaperTest();
            await ExpensesCapitalisedForTaxWorkpaperTest();
            await ForeignDeductionNonIndividualWorkpaperTest();
            await ForeignIncomeNonIndividualWorkpaperTest();
            await DepreciationWorkpaperTest();
            await PermanentDifferenceWorkpaperTest();
            await TemporaryDifferenceWorkpaperTest();
            
            await CapitalGainsWorkpaperTest();
            await ForeignIncomeTaxOffsetsWorkpaperTest();
            await LossesScheduleWorkpaperTest();
            await TaxOnTaxableIncomeWorkpaperTest();
        }

        [Fact]
        public void CreateTaxpayerTest()
        {
            Assert.Equal($"{FirstName}", _taxpayer.TaxpayerName);
        }

        [Fact]
        public async Task DividendIncomeNonIndividualWorkpaperTest()
        {
            var dividendIncomeNonIndividualRepository = new DividendIncomeNonIndividualRepository(Client);
            var workpaperResponseOfDividendIncomeNonIndividualWorkpaper = await dividendIncomeNonIndividualRepository.CreateAsync(
                _taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfDividendIncomeNonIndividualWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task DepreciatingAssetsDisposalsWorkpaperTest()
        {
            var depreciatingAssetsDisposalsRepository = new DepreciatingAssetsDisposalsRepository(Client);
            var workpaperResponseOfDepreciatingAssetsDisposalsWorkpaper = await depreciatingAssetsDisposalsRepository.CreateAsync(
                _taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfDepreciatingAssetsDisposalsWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task DepreciatingAssetsFirstDeductedWorkpaperTest()
        {
            var depreciatingAssetsFirstDeductedRepository = new DepreciatingAssetsFirstDeductedRepository(Client);
            var workpaperResponseOfDepreciatingAssetsFirstDeductedWorkpaper = await depreciatingAssetsFirstDeductedRepository.CreateAsync(
                _taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfDepreciatingAssetsFirstDeductedWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task DepreciationWorkpaperTest()
        {
            var depreciationRepository = new DepreciationRepository(Client);
            var workpaperResponseOfDepreciationWorkpaper = await depreciationRepository.CreateAsync(
                _taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfDepreciationWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task ExpensesCapitalisedForTaxWorkpaperTest()
        {
            var expensesCapitalisedForTaxRepository = new ExpensesCapitalisedForTaxRepository(Client);
            var workpaperResponseOfExpensesCapitalisedForTaxWorkpaper = await expensesCapitalisedForTaxRepository.CreateAsync(
                _taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfExpensesCapitalisedForTaxWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task ForeignDeductionNonIndividualWorkpaperTest()
        {
            var foreignDeductionNonIndividualRepository = new ForeignDeductionNonIndividualRepository(Client);
            var workpaperResponseOfForeignDeductionNonIndividualWorkpaper = await foreignDeductionNonIndividualRepository.CreateAsync(
                _taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfForeignDeductionNonIndividualWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task ForeignIncomeNonIndividualWorkpaperTest()
        {
            var foreignIncomeNonIndividualRepository = new ForeignIncomeNonIndividualRepository(Client);
            var workpaperResponseOfForeignIncomeNonIndividualWorkpaper = await foreignIncomeNonIndividualRepository.CreateAsync(
                _taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfForeignIncomeNonIndividualWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task InterestIncomeNonIndividualWorkpaperTest()
        {
            var interestIncomeNonIndividualRepository = new InterestIncomeNonIndividualRepository(Client);
            var workpaperResponseOfForeignIncomeNonIndividualWorkpaper = await interestIncomeNonIndividualRepository.CreateAsync(
                _taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfForeignIncomeNonIndividualWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task PermanentDifferenceWorkpaperTest()
        {
            var permanentDifferenceRepository = new PermanentDifferenceRepository(Client);
            var workpaperResponseOfPermanentDifferenceWorkpaper = await permanentDifferenceRepository.CreateAsync(
                _taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfPermanentDifferenceWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task TemporaryDifferenceWorkpaperTest()
        {
            var temporaryDifferenceRepository = new TemporaryDifferenceRepository(Client);
            var workpaperResponseOfTemporaryDifferenceWorkpaperWorkpaper = await temporaryDifferenceRepository.CreateAsync(
                _taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfTemporaryDifferenceWorkpaperWorkpaper.Workpaper.Slug.TaxpayerId);
        }
        
        [Fact]
        public async Task CapitalGainsWorkpaperTest()
        {
            var repository = new CapitalGainsRepository(Client);
            var response = await repository.GetAndUpsertAsync(_taxpayer.Id, TaxYear);

            Assert.NotNull(response.Workpaper);
            Assert.True(response.Success);
            Assert.Equal(_taxpayer.Id, response.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task ForeignIncomeTaxOffsetsWorkpaperTest()
        {
            var repository = new ForeignIncomeTaxOffsetsRepository(Client);
            var response = await repository.GetAndUpsertAsync(_taxpayer.Id, TaxYear);

            Assert.NotNull(response.Workpaper);
            Assert.True(response.Success);
            Assert.Equal(_taxpayer.Id, response.Workpaper.Slug.TaxpayerId);
        }
        
        [Fact]
        public async Task LossesScheduleWorkpaperTest()
        {
            var repository = new LossesScheduleRepository(Client);
            var response = await repository.GetAndUpsertAsync(_taxpayer.Id, TaxYear);

            Assert.NotNull(response.Workpaper);
            Assert.True(response.Success);
            Assert.Equal(_taxpayer.Id, response.Workpaper.Slug.TaxpayerId);
        }
        
        [Fact]
        public async Task TaxOnTaxableIncomeWorkpaperTest()
        {
            var repository = new TaxOnTaxableIncomeRepository(Client);
            var response = await repository.GetAsync(_taxpayer.Id, TaxYear);

            Assert.NotNull(response.Workpaper);
            Assert.True(response.Success);
            Assert.Equal(_taxpayer.Id, response.Workpaper.Slug.TaxpayerId);
        }
    }
}
