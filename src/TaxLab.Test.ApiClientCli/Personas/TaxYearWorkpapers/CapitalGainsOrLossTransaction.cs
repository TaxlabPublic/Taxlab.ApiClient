using System;
using System.Collections.Generic;
using System.Net.Http;
using Taxlab.ApiClientCli.Implementations;
using Taxlab.ApiClientLibrary;
using Xunit;

namespace TaxLab.Test.ApiClientCli.Personas.TaxYearWorkpapers
{
    public class CapitalGainsOrLossTransaction
    {
        [Fact]
        public async void CreateCapitalGainsOrLossTransactionWorkpapers()
        {
            string baseUrl = "https://preview.taxlab.online/api-internal/";
            HttpClient httpclient = new HttpClient();
            var authService = new AuthService();
            TaxlabApiClient client = new TaxlabApiClient(baseUrl, httpclient, authService);

            var taxpayerId = new Guid("af77567b-6968-4b2a-b4c4-6db0fa4f9647");
            var taxYear = 2023;

            var createCapitalGainOrLossTransactionWorkpaperCommand =
                new CreateCapitalGainOrLossTransactionWorkpaperCommand
                {
                    TaxpayerId = taxpayerId,
                    TaxYear = taxYear,
                    InitialValue = new CapitalGainOrLossTransactionWorkpaperInitialValue()
                    {
                        Category = "1",
                        Description = "Capital Gain Or Loss Transaction",
                        DisposalAmount = 10000,
                        PurchaseAmount = 5000
                    }
                };


            var createCapitalGainOrLossTransactionWorkpaperResponse = await client
                .Workpapers_CreateCapitalGainOrLossTransactionWorkpaperAsync(
                    createCapitalGainOrLossTransactionWorkpaperCommand).ConfigureAwait(false);


            var getCapitalGainOrLossTransactionWorkpaperResponse = await client
                .Workpapers_GetCapitalGainOrLossTransactionWorkpaperAsync(taxpayerId, taxYear,
                    WorkpaperType.CapitalGainOrLossTransactionWorkpaper,
                    createCapitalGainOrLossTransactionWorkpaperResponse.DocumentId, false, false, false)
                .ConfigureAwait(false);


            var capitalGainOrLossTransactionWorkpaper = getCapitalGainOrLossTransactionWorkpaperResponse.Workpaper;

            capitalGainOrLossTransactionWorkpaper.PurchaseDate = "2022-07-01";
            capitalGainOrLossTransactionWorkpaper.PurchaseAdjustment = new List<CapitalGainOrLossTransaction>
            {
                new CapitalGainOrLossTransaction()
                {
                    Amount = 250.0m,
                    Description = "Purchase adjustment 1"
                },
                new CapitalGainOrLossTransaction()
                {
                    Amount = 250.0m,
                    Description = "Purchase adjustment 2"
                }
            };
            capitalGainOrLossTransactionWorkpaper.DisposalDate = "2022-07-02";
            capitalGainOrLossTransactionWorkpaper.DisposalAdjustment = new List<CapitalGainOrLossTransaction>
            {
                new CapitalGainOrLossTransaction()
                {
                    Amount = 500.0m,
                    Description = "Disposal adjustment 1"
                },
                new CapitalGainOrLossTransaction()
                {
                    Amount = 500.0m,
                    Description = "Disposal adjustment 2"
                }
            };
            capitalGainOrLossTransactionWorkpaper.IsEligibleForActiveAssetReduction = true;
            capitalGainOrLossTransactionWorkpaper.IsEligibleForDiscount = true;
            capitalGainOrLossTransactionWorkpaper.IsEligibleForRetirementExemption = true;
            capitalGainOrLossTransactionWorkpaper.IsEligibleForRolloverConcession = true;

            var upsertCapitalGainsOrLossTransactionCommand = new UpsertCapitalGainOrLossTransactionWorkpaperCommand()
            {
                CompositeRequest = true,
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = getCapitalGainOrLossTransactionWorkpaperResponse.DocumentIndexId,
                Workpaper = capitalGainOrLossTransactionWorkpaper,
                WorkpaperType = WorkpaperType.CapitalGainOrLossTransactionWorkpaper
            };

            var upsert =
                await client.Workpapers_PostCapitalGainOrLossTransactionWorkpaperAsync(
                    upsertCapitalGainsOrLossTransactionCommand).ConfigureAwait(false);

            Assert.Equal(capitalGainOrLossTransactionWorkpaper.PurchaseDate, upsert.Workpaper.PurchaseDate);

            Assert.Equal(capitalGainOrLossTransactionWorkpaper.DisposalDate, upsert.Workpaper.DisposalDate);

            Assert.Equal(capitalGainOrLossTransactionWorkpaper.IsEligibleForActiveAssetReduction,
                upsert.Workpaper.IsEligibleForActiveAssetReduction);
            Assert.Equal(capitalGainOrLossTransactionWorkpaper.IsEligibleForDiscount,
                upsert.Workpaper.IsEligibleForDiscount);
            Assert.Equal(capitalGainOrLossTransactionWorkpaper.IsEligibleForRetirementExemption,
                upsert.Workpaper.IsEligibleForRetirementExemption);

            Assert.Equal(capitalGainOrLossTransactionWorkpaper.PurchaseAdjustment.Count, upsert.Workpaper.PurchaseAdjustment.Count);
            Assert.Equal(capitalGainOrLossTransactionWorkpaper.DisposalAdjustment.Count, upsert.Workpaper.DisposalAdjustment.Count);
        }
    }
}
