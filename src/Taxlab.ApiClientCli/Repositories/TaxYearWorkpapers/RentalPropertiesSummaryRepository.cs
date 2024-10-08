using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class RentalPropertiesSummaryRepository : RepositoryBase
    {
        public RentalPropertiesSummaryRepository(TaxlabApiClient client) : base(client)
        {

        }

        public async Task<WorkpaperResponseOfRentalPropertiesSummaryWorkpaperView> GetRentalSummaryWorkpaperAsync(Guid taxpayerId, int taxYear)
        {
            var workpaperResponse = await Client
              .Workpapers_GetRentalPropertiesSummaryWorkpaperAsync(taxpayerId, taxYear)
              .ConfigureAwait(false);

            return workpaperResponse;
        }
    }
}