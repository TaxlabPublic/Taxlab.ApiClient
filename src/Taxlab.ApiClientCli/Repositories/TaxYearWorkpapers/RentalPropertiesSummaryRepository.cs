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

        public async Task<WorkpaperResponseOfRentalPropertiesSummaryWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            RentalPropertySummaryItem[] rentalProperties,
            decimal totalPurchasePrice,
            decimal totalNetRent,
            decimal totalShareNetRent)
        {
            var workpaperResponse = await GetRentalSummaryWorkpaperAsync(taxpayerId, taxYear);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.RentalProperties = rentalProperties;
            workpaper.TotalPurchasePrice = totalPurchasePrice;
            workpaper.TotalNetRent = totalNetRent;
            workpaper.TotalShareNetRent = totalShareNetRent;

            // Update command for our new workpaper
            var upsertCommand = new UpsertRentalPropertiesSummaryWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = workpaperResponse.DocumentIndexId,
                CompositeRequest = true,
                WorkpaperType = WorkpaperType.RentalPropertiesSummaryWorkpaper,
                Workpaper = workpaperResponse.Workpaper
            };

            var upsertResponse = await Client.Workpapers_PostRentalPropertiesSummaryWorkpaperAsync(upsertCommand)
                .ConfigureAwait(false);

            return upsertResponse;
        }

        public async Task<WorkpaperResponseOfRentalPropertiesSummaryWorkpaper> GetRentalSummaryWorkpaperAsync(Guid taxpayerId, int taxYear)
        {
            var workpaperResponse = await Client
              .Workpapers_GetRentalPropertiesSummaryWorkpaperAsync(taxpayerId, taxYear, WorkpaperType.RentalPropertiesSummaryWorkpaper, Guid.Empty, false, false, true)
              .ConfigureAwait(false);

            return workpaperResponse;
        }
    }
}