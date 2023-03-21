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
    public class FrankingAccountRepository : RepositoryBase
    {
        public FrankingAccountRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfFrankingAccountWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear
        )
        {
            var workpaperResponse = await Client
                .Workpapers_GetFrankingAccountWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var command = new UpsertFrankingAccountWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                Workpaper = workpaperResponse.Workpaper,
                WorkpaperType = WorkpaperType.FrankingAccountWorkpaper
            };

            var commandResponse = await Client.Workpapers_PostFrankingAccountWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
