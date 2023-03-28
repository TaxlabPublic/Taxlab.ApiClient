using System;
using System.Threading;
using System.Threading.Tasks;
using NodaTime;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;
using Xunit;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class NonCollectableCapitalLossesRepository : RepositoryBase
    {
        public NonCollectableCapitalLossesRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfNonCollectableCapitalLossesWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetNonCollectableCapitalLossesWorkpaperAsync(
                    taxpayerId,
                    taxYear, 
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var command = new UpsertNonCollectableCapitalLossesWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                Workpaper = workpaperResponse.Workpaper,
                WorkpaperType = WorkpaperType.NonCollectableCapitalLossesWorkpaper
            };

            var commandResponse = await Client.Workpapers_PostNonCollectableCapitalLossesWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
