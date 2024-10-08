
using System;
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
             DateOnly? MarriedFrom = null,
             DateOnly? MarriedTo = null,
             bool HasDiedThisYear = false

            )
        {
            var spouseWorkpaperResponse = await Client
                .Workpapers_GetSpouseWorkpaperAsync(taxpayerId, taxYear)
                .ConfigureAwait(false);

            var workpaper = spouseWorkpaperResponse.Workpaper;
            workpaper.LinkedSpouseTaxpayerId = LinkedSpouseTaxpayerId;
            workpaper.HasSpouse = false;
            workpaper.IsMarriedFullYear = false;
            workpaper.MarriedFrom = MarriedFrom?.ToDateTime(default);
            workpaper.MarriedTo = MarriedTo?.ToDateTime(default);
            workpaper.HasDiedThisYear = true;

            // Update command for our new workpaper
            var upsertSpouseDetailsCommand = new UpsertSpouseWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = Guid.Empty,
                CompositeRequest = true,
                Workpaper = spouseWorkpaperResponse.Workpaper
            };

            var upsertSpouseResponse = await Client.Workpapers_UpsertSpouseWorkpaperAsync(upsertSpouseDetailsCommand)
                .ConfigureAwait(false);           

            return upsertSpouseResponse;
        }
    }
}
