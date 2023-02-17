﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NodaTime;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Repositories.TaxYearWorkpapers
{
    public class CapitalGainOrLossTransactionRepository : RepositoryBase
    {
        public CapitalGainOrLossTransactionRepository(TaxlabApiClient client) : base(client)
        {

        }

        public async Task<WorkpaperResponseOfCapitalGainOrLossTransactionWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string description,
            string category = "1",
            LocalDate? purchaseDate = null,
            decimal purchaseAmount = 0m,
            List<CapitalGainOrLossTransaction> purchaseAdjustment = null,
            LocalDate? disposalDate = null,
            List<CapitalGainOrLossTransaction> disposalAdjustment = null,
            decimal disposalAmount = 0m,
            decimal discountAmount = 0m,
            decimal currentYearLossesApplied = 0m,
            decimal priorLossesApplied = 0m,
            decimal capitalLossesTransferredInApplied = 0m,
            bool isEligibleForDiscount = false,
            bool isEligibleForActiveAssetReduction = false,
            bool isEligibleForRetirementExemption = false,
            decimal retirementExemptionAmount = 0m,
            bool isEligibleForRolloverConcession = false,
            decimal rolloverConcessionAmount = 0m)
        {
            var createCapitalGainOrLossTransactionCommand = new CreateCapitalGainOrLossTransactionWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                InitialValue = new CapitalGainOrLossTransactionWorkpaperInitialValue
                {
                    Category = category,
                    Description = description,
                    DisposalAmount = disposalAmount,
                    PurchaseAmount = purchaseAmount
                }
            };

            var createCapitalGainOrLossTransactionResponse = await Client.Workpapers_CreateCapitalGainOrLossTransactionWorkpaperAsync(createCapitalGainOrLossTransactionCommand)
                .ConfigureAwait(false);

            var getWorkpaperResponse = await GetCapitalGainOrLossTransactionWorkpaperAsync(taxpayerId, taxYear, createCapitalGainOrLossTransactionResponse);

            purchaseAdjustment ??= new List<CapitalGainOrLossTransaction>();
            disposalAdjustment ??= new List<CapitalGainOrLossTransaction>();

            var workpaper = getWorkpaperResponse.Workpaper;
            workpaper.Description = description;
            workpaper.DisposalAmount = disposalAmount;
            workpaper.Category = category;
            workpaper.PurchaseDate = purchaseDate.ToAtoDateString();
            workpaper.PurchaseAmount = purchaseAmount;
            workpaper.PurchaseAdjustment = purchaseAdjustment;
            workpaper.DisposalDate = disposalDate.ToAtoDateString();
            workpaper.DisposalAmount = disposalAmount;
            workpaper.DisposalAdjustment = disposalAdjustment;
            workpaper.DiscountAmount = discountAmount;
            workpaper.CurrentYearLossesApplied = currentYearLossesApplied;
            workpaper.PriorLossesApplied = priorLossesApplied;
            workpaper.CapitalLossesTransferredInApplied = capitalLossesTransferredInApplied;
            workpaper.IsEligibleForDiscount = isEligibleForDiscount;
            workpaper.IsEligibleForActiveAssetReduction = isEligibleForActiveAssetReduction;
            workpaper.IsEligibleForRetirementExemption = isEligibleForRetirementExemption;
            workpaper.RetirementExemptionAmount = retirementExemptionAmount;
            workpaper.IsEligibleForRolloverConcession = isEligibleForRolloverConcession;
            workpaper.RolloverConcessionAmount = rolloverConcessionAmount;

            // Update command for our new workpaper
            var upsertCommand = new UpsertCapitalGainOrLossTransactionWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                DocumentIndexId = getWorkpaperResponse.DocumentIndexId,
                CompositeRequest = true,
                WorkpaperType = WorkpaperType.CapitalGainOrLossTransactionWorkpaper,
                Workpaper = getWorkpaperResponse.Workpaper
            };

            var upsertResponse = await Client.Workpapers_PostCapitalGainOrLossTransactionWorkpaperAsync(upsertCommand)
                .ConfigureAwait(false);

            return upsertResponse;
        }

        public async Task<WorkpaperResponseOfCapitalGainOrLossTransactionWorkpaper> GetCapitalGainOrLossTransactionWorkpaperAsync(Guid taxpayerId, int taxYear, CreateCapitalGainOrLossTransactionWorkpaperResponse createCapitalGainOrLossTransactionResponse)
        {
            var workpaperResponse = await Client
                .Workpapers_GetCapitalGainOrLossTransactionWorkpaperAsync(taxpayerId, taxYear, createCapitalGainOrLossTransactionResponse.DocumentId)
                .ConfigureAwait(false);

            return workpaperResponse;
        }
    }
}