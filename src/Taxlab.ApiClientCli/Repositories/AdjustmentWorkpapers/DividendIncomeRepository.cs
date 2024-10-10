﻿using System;
using System.Threading;
using System.Threading.Tasks;
using TaxLab;
using Taxlab.ApiClientCli.Workpapers.Shared;
using Taxlab.ApiClientLibrary;

namespace Taxlab.ApiClientCli.Workpapers.AdjustmentWorkpapers
{
    public class DividendIncomeRepository : RepositoryBase
    {
        public DividendIncomeRepository(TaxlabApiClient client) : base(client)
        {
        }

        public async Task<WorkpaperResponseOfDividendIncomeWorkpaper> CreateAsync(
            Guid taxpayerId,
            int taxYear,
            string companyName = "",
            string referenceNumber = "",
            int howManyPeopleHoldThisAccount = 0,
            decimal unfrankedAmount = 0m,
            decimal frankedAmount = 0m,
            decimal frankingCredits = 0m,
            decimal taxPaid = 0m
            )
        {
            var workpaperResponse = await Client
                .Workpapers_GetDividendIncomeWorkpaperAsync(
                    taxpayerId,
                    taxYear,
                    Guid.Empty,
                    null,
                    null,
                    null,
                    CancellationToken.None)
                .ConfigureAwait(false);

            var workpaper = workpaperResponse.Workpaper;
            workpaper.CompanyName = companyName;
            workpaper.ReferenceNumber = referenceNumber;
            workpaper.AccountHoldersNumbers = howManyPeopleHoldThisAccount;
            workpaper.UnfrankedAmount = unfrankedAmount.ToNumericCell();
            workpaper.FrankedAmount = frankedAmount.ToNumericCell();
            workpaper.FrankingCredits = frankingCredits.ToNumericCell();
            workpaper.TaxPaid = taxPaid.ToNumericCell();

            var command = new UpsertDividendIncomeWorkpaperCommand()
            {
                TaxpayerId = taxpayerId,
                TaxYear = taxYear,
                AccountRecordId = workpaperResponse.Workpaper.Slug.AccountRecordId,
                Workpaper = workpaperResponse.Workpaper,
                CompositeRequest = true
            };

            var commandResponse = await Client.Workpapers_UpsertDividendIncomeWorkpaperAsync(command)
                .ConfigureAwait(false);

            return commandResponse;
        }
    }
}
