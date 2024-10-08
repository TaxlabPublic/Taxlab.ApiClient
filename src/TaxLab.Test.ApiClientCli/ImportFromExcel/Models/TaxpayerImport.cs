
using System;
using System.Collections.Generic;
using System.Text;
using Taxlab.ApiClientLibrary;

namespace TaxLab.Test.ApiClientCli.ImportFromExcel.Models
{
    public class TaxpayerImport
    {
        public bool Import { get; set; }
        public string EntityCode { get; set; }
        public string TaxNumber { get; set; }
        public string TaxpayerOrFirstName { get; set; }
        public string LastName { get; set; }
        public EntityType EntityType { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string BsbNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string BalanceAccountName { get; set; }
        public bool BaseRateEntityIndicator { get; set; }        
        public string NoticesRecipientCorporateName { get; set; }
        public string NoticesRecipientCorporateABN { get; set; }
        public string NoticesRecipientIndividualFirstName { get; set; }
        public string NoticesRecipientIndividualLastName { get; set; }
        public int? FamilyTrustElectionYear { get; set; }
        public string FamilyTrustElection { get; set; }
    }
}
