using NodaTime;
using NodaTime.Text;
using System;

namespace TaxLab
{
    public static class NodaTimeExtensions
    {
        private static LocalDatePattern OurLocalDatePattern = LocalDatePattern.CreateWithInvariantCulture(LocalDatePattern.Iso.PatternText);

        public static LocalDate ToLocalDate(this string dateString)
        {
            var result = OurLocalDatePattern.Parse(dateString).Value;
            return result;
        }

        public static LocalDate? ToLocalDateOrDefault(this string dateString)
        {
            var success = OurLocalDatePattern.Parse(dateString).TryGetValue(LocalDate.FromDateTime(DateTime.Now), out LocalDate result);
            return success ? result : null as LocalDate?;
        }

        public static string ToAtoDateString(this LocalDate date)
        {
            var result = date.ToString(NodaTime.Text.LocalDatePattern.Iso.PatternText, null);
            return result;
        }

        public static string ToAtoDateString(this LocalDate? date)
        {
            if (date == null)
            {
                return string.Empty;
            }

            var result = ((LocalDate)date).ToAtoDateString();
            return result;
        }
    }
}