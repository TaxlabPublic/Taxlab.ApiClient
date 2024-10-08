using System;
using System.Threading.Tasks;
using Taxlab.ApiClientLibrary;
using Taxlab.ApiClientCli.Repositories.Taxpayer;
using Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers;
using Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers;
using Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers;
using System.Collections.Generic;
using System.Linq;

namespace Taxlab.ApiClientCli.Personas
{
    public class ComplexEmployee
    {
        public async Task<TaxpayerDto> CreateAsync(TaxlabApiClient client)
        {
            Console.WriteLine("== Step: Getting Country Lookups ==========================================================");
            var lookups = new LookupsRepository(client);
            var lookupsArray = await lookups.GetAllLookups();

            ICollection<BaseLookupMapperItem> countryLookup = new List<BaseLookupMapperItem>();

            var countryListFetched = lookupsArray.Lookups.TryGetValue("Common/RegionLookups/CountryCodes", out countryLookup);

            if (countryListFetched)
            {
                Console.WriteLine($"== Country Lookup count: {countryLookup.Count} fetched==========================================================");
            }
            else
            {
                Console.WriteLine($"== Check Lookup call ==========================================================");
            }

            const int taxYear = 2021;
            var balanceDate = new DateOnly(2021, 6, 30);
            var startDate = balanceDate.AddYears(-1).AddDays(1);
            const string firstName = "Johnny";
            const string lastName = "Citizen";
            const string taxFileNumber = "32989432";
            var taxpayerResponse = await CreateTaxpayer(client, firstName, lastName, taxFileNumber);
            var taxpayer = taxpayerResponse.Content;

            Console.WriteLine("== Step: Creating employment income workpaper ==========================================================");

            var employmentIncomeWorkpaperFactory = new EmploymentIncomeRepository(client);
            await employmentIncomeWorkpaperFactory.CreateAsync(taxpayer.Id,
                taxYear,
              employerName: "ABC Company Limited",
              governmentIdentifier: "",
              salaryAndWagesIncome: 343434m,
              salaryAndWagesTaxWithheld: -213400m,
              allowanceIncome: 310m
              );

            Console.WriteLine("== Step: Creating dividend income workpaper ==========================================================");

            var dividendWorkpaperFactory = new DividendIncomeRepository(client);
            await dividendWorkpaperFactory.CreateAsync(taxpayer.Id,
                taxYear,
                "ABC Company Limited",
                "123456789",
                1,
                1000m,
                200m,
                300m,
                -10m);

            Console.WriteLine("== Step: Creating interest income workpaper ==========================================================");
            var interestWorkpaperFactory = new InterestIncomeRepository(client);
            await interestWorkpaperFactory.CreateAsync(taxpayer.Id,
                taxYear,
                "Our financial institution name",
                "123456789",
                1,
                1000m,
                -200m);

            Console.WriteLine("== Step: Creating supercontribution workpaper ==========================================================");
            var personalSuperannuationContributionFactory = new PersonalSuperannuationContributionRepository(client);
            await personalSuperannuationContributionFactory.CreateAsync(taxpayer.Id,
                taxYear,
                "XYZ Company Limited",
                "123456789",
                "9867565423",
                "5235423423423",
                balanceDate,
                10000m,
                true
            );

            Console.WriteLine("== Step: Creating Government Allowance workpaper ==========================================================");
            var governmentAllowanceFactory = new GovernmentAllowanceRepository(client);
            await governmentAllowanceFactory.CreateAsync(taxpayer.Id,
                taxYear,
                "Allowance",
                "testDescription",
                1000m,
                1000m
            );

            Console.WriteLine("== Step: Creating Government Pension workpaper ==========================================================");
            var governmentPensionFactory = new GovernmentPensionRepository(client);
            await governmentPensionFactory.CreateAsync(taxpayer.Id,
                taxYear,
                "testDescription",
                1000m,
                1000m
            );

            Console.WriteLine("== Step: Creating donation deduction workpaper ==========================================================");
            var chartiableDonation = new OtherDeductionRepository(client);
            await chartiableDonation.CreateAsync(taxpayer.Id,
                taxYear,
                -10000m,
                ReturnDisclosureTypes.AUIndividualGiftsOrDonations
            );

            Console.WriteLine("== Step: Creating other deduction workpaper ==========================================================");
            var expense = new OtherDeductionRepository(client);
            await expense.CreateAsync(taxpayer.Id,
                taxYear,
                -10000m,
                ReturnDisclosureTypes.AUDividend
            );

            Console.WriteLine("== Step: Creating declarations workpaper ==========================================================");

            var declarationsWorkpaperFactory = new DeclarationsRepository(client);
            await declarationsWorkpaperFactory.CreateAsync(taxpayer.Id,
                taxYear,
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

            Console.WriteLine("== Step: Creating rental property workpaper ==========================================================");

            var rentalPropertyWorkpaperFactory = new RentalPropertyRepository(client);

            var rentalPropertyInformation = new RentalPropertyInformation
            {
                Description = "Rental property 1",
                OfficialName = null,
                AddressLine1 = "3 Savannah Ct",
                AddressLine2 = null,
                AddressSuburb = "Hillside",
                AddressState = "VIC",
                AddressPostcode = "3037",
                DateFirstEarnedIncome = new DateTimeOffset(DateTime.Parse("2017-03-15")),
                PurchaseDate = new DateTimeOffset(DateTime.Parse("2017-01-10")),
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
               taxYear,
               $"{firstName} {lastName}",
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

            Console.WriteLine("== Step: Get rental summary property workpaper ==========================================================");
            var rentalSummaryWorkpaper = await new RentalPropertiesSummaryRepository(client).GetRentalSummaryWorkpaperAsync(taxpayer.Id, taxYear).ConfigureAwait(false);

            return taxpayer;
        }

        private async Task<TaxpayerResponse> CreateTaxpayer(TaxlabApiClient client, string fName, string lName, string tfn)
        {
            string firstName = fName;
            string lastName = lName;
            string taxFileNumber = tfn;
            const int taxYear = 2021;
            var balanceDate = new DateOnly(2021, 6, 30);
            var startDate = balanceDate.AddYears(-1).AddDays(1);

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

            if (taxReturnResponse.Success == false)
            {
                throw new Exception(taxReturnResponse.Message);
            }

            Console.WriteLine("== Step: Populating taxpayer details workpaper ==========================================================");
            var details = new TaxpayerDetailsRepository(client);
            await details.CreateAsync(taxpayer.Id,
                taxYear,
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

    }
}
