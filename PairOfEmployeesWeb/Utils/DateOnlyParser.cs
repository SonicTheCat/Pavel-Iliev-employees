namespace PairOfEmployeesWeb.Utils
{
    using Microsoft.AspNetCore.Http;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    public static class DateOnlyParser
    {
        public static DateOnly? Parse(string value)
        {
            var isDate = DateOnly.TryParse(value, out DateOnly result);
            if (isDate)
            {
                return result;
            }

            return TryOtherFormats(value);
        }

        public static DateOnly? TryOtherFormats(string value)
        {
            var culture = CultureInfo.CurrentCulture;
            var formatInfo = culture.DateTimeFormat;
            var allDateTimePatterns = formatInfo.GetAllDateTimePatterns();

            foreach (string format in allDateTimePatterns)
            {
                var isDate = DateOnly.TryParseExact(value, format, culture, DateTimeStyles.None, out DateOnly parsedDate);
                if (isDate)
                {
                    return parsedDate;
                }
            }

            return null;
        }
    }
}