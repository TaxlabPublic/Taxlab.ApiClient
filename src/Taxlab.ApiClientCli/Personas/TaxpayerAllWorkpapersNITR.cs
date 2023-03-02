using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Taxlab.ApiClientCli.Implementations;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Taxlab.ApiClientCli.Implementations;
using Taxlab.ApiClientCli.Repositories.Taxpayer;
using Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers;
using Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers;
using Taxlab.ApiClientCli.Workpapers.TaxYearWorkpapers;
using Taxlab.ApiClientLibrary;
using Xunit;
using Xunit.Abstractions;

namespace Taxlab.ApiClientCli.Personas
{

    public class TaxpayerAllWorkpapersNITR : BaseScopedTests
    {
        private TaxlabApiClient Client;

        //set your test taxpayer values here
        private const string FirstName = "Notindividual";
        private const string LastName = "Zey";
        private const string TaxFileNumber = "32989432";
        private const EntityType TaxpayerEntity = EntityType.CompanyAU;
        private const int TaxYear = 2021;
        private  LocalDate _balanceDate = new LocalDate(2021, 6, 30);
        private TaxpayerDto _taxpayer;

        public TaxpayerAllWorkpapersNITR(ITestOutputHelper output) : base(output)
        {

        }

        private async Task<TaxpayerResponse> CreateTaxpayer(TaxlabApiClient Client)
        {
            var taxpayerService = new TaxpayerRepository(Client);

            var taxpayerResponse = await taxpayerService.CreateAsync(TaxYear,
                FirstName,
                LastName,
                TaxFileNumber);

            var taxpayer = taxpayerResponse.Content;
            Client.TaxpayerId = taxpayer.Id;
            Client.Taxyear = TaxYear;
            Client.TaxpayerEntity = TaxpayerEntity;

            var taxReturnRepository = new TaxReturnRepository(Client);
            var taxReturnResponse = await taxReturnRepository.CreateAsync(taxpayer.Id,
                TaxYear,
                _balanceDate,
                _balanceDate.PlusYears(-1).PlusDays(-1));


            if (taxReturnResponse.Success == false)
            {
                throw new Exception(taxReturnResponse.Message);
            }

            var details = new TaxpayerDetailsRepository(Client);
            await details.CreateAsync(taxpayer.Id,
                TaxYear,
                dateOfBirth: new LocalDate(1975, 4, 12),
                dateOfDeath: new LocalDate(2020, 12, 31),
                finalReturn: true,
                mobilePhoneNumber: "0402698741",
                daytimeAreaPhoneCode: "613",
                daytimePhoneNumber: "54835123",
                emailAddress: "JohnC12@hotmail.com",
                bsbNumber: "553026",
                bankAccountName: "Bank of Melbourne",
                bankAccountNumber: "15987456",
                higherEducationLoanProgramBalance: 100,
                vetStudentLoanBalance: 200,
                studentFinancialSupplementSchemeBalance: 300,
                studentStartupLoanBalance: 400,
                abstudyStudentStartupLoanBalance: 500,
                tradeSupportLoanBalance: 600,
                smallBusinessIndicator: false
            );

            return taxpayerResponse;
        }


        protected override async Task SetupTest()
        {
            Client = TestSetup.GetTaxlabApiClient();
            var taxpayerResponse = await CreateTaxpayer(Client);
            _taxpayer = taxpayerResponse.Content;
        }

        [Fact]
        public async void NITRWorkpapersInRepositoriesTest()
        {
            CreateTaxpayerTest();
        }

        [Fact]
        public void CreateTaxpayerTest()
        {
            Assert.Equal($"{FirstName}", _taxpayer.TaxpayerName);
        }


    }
}
