using System.Globalization;

namespace RabbitApi.Utils {
    public static class Utils {
        public static bool IsIsoDateFormat(string date) {
            string format = "yyyy-MM-dd";

            if (DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _)) {
                return true;
            }
            return false;
        }
    }
}
