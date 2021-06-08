using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Taxlab.ApiClientCli.Implementations;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli
{
    internal class Program
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private const string BaseUrl = "https://preview.taxlab.online/api-internal";
        private static TaxlabApiClient Client;

        private static readonly Guid TaxpayerId = new Guid("0e9dd2d5-d3a2-4e93-ae38-bc3d362fd744");   // Change this to your taxpayer Id
        private static readonly Guid AccountRecordId = new Guid("5ac1b646-2288-42d3-ae9b-dae931bf85f3"); // Change this to your AccountRecordId
        private const int TaxYear = 2021;  // Change this to your taxYear

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Taxlab Client Cli!");

            var authService = new AuthService();
            Client = new TaxlabApiClient(BaseUrl, HttpClient, authService);

            Console.WriteLine("== Step1: Get TaxpayerDetails workpaper ==========================================================");

            // To create a new empty workpaper we will call Get.
            // Get will create and return a new workpaper if it does not exist. (please use a new taxpayer here if you want to test this multiple times.)
            // We pass a empty DocumentIndexId here as an example of how you would do this.
            var taxpayerDetailsWorkpaperResponse = await Client
                .Workpapers_GetTaxpayerDetailsWorkpaperAsync(
                    TaxpayerId,
                    TaxYear,
                    WorkpaperType.TaxpayerDetailsWorkpaper,
                    Guid.Empty,
                    false,
                    false,
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var jsonString = JsonConvert.SerializeObject(taxpayerDetailsWorkpaperResponse.Workpaper, Formatting.Indented);
            Console.Write(jsonString);

            // We are updating BankAccountName property on our new Workpaper.
            // Post below will upsert this workpaper with our new property.
            taxpayerDetailsWorkpaperResponse.Workpaper.BankAccountName = "Test bank Account Name";
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("== Step 2: Post TaxpayerDetails Workpaper ==========================================================");
            // Update command for our new workpaper
            var upsertTaxpayerDetailsCommand = new UpsertTaxpayerDetailsWorkpaperCommand()
            {
                TaxpayerId = TaxpayerId,
                TaxYear = TaxYear,
                DocumentIndexId = taxpayerDetailsWorkpaperResponse.DocumentIndexId,
                CompositeRequest = false,
                WorkpaperType = WorkpaperType.TaxpayerDetailsWorkpaper,
                Workpaper = taxpayerDetailsWorkpaperResponse.Workpaper
            };

            var UpsertTaxpayerDetailsResponse = await Client.Workpapers_PostTaxpayerDetailsWorkpaperAsync(upsertTaxpayerDetailsCommand)
                .ConfigureAwait(false);
            jsonString = JsonConvert.SerializeObject(UpsertTaxpayerDetailsResponse.Workpaper, Formatting.Indented);
            Console.Write(jsonString);

            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("== Step: Get All adjustment workpapers ==========================================================");
            // Gets a list of all adjustment workpapers for this taxpayer. 
            // This does not create any new workpapers.
            var allAdjustmentWorkpapers = await Client.Workpapers_AdjustmentWorkpapersAsync(TaxpayerId, TaxYear)
                .ConfigureAwait(false);

            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("== Step: Get All Taxyear workpapers ==========================================================");
            // Gets a list of all Taxyear workpapers for this taxpayer. 
            // This does not create any new workpapers.
            var allATaxYearWorkpapers = await Client.Workpapers_TaxYearWorkpapersAsync(TaxpayerId, TaxYear)
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