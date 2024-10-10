using System;
using System.Collections.Generic;
using System.Text;

namespace Taxlab.ApiClientLibrary
{
    public interface IMultiTaxYearWorkpaperResponse
    {
        Guid DocumentIndexId { get; set; }
    }

    public partial class WorkpaperResponseOfAccountingProfitWorkpaper : IMultiTaxYearWorkpaperResponse { }
    public partial class WorkpaperResponseOfLossDisclosuresWorkpaper : IMultiTaxYearWorkpaperResponse { }
    public partial class WorkpaperResponseOfTaxOnTaxableIncomeWorkpaper : IMultiTaxYearWorkpaperResponse {}
    public partial class WorkpaperResponseOfCapitalGainsWorkpaper : IMultiTaxYearWorkpaperResponse { }
    public partial class WorkpaperResponseOfForeignIncomeTaxOffsetsWorkpaper : IMultiTaxYearWorkpaperResponse { }
    public partial class WorkpaperResponseOfGovernmentSuperContributionsWorkpaper : IMultiTaxYearWorkpaperResponse { }
    public partial class WorkpaperResponseOfSelfEducationWorkpaper : IMultiTaxYearWorkpaperResponse { }
    public partial class WorkpaperResponseOfSmallBusinessIncomeWorkpaper : IMultiTaxYearWorkpaperResponse { }
    public partial class WorkpaperResponseOfOutgoingDistributionsWorkpaper : IMultiTaxYearWorkpaperResponse { }
}
