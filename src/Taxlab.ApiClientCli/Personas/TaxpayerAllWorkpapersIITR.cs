using NodaTime;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Implementations;
using Taxlab.ApiClientCli.Repositories.Taxpayer;
using Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers;
using Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers;
using Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers;
using Taxlab.ApiClientLibrary;
using Xunit;

namespace Taxlab.ApiClientCli.Personas
{
    public class TaxpayerAllWorkpapersIITR
    {
        private TaxlabApiClient Client;

        //set your test taxpayer values here
        private const string FirstName = "IndividualThree";
        private const string LastName = "Zey";
        private const string TaxFileNumber = "32989432";
        private const EntityType TaxpayerEntity = EntityType.IndividualAU;
        private const int TaxYear = 2021;
        private LocalDate _balanceDate = new LocalDate(2021, 6, 30);

        [Fact]
        public async void TestAllWorkpapers()
        {
            Client = TestSetup.GetTaxlabApiClient();
            var taxpayerResponse = await CreateTaxpayer(Client);
            var taxpayer = taxpayerResponse.Content;

            var lookups = new LookupsRepository(Client);
            var lookupsArray = await lookups.GetAllLookups();

            ICollection<BaseLookupMapperItem> countryLookup = new List<BaseLookupMapperItem>();

            var countryListFetched = lookupsArray.Lookups.TryGetValue("Common/RegionLookups/CountryCodes", out countryLookup);

            var annuityWorkpaperFactory = new AnnuityRepository(Client);
            await annuityWorkpaperFactory.CreateAsync(taxpayer.Id,
                TaxYear,
                payersName: $"{FirstName} {LastName}",
                abn: "3874923",
                grossIncome: 1000m,
                deductibleAmount: 300m,
                taxPaid: 100m);

            var attributedPersonalServicesIncomeWorkpaperFactory = new AttributedPersonalServicesIncomeRepository(Client);
            await attributedPersonalServicesIncomeWorkpaperFactory.CreateAsync(taxpayer.Id,
                TaxYear,
                payersName: $"{FirstName} {LastName}",
                abn: "3874923",
                grossIncome: 1000m,
                taxPaid: 300m,
                reportableSuperContributions: 100m);

            var deductibleCarExpenseWorkpaperFactory = new DeductibleCarExpenseRepository(Client);
            await deductibleCarExpenseWorkpaperFactory.CreateAsync(taxpayer.Id,
                TaxYear);

            var employmentTerminationPaymentWorkpaperFactory = new EmploymentTerminationPaymentRepository(Client);
            await employmentTerminationPaymentWorkpaperFactory.CreateAsync(taxpayer.Id,
                TaxYear);

            var firstHomeSuperSaverWorkpaperFactory = new FirstHomeSuperSaverRepository(Client);
            await firstHomeSuperSaverWorkpaperFactory.CreateAsync(taxpayer.Id,
                TaxYear);

            var foreignEmploymentIncomeWorkpaperFactory = new ForeignEmploymentIncomeRepository(Client);
            await foreignEmploymentIncomeWorkpaperFactory.CreateAsync(taxpayer.Id,
                TaxYear);

            var foreignIncomeWorkpaperFactory = new ForeignIncomeRepository(Client);
            await foreignIncomeWorkpaperFactory.CreateAsync(taxpayer.Id,
                TaxYear);

            var foreignPensionOrAnnuityWorkpaperFactory = new ForeignPensionOrAnnuityRepository(Client);
            await foreignPensionOrAnnuityWorkpaperFactory.CreateAsync(taxpayer.Id,
                TaxYear);

            var otherIncomeWorkpaperFactory = new OtherIncomeRepository(Client);
            await otherIncomeWorkpaperFactory.CreateAsync(taxpayer.Id,
                TaxYear);

            var superannuationIncomeStreamWorkpaperFactory = new SuperannuationIncomeStreamRepository(Client);
            await superannuationIncomeStreamWorkpaperFactory.CreateAsync(taxpayer.Id,
                TaxYear);

            var superannuationLumpSumPaymentWorkpaperFactory = new SuperannuationLumpSumPaymentRepository(Client);
            await superannuationLumpSumPaymentWorkpaperFactory.CreateAsync(taxpayer.Id,
                TaxYear);

            var employmentIncomeWorkpaperFactory = new EmploymentIncomeRepository(Client);
            await employmentIncomeWorkpaperFactory.CreateAsync(taxpayer.Id,
                TaxYear,
                employerName: "ABC Company Limited",
                governmentIdentifier: "",
                salaryAndWagesIncome: 343434m,
                salaryAndWagesTaxWithheld: -213400m,
                allowanceIncome: 310m
            );

            var dividendWorkpaperFactory = new DividendIncomeRepository(Client);
            await dividendWorkpaperFactory.CreateAsync(taxpayer.Id,
                TaxYear,
                "ABC Company Limited",
                "123456789",
                1,
                1000m,
                200m,
                300m,
                -10m);

            var interestWorkpaperFactory = new InterestIncomeRepository(Client);
            await interestWorkpaperFactory.CreateAsync(taxpayer.Id,
                TaxYear,
                "Our financial institution name",
                "123456789",
                1,
                1000m,
                -200m);

            var personalSuperannuationContributionFactory = new PersonalSuperannuationContributionRepository(Client);
            await personalSuperannuationContributionFactory.CreateAsync(taxpayer.Id,
                TaxYear,
                "XYZ Company Limited",
                "123456789",
                "9867565423",
                "5235423423423",
                _balanceDate,
                10000m,
                true
            );

            var governmentAllowanceFactory = new GovernmentAllowanceRepository(Client);
            await governmentAllowanceFactory.CreateAsync(taxpayer.Id,
                TaxYear,
                "Allowance",
                "testDescription",
                1000m,
                1000m
            );

            var governmentPensionFactory = new GovernmentPensionRepository(Client);
            await governmentPensionFactory.CreateAsync(taxpayer.Id,
                TaxYear,
                "testDescription",
                1000m,
                1000m
            );

            var chartiableDonation = new OtherDeductionRepository(Client);
            await chartiableDonation.CreateAsync(taxpayer.Id,
                TaxYear,
                -10000m,
                ReturnDisclosureTypes.AUIndividualGiftsOrDonations
            );
            
            var expense = new OtherDeductionRepository(Client);
            await expense.CreateAsync(taxpayer.Id,
                TaxYear,
                -10000m,
                ReturnDisclosureTypes.AUDividend
            );

            var declarationsWorkpaperFactory = new DeclarationsRepository(Client);
            await declarationsWorkpaperFactory.CreateAsync(taxpayer.Id,
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

            var rentalPropertyWorkpaperFactory = new RentalPropertyRepository(Client);

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
                //Total = 216000,
                //Mine = 151200,
                RentalTransactions = new List<RentalTransactionOfNumericCellAndDecimal>
                    {
                        new RentalTransactionOfNumericCellAndDecimal {
                            Description = "Insurance",
                            Total = new NumericCell
                            {
                                //Value = 216000,
                                Formula = "700"
                            },
                            //Mine = 151200
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

            await rentalPropertyWorkpaperFactory.CreateAsync(taxpayer.Id,
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

            var medicareWorkpaperFactory = new MedicareRepository(Client);
            await medicareWorkpaperFactory.CreateAsync(taxpayer.Id,
                TaxYear);



            var allAdjustmentWorkpapers = await Client.Workpapers_AdjustmentWorkpapersAsync(taxpayer.Id, TaxYear, null)
                .ConfigureAwait(false);

            var allATaxYearWorkpapers = await Client.Workpapers_TaxYearWorkpapersAsync(taxpayer.Id, TaxYear, null)
                .ConfigureAwait(false);

            Assert.True(countryListFetched);

            
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
    }
}
