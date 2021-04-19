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

        private static readonly Guid TaxpayerId = new Guid("0e9dd2d5-d3a2-4e93-ae38-bc3d362fd744");
        private static readonly Guid AccountRecordId = new Guid("5ac1b646-2288-42d3-ae9b-dae931bf85f3");
        private const int TaxYear = 2021;

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Taxlab Client Cli!");

            var authService = new AuthService();
            Client = new TaxlabApiClient(BaseUrl, HttpClient, authService);

            Console.WriteLine("== Step 1: Get Workpaper ==========================================================");

            var workpaperResponse = await Client
                .Workpapers_GetPersonalSuperannuationContributionWorkpaperAsync(TaxpayerId, TaxYear, AccountRecordId, null, null, null, CancellationToken.None)
                .ConfigureAwait(false);

            var jsonString = JsonConvert.SerializeObject(workpaperResponse.Workpaper, Formatting.Indented);
            Console.Write(jsonString);

            var newValue = workpaperResponse.Workpaper.PersonalSuperannuationContribution.Value + -500;
            workpaperResponse.Workpaper.PersonalSuperannuationContribution.Formula = newValue.ToString();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("== Step 2: Post Workpaper ==========================================================");

            var command = new UpsertPersonalSuperannuationContributionWorkpaperCommand()
            {
                WorkpaperDto = new AdjustmentWorkpaperDtoOfPersonalSuperannuationContributionWorkpaper()
                {
                    TaxpayerId = TaxpayerId,
                    TaxYear = TaxYear,
                    AccountRecordId = AccountRecordId,
                    Workpaper = workpaperResponse.Workpaper
                }
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