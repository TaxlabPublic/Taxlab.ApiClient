using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class RentalPropertyRepository : RepositoryBase
    {
        public RentalPropertyRepository(TaxlabApiClient client) : base(client)
        {

        }

        public async Task<WorkpaperResponseOfRentalPropertyWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear)
        {
            var createRentalPropertyCommand = new CreateRentalPropertyWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
            };

            var createRentalPropertyReponse = await Client.Workpapers_CreateRentalPropertyWorkpaperAsync(createRentalPropertyCommand)
                .ConfigureAwait(false);

            // Update command for our new workpaper
            var upsertCommand = new UpsertRentalPropertyWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = createRentalPropertyReponse.DocumentId,
                CompositeRequest = true,
                WorkpaperType = WorkpaperType.RentalPropertyWorkpaper
            };

            var upsertResponse = await Client.Workpapers_PostRentalPropertyWorkpaperAsync(upsertCommand)
                .ConfigureAwait(false);

            return upsertResponse;
        }

        public async Task<DeleteRentalPropertyWorkpaperResponse> DeleteAsync(Guid taxpayerId, int taxYear, Guid documentId)
        {
            // Delete command for our workpaper
            var deleteCommand = new DeleteRentalPropertyWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentId = documentId
            };

            var deleteResponse = await Client
              .Workpapers_DeleteRentalPropertyWorkpaperAsync(deleteCommand)
              .ConfigureAwait(false);

            return deleteResponse;
        }

        public async Task<WorkpaperResponseOfRentalPropertyWorkpaper> GetRentalPropertyWorkpaperAsync(Guid taxpayerId, int taxYear)
        {
            var workpaperResponse = await Client
                .Workpapers_GetRentalPropertyWorkpaperAsync(taxpayerId, taxYear, WorkpaperType.RentalPropertyWorkpaper, Guid.Empty, false, false, false)
                .ConfigureAwait(false);

            return workpaperResponse;
        }
    }
}