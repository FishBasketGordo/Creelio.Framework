namespace Creelio.Framework.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public static DateTime ChangeTime(this DateTime dt, int hour, int minute, int second)
        {
            return ChangeTime(dt, hour, minute, second, 0);
        }

        public static DateTime ChangeTime(this DateTime dt, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, hour, minute, second, millisecond);
        }
    }
}