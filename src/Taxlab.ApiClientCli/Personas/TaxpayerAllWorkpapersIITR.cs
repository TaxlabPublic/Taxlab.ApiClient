using NodaTime;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Taxlab.ApiClientCli.Implementations;
using Taxlab.ApiClientCli.Repositories.AdjustmentWorkpapers;
using Taxlab.ApiClientCli.Repositories.Taxpayer;
using Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers;
using Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers;
using Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers;
using Taxlab.ApiClientLibrary;
using Xunit;
using Xunit.Abstractions;

namespace Taxlab.ApiClientCli.Personas
{
    public class TaxpayerAllWorkpapersIITR  : BaseScopedTests
    {
        

        private TaxlabApiClient Client;

        //set your test taxpayer values here
        private const string FirstName = "Individual";
        private const string LastName = "Zeyb";
        private const string TaxFileNumber = "32989432";
        private const EntityType TaxpayerEntity = EntityType.IndividualAU;
        private const int TaxYear = 2021;
        private LocalDate _balanceDate = new LocalDate(2021, 6, 30);
        private TaxpayerDto _taxpayer;

        public TaxpayerAllWorkpapersIITR(ITestOutputHelper output) : base(output)
        {

        }

        private async Task<TaxpayerResponse> CreateTaxpayer(TaxlabApiClient Client)
        {
            var taxpayerService = new TaxpayerRepository(Client);

            var taxpayerResponse = await taxpayerService.CreateAsync(TaxYear,
                FirstName,
                LastName,
                TaxFileNumber);

            var taxpayer = taxpayerResponse.Content;
            Client.TaxpayerId = taxpayer.Id;
            Client.Taxyear = TaxYear;
            Client.TaxpayerEntity = TaxpayerEntity;

            var taxReturnRepository = new TaxReturnRepository(Client);
            var taxReturnResponse = await taxReturnRepository.CreateAsync(taxpayer.Id,
                TaxYear,
                _balanceDate,
                _balanceDate.PlusYears(-1).PlusDays(-1));


            if (taxReturnResponse.Success == false)
            {
                throw new Exception(taxReturnResponse.Message);
            }

            var details = new TaxpayerDetailsRepository(Client);
            await details.CreateAsync(taxpayer.Id,
                TaxYear,
                dateOfBirth: new LocalDate(1975, 4, 12),
                dateOfDeath: new LocalDate(2020, 12, 31),
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
        public async void IITRWorkpapersInRepositoriesTest()
        {
            CreateTaxpayerTest();
            await TestLookUpFetch();
            await SpouseWorkpaperTest();
            await AnnuityWorkpaperTest();
            await AttributedPersonalServicesIncomeWorkpaperTest();
            await DeductibleCarExpenseWorkpaperTest();
            await EmploymentTerminationPaymentWorkpaperTest();
            await FirstHomeSuperSaverWorkpaperWorkpaperTest();
            await ForeignEmploymentIncomeWorkpaperTest();
            await ForeignIncomeWorkpaperTest();
            await ForeignPensionOrAnnuityWorkpaperTest();
            await OtherIncomeWorkpaperTest();
            await SuperannuationIncomeStreamWorkpaperTest();
            await SuperannuationLumpSumPaymentWorkpaperTest();
            await EmploymentIncomeWorkpaperTest();
            await DividendIncomeWorkpaperTest();
            await InterestIncomeWorkpaperTest();
            await PersonalSuperannuationContributionWorkpaperTest();
            await GovernmentAllowanceWorkpaperTest();
            await GovernmentPensionWorkpaperTest();
            await OtherDeductionWorkpaperTest();
            await DeclarationsWorkpaperTest();
            await RentalPropertyWorkpaperTest();
            await MedicareWorkpaperTest();
            await BusinessIncomeExpensesWorkpaperTest();
            await DistributionsWorkpaperTest();
            await IncomingDistributionsWorkpaperTest();
            await CapitalGainOrLossTransactionWorkpaperTest();
            await DepreciatingAssetsDisposalsWorkpaperTest();
            await DepreciatingAssetsFirstDeductedWorkpaperTest();
            await DepreciationWorkpaperTest();
            await ExpensesCapitalisedForTaxWorkpaperTest();
            await ForeignEmploymentIncomeStatementWorkpaperTest();
            await SelfEducationDeductionWorkpaperTest();
            await AllAdjustmentWorkpapersFetch();
            await allTaxYearWorkpapersFetch();
        }


        [Fact]
        public void CreateTaxpayerTest()
        {
            Assert.Equal($"{FirstName}",_taxpayer.TaxpayerName); 
        }

        [Fact]
        public async Task TestLookUpFetch()
        {
            var lookups = new LookupsRepository(Client);
            var lookupsArray = await lookups.GetAllLookups();

            ICollection<BaseLookupMapperItem> countryLookup = new List<BaseLookupMapperItem>();

            var countryListFetched = lookupsArray.Lookups.TryGetValue("Common/RegionLookups/CountryCodes", out countryLookup);

            Assert.True(countryListFetched);
        }



        [Fact]
        public async Task AnnuityWorkpaperTest()
        {
            var annuityRepository = new AnnuityRepository(Client);
            var workpaperResponseOfAnnuityWorkpaper = await annuityRepository.CreateAsync(_taxpayer.Id,
                TaxYear,
                payersName: $"{FirstName} {LastName}",
                abn: "3874923",
                grossIncome: 1000m,
                deductibleAmount: 300m,
                taxPaid: 100m);

            Assert.Equal(_taxpayer.Id,workpaperResponseOfAnnuityWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task AttributedPersonalServicesIncomeWorkpaperTest()
        {
            var attributedPersonalServicesIncomeRepository = new AttributedPersonalServicesIncomeRepository(Client);
            var workpaperResponseOfAttributedPersonalServicesIncomeWorkpaper = await attributedPersonalServicesIncomeRepository.CreateAsync(_taxpayer.Id,
                TaxYear,
                payersName: $"{FirstName} {LastName}",
                abn: "3874923",
                grossIncome: 1000m,
                taxPaid: 300m,
                reportableSuperContributions: 100m);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfAttributedPersonalServicesIncomeWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task DeductibleCarExpenseWorkpaperTest()
        {
            var deductibleCarExpenseRepository = new DeductibleCarExpenseRepository(Client);
            var workpaperResponseOfDeductibleCarExpenseWorkpaper = await deductibleCarExpenseRepository.CreateAsync(_taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfDeductibleCarExpenseWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task EmploymentTerminationPaymentWorkpaperTest()
        {
            var employmentTerminationPaymentRepository = new EmploymentTerminationPaymentRepository(Client);
            var workpaperResponseOfEmploymentTerminationPaymentWorkpaper = await employmentTerminationPaymentRepository.CreateAsync(_taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfEmploymentTerminationPaymentWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task FirstHomeSuperSaverWorkpaperWorkpaperTest()
        {
            var firstHomeSuperSaverRepository = new FirstHomeSuperSaverRepository(Client);
            var workpaperResponseOfFirstHomeSuperSaverWorkpaper = await firstHomeSuperSaverRepository.CreateAsync(_taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfFirstHomeSuperSaverWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task ForeignEmploymentIncomeWorkpaperTest()
        {
            var foreignEmploymentIncomeRepository = new ForeignEmploymentIncomeRepository(Client);
            var workpaperResponseOfForeignEmploymentIncomeWorkpaper =
                await foreignEmploymentIncomeRepository.CreateAsync(_taxpayer.Id,
                    TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfForeignEmploymentIncomeWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task ForeignIncomeWorkpaperTest()
        {
            var foreignIncomeRepository = new ForeignIncomeRepository(Client);
            var workpaperResponseOfForeignIncomeWorkpaper = await foreignIncomeRepository.CreateAsync(_taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfForeignIncomeWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task ForeignPensionOrAnnuityWorkpaperTest()
        {
            var foreignPensionOrAnnuityRepository = new ForeignPensionOrAnnuityRepository(Client);
            var workpaperResponseOfForeignPensionOrAnnuityWorkpaper = await foreignPensionOrAnnuityRepository.CreateAsync(_taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfForeignPensionOrAnnuityWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task OtherIncomeWorkpaperTest()
        {
            var otherIncomeRepository = new OtherIncomeRepository(Client);
            var workpaperResponseOfOtherIncomeWorkpaper = await otherIncomeRepository.CreateAsync(_taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfOtherIncomeWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task SuperannuationIncomeStreamWorkpaperTest()
        {
            var superannuationIncomeStreamRepository = new SuperannuationIncomeStreamRepository(Client);
            var workpaperResponseOfSuperannuationIncomeStreamWorkpaper = await superannuationIncomeStreamRepository.CreateAsync(_taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfSuperannuationIncomeStreamWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task SuperannuationLumpSumPaymentWorkpaperTest()
        {
            var superannuationLumpSumPaymentRepository = new SuperannuationLumpSumPaymentRepository(Client);
            var workpaperResponseOfSuperannuationLumpSumPaymentWorkpaper = await superannuationLumpSumPaymentRepository.CreateAsync(_taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfSuperannuationLumpSumPaymentWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task EmploymentIncomeWorkpaperTest()
        {
            var employmentIncomeRepository = new EmploymentIncomeRepository(Client);
            var workpaperResponseOfEmploymentIncomeStatementWorkpaper = await employmentIncomeRepository.CreateAsync(_taxpayer.Id,
                TaxYear,
                employerName: "ABC Company Limited",
                governmentIdentifier: "",
                salaryAndWagesIncome: 343434m,
                salaryAndWagesTaxWithheld: -213400m,
                allowanceIncome: 310m
            );

            Assert.Equal(_taxpayer.Id, workpaperResponseOfEmploymentIncomeStatementWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task DividendIncomeWorkpaperTest()
        {
            var dividendIncomeRepository = new DividendIncomeRepository(Client);
            var workpaperResponseOfDividendIncomeWorkpaper = await dividendIncomeRepository.CreateAsync(_taxpayer.Id,
                TaxYear,
                "ABC Company Limited",
                "123456789",
                1,
                1000m,
                200m,
                300m,
                -10m);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfDividendIncomeWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task InterestIncomeWorkpaperTest()
        {
            var interestIncomeRepository = new InterestIncomeRepository(Client);
            var workpaperResponseOfInterestIncomeWorkpaper = await interestIncomeRepository.CreateAsync(_taxpayer.Id,
                TaxYear,
                "Our financial institution name",
                "123456789",
                1,
                1000m,
                -200m);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfInterestIncomeWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task PersonalSuperannuationContributionWorkpaperTest()
        {
            var personalSuperannuationContributionRepository = new PersonalSuperannuationContributionRepository(Client);
            var workpaperResponseOfPersonalSuperannuationContributionWorkpaper = await personalSuperannuationContributionRepository.CreateAsync(_taxpayer.Id,
                TaxYear,
                "XYZ Company Limited",
                "123456789",
                "9867565423",
                "5235423423423",
                _balanceDate,
                10000m,
                true
            );

            Assert.Equal(_taxpayer.Id, workpaperResponseOfPersonalSuperannuationContributionWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task GovernmentAllowanceWorkpaperTest()
        {
            var governmentAllowanceRepository = new GovernmentAllowanceRepository(Client);
            var workpaperResponseOfGovernmentAllowanceWorkpaper = await governmentAllowanceRepository.CreateAsync(_taxpayer.Id,
                TaxYear,
                "Allowance",
                "testDescription",
                1000m,
                1000m
            );


            Assert.Equal(_taxpayer.Id, workpaperResponseOfGovernmentAllowanceWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task GovernmentPensionWorkpaperTest()
        {
            var governmentPensionRepository = new GovernmentPensionRepository(Client);
            var workpaperResponseOfGovernmentPensionWorkpaper = await governmentPensionRepository.CreateAsync(_taxpayer.Id,
                TaxYear,
                "testDescription",
                1000m,
                1000m
            );

            Assert.Equal(_taxpayer.Id, workpaperResponseOfGovernmentPensionWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task OtherDeductionWorkpaperTest()
        {
            var otherDeductionRepository = new OtherDeductionRepository(Client);
            var workpaperResponseOfOtherDeductionsWorkpaper = await otherDeductionRepository.CreateAsync(_taxpayer.Id,
                TaxYear,
                -10000m,
                ReturnDisclosureTypes.AUIndividualGiftsOrDonations
            );

            Assert.Equal(_taxpayer.Id, workpaperResponseOfOtherDeductionsWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task DeclarationsWorkpaperTest()
        {
            var declarationsRepository = new DeclarationsRepository(Client);
            var workpaperResponseOfDeclarationsWorkpaper = await declarationsRepository.CreateAsync(_taxpayer.Id,
                TaxYear,
                taxAgentNumber: "",
                taxAgentAbn: "",
                taxAgentDeclarationStatementAccepted: true,
                taxAgentContactFirstName: "Jason",
                taxAgentContactLastName: "Campbell",
                taxAgentContactPhoneAreaCode: "03",
                taxAgentContactPhoneNumber: "88435751",
                taxAgentClientReference: "CR010011JC",
                taxPayerDeclarationStatementAccepted: true,
                taxAgentSignatureFirstName: "John",
                taxAgentSignatureLastName: "Citizen"
            );

            Assert.Equal(_taxpayer.Id, workpaperResponseOfDeclarationsWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task RentalPropertyWorkpaperTest()
        {

            var rentalPropertyRepository = new RentalPropertyRepository(Client);

            var rentalPropertyInformation = new RentalPropertyInformation
            {
                Description = "Rental property 1",
                OfficialName = null,
                AddressLine1 = "3 Savannah Ct",
                AddressLine2 = null,
                AddressSuburb = "Hillside",
                AddressState = "VIC",
                AddressPostcode = "3037",
                DateFirstEarnedIncome = "2017-03-15",
                PurchaseDate = "2017-01-10",
                PurchasePrice = new NumericCell
                {
                    Value = 4900000,
                    Formula = "4900000"
                },
                SaleDate = null,
                SalePrice = new NumericCell(),
                CapitalGainOrLossOnSale = new NumericCell(),
                CapitalAllowancesRecoupedOnSale = new NumericCell(),
                CapitalWorksRecoupedOnSale = new NumericCell(),
                Status = RentalPropertyStatus.Active,
                LoanRenegotiatedIndicator = false
            };

            var rentalIncome = new RentalTransactionCollectionOfNumericCellAndDecimal
            {
                Description = "Rental Income",
                Total = 216000,
                Mine = 151200,
                RentalTransactions = new List<RentalTransactionOfNumericCellAndDecimal>
                     {
                         new RentalTransactionOfNumericCellAndDecimal {
                             Description = "example rent",
                             Total = new NumericCell
                             {
                                 Value = 216000,
                                 Formula = "216000"
                             },
                             Mine = 151200
                         }
                     }
            };

            var otherIncome = new RentalTransactionCollectionOfNumericCellAndDecimal
            {
                Description = "Other Income",
                Total = 700,
                Mine = 490,
                RentalTransactions = new List<RentalTransactionOfNumericCellAndDecimal>
                     {
                         new RentalTransactionOfNumericCellAndDecimal {
                             Description = "example other income",
                             Total = new NumericCell
                             {
                                 Value = 700,
                                 Formula = "700"
                             },
                             Mine = 490
                         }
                     }
            };

            var insurance = new RentalTransactionCollectionOfNumericCellAndDecimal
            {
                Description = "Insurance",
                RentalTransactions = new List<RentalTransactionOfNumericCellAndDecimal>
                     {
                         new RentalTransactionOfNumericCellAndDecimal {
                             Description = "Insurance",
                             Total = new NumericCell
                             {
                                 Formula = "700"
                             },
                         }
                     }
            };

            var grossIncome = new RentalTransactionOfDecimalAndDecimal
            {
                Description = "Gross Income",
                Total = 216700,
                Mine = 151690
            };

            var interestOnLoans = new RentalTransactionCollectionOfNumericCellAndNumericCell
            {
                Description = "Interest On Loans",
                Total = -78479,
                Mine = -2480,
                RentalTransactions = new List<RentalTransactionOfNumericCellAndNumericCell>
                     {
                         new RentalTransactionOfNumericCellAndNumericCell {
                             Description = "example loan 1",
                             Total = new NumericCell
                             {
                                 Value = -48024,
                                 Formula = "-48024"
                             },
                              Mine = new NumericCell
                             {
                                 Value = 0,
                                 Formula = "0"
                             },
                         },
                         new RentalTransactionOfNumericCellAndNumericCell {
                             Description = "example loan 2",
                             Total = new NumericCell
                             {
                                 Value = -30455,
                                 Formula = "-30455"
                             },
                               Mine = new NumericCell
                             {
                                 Value = -2480,
                                 Formula = "-2480"
                             },

                         }
                     }
            };

            var workpaperResponseOfRentalPropertyWorkpaper = await rentalPropertyRepository.CreateAsync(_taxpayer.Id,
               TaxYear,
               $"{FirstName} {LastName}",
               rentalPropertyInformation,
               rentalIncome,
               otherIncome,
               grossIncome,
               null,
               null,
               null,
               null,
               null,
               null,
               null,
               null,
               insurance,
               interestOnLoans,
               null,
               null,
               null,
               null,
               null,
               null,
               null,
               null,
               null,
               0,
               0,
               0.7m,
               1m
           ).ConfigureAwait(false);

            var workpaperResponseOfRentalPropertiesSummaryWorkpaper = await new RentalPropertiesSummaryRepository(Client).GetRentalSummaryWorkpaperAsync(_taxpayer.Id, TaxYear).ConfigureAwait(false);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfRentalPropertyWorkpaper.Workpaper.Slug.TaxpayerId);
            Assert.True(workpaperResponseOfRentalPropertiesSummaryWorkpaper.Success);
        }

        [Fact]
        public async Task MedicareWorkpaperTest()
        {
            var medicareRepository = new MedicareRepository(Client);
            var workpaperResponseOfMedicareWorkpaper = await medicareRepository.CreateAsync(_taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfMedicareWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task BusinessIncomeExpensesWorkpaperTest()
        {
            var businessIncomeExpensesRepository = new BusinessIncomeExpensesRepository(Client);
            var workpaperResponseOfBusinessIncomeExpensesWorkpaper = await businessIncomeExpensesRepository.CreateAsync(_taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfBusinessIncomeExpensesWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task DistributionsWorkpaperTest()
        {
            var distributionsRepository = new DistributionsRepository(Client);
            var workpaperResponseOfDistributionsWorkpaper = await distributionsRepository.CreateAsync(taxpayerId: _taxpayer.Id,
                taxYear: TaxYear,
                taxpayerName: "Distribution From Trust ABC",
                typeOfTrustCode: "C",
                shareOfIncomeNonPrimaryProduction: 10000m,
                frankingCredit: -1000m

            );

            Assert.Equal(_taxpayer.Id, workpaperResponseOfDistributionsWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task IncomingDistributionsWorkpaperTest()
        {
            var incomingDistributionsRepository = new IncomingDistributionsRepository(Client);
            var workpaperResponseOfIncomingDistributionsWorkpaper = await incomingDistributionsRepository.CreateAsync(
                _taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfIncomingDistributionsWorkpaper.Workpaper.Slug.TaxpayerId);
        }

        [Fact]
        public async Task CapitalGainOrLossTransactionWorkpaperTest()
        {
            var capitalGainOrLossTransactionRepository = new CapitalGainOrLossTransactionRepository(Client);
            var workpaperResponseOfCapitalGainOrLossTransactionWorkpaper = await capitalGainOrLossTransactionRepository.CreateAsync(taxpayerId: _taxpayer.Id,
                taxYear: TaxYear,
                description: "Capital Gain Or Loss Transaction",
                category: "1",
                purchaseDate: null,
                purchaseAmount: 5000m,
                purchaseAdjustment: null,
                disposalDate: null,
                disposalAdjustment: null,
                disposalAmount: 10000m,
                discountAmount: 0m,
                currentYearLossesApplied: 0m,
                priorLossesApplied: 0m,
                capitalLossesTransferredInApplied: 0m,
                isEligibleForDiscount: false,
                isEligibleForActiveAssetReduction: false,
                isEligibleForRetirementExemption: false,
                retirementExemptionAmount: 0m,
                isEligibleForRolloverConcession: false,
                rolloverConcessionAmount: 0m
            );

            Assert.Equal(_taxpayer.Id, workpaperResponseOfCapitalGainOrLossTransactionWorkpaper.Workpaper.Slug.TaxpayerId);
        }


        [Fact]
        public async Task SpouseWorkpaperTest()
        {

            string firstName = "Mary";
            string lastName = "NzCitizen";
            string taxFileNumber = "329823478";
            var balanceDate = new LocalDate(2021, 6, 30);
            var startDate = balanceDate.PlusYears(-1).PlusDays(-1);

            var taxpayerService = new TaxpayerRepository(Client);
            var taxpayerResponse = await taxpayerService.CreateAsync(TaxYear,
                firstName,
                lastName,
                taxFileNumber);

            var spouseTaxpayer = taxpayerResponse.Content;
            Client.TaxpayerId = spouseTaxpayer.Id;

            var taxReturnRepository = new TaxReturnRepository(Client);
            var taxReturnResponse = await taxReturnRepository.CreateAsync(spouseTaxpayer.Id,
                 TaxYear,
                 balanceDate,
                 startDate);

            if (taxReturnResponse.Success == false)
            {
                throw new Exception(taxReturnResponse.Message);
            }

            var details = new TaxpayerDetailsRepository(Client);
            await details.CreateAsync(spouseTaxpayer.Id,
                TaxYear,
                dateOfBirth: new LocalDate(1975, 4, 12),
                dateOfDeath: new LocalDate(2020, 12, 31),
                finalReturn: true,
                mobilePhoneNumber: "0402698741",
                daytimeAreaPhoneCode: "613",
                daytimePhoneNumber: "54835123",
                emailAddress: "mary@hotmail.com",
                bsbNumber: "553026",
                bankAccountName: "Bank of Melbourne",
                bankAccountNumber: "15987456"
            );

            Client.TaxpayerId = _taxpayer.Id;
            var spouseRepository = new SpouseRepository(Client);
            var workpaperResponseOfSpouseWorkpaper = await spouseRepository.CreateAsync(_taxpayer.Id,
                TaxYear,
                LinkedSpouseTaxpayerId: spouseTaxpayer.Id,
                IsMarriedFullYear: false,
                MarriedFrom: startDate,
                MarriedTo: startDate.PlusDays(100),
                HasDiedThisYear: false
            );

            Assert.Equal(_taxpayer.Id, workpaperResponseOfSpouseWorkpaper.Workpaper.Slug.TaxpayerId);
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
        public async Task ForeignEmploymentIncomeStatementWorkpaperTest()
        {
            var foreignEmploymentIncomeStatementRepository = new ForeignEmploymentIncomeStatementRepository(Client);
            var workpaperResponseOfForeignEmploymentIncomeStatementWorkpaper = await foreignEmploymentIncomeStatementRepository.CreateAsync(
                taxpayerId : _taxpayer.Id,
                taxYear : TaxYear,
                employerName : "",
                governmentIdentifier : "",
                foreignIncomeType : "",
                grossIncome : 0m,
                taxWithheld : 0m,
                reportableFringeBenefits : 0m,
                isEmployerExemptFromFbt : TriState.Unset,
                superannuationContributions : 0m,
                paymentAIncome : 0m,
                paymentAType : "",
                paymentDIncome : 0m,
                paymentEIncome : 0m,
                paymentsInArrears : new List<Payments>(),
                paymentsInArrearsTotal : 0m,
                foreignWorkRelatedDeductions : 0m,
                netIncome : 0m,
                taxPaid : 0m,
                nonRefundableTaxOffset : 0m,
                residencyStatus : "",
                countriesOfResidence : new List<CountryOfResidence>(),
                permanentDifference : 0m);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfForeignEmploymentIncomeStatementWorkpaper.Workpaper.Slug.TaxpayerId);
        }


        [Fact]
        public async Task SelfEducationDeductionWorkpaperTest()
        {
            var selfEducationDeductionRepository = new SelfEducationDeductionRepository(Client);
            var workpaperResponseOfSelfEducationDeductionWorkpaper = await selfEducationDeductionRepository.CreateAsync(
                _taxpayer.Id,
                TaxYear);

            Assert.Equal(_taxpayer.Id, workpaperResponseOfSelfEducationDeductionWorkpaper.Workpaper.Slug.TaxpayerId);
        }


        [Fact]
        public async Task AllAdjustmentWorkpapersFetch()
        {
            var allAdjustmentWorkpapers = await Client.Workpapers_AdjustmentWorkpapersAsync(_taxpayer.Id, TaxYear, null)
                .ConfigureAwait(false);

            Assert.True(allAdjustmentWorkpapers.Success);
        }

        [Fact]
        public async Task allTaxYearWorkpapersFetch()
        {
            var allTaxYearWorkpapers = await Client.Workpapers_TaxYearWorkpapersAsync(_taxpayer.Id, TaxYear, null)
                .ConfigureAwait(false);

            Assert.True(allTaxYearWorkpapers.Success);
        }

        
    }
}
