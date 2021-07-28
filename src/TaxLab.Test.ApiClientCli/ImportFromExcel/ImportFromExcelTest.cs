using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xunit;
using System.Linq;
using System.Data;
using Spire.Xls;
using Taxlab.ApiClientCli.Implementations;
using Taxlab.ApiClientLibrary;
using System.Net.Http;
using Taxlab.ApiClientCli.Repositories.Taxpayer;
using TaxLab.Test.ApiClientCli.ImportFromExcel.Services;
using TaxLab.Test.ApiClientCli.ImportFromExcel.Models;
using Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers;
using NodaTime;
using Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers;

namespace TaxLab.Test.ApiClientCli.ImportFromExcel
{

    public class ImportFromExcelTest
    {
        private readonly ResourceFileLoader _resourceFileLoader = new ResourceFileLoader(typeof(ImportFromExcelTest));

        [Fact]
        public async void MapTaxPayerImports()
        {
            string baseUrl = "https://preview.taxlab.online/api-internal/";
            HttpClient httpclient = new HttpClient();
            var authService = new AuthService();
            TaxlabApiClient client = new TaxlabApiClient(baseUrl, httpclient, authService);

            var filename = "ImportData.xlsx";

            var importService = new ExcelImportService();
            List<TaxpayerImport> imports = importService.CreateTaxpayerFromExcelAsync(filename);

            const int taxYear = 2021;
            var balanceDate = new LocalDate(2021, 6, 30);
            var startDate = balanceDate.PlusYears(-1).PlusDays(-1);

            foreach (var import in imports)
            {
                if (import.Import)
                {
                    Console.WriteLine("== Step: Creating taxpayer ==========================================================");
                    var taxpayerService1 = new TaxpayerRepository(client);
                    var taxpayerResponse = await taxpayerService1.CreateAsync(taxYear,
                        import.TaxpayerOrFirstName,
                        import.LastName,
                        import.TaxNumber,
                        entityType: import.EntityType);

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
                        dateOfBirth: import.DateOfBirth,
                        dateOfDeath: new LocalDate(2020, 12, 31),
                        finalReturn: true,
                        mobilePhoneNumber: "0402698741",
                        daytimeAreaPhoneCode: "613",
                        daytimePhoneNumber: "54835123",
                        emailAddress: "JohnC12@hotmail.com",
                        bsbNumber: import.BsbNumber,
                        bankAccountName: import.BalanceAccountName,
                        bankAccountNumber: import.BankAccountNumber,
                        baseRateEntityIndicator: import.BaseRateEntityIndicator,
                        familyTrustElectionYear: import.FamilyTrustElectionYear,
                        familyTrustElection: import.FamilyTrustElection
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
                        taxAgentSignatureLastName: "Citizen",
                        noticesRecipientCorporateName: import.NoticesRecipientCorporateName,
                        noticesRecipientCorporateABN: import.NoticesRecipientCorporateABN,
                        noticesRecipientIndividualFirstName: import.NoticesRecipientIndividualFirstName,
                        noticesRecipientIndividualLastName: import.NoticesRecipientIndividualLastName
                    );
                }
            }
        }
    }
}
