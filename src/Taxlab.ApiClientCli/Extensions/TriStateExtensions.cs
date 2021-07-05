using System.Globalization;
using Taxlab.ApiClientLibrary;

namespace TaxLab
{
    public static class TriStateExtensions
    {

        public static TriState ToTriState(this bool value)
        {
            switch (value)
            {
                case true:
                    return TriState.True;
                case false:
                    return TriState.False;
                default:
                    return TriState.Unset;
            }
        }
    }
}
