
using System;
using Taxlab.ApiClientLibrary;
using Taxlab.ApiClientCli.Repositories.Taxpayer;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers;
using Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers;

namespace Taxlab.ApiClientCli.Personas
{
    public class IndividualWithTrustDistribution
    {
        public async Task<TaxpayerDto> CreateAsync(TaxlabApiClient client, string firstName, string lastName, string taxFileNumber, int taxYear)
        {
            var balanceDate = new DateOnly(taxYear, 6, 30);
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
