using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class MedicareRepository : RepositoryBase
    {
        public MedicareRepository(TaxlabApiClient client) : base(client)
        {

        }

        public async Task<WorkpaperResponseOfMedicareWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            int medicareDependentChildren = 0,
            decimal medicareFullExemptionDays = 0,
            decimal medicareHalfExemptionDays = 0,
            PrivateHealthInsuranceDetails[] privateHealthInsuranceDetails = null,
            TriState privateHealthInsuranceWholeYear = TriState.Unset,
            int medicareSurchargeDaysNotLiable = 0)
        {
            var workpaperResponse = await Client
                .Workpapers_GetMedicareWorkpaperAsync(taxpayerId, taxYear, WorkpaperType.MedicareWorkpaper, Guid.Empty, false, false, false)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.MedicareDependentChildren = medicareDependentChildren;
            workpaper.MedicareFullExemptionDays = medicareFullExemptionDays;
            workpaper.MedicareHalfExemptionDays = medicareHalfExemptionDays;
            workpaper.PrivateHealthInsuranceDetails = privateHealthInsuranceDetails;
            workpaper.PrivateHealthInsuranceWholeYear = privateHealthInsuranceWholeYear;
            workpaper.MedicareSurchargeDaysNotLiable = medicareSurchargeDaysNotLiable;

            // Update command for our new workpaper
            var upsertCommand = new UpsertMedicareWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = workpaperResponse.DocumentIndexId,
                CompositeRequest = true,
                WorkpaperType = WorkpaperType.MedicareWorkpaper,
                Workpaper = workpaperResponse.Workpaper
            };

            var upsertResponse = await Client.Workpapers_PostMedicareWorkpaperAsync(upsertCommand)
                .ConfigureAwait(false);

            return upsertResponse;
        }

        public async Task<WorkpaperResponseOfMedicareWorkpaper> GetMedicareWorkpaperAsync(Guid taxpayerId, int taxYear)
        {
            var workpaperResponse = await Client
              .Workpapers_GetMedicareWorkpaperAsync(taxpayerId, taxYear, WorkpaperType.MedicareWorkpaper, Guid.Empty, false, false, false)
              .ConfigureAwait(false);

            return workpaperResponse;
        }
    }
}