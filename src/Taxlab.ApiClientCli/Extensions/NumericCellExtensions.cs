using System.Globalization;
using Taxlab.ApiClientLibrary;

namespace TaxLab
{
    public static class NumericCellExtensions
    {
        public static NumericCell ToNumericCell(this decimal value)
        {
            return new NumericCell() {Formula = value.ToString(CultureInfo.InvariantCulture)};
        }
    }
}
