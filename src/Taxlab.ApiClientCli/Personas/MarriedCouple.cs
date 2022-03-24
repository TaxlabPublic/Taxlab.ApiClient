using NodaTime;
using System;
using System.Threading.Tasks;
using Taxlab.ApiClientLibrary;
using Taxlab.ApiClientCli.Repositories.Taxpayer;
using Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers;
using Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers;
using System.Collections.Generic;

namespace Taxlab.ApiClientCli.Personas
{
    public class MarriedCouple
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
            var balanceDate = new LocalDate(2021, 6, 30);
            var startDate = balanceDate.PlusYears(-1).PlusDays(-1);

            Console.WriteLine("== Step: Creating taxpayer =========================================================="); 
            var taxpayerResponse = await CreateTaxpayer(client, "John", "Citizen", "32989432");
            var taxpayer = taxpayerResponse.Content;

            var spouseTaxpayerResponse = await CreateTaxpayer(client, "Mary", "Citizen", "32989432");
            var spouseTaxpayer = spouseTaxpayerResponse.Content;

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

            Console.WriteLine("== Step: Populating spouse workpaper ==========================================================");
            var spouse = new SpouseRepository(client);
            await spouse.CreateAsync(taxpayer.Id,
                taxYear,
                LinkedSpouseTaxpayerId: spouseTaxpayer.Id,
                IsMarriedFullYear: false,
                MarriedFrom: startDate,
                MarriedTo: startDate.PlusDays(100),
                HasDiedThisYear: false
            );

            return taxpayer;
        }

        private async Task<TaxpayerResponse> CreateTaxpayer(TaxlabApiClient client, string fName, string lName, string tfn)
        {
            string firstName = fName;
            string lastName = lName;
            string taxFileNumber = tfn;
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

            return taxpayerResponse;
        }
    }
}
