using Newtonsoft.Json;
using NodaTime;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Implementations;
using Taxlab.ApiClientLibrary;
using TaxLab;

namespace Taxlab.ApiClientCli
{
    internal class Program
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private const string BaseUrl = "https://localhost:44359/";
        private static TaxlabApiClient Client;

        private static readonly Guid TaxpayerId = new Guid("5c6f8573-5de1-4253-a687-527e345165e9");   // Change this to your taxpayer Id
        private static readonly Guid AccountRecordId = new Guid("a0c29eb2-7533-4959-be3f-2e8c89977af2"); // Change this to your AccountRecordId
        private const int TaxYear = 2020;  // Change this to your taxYear

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Taxlab Client Cli!");

            TaxpayerDto taxPayer;
            var authService = new AuthService();
            Client = new TaxlabApiClient(BaseUrl, HttpClient, authService);


            // Get all taxpayers for this user.
            var getAllTaxpayers = await Client.Taxpayers_GetTaxpayersAsync().ConfigureAwait(false);

            //see if taxpayerName exisits in list.
            var taxPayerName = "ilyaTestFirstName1";

            //Logic to check if taxpayer exists
            var existingTaxpayer = getAllTaxpayers.TaxpayerListItems.FirstOrDefault(_ => _.TaxpayerName.Contains(taxPayerName));
            if (existingTaxpayer != null)
            {
                //upsert new Taxpayer Command.
                var newtaxpayerCommand = new UpsertTaxpayerCommand
                {
                    EntityType = EntityType.IndividualAU,
                    TaxpayerId = existingTaxpayer.Id,
                    TaxpayerOrFirstName = taxPayerName,
                    LastName = "ilyaTestLastName3",  
                    TaxFileNumber = "123123124",
                    TaxYear = 2020
                };

                var updateTaxpayerResponse = await Client.Taxpayers_PutTaxpayerAsync(newtaxpayerCommand)
                    .ConfigureAwait(false);

                taxPayer = updateTaxpayerResponse.Content;
            }
            //Create a new taxpayer if not found
            else
            {
                //upsert new Taxpayer Command.
                var newtaxpayerCommand = new UpsertTaxpayerCommand
                {
                    EntityType = EntityType.IndividualAU,
                    TaxpayerId = Guid.Empty,
                    TaxpayerOrFirstName = taxPayerName,
                    LastName = "ilyaTestLastName2",
                    TaxFileNumber = "123123123",
                    TaxYear = 2020
                };

                var newTaxpayerResponse = await Client.Taxpayers_PutTaxpayerAsync(newtaxpayerCommand)
                    .ConfigureAwait(false);

                taxPayer = newTaxpayerResponse.Content;
            }

            var balanceDate = new LocalDate(2020, 3, 1);
            var startDate = balanceDate.PlusYears(-1).PlusDays(-1);
            var newTaxReturnCommand = new UpsertTaxReturnCommand()
            {
                TaxpayerId = taxPayer.Id,
                TaxYear = TaxYear,
                BalanceDate = balanceDate.ToAtoDateString(),
                StartDate = startDate.ToAtoDateString()
            };

            var taxReturnResponse = await Client.Taxpayers_PutTaxReturnAsync(newTaxReturnCommand).ConfigureAwait(false);
            var taxReturn = taxReturnResponse.TaxReturnDetail;


            Console.WriteLine("== Step1: Get TaxpayerDetails workpaper ==========================================================");
            // To create a new empty workpaper we will call Get.
            // Get will create and return a new workpaper if it does not exist. (please use a new taxpayer here if you want to test this multiple times.)
            // We pass a empty DocumentIndexId here as an example of how you would do this.
            var taxpayerDetailsWorkpaperResponse = await Client
                .Workpapers_GetDividendIncomeWorkpaperAsync(taxPayer.Id, taxReturn.TaxYear, Guid.NewGuid());

            var jsonString = JsonConvert.SerializeObject(taxpayerDetailsWorkpaperResponse.Workpaper, Formatting.Indented);
            Console.Write(jsonString);

            // We are updating BankAccountName property on our new Workpaper.
            // Post below will upsert this workpaper with our new property.
            //taxpayerDetailsWorkpaperResponse.Workpaper.BankAccountName = "Test bank Account Name";
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("== Step 2: Post TaxpayerDetails Workpaper ==========================================================");
            // Update command for our new workpaper
            var upsertTaxpayerDetailsCommand = new UpsertTaxpayerDetailsWorkpaperCommand()
            {
                TaxpayerId = TaxpayerId,
                TaxYear = TaxYear,
                DocumentIndexId = taxpayerDetailsWorkpaperResponse.DocumentIndexId,
                CompositeRequest = true,
                WorkpaperType = WorkpaperType.TaxpayerDetailsWorkpaper,
                // Workpaper = taxpayerDetailsWorkpaperResponse.Workpaper
            };

            var UpsertTaxpayerDetailsResponse = await Client.Workpapers_PostTaxpayerDetailsWorkpaperAsync(upsertTaxpayerDetailsCommand)
                .ConfigureAwait(false);
            jsonString = JsonConvert.SerializeObject(UpsertTaxpayerDetailsResponse.Workpaper, Formatting.Indented);
            Console.Write(jsonString);

            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("== Step: Get All adjustment workpapers ==========================================================");
            // Gets a list of all adjustment workpapers for this taxpayer. 
            // This does not create any new workpapers.
            var allAdjustmentWorkpapers = await Client.Workpapers_AdjustmentWorkpapersAsync(TaxpayerId, TaxYear, null)
                .ConfigureAwait(false);

            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("== Step: Get All Taxyear workpapers ==========================================================");
            // Gets a list of all Taxyear workpapers for this taxpayer. 
            // This does not create any new workpapers.
            var allATaxYearWorkpapers = await Client.Workpapers_TaxYearWorkpapersAsync(TaxpayerId, TaxYear, null)
                .ConfigureAwait(false);

            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("== Step 1: Get Workpaper ==========================================================");

            var workpaperResponse = await Client
                .Workpapers_GetPersonalSuperannuationContributionWorkpaperAsync(
                    TaxpayerId,
                    TaxYear,
                    AccountRecordId,
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            jsonString = JsonConvert.SerializeObject(workpaperResponse.Workpaper, Formatting.Indented);
            Console.Write(jsonString);

            var newValue = workpaperResponse.Workpaper.PersonalSuperannuationContribution.Value + -500;
            workpaperResponse.Workpaper.PersonalSuperannuationContribution.Formula = newValue.ToString();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("== Step 2: Post Workpaper ==========================================================");

            var command = new UpsertPersonalSuperannuationContributionWorkpaperCommand()
            {
                TaxpayerId = TaxpayerId,
                TaxYear = TaxYear,
                AccountRecordId = AccountRecordId,
                Workpaper = workpaperResponse.Workpaper
            };

            var commandResponse = await Client.Workpapers_PostPersonalSuperannuationContributionWorkpaperAsync(command)
                .ConfigureAwait(false);
            jsonString = JsonConvert.SerializeObject(commandResponse.Workpaper, Formatting.Indented);
            Console.Write(jsonString);

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("== End ==========================================================");
        }
    }
}