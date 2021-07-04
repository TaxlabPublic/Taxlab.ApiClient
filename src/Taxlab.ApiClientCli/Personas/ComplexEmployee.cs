using NodaTime;
using System;
using System.Threading.Tasks;
using Taxlab.ApiClientLibrary;
using Taxlab.ApiClientCli.Repositories.Taxpayer;
using Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers;
using Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers;

namespace Taxlab.ApiClientCli.Personas
{
    public class ComplexEmployee
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
                dateOfBirth: new LocalDate(1975, 4, 12),
                dateOfDeath: new LocalDate(2020, 12, 31),
                finalReturn: true,
                mobilePhoneNumber: "0402698741",
                daytimeAreaPhoneCode: "613",
                daytimePhoneNumber: "54835123",
                emailAddress: "JohnC12@hotmail.com",
                bsbNumber: "553026",
                bankAccountName: "Bank of Melbourne",
                bankAccountNumber: "15987456"
            );

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
                ReturnDisclosureTypes.OtherDeductibleExpenses
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

            return taxpayer;
        }
    }
}
