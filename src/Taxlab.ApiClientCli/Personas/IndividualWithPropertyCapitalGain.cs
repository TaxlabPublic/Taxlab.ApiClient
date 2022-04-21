using NodaTime;
using System;
using Taxlab.ApiClientLibrary;
using Taxlab.ApiClientCli.Repositories.Taxpayer;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers;
using Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers;

namespace Taxlab.ApiClientCli.Personas
{
    public class IndividualWithPropertyCapitalGain
    {
        public async Task<TaxpayerDto> CreateAsync(TaxlabApiClient client, string firstName, string lastName, string taxFileNumber, int taxYear)
        {
            var balanceDate = new LocalDate(taxYear, 6, 30);
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

            if (taxReturnResponse.Success == false)
            {
                throw new Exception(taxReturnResponse.Message);
            }

            Console.WriteLine("== Step: Creating Capital Gains workpaper ==========================================================");
            var rentalPropertySale = new CapitalGainOrLossTransactionRepository(client);
            await rentalPropertySale.CreateAsync(taxpayerId: taxpayer.Id,
                taxYear: taxYear,
                description: "Real estate in AU",
                category: "5",
                purchaseDate: new LocalDate(2019, 5, 1),
                purchaseAmount: 10000m,
                purchaseAdjustment: 0m,
                disposalDate: new LocalDate(2021, 5, 31),
                disposalAmount: 130000m,
                discountAmount: 0m,
                currentYearLossApplied: -1000,
                priorLossApplied: -27915.64m,
                capitalLossesTransferredInApplied: -10000,
                isEligibleForDiscount: true,
                isEligibleForActiveAssetReduction: true,
                isEligibleForRetirementExemption: true,
                retirementExemptionAmount: -5000,
                isEligibleForRolloverConcession: true,
                rolloverConcessionAmount: -3000
            );

            Console.WriteLine("== Step: Creating another Capital Gains workpaper ==========================================================");
            var shareSale = new CapitalGainOrLossTransactionRepository(client);
            await shareSale.CreateAsync(taxpayerId: taxpayer.Id,
                taxYear: taxYear,
                description: "Shares on ASX",
                category: "1",
                purchaseDate: new LocalDate(2019, 5, 1),
                purchaseAmount: 2000m,
                disposalDate: new LocalDate(2021, 5, 31),
                disposalAmount: 85896.52m,
                priorLossApplied: -1000m,
                isEligibleForDiscount: true
            );

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
