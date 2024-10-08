using System;
using System.Threading.Tasks;

using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers
{
    public class DeclarationsRepository : RepositoryBase
    {
        public DeclarationsRepository(TaxlabApiClient client) : base(client)
        {

        }

        public async Task<WorkpaperResponseOfDeclarationsWorkpaper> CreateAsync(
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
                .Workpapers_GetDeclarationsWorkpaperAsync(taxpayerId, taxYear)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.TaxAgencyName = taxAgencyName;
            workpaper.TaxAgentNumber = taxAgentNumber;
            workpaper.TaxAgentABN = taxAgentAbn;
            workpaper.TaxAgentDeclarationStatement = taxAgentDeclarationStatement;
            workpaper.TaxAgentDeclarationStatementAccepted = taxAgentDeclarationStatementAccepted;
            workpaper.TaxAgentSignatureFirstName = taxAgentSignatureFirstName;
            workpaper.TaxAgentSignatureLastName = taxAgentSignatureLastName;
            workpaper.TaxAgentSignatureFullName = taxAgentSignatureFullName;
            workpaper.TaxAgentContactFirstName = taxAgentContactFirstName;
            workpaper.TaxAgentContactLastName = taxAgentContactLastName;
            workpaper.TaxAgentContactFullName = taxAgentContactFullName;
            workpaper.TaxAgentContactPhoneType = taxAgentContactPhoneType;
            workpaper.TaxAgentContactPhoneAreaCode = taxAgentContactPhoneAreaCode;
            workpaper.TaxAgentContactPhoneNumber = taxAgentContactPhoneNumber;
            workpaper.TaxAgentClientReference = taxAgentClientReference;
            workpaper.TaxPayerDeclarationStatement = taxPayerDeclarationStatement;
            workpaper.TaxPayerDeclarationStatementAccepted = taxPayerDeclarationStatementAccepted;
            workpaper.TaxPayerSignatureFirstName = taxPayerSignatureFirstName;
            workpaper.TaxPayerSignatureLastName = taxPayerSignatureLastName;
            workpaper.TaxPayerSignatureFullName = taxPayerSignatureFullName;
            workpaper.NoticesRecipientCorporateName = noticesRecipientCorporateName;
            workpaper.NoticesRecipientCorporateABN = noticesRecipientCorporateABN;
            workpaper.NoticesRecipientIndividualFirstName = noticesRecipientIndividualFirstName;
            workpaper.NoticesRecipientIndividualLastName = noticesRecipientIndividualLastName;

            // Update command for our new workpaper
            var upsertCommand = new UpsertDeclarationsWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = workpaperResponse.DocumentIndexId,
                CompositeRequest = true,
                Workpaper = workpaperResponse.Workpaper
            };

            var upsertResponse = await Client.Workpapers_UpsertDeclarationsWorkpaperAsync(upsertCommand)
                .ConfigureAwait(false);

            return upsertResponse;
        }
    }
}
