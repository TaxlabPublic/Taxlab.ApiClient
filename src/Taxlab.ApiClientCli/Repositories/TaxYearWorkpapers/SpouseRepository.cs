using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;
using TaxLab;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class SpouseRepository : RepositoryBase
    {
        public SpouseRepository(TaxlabApiClient client) : base(client)
        {

        }
        public async Task<WorkpaperResponseOfSpouseWorkpaper> CreateAsync(
             Guid taxpayerId,
             int taxYear,
             Guid? LinkedSpouseTaxpayerId,
             bool IsMarriedFullYear = true,
             LocalDate? MarriedFrom = null,
             LocalDate? MarriedTo = null,
             bool HasDiedThisYear = false

            )
        {
            var spouseWorkpaperResponse = await Client
                .Workpapers_GetSpouseWorkpaperAsync(taxpayerId, taxYear, WorkpaperType.SpouseWorkpaper, Guid.Empty, false, false, true)
                .ConfigureAwait(false);

            var workpaper = spouseWorkpaperResponse.Workpaper;
            workpaper.LinkedSpouseTaxpayerId = LinkedSpouseTaxpayerId;
            workpaper.IsMarriedFullYear = IsMarriedFullYear;
            workpaper.MarriedFrom = MarriedFrom.ToAtoDateString();
            workpaper.MarriedTo = MarriedTo.ToAtoDateString();
            workpaper.HasDiedThisYear = HasDiedThisYear;

            // Update command for our new workpaper
            var upsertSpouseDetailsCommand = new UpsertSpouseWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = spouseWorkpaperResponse.DocumentIndexId,
                CompositeRequest = true,
                WorkpaperType = WorkpaperType.TaxpayerDetailsWorkpaper,
                Workpaper = spouseWorkpaperResponse.Workpaper
            };

            var upsertSpouseResponse = await Client.Workpapers_PostSpouseWorkpaperAsync(upsertSpouseDetailsCommand)
                .ConfigureAwait(false);

            return upsertSpouseResponse;
        }
    }
}
