namespace System
{
    public static class DateTimeExtensions
    {
        public static string Timestamp(this DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        public static int Age(this DateTime value)
        {
            return DateTime.Now.AddYears(-value.Year).Year;
        }
    }
}
