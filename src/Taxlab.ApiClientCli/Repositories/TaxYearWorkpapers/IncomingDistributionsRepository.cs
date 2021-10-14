using System;
using System.Threading.Tasks;
using NodaTime;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers
{
    public class IncomingDistributionsRepository : RepositoryBase
    {
        public IncomingDistributionsRepository(TaxlabApiClient client) : base(client)
        {

        }

        public async Task<WorkpaperResponseOfIncomingDistributionsWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string taxAgencyName = "",
            string taxAgentNumber = "",
            string taxAgentAbn = "",
            string taxAgentDeclarationStatement = "",
            bool taxAgentDeclarationStatementAccepted = false,
            string taxAgentSignatureFirstName = "",
            string taxAgentSignatureLastName = "",
            string taxAgentSignatureFullName = "",
            string taxAgentContactFirstName = "",
            string taxAgentContactLastName = "",
            string taxAgentContactFullName = "",
            PhoneType taxAgentContactPhoneType = PhoneType.Landline,
            string taxAgentContactPhoneAreaCode = "",
            string taxAgentContactPhoneNumber = "",
            string taxAgentClientReference = "",
            string taxPayerDeclarationStatement = "",
            bool taxPayerDeclarationStatementAccepted = false,
            string taxPayerSignatureFirstName = "",
            string taxPayerSignatureLastName = "",
            string taxPayerSignatureFullName = "",
            string noticesRecipientCorporateName = "",
            string noticesRecipientCorporateABN = "",
            string noticesRecipientIndividualFirstName = "",
            string noticesRecipientIndividualLastName = ""
        )
        {
            var workpaperResponse = await Client
                .Workpapers_GetIncomingDistributionsWorkpaperAsync(taxpayerId, taxYear, WorkpaperType.TaxpayerDetailsWorkpaper, Guid.Empty, false, false)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            

            // Update command for our new workpaper
            var upsertCommand = new UpsertIncomingDistributionsWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = workpaperResponse.DocumentIndexId,
                CompositeRequest = true,
                WorkpaperType = WorkpaperType.DeclarationsWorkpaper,
                Workpaper = workpaperResponse.Workpaper
            };

            var upsertResponse = await Client.Workpapers_PostIncomingDistributionsWorkpaperAsync(upsertCommand)
                .ConfigureAwait(false);

            return upsertResponse;
        }
    }
}
