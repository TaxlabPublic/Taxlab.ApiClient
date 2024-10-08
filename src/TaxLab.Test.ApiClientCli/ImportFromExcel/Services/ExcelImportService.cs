
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using Taxlab.ApiClientLibrary;
using TaxLab.Test.ApiClientCli.ImportFromExcel.Models;

namespace TaxLab.Test.ApiClientCli.ImportFromExcel.Services
{
    public class ExcelImportService
    {
        private readonly ResourceFileLoader _resourceFileLoader = new ResourceFileLoader(typeof(ExcelImportService));

        public List<TaxpayerImport> CreateTaxpayerFromExcelAsync(string filename)
        {
            Workbook workbook = new Workbook();

            using System.IO.Stream stream = _resourceFileLoader.LoadStreamResource(str => str.EndsWith(filename));
            
            workbook.LoadFromStream(stream);

            Worksheet sheet = workbook.Worksheets[0];

            DataTable data = sheet.ExportDataTable();

            List<TaxpayerImport> imports = new List<TaxpayerImport>();

            foreach (DataRow row in data.Rows)
            {
                TaxpayerImport import = CreateTaxpayerImport(row);

                imports.Add(import);
            }
            return imports;
        }

        public TaxpayerImport CreateTaxpayerImport(DataRow row)
        {
            var taxpayer = new TaxpayerImport
            {
                Import = GetBool(row["Import"].ToString().Trim().ToLower()),
                EntityCode = row["EntityCode"].ToString().Trim(),
                TaxNumber = row["TaxNumber"].ToString().Trim(),
                TaxpayerOrFirstName = row["FirstName"].ToString().Trim(),
                LastName = row["LastName"].ToString().Trim(),
                EntityType = GetEntityType(row["EntityType"].ToString().Trim().ToLower()),
                DateOfBirth = GetDate(row["DateOfBirth"].ToString().Trim()),
                BsbNumber = row["BsbNumber"].ToString().Trim(),
                BankAccountNumber = row["BankAccountNumber"].ToString().Trim(),
                BalanceAccountName = row["BalanceAccountName"].ToString().Trim(),
                BaseRateEntityIndicator = GetBool(row["BaseRateEntityIndicator"].ToString().Trim().ToLower()),
                NoticesRecipientCorporateName = row["NoticesRecipientCorporateName"].ToString().Trim(),
                NoticesRecipientCorporateABN = row["NoticesRecipientCorporateABN"].ToString().Trim(),
                NoticesRecipientIndividualFirstName = row["NoticesRecipientIndividualFirstName"].ToString().Trim(),
                NoticesRecipientIndividualLastName = row["NoticesRecipientIndividualLastName"].ToString().Trim(),
                FamilyTrustElectionYear = GetFamilyTrustElectionYear(row["FamilyTrustElectionYear"].ToString().Trim()),
                FamilyTrustElection = row["FamilyTrustElection"].ToString().Trim(),
            };
            return taxpayer;
        }

        public EntityType GetEntityType(string entity)
        {
            if (entity.Contains("au"))
            {
                if (entity.Contains("individual"))
                {
                    return EntityType.IndividualAU;
                }
                else if (entity.Contains("company"))
                {
                    return EntityType.CompanyAU;
                }
                else if (entity.Contains("trust"))
                {
                    return EntityType.TrustAU;
                }
                else
                {
                    throw new Exception("Entity type not supported");
                }
            }
            else
            {
                if (entity.Contains("individual"))
                {
                    return EntityType.Individual;
                }
                else if (entity.Contains("company"))
                {
                    return EntityType.Entity;
                }
                else if (entity.Contains("trust"))
                {
                    return EntityType.Trust;
                }
                else
                {
                    throw new Exception("Entity type not supported");
                }
            }
        }

        public DateOnly GetDate(string date)
        {
            if (date == "")
            {
                return DateOnly.FromDateTime(DateTime.Now);
            }else
            {
                return DateOnly.FromDateTime(DateTime.Parse(date));
            }
        }
        public bool GetBool(string boolStr)
        {
            if (boolStr.Contains("t"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int? GetFamilyTrustElectionYear(string electionYear)
        {
            if (electionYear != "")
            {
                return Int32.Parse(electionYear);
            }
            else
            {
                return null;
            }
        }
    }
}
