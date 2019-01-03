using System;
using System.Globalization;
using System.Text;
/// <summary>
/// 	Extension methods for the DateTimeOffset data type.
/// </summary>
public static class DateTimeExtensions {
    const int EveningEnds = 2;
    const int MorningEnds = 12;
    const int AfternoonEnds = 6;
    static readonly DateTime Date1970 = new DateTime (1970, 1, 1);

    #region To adapt Chinese

    /// <summary>
    /// 十天干
    /// </summary>
    private static string[] _tiangan = { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };

    /// <summary>
    /// 十二地支
    /// </summary>
    private static string[] _dizhi = { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

    /// <summary>
    /// 十二生肖
    /// </summary>
    private static string[] _shengxiao = { "鼠", "牛", "虎", "免", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };

    /// <summary>
    /// 返回农历天干地支年 
    /// </summary>
    /// <param name="year">农历年</param>
    /// <returns></returns>
    public static string ToLunisolarYear (this int year) {
        if (year > 3) {
            int tgIndex = (year - 4) % 10;
            int dzIndex = (year - 4) % 12;

            return string.Concat (_tiangan[tgIndex], _dizhi[dzIndex] /*, "[", shengxiao[dzIndex], "]"*/ );
        }

        throw new ArgumentOutOfRangeException ("无效的年份!");
    }

    /// <summary>
    /// 获得指定公历日期中对应的农历天干地支年
    /// </summary>
    /// <param name="calendar"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string ToLunisolarYear (this ChineseLunisolarCalendar calendar, DateTime date) {
        return ToLunisolarYear (calendar.GetYear (date));
    }

    /// <summary>
    /// 农历月
    /// </summary>
    private static string[] months = { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二(腊)" };

    /// <summary>
    /// 返回农历月
    /// </summary>
    /// <param name="month">月份</param>
    /// <returns></returns>
    public static string ToLunisolarMonth (this int month) {
        if (month < 13 && month > 0) {
            return months[month - 1];
        }

        throw new ArgumentOutOfRangeException ("无效的月份!");
    }

    /// <summary>
    /// 返回指定公历日期对应的农历月
    /// </summary>
    /// <param name="calendar"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string ToLunisolarMonth (this ChineseLunisolarCalendar calendar, DateTime date) {
        var month = calendar.GetMonth (date);
        var leapMonth = calendar.GetLeapMonth (calendar.GetYear (date), calendar.GetEra (date));
        var isleep = leapMonth > 0 && leapMonth == month;

        if (leapMonth > 0) {
            if (leapMonth <= month)
                month--;
        }

        return string.Concat (isleep ? "闰" : string.Empty, ToLunisolarMonth (month), "月");
    }

    /// <summary>
    /// 日期前缀
    /// </summary>
    private static string[] days1 = { "初", "十", "廿", "三" };

    /// <summary>
    /// 日
    /// </summary>
    private static string[] days = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };

    /// <summary>
    /// 返回农历日
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    public static string ToLunisolarDay (this int day) {
        if (day > 0 && day < 32) {
            if (day != 20 && day != 30) {
                return string.Concat (days1[(day - 1) / 10], days[(day - 1) % 10]);
            } else {
                return string.Concat (days[(day - 1) / 10], days1[1]);
            }
        }

        throw new ArgumentOutOfRangeException ("无效的日!");
    }

    /// <summary>
    /// 返回农历日
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    public static string ToLunisolarDay (this ChineseLunisolarCalendar calendar, DateTime datetime) {
        return ToLunisolarDay (calendar.GetDayOfMonth (datetime));
    }

    /// <summary>
    /// 返回生肖
    /// </summary>
    /// <param name="datetime">公历日期</param>
    /// <returns></returns>
    public static string ToShengXiao (this ChineseLunisolarCalendar calendar, DateTime datetime) {
        return _shengxiao[calendar.GetTerrestrialBranch (calendar.GetSexagenaryYear (datetime)) - 1];
    }

    /// <summary>
    /// 获得指定公历的农历表述方式
    /// </summary>
    /// <param name="calendar"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string ToLunisolarDateString (this ChineseLunisolarCalendar calendar, DateTime date) {
        return calendar.ToLunisolarYear (date) + "年" + calendar.ToLunisolarMonth (date) + calendar.ToLunisolarDay (date);
    }

    /// <summary>
    /// 本周最后一天
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime EndOfWeek (this DateTime date) {
        DayOfWeek monday = DayOfWeek.Monday;
        DayOfWeek saturday = monday - 1;
        if (saturday < DayOfWeek.Sunday) {
            saturday = DayOfWeek.Saturday;
        }
        if (date.DayOfWeek == saturday) {
            return date;
        }
        if (saturday == date.DayOfWeek) {
            return date.AddDays (6.0);
        }
        if (saturday < date.DayOfWeek) {
            return date.AddDays ((double) (7 - (date.DayOfWeek - saturday)));
        }
        return date.AddDays ((double) (saturday - date.DayOfWeek));
    }

    public static DateTime FirstWeek (this DateTime date) {
        int dayOfWeek = (int) date.DayOfWeek;
        DateTime time = new DateTime (date.Year, date.Month, 1);
        return time.AddDays ((double) (-dayOfWeek + 1));
    }

    public static DateTime FirstWeek (this DateTime date, DayOfWeek dayOfWeek) {
        DateTime time = date.StartOfMonth ();
        if (time.DayOfWeek != dayOfWeek) {
            time = time.NextWeek (dayOfWeek);
        }
        return time;
    }

    /// <summary>
    /// 指定日期是否闰年
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static bool IsLeapYear (this DateTime date) {
        if ((date.Year % 4) != 0) {
            return false;
        }
        if ((date.Year % 100) == 0) {
            return ((date.Year % 400) == 0);
        }
        return true;
    }

    /// <summary>
    /// 本月最后一周的星期几
    /// </summary>
    /// <param name="date"></param>
    /// <param name="dayOfWeek"></param>
    /// <returns></returns>
    public static DateTime LastWeek (this DateTime date, DayOfWeek dayOfWeek) {
        DateTime time = date.EndOfMonth ();
        int num = (dayOfWeek <= time.DayOfWeek) ? ((int) (time.DayOfWeek - dayOfWeek)) : (7 - Math.Abs ((int) (dayOfWeek - time.DayOfWeek)));
        return time.AddDays ((double) (num * -1));
    }
    /// <summary>
    /// 下一周的星期几
    /// </summary>
    /// <param name="date"></param>
    /// <param name="dayOfWeek"></param>
    /// <returns></returns>
    public static DateTime NextWeek (this DateTime date, DayOfWeek dayOfWeek) {
        return date.NextWeek (dayOfWeek, 1);
    }
    /// <summary>
    /// 下几周的星期几
    /// </summary>
    /// <param name="date"></param>
    /// <param name="dayOfWeek"></param>
    /// <param name="week"></param>
    /// <returns></returns>
    public static DateTime NextWeek (this DateTime date, DayOfWeek dayOfWeek, int week) {
        int num = (int) (dayOfWeek - date.DayOfWeek);
        if (num <= 0) {
            num += 7;
        }
        if (week > 1) {
            num += (week - 1) * 7;
        }
        return date.AddDays ((double) num);
    }
    /// <summary>
    /// 上一周星期几
    /// </summary>
    /// <param name="date"></param>
    /// <param name="dayOfWeek"></param>
    /// <returns></returns>
    public static DateTime PreviousWeek (this DateTime date, DayOfWeek dayOfWeek) {
        return date.PreviousWeek (dayOfWeek, 1);
    }
    /// <summary>
    /// 上几周星期几
    /// </summary>
    /// <param name="date"></param>
    /// <param name="dayOfWeek"></param>
    /// <param name="week"></param>
    /// <returns></returns>
    public static DateTime PreviousWeek (this DateTime date, DayOfWeek dayOfWeek, int week) {
        int num = (int) (dayOfWeek - date.DayOfWeek);
        if (num >= 0) {
            num -= 7;
        }
        if (week > 1) {
            num -= (week - 1) * 7;
        }
        return date.AddDays ((double) num);
    }

    /// <summary>
    /// 设定当前时间的第几天
    /// </summary>
    /// <param name="time"></param>
    /// <param name="day"></param>
    /// <returns></returns>
    public static DateTime SetDay (this DateTime time, int day) {
        return new DateTime (time.Year, time.Month, day);
    }
    /// <summary>
    /// 设定当前时间的月份
    /// </summary>
    /// <param name="time"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    public static DateTime SetMonth (this DateTime time, int month) {
        return new DateTime (time.Year, month, time.Day);
    }
    /// <summary>
    /// 设定当前时间的年份
    /// </summary>
    /// <param name="time"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    public static DateTime SetYear (this DateTime time, int year) {
        return new DateTime (year, time.Month, time.Day);
    }
    /// <summary>
    /// 转换为日期时间
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static DateTime ToDateTime (this long timeStamp) {
        DateTime UnixTimestampLocalZero = System.TimeZoneInfo.ConvertTime (new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
        return UnixTimestampLocalZero.AddSeconds (timeStamp);
    }

    /// <summary>
    /// 转换为时间截
    /// </summary>
    /// <param name="datetime"></param>
    /// <returns></returns>
    public static long ToTimeStamp (this DateTime datetime) {
        return ((datetime.ToUniversalTime ().Ticks - 0x89f7ff5f7b58000L) / 0x989680L);
    }
    #endregion

    ///<summary>
    ///	Return System UTC Offset
    ///</summary>
    public static double UtcOffset {
        get { return DateTime.Now.Subtract (DateTime.UtcNow).TotalHours; }
    }

    /// <summary>
    /// 	Calculates the age based on today.
    /// </summary>
    /// <param name = "dateOfBirth">The date of birth.</param>
    /// <returns>The calculated age.</returns>
    public static int CalculateAge (this DateTime dateOfBirth) {
        return CalculateAge (dateOfBirth, Clock.Now.Date);
    }

    /// <summary>
    /// 	Calculates the age based on a passed reference date.
    /// </summary>
    /// <param name = "dateOfBirth">The date of birth.</param>
    /// <param name = "referenceDate">The reference date to calculate on.</param>
    /// <returns>The calculated age.</returns>
    public static int CalculateAge (this DateTime dateOfBirth, DateTime referenceDate) {
        var years = referenceDate.Year - dateOfBirth.Year;
        if (referenceDate.Month < dateOfBirth.Month || (referenceDate.Month == dateOfBirth.Month && referenceDate.Day < dateOfBirth.Day))
            --years;
        return years;
    }

    /// <summary>
    /// 	Returns the number of days in the month of the provided date.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <returns>The number of days.</returns>
    public static int GetCountDaysOfMonth (this DateTime date) {
        var nextMonth = date.AddMonths (1);
        return new DateTime (nextMonth.Year, nextMonth.Month, 1).AddDays (-1).Day;
    }

    /// <summary>
    /// 	Returns the first day of the month of the provided date.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <returns>The first day of the month</returns>
    public static DateTime FirstDayOfMonth (this DateTime date) {
        return new DateTime (date.Year, date.Month, 1);
    }

    /// <summary>
    /// 	Returns the first day of the month of the provided date.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <param name = "dayOfWeek">The desired day of week.</param>
    /// <returns>The first day of the month</returns>
    public static DateTime FirstDayOfMonth (this DateTime date, DayOfWeek dayOfWeek) {
        var dt = date.FirstDayOfMonth ();
        while (dt.DayOfWeek != dayOfWeek)
            dt = dt.AddDays (1);
        return dt;
    }

    /// <summary>
    /// 	Returns the last day of the month of the provided date.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <returns>The last day of the month.</returns>
    public static DateTime LastDayOfMonth (this DateTime date) {
        return new DateTime (date.Year, date.Month, GetCountDaysOfMonth (date));
    }

    /// <summary>
    /// 	Returns the last day of the month of the provided date.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <param name = "dayOfWeek">The desired day of week.</param>
    /// <returns>The date time</returns>
    public static DateTime LastDayOfMonth (this DateTime date, DayOfWeek dayOfWeek) {
        var dt = date.LastDayOfMonth ();
        while (dt.DayOfWeek != dayOfWeek)
            dt = dt.AddDays (-1);
        return dt;
    }

    /// <summary>
    /// 取每季度的第一/最末一天
    /// </summary>
    /// <param name="time">传入时间</param>
    /// <param name="firstDay">第一天还是最末一天</param>
    /// <returns></returns>
    public static DateTime LastDayOfQuarter (this DateTime time) {
        int m = 0;
        switch (time.Month) {
            case 1:
            case 2:
            case 3:
                m = 1;
                break;
            case 4:
            case 5:
            case 6:
                m = 4;
                break;
            case 7:
            case 8:
            case 9:
                m = 7;
                break;
            case 10:
            case 11:
            case 12:
                m = 11;
                break;
        }

        DateTime time1 = new DateTime (time.Year, m, 1);
        return time1.AddMonths (3).AddDays (-1);
    }
    public static DateTime FirstDayOfQuarter (this DateTime time) {
        int m = 0;
        switch (time.Month) {
            case 1:
            case 2:
            case 3:
                m = 1;
                break;
            case 4:
            case 5:
            case 6:
                m = 4;
                break;
            case 7:
            case 8:
            case 9:
                m = 7;
                break;
            case 10:
            case 11:
            case 12:
                m = 11;
                break;
        }

        DateTime time1 = new DateTime (time.Year, m, 1);
        return time1;
    }
    /// <summary>
    /// 	Indicates whether the date is today.
    /// </summary>
    /// <param name = "dt">The date.</param>
    /// <returns>
    /// 	<c>true</c> if the specified date is today; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsToday (this DateTime dt) {
        return (dt.Date == DateTime.Today);
    }

    /// <summary>
    /// 	Sets the time on the specified DateTime value.
    /// </summary>
    /// <param name = "date">The base date.</param>
    /// <param name = "hours">The hours to be set.</param>
    /// <param name = "minutes">The minutes to be set.</param>
    /// <param name = "seconds">The seconds to be set.</param>
    /// <returns>The DateTime including the new time value</returns>
    public static DateTime SetTime (this DateTime date, int hours, int minutes, int seconds) {
        return date.SetTime (new TimeSpan (hours, minutes, seconds));
    }

    /// <summary>
    /// 	Sets the time on the specified DateTime value.
    /// </summary>
    /// <param name = "date">The base date.</param>
    /// <param name="hours">The hour</param>
    /// <param name="minutes">The minute</param>
    /// <param name="seconds">The second</param>
    /// <param name="milliseconds">The millisecond</param>
    /// <returns>The DateTime including the new time value</returns>
    /// <remarks>Added overload for milliseconds - jtolar</remarks>
    public static DateTime SetTime (this DateTime date, int hours, int minutes, int seconds, int milliseconds) {
        return date.SetTime (new TimeSpan (0, hours, minutes, seconds, milliseconds));
    }

    /// <summary>
    /// 	Sets the time on the specified DateTime value.
    /// </summary>
    /// <param name = "date">The base date.</param>
    /// <param name = "time">The TimeSpan to be applied.</param>
    /// <returns>
    /// 	The DateTime including the new time value
    /// </returns>
    public static DateTime SetTime (this DateTime date, TimeSpan time) {
        return date.Date.Add (time);
    }

    /// <summary>
    /// 	Converts a DateTime into a DateTimeOffset using the local system time zone.
    /// </summary>
    /// <param name = "localDateTime">The local DateTime.</param>
    /// <returns>The converted DateTimeOffset</returns>
    public static DateTimeOffset ToDateTimeOffset (this DateTime localDateTime) {
        return ToDateTimeOffset (localDateTime, null);
    }

    /// <summary>
    /// 	Converts a DateTime into a DateTimeOffset using the specified time zone.
    /// </summary>
    /// <param name = "localDateTime">The local DateTime.</param>
    /// <param name = "localTimeZone">The local time zone.</param>
    /// <returns>The converted DateTimeOffset</returns>
    public static DateTimeOffset ToDateTimeOffset (this DateTime localDateTime, TimeZoneInfo localTimeZone) {
        localTimeZone = (localTimeZone ?? TimeZoneInfo.Local);

        if (localDateTime.Kind != DateTimeKind.Unspecified)
            localDateTime = new DateTime (localDateTime.Ticks, DateTimeKind.Unspecified);

        return TimeZoneInfo.ConvertTimeToUtc (localDateTime, localTimeZone);
    }

    /// <summary>
    /// 	Gets the first day of the week using the current culture.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <returns>The first day of the week</returns>
    /// <remarks>
    ///     modified by jtolar to implement culture settings
    /// </remarks>
    public static DateTime FirstDayOfWeek (this DateTime date) {
        return date.FirstDayOfWeek (ExtensionMethodSetting.DefaultCulture);
    }

    /// <summary>
    /// 	Gets the first day of the week using the specified culture.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <param name = "cultureInfo">The culture to determine the first weekday of a week.</param>
    /// <returns>The first day of the week</returns>
    public static DateTime FirstDayOfWeek (this DateTime date, CultureInfo cultureInfo) {
        cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

        var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
        while (date.DayOfWeek != firstDayOfWeek)
            date = date.AddDays (-1);

        return date;
    }

    /// <summary>
    /// 	Gets the last day of the week using the current culture.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <returns>The first day of the week</returns>
    /// <remarks>
    ///     modified by jtolar to implement culture settings
    /// </remarks>
    public static DateTime LastDayOfWeek (this DateTime date) {
        return date.LastDayOfWeek (ExtensionMethodSetting.DefaultCulture);
    }

    /// <summary>
    /// 	Gets the last day of the week using the specified culture.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <param name = "cultureInfo">The culture to determine the first weekday of a week.</param>
    /// <returns>The first day of the week</returns>
    public static DateTime LastDayOfWeek (this DateTime date, CultureInfo cultureInfo) {
        return date.FirstDayOfWeek (cultureInfo).AddDays (6);
    }

    /// <summary>
    /// 	Gets the next occurence of the specified weekday within the current week using the current culture.
    /// </summary>
    /// <param name = "date">The base date.</param>
    /// <param name = "weekday">The desired weekday.</param>
    /// <returns>The calculated date.</returns>
    /// <example>
    /// 	<code>
    /// 		var thisWeeksMonday = DateTime.Now.GetWeekday(DayOfWeek.Monday);
    /// 	</code>
    /// </example>
    /// <remarks>
    ///     modified by jtolar to implement culture settings
    /// </remarks>
    public static DateTime GetWeeksWeekday (this DateTime date, DayOfWeek weekday) {
        return date.GetWeeksWeekday (weekday, ExtensionMethodSetting.DefaultCulture);
    }

    /// <summary>
    /// 	Gets the next occurence of the specified weekday within the current week using the specified culture.
    /// </summary>
    /// <param name = "date">The base date.</param>
    /// <param name = "weekday">The desired weekday.</param>
    /// <param name = "cultureInfo">The culture to determine the first weekday of a week.</param>
    /// <returns>The calculated date.</returns>
    /// <example>
    /// 	<code>
    /// 		var thisWeeksMonday = DateTime.Now.GetWeekday(DayOfWeek.Monday);
    /// 	</code>
    /// </example>
    public static DateTime GetWeeksWeekday (this DateTime date, DayOfWeek weekday, CultureInfo cultureInfo) {
        var firstDayOfWeek = date.FirstDayOfWeek (cultureInfo);
        return firstDayOfWeek.GetNextWeekday (weekday);
    }

    /// <summary>
    /// 	Gets the next occurence of the specified weekday.
    /// </summary>
    /// <param name = "date">The base date.</param>
    /// <param name = "weekday">The desired weekday.</param>
    /// <returns>The calculated date.</returns>
    /// <example>
    /// 	<code>
    /// 		var lastMonday = DateTime.Now.GetNextWeekday(DayOfWeek.Monday);
    /// 	</code>
    /// </example>
    public static DateTime GetNextWeekday (this DateTime date, DayOfWeek weekday) {
        while (date.DayOfWeek != weekday)
            date = date.AddDays (1);
        return date;
    }

    /// <summary>
    /// 	Gets the previous occurence of the specified weekday.
    /// </summary>
    /// <param name = "date">The base date.</param>
    /// <param name = "weekday">The desired weekday.</param>
    /// <returns>The calculated date.</returns>
    /// <example>
    /// 	<code>
    /// 		var lastMonday = DateTime.Now.GetPreviousWeekday(DayOfWeek.Monday);
    /// 	</code>
    /// </example>
    public static DateTime GetPreviousWeekday (this DateTime date, DayOfWeek weekday) {
        while (date.DayOfWeek != weekday)
            date = date.AddDays (-1);
        return date;
    }

    /// <summary>
    /// 	Determines whether the date only part of twi DateTime values are equal.
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <param name = "dateToCompare">The date to compare with.</param>
    /// <returns>
    /// 	<c>true</c> if both date values are equal; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsDateEqual (this DateTime date, DateTime dateToCompare) {
        return (date.Date == dateToCompare.Date);
    }

    /// <summary>
    /// 	Determines whether the time only part of two DateTime values are equal.
    /// </summary>
    /// <param name = "time">The time.</param>
    /// <param name = "timeToCompare">The time to compare.</param>
    /// <returns>
    /// 	<c>true</c> if both time values are equal; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsTimeEqual (this DateTime time, DateTime timeToCompare) {
        return (time.TimeOfDay == timeToCompare.TimeOfDay);
    }

    /// <summary>
    /// 	Get milliseconds of UNIX area. This is the milliseconds since 1/1/1970
    /// </summary>
    /// <param name = "datetime">Up to which time.</param>
    /// <returns>number of milliseconds.</returns>
    /// <remarks>
    /// </remarks>
    public static long GetMillisecondsSince1970 (this DateTime datetime) {
        var ts = datetime.Subtract (Date1970);
        return (long) ts.TotalMilliseconds;
    }

    /// <summary>
    /// Get milliseconds of UNIX area. This is the milliseconds since 1/1/1970
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns></returns>
    /// <remarks>This is the same as GetMillisecondsSince1970 but more descriptive</remarks>
    public static long ToUnixEpoch (this DateTime dateTime) {
        return GetMillisecondsSince1970 (dateTime);
    }
    /// <summary>
    /// Get seconds of UNIX area. This is the milliseconds since 1/1/1970
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long ToUnixTime (this DateTime dateTime) {
        var ts = dateTime.Subtract (Date1970);
        return (long) (ts.TotalSeconds);
    }

    /// <summary>
    /// 	Indicates whether the specified date is a weekend (Saturday or Sunday).
    /// </summary>
    /// <param name = "date">The date.</param>
    /// <returns>
    /// 	<c>true</c> if the specified date is a weekend; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsWeekend (this DateTime date) {
        return date.DayOfWeek.EqualsAny (DayOfWeek.Saturday, DayOfWeek.Sunday);
    }

    /// <summary>
    /// 	Adds the specified amount of weeks (=7 days gregorian calendar) to the passed date value.
    /// </summary>
    /// <param name = "date">The origin date.</param>
    /// <param name = "value">The amount of weeks to be added.</param>
    /// <returns>The enw date value</returns>
    public static DateTime AddWeeks (this DateTime date, int value) {
        return date.AddDays (value * 7);
    }

    ///<summary>
    ///	Get the number of days within that year.
    ///</summary>
    ///<param name = "year">The year.</param>
    ///<returns>the number of days within that year</returns>
    /// <remarks>

    ///     Modified by JTolar to implement Culture Settings
    /// </remarks>
    public static int GetDays (int year) {
        return GetDays (year, ExtensionMethodSetting.DefaultCulture);
    }

    ///<summary>
    ///	Get the number of days within that year. Uses the culture specified.
    ///</summary>
    ///<param name = "year">The year.</param>
    ///<param name="culture">Specific culture</param>
    ///<returns>the number of days within that year</returns>
    /// <remarks>

    ///     Modified by JTolar to implement Culture Settings
    /// </remarks>
    public static int GetDays (int year, CultureInfo culture) {
        var first = new DateTime (year, 1, 1, culture.Calendar);
        var last = new DateTime (year + 1, 1, 1, culture.Calendar);
        return GetDays (first, last);
    }

    ///<summary>
    ///	Get the number of days within that date year. Allows user to specify culture.
    ///</summary>
    ///<param name = "date">The date.</param>
    ///<param name="culture">Specific culture</param>
    ///<returns>the number of days within that year</returns>
    /// <remarks>

    ///     Modified by JTolar to implement Culture Settings 
    /// </remarks>
    public static int GetDays (this DateTime date) {
        return GetDays (date.Year, ExtensionMethodSetting.DefaultCulture);
    }

    ///<summary>
    ///	Get the number of days within that date year. Allows user to specify culture
    ///</summary>
    ///<param name = "date">The date.</param>
    ///<param name="culture">Specific culture</param>
    ///<returns>the number of days within that year</returns>
    /// <remarks>

    ///     Modified by JTolar to implement Culture Settings 
    /// </remarks>
    public static int GetDays (this DateTime date, CultureInfo culture) {
        return GetDays (date.Year, culture);
    }

    ///<summary>
    ///	Get the number of days between two dates.
    ///</summary>
    ///<param name = "fromDate">The origin year.</param>
    ///<param name = "toDate">To year</param>
    ///<returns>The number of days between the two years</returns>
    /// <remarks>

    /// </remarks>
    public static int GetDays (this DateTime fromDate, DateTime toDate) {
        return Convert.ToInt32 (toDate.Subtract (fromDate).TotalDays);
    }

    ///<summary>
    ///	Return a period "Morning", "Afternoon", or "Evening"
    ///</summary>
    ///<param name = "date">The date.</param>
    ///<returns>The period "morning", "afternoon", or "evening"</returns>
    /// <remarks>

    /// </remarks>
    public static string GetPeriodOfDay (this DateTime date) {
        var hour = date.Hour;
        if (hour < EveningEnds)
            return "evening";
        if (hour < MorningEnds)
            return "morning";
        return hour < AfternoonEnds ? "afternoon" : "evening";
    }

    /// <summary>
    /// Gets the week number for a provided date time value based on a specific culture.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <param name="culture">Specific culture</param>
    /// <returns>The week number</returns>
    /// <remarks>
    ///     modified by jtolar to implement culture settings
    /// </remarks>
    public static int GetWeekOfYear (this DateTime dateTime, CultureInfo culture) {
        var calendar = culture.Calendar;
        var dateTimeFormat = culture.DateTimeFormat;

        return calendar.GetWeekOfYear (dateTime, dateTimeFormat.CalendarWeekRule, dateTimeFormat.FirstDayOfWeek);
    }

    /// <summary>
    /// Gets the week number for a provided date time value based on the current culture settings. 
    /// Uses DefaultCulture from ExtensionMethodSetting
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <returns>The week number</returns>
    /// <remarks>
    ///     modified by jtolar to implement culture settings
    /// </remarks>
    public static int GetWeekOfYear (this DateTime dateTime) {
        return GetWeekOfYear (dateTime, ExtensionMethodSetting.DefaultCulture);
    }

    /// <summary>
    ///     Indicates whether the specified date is Easter in the Christian calendar.
    /// </summary>
    /// <param name="date">Instance value.</param>
    /// <returns>True if the instance value is a valid Easter Date.</returns>
    public static bool IsEaster (this DateTime date) {
        int Y = date.Year;
        int a = Y % 19;
        int b = Y / 100;
        int c = Y % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int L = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * L) / 451;
        int Month = (h + L - 7 * m + 114) / 31;
        int Day = ((h + L - 7 * m + 114) % 31) + 1;

        DateTime dtEasterSunday = new DateTime (Y, Month, Day);

        return date == dtEasterSunday;
    }

    /// <summary>
    ///     Indicates whether the source DateTime is before the supplied DateTime.
    /// </summary>
    /// <param name="source">The source DateTime.</param>
    /// <param name="other">The compared DateTime.</param>
    /// <returns>True if the source is before the other DateTime, False otherwise</returns>
    public static bool IsBefore (this DateTime source, DateTime other) {
        return source.CompareTo (other) < 0;
    }

    /// <summary>
    ///     Indicates whether the source DateTime is before the supplied DateTime.
    /// </summary>
    /// <param name="source">The source DateTime.</param>
    /// <param name="other">The compared DateTime.</param>
    /// <returns>True if the source is before the other DateTime, False otherwise</returns>
    public static bool IsAfter (this DateTime source, DateTime other) {
        return source.CompareTo (other) > 0;
    }

    /// <summary>
    /// The ToFriendlyString() method represents dates in a user friendly way. 
    /// For example, when displaying a news article on a webpage, you might want 
    /// articles that were published one day ago to have their publish dates 
    /// represented as "yesterday at 12:30 PM". Or if the article was publish today, 
    /// show the date as "Today, 3:33 PM".
    /// </summary>
    /// <param name="date">The date.</param>
    /// <param name="culture">Specific Culture</param>
    /// <returns>string</returns>
    /// <remarks>
    ///     modified by jtolar to implement culture settings
    /// </remarks>/// <remarks></remarks>
    public static string ToFriendlyDateString (this DateTime date, CultureInfo culture) {
        var sbFormattedDate = new StringBuilder ();
        if (date.Date == DateTime.Today) {
            sbFormattedDate.Append ("Today");
        } else if (date.Date == DateTime.Today.AddDays (-1)) {
            sbFormattedDate.Append ("Yesterday");
        } else if (date.Date > DateTime.Today.AddDays (-6)) {
            // *** Show the Day of the week
            sbFormattedDate.Append (date.ToString ("dddd").ToString (culture));
        } else {
            sbFormattedDate.Append (date.ToString ("MMMM dd, yyyy").ToString (culture));
        }

        //append the time portion to the output
        sbFormattedDate.Append (" at ").Append (date.ToString ("t").ToLower ());
        return sbFormattedDate.ToString ();
    }

    ///<summary>
    /// The ToFriendlyString() method represents dates in a user friendly way. 
    /// For example, when displaying a news article on a webpage, you might want 
    /// articles that were published one day ago to have their publish dates 
    /// represented as "yesterday at 12:30 PM". Or if the article was publish today, 
    /// show the date as "Today, 3:33 PM". Uses DefaultCulture from ExtensionMethodSetting.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <returns>string</returns>
    /// <remarks>
    ///     modified by jtolar to implement culture settings
    /// </remarks>/// <remarks></remarks>
    public static string ToFriendlyDateString (this DateTime date) {
        return ToFriendlyDateString (date, ExtensionMethodSetting.DefaultCulture);
    }

    /// <summary>
    /// Returns the date at 23:59.59.999 for the specified DateTime
    /// </summary>
    /// <param name="date">The DateTime to be processed</param>
    /// <returns>The date at 23:50.59.999</returns>
    public static DateTime EndOfDay (this DateTime date) {
        return date.SetTime (23, 59, 59, 999);
    }

    /// <summary>
    /// Returns the date at 12:00:00 for the specified DateTime
    /// </summary>
    /// <param name="time">The current date</param>
    public static DateTime Noon (this DateTime time) {
        return time.SetTime (12, 0, 0);
    }

    /// <summary>
    /// Returns the date at 00:00:00 for the specified DateTime
    /// </summary>
    /// <param name="time">The current date</param>
    public static DateTime Midnight (this DateTime time) {
        return time.SetTime (0, 0, 0, 0);
    }

    /// <summary>
    /// Returns whether the DateTime falls on a weekday
    /// </summary>
    /// <param name="date">The date to be processed</param>
    /// <returns>Whether the specified date occurs on a weekday</returns>
    public static bool IsWeekDay (this DateTime date) {
        return !date.IsWeekend ();
    }

    #region Z.EXT
    /// <summary>
    ///     A DateTime extension method that yesterdays the given this.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>Yesterday date at same time.</returns>
    public static DateTime Yesterday (this DateTime @this) {
        return @this.AddDays (-1);
    }
    /// <summary>
    ///     A DateTime extension method that tomorrows the given this.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>Tomorrow date at same time.</returns>
    public static DateTime Tomorrow (this DateTime @this) {
        return @this.AddDays (1);
    }
    /// <summary>
    ///     A DateTime extension method that converts the @this to an epoch time span.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>@this as a TimeSpan.</returns>
    public static TimeSpan ToEpochTimeSpan (this DateTime @this) {
        return @this.Subtract (new DateTime (1970, 1, 1));
    }
    /// <summary>
    ///     A DateTime extension method that return a DateTime of the first day of the year with the time set to
    ///     "00:00:00:000". The first moment of the first day of the year.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A DateTime of the first day of the year with the time set to "00:00:00:000".</returns>
    public static DateTime StartOfYear (this DateTime @this) {
        return new DateTime (@this.Year, 1, 1);
    }
    /// <summary>
    ///     A DateTime extension method that starts of week.
    /// </summary>
    /// <param name="dt">The dt to act on.</param>
    /// <param name="startDayOfWeek">(Optional) the start day of week.</param>
    /// <returns>A DateTime.</returns>
    public static DateTime StartOfWeek (this DateTime dt, DayOfWeek startDayOfWeek = DayOfWeek.Sunday) {
        var start = new DateTime (dt.Year, dt.Month, dt.Day);

        if (start.DayOfWeek != startDayOfWeek) {
            int d = startDayOfWeek - start.DayOfWeek;
            if (startDayOfWeek <= start.DayOfWeek) {
                return start.AddDays (d);
            }
            return start.AddDays (-7 + d);
        }

        return start;
    }
    /// <summary>
    ///     A DateTime extension method that return a DateTime of the first day of the month with the time set to
    ///     "00:00:00:000". The first moment of the first day of the month.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A DateTime of the first day of the month with the time set to "00:00:00:000".</returns>
    public static DateTime StartOfMonth (this DateTime @this) {
        return new DateTime (@this.Year, @this.Month, 1);
    }
    /// <summary>
    ///     A DateTime extension method that return a DateTime with the time set to "00:00:00:000". The first moment of
    ///     the day.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A DateTime of the day with the time set to "00:00:00:000".</returns>
    public static DateTime StartOfDay (this DateTime @this) {
        return new DateTime (@this.Year, @this.Month, @this.Day);
    }

    /// <summary>
    ///     A DateTime extension method that query if '@this' is in the past.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if the value is in the past, false if not.</returns>
    public static bool IsPast (this DateTime @this) {
        return @this < DateTime.Now;
    }
    /// <summary>
    ///     A DateTime extension method that query if '@this' is now.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if now, false if not.</returns>
    public static bool IsNow (this DateTime @this) {
        return @this == DateTime.Now;
    }
    /// <summary>
    ///     A DateTime extension method that query if '@this' is morning.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if morning, false if not.</returns>
    public static bool IsMorning (this DateTime @this) {
        return @this.TimeOfDay < new DateTime (2000, 1, 1, 12, 0, 0).TimeOfDay;
    }
    /// <summary>
    ///     A DateTime extension method that query if '@this' is in the future.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if the value is in the future, false if not.</returns>
    public static bool IsFuture (this DateTime @this) {
        return @this > DateTime.Now;
    }
    /// <summary>
    ///     A DateTime extension method that query if '@this' is afternoon.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if afternoon, false if not.</returns>
    public static bool IsAfternoon (this DateTime @this) {
        return @this.TimeOfDay >= new DateTime (2000, 1, 1, 12, 0, 0).TimeOfDay;
    }

    /// <summary>
    ///     A DateTime extension method that return a DateTime of the last day of the year with the time set to
    ///     "23:59:59:999". The last moment of the last day of the year.  Use "DateTime2" column type in sql to keep the
    ///     precision.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A DateTime of the last day of the year with the time set to "23:59:59:999".</returns>
    public static DateTime EndOfYear (this DateTime @this) {
        return new DateTime (@this.Year, 1, 1).AddYears (1).Subtract (new TimeSpan (0, 0, 0, 0, 1));
    }
    /// <summary>
    ///     A System.DateTime extension method that ends of week.
    /// </summary>
    /// <param name="dt">Date/Time of the dt.</param>
    /// <param name="startDayOfWeek">(Optional) the start day of week.</param>
    /// <returns>A DateTime.</returns>
    public static DateTime EndOfWeek (this DateTime dt, DayOfWeek startDayOfWeek = DayOfWeek.Sunday) {
        DateTime end = dt;
        DayOfWeek endDayOfWeek = startDayOfWeek - 1;
        if (endDayOfWeek < 0) {
            endDayOfWeek = DayOfWeek.Saturday;
        }

        if (end.DayOfWeek != endDayOfWeek) {
            if (endDayOfWeek < end.DayOfWeek) {
                end = end.AddDays (7 - (end.DayOfWeek - endDayOfWeek));
            } else {
                end = end.AddDays (endDayOfWeek - end.DayOfWeek);
            }
        }

        return new DateTime (end.Year, end.Month, end.Day, 23, 59, 59, 999);
    }
    /// <summary>
    ///     A DateTime extension method that return a DateTime of the last day of the month with the time set to
    ///     "23:59:59:999". The last moment of the last day of the month.  Use "DateTime2" column type in sql to keep the
    ///     precision.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A DateTime of the last day of the month with the time set to "23:59:59:999".</returns>
    public static DateTime EndOfMonth (this DateTime @this) {
        return new DateTime (@this.Year, @this.Month, 1).AddMonths (1).Subtract (new TimeSpan (0, 0, 0, 0, 1));
    }

    /// <summary>
    ///     A DateTime extension method that elapsed the given datetime.
    /// </summary>
    /// <param name="datetime">The datetime to act on.</param>
    /// <returns>A TimeSpan.</returns>
    public static TimeSpan Elapsed (this DateTime datetime) {
        return DateTime.Now - datetime;
    }
    /// <summary>
    ///     A DateTime extension method that ages the given this.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>An int.</returns>
    public static int Age (this DateTime @this) {
        if (DateTime.Today.Month < @this.Month ||
            DateTime.Today.Month == @this.Month &&
            DateTime.Today.Day < @this.Day) {
            return DateTime.Today.Year - @this.Year - 1;
        }
        return DateTime.Today.Year - @this.Year;
    }
    /// <summary>
    ///     A T extension method that check if the value is between (exclusif) the minValue and maxValue.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>true if the value is between the minValue and maxValue, otherwise false.</returns>
    /// ###
    /// <typeparam name="T">Generic type parameter.</typeparam>
    public static bool Between (this DateTime @this, DateTime minValue, DateTime maxValue) {
        return minValue.CompareTo (@this) == -1 && @this.CompareTo (maxValue) == -1;
    }
    /// <summary>
    ///     A T extension method to determines whether the object is equal to any of the provided values.
    /// </summary>
    /// <param name="this">The object to be compared.</param>
    /// <param name="values">The value list to compare with the object.</param>
    /// <returns>true if the values list contains the object, else false.</returns>
    /// ###
    /// <typeparam name="T">Generic type parameter.</typeparam>
    public static bool In (this DateTime @this, params DateTime[] values) {
        return Array.IndexOf (values, @this) != -1;
    }
    /// <summary>
    ///     A T extension method that check if the value is between inclusively the minValue and maxValue.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="minValue">The minimum value.</param>
    /// <param name="maxValue">The maximum value.</param>
    /// <returns>true if the value is between inclusively the minValue and maxValue, otherwise false.</returns>
    /// ###
    /// <typeparam name="T">Generic type parameter.</typeparam>
    public static bool InRange (this DateTime @this, DateTime minValue, DateTime maxValue) {
        return @this.CompareTo (minValue) >= 0 && @this.CompareTo (maxValue) <= 0;
    }
    /// <summary>
    ///     A T extension method to determines whether the object is not equal to any of the provided values.
    /// </summary>
    /// <param name="this">The object to be compared.</param>
    /// <param name="values">The value list to compare with the object.</param>
    /// <returns>true if the values list doesn't contains the object, else false.</returns>
    /// ###
    /// <typeparam name="T">Generic type parameter.</typeparam>
    public static bool NotIn (this DateTime @this, params DateTime[] values) {
        return Array.IndexOf (values, @this) == -1;
    }
    /// <summary>
    ///     Converts the current date and time to Coordinated Universal Time (UTC).
    /// </summary>
    /// <param name="dateTime">The date and time to convert.</param>
    /// <returns>
    ///     The Coordinated Universal Time (UTC) that corresponds to the  parameter. The  value&#39;s  property is always
    ///     set to .
    /// </returns>
    public static DateTime ConvertTimeToUtc (this DateTime dateTime) {
        return TimeZoneInfo.ConvertTimeToUtc (dateTime);
    }

    /// <summary>
    ///     Converts the time in a specified time zone to Coordinated Universal Time (UTC).
    /// </summary>
    /// <param name="dateTime">The date and time to convert.</param>
    /// <param name="sourceTimeZone">The time zone of .</param>
    /// <returns>
    ///     The Coordinated Universal Time (UTC) that corresponds to the  parameter. The  object&#39;s  property is
    ///     always set to .
    /// </returns>
    public static DateTime ConvertTimeToUtc (this DateTime dateTime, TimeZoneInfo sourceTimeZone) {
        return TimeZoneInfo.ConvertTimeToUtc (dateTime, sourceTimeZone);
    }
    /// <summary>
    ///     Converts a Coordinated Universal Time (UTC) to the time in a specified time zone.
    /// </summary>
    /// <param name="dateTime">The Coordinated Universal Time (UTC).</param>
    /// <param name="destinationTimeZone">The time zone to convert  to.</param>
    /// <returns>
    ///     The date and time in the destination time zone. Its  property is  if  is ; otherwise, its  property is .
    /// </returns>
    public static DateTime ConvertTimeFromUtc (this DateTime dateTime, TimeZoneInfo destinationTimeZone) {
        return TimeZoneInfo.ConvertTimeFromUtc (dateTime, destinationTimeZone);
    }
    /// <summary>
    ///     Converts a time to the time in another time zone based on the time zone&#39;s identifier.
    /// </summary>
    /// <param name="dateTime">The date and time to convert.</param>
    /// <param name="destinationTimeZoneId">The identifier of the destination time zone.</param>
    /// <returns>The date and time in the destination time zone.</returns>
    public static DateTime ConvertTimeBySystemTimeZoneId (this DateTime dateTime, String destinationTimeZoneId) {
        return TimeZoneInfo.ConvertTimeBySystemTimeZoneId (dateTime, destinationTimeZoneId);
    }

    /// <summary>
    ///     Converts a time from one time zone to another based on time zone identifiers.
    /// </summary>
    /// <param name="dateTime">The date and time to convert.</param>
    /// <param name="sourceTimeZoneId">The identifier of the source time zone.</param>
    /// <param name="destinationTimeZoneId">The identifier of the destination time zone.</param>
    /// <returns>
    ///     The date and time in the destination time zone that corresponds to the  parameter in the source time zone.
    /// </returns>
    public static DateTime ConvertTimeBySystemTimeZoneId (this DateTime dateTime, String sourceTimeZoneId, String destinationTimeZoneId) {
        return TimeZoneInfo.ConvertTimeBySystemTimeZoneId (dateTime, sourceTimeZoneId, destinationTimeZoneId);
    }
    /// <summary>
    ///     Converts a time to the time in a particular time zone.
    /// </summary>
    /// <param name="dateTime">The date and time to convert.</param>
    /// <param name="destinationTimeZone">The time zone to convert  to.</param>
    /// <returns>The date and time in the destination time zone.</returns>
    public static DateTime ConvertTime (this DateTime dateTime, TimeZoneInfo destinationTimeZone) {
        return TimeZoneInfo.ConvertTime (dateTime, destinationTimeZone);
    }

    /// <summary>
    ///     Converts a time from one time zone to another.
    /// </summary>
    /// <param name="dateTime">The date and time to convert.</param>
    /// <param name="sourceTimeZone">The time zone of .</param>
    /// <param name="destinationTimeZone">The time zone to convert  to.</param>
    /// <returns>
    ///     The date and time in the destination time zone that corresponds to the  parameter in the source time zone.
    /// </returns>
    public static DateTime ConvertTime (this DateTime dateTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone) {
        return TimeZoneInfo.ConvertTime (dateTime, sourceTimeZone, destinationTimeZone);
    }

    /// <summary>
    /// Gets a TimeSpan from a double number of days.
    /// </summary>
    /// <param name="days">The number of days the TimeSpan will contain.</param>
    /// <returns>A TimeSpan containing the specified number of days.</returns>
    /// <remarks>
    ///		Contributed by jceddy
    /// </remarks>
    public static TimeSpan Days (this double days) {
        return TimeSpan.FromDays (days);
    }

    /// <summary>
    /// Gets a TimeSpan from a double number of hours.
    /// </summary>
    /// <param name="days">The number of hours the TimeSpan will contain.</param>
    /// <returns>A TimeSpan containing the specified number of hours.</returns>
    /// <remarks>
    ///		Contributed by jceddy
    /// </remarks>
    public static TimeSpan Hours (this double hours) {
        return TimeSpan.FromHours (hours);
    }

    /// <summary>
    /// Gets a TimeSpan from a double number of milliseconds.
    /// </summary>
    /// <param name="days">The number of milliseconds the TimeSpan will contain.</param>
    /// <returns>A TimeSpan containing the specified number of milliseconds.</returns>
    /// <remarks>
    ///		Contributed by jceddy
    /// </remarks>
    public static TimeSpan Milliseconds (this double milliseconds) {
        return TimeSpan.FromMilliseconds (milliseconds);
    }

    /// <summary>
    /// Gets a TimeSpan from a double number of minutes.
    /// </summary>
    /// <param name="days">The number of minutes the TimeSpan will contain.</param>
    /// <returns>A TimeSpan containing the specified number of minutes.</returns>
    /// <remarks>
    ///		Contributed by jceddy
    /// </remarks>
    public static TimeSpan Minutes (this double minutes) {
        return TimeSpan.FromMinutes (minutes);
    }

    /// <summary>
    /// Gets a TimeSpan from a double number of seconds.
    /// </summary>
    /// <param name="days">The number of seconds the TimeSpan will contain.</param>
    /// <returns>A TimeSpan containing the specified number of seconds.</returns>
    /// <remarks>
    ///		Contributed by jceddy
    /// </remarks>
    public static TimeSpan Seconds (this double seconds) {
        return TimeSpan.FromSeconds (seconds);
    }
    #endregion

    #region 判断今天是第几周
    /// <summary>
    /// 判断今天是一年中第几日
    /// </summary>
    /// <param name="dateSrc"></param>
    /// <returns></returns>
    public static int DayInYear (this DateTime dateSrc) {
        return dateSrc.DayOfYear;
    }
    /// <summary>
    /// 判断今天是一年中第几日
    /// </summary>
    /// <param name="dateSrc"></param>
    /// <returns></returns>
    public static string ToStringOfWeekChinese (this DateTime dateSrc) {
        DayOfWeek day = dateSrc.DayOfWeek;
        string strTMP = "";
        switch (day) {
            case DayOfWeek.Friday:
                strTMP = "星期五";
                break;
            case DayOfWeek.Monday:
                strTMP = "星期一";
                break;
            case DayOfWeek.Saturday:
                strTMP = "星期六";
                break;
            case DayOfWeek.Sunday:
                strTMP = "星期日";
                break;
            case DayOfWeek.Thursday:
                strTMP = "星期四";
                break;
            case DayOfWeek.Tuesday:
                strTMP = "星期二";
                break;
            case DayOfWeek.Wednesday:
                strTMP = "星期三";
                break;
            default:
                strTMP = "星期一";
                break;
        }
        return strTMP;
    }
    public static int DayInWeek (this DateTime dateSrc) {
        DayOfWeek day = dateSrc.DayOfWeek;
        int iTMP = -1;
        switch (day) {
            case DayOfWeek.Friday:
                iTMP = 5;
                break;
            case DayOfWeek.Monday:
                iTMP = 1;
                break;
            case DayOfWeek.Saturday:
                iTMP = 6;
                break;
            case DayOfWeek.Sunday:
                iTMP = 7;
                break;
            case DayOfWeek.Thursday:
                iTMP = 4;
                break;
            case DayOfWeek.Tuesday:
                iTMP = 2;
                break;
            case DayOfWeek.Wednesday:
                iTMP = 3;
                break;
            default:
                iTMP = 1;
                break;
        }
        return iTMP;
    }

    /// <summary>
    /// 判断今天是一年中第几周
    /// </summary>
    /// <param name="dateSrc"></param>
    /// <returns></returns>
    public static int WeekInYear (this DateTime dateSrc) {
        DateTime firstDay = new DateTime (dateSrc.Year, 1, 1).Date;
        int theday;
        switch (firstDay.DayOfWeek) {
            case DayOfWeek.Monday:
                theday = -1;
                break;
            case DayOfWeek.Tuesday:
                theday = 0;
                break;
            case DayOfWeek.Wednesday:
                theday = 1;
                break;
            case DayOfWeek.Thursday:
                theday = 2;
                break;
            case DayOfWeek.Friday:
                theday = 3;
                break;
            case DayOfWeek.Saturday:
                theday = 4;
                break;
            default:
                theday = 5;
                break;
        }
        int weekNum = (dateSrc.DayOfYear + theday) / 7 + 1;
        return weekNum;
    }
    /// <summary>
    /// 指定日期所在一周日期范围
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime[] InWeekRange (this DateTime date) {
        int iYearNum = date.Year;
        int iWeekNum = date.WeekInYear ();
        DateTime firstOfYear = new DateTime (iYearNum, 1, 1);
        System.DayOfWeek dayofweek = firstOfYear.DayOfWeek;
        DateTime stand = firstOfYear.AddDays (iWeekNum * 7);
        DateTime start = default (DateTime);
        DateTime end = default (DateTime);
        switch (dayofweek) {
            case DayOfWeek.Monday:
                start = stand.AddDays (0);
                end = stand.AddDays (6);
                break;
            case DayOfWeek.Tuesday:
                start = stand.AddDays (-1);
                end = stand.AddDays (5);
                break;
            case DayOfWeek.Wednesday:
                start = stand.AddDays (-2);
                end = stand.AddDays (4);
                break;
            case DayOfWeek.Thursday:
                start = stand.AddDays (-3);
                end = stand.AddDays (3);
                break;
            case DayOfWeek.Friday:
                start = stand.AddDays (-4);
                end = stand.AddDays (2);
                break;
            case DayOfWeek.Saturday:
                start = stand.AddDays (-5);
                end = stand.AddDays (1);
                break;
            default:
                start = stand.AddDays (-6);
                end = stand.AddDays (0);
                break;
        }
        DateTime[] result = new DateTime[2];
        result[0] = start;
        result[1] = end;
        return result;
    }

    /// <summary>
    /// 转换为相对时间如多少小时前
    /// </summary>
    /// <param name="utcDateTime"></param>
    /// <param name="baseDate"></param>
    /// <returns></returns>
    public static string ToRelativeTime (this DateTime utcDateTime, DateTime? baseDate = null) {
        if (baseDate == null)
            baseDate = DateTime.UtcNow;

        var ts = baseDate.Value - utcDateTime;
        var delta = Math.Round (ts.TotalSeconds, 0);

        if (delta < -0.1) {
            // In the future
            delta = -delta;
            ts = -ts;

            if (delta < 60) {
                return ts.Seconds == 1 ? "1 秒内" : ts.Seconds + " 秒内";
            }
            if (delta < 120) {
                return "1 分钟内";
            }
            if (delta < 3000) // 50 * 60
            {
                return ts.Minutes + " 分钟内";
            }
            if (delta < 5400) // 90 * 60
            {
                return "1 小时内";
            }
            if (delta < 86400) // 24 * 60 * 60
            {
                return ts.Hours + " 小时内";
            }
            if (delta < 172800) // 48 * 60 * 60
            {
                return "1 天内";
            }
            if (delta < 28 * 24 * 60 * 60) // 28 * 24 * 60 * 60
            {
                return ts.Days + " 天内";
            }
            if (delta < 12 * 29 * 24 * 60 * 60) // 12 * 29 * 24 * 60 * 60
            {
                var months = Convert.ToInt32 (Math.Floor ((double) ts.Days / 30));
                return ((months <= 1) ? "1 月内" : months + " 月内");
            } else {
                var years = Convert.ToInt32 (Math.Floor ((double) ts.Days / 365));
                return years <= 1 ? "1 年前" : years + " 年前";
            }
        }
        if (delta < 0.1)
            return "现在";
        if (delta < 60) {
            return ts.Seconds == 1 ? "1 秒前" : ts.Seconds + " 秒前";
        }
        if (delta < 120) {
            return "1 分钟前";
        }
        if (delta < 3000) // 50 * 60
        {
            return ts.Minutes + " 分钟前";
        }
        if (delta < 5400) // 90 * 60
        {
            return "1 小时前";
        }
        if (delta < 86400) // 24 * 60 * 60
        {
            return ts.Hours + " 小时前";
        }
        if (delta < 172800) // 48 * 60 * 60
        {
            return "1 天前";
        }
        if (delta < 29 * 24 * 60 * 60) // 29 * 24 * 60 * 60
        {
            return ts.Days + " 天前";
        }
        if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
        {
            var months = Convert.ToInt32 (Math.Floor ((double) ts.Days / 30));
            return months <= 1 ? "1 个月钱" : months + " 个月前";
        } else {
            var years = Convert.ToInt32 (Math.Floor ((double) ts.Days / 365));
            return years <= 1 ? "1 年前" : years + " 年前";
        }
    }

    /// <summary>
    /// 转换为相对时间如多少小时前(英文)
    /// </summary>
    /// <param name="utcDateTime"></param>
    /// <param name="baseDate"></param>
    /// <returns></returns>
    public static string ToRelativeTimeEN (this DateTime utcDateTime, DateTime? baseDate = null) {
        if (baseDate == null)
            baseDate = DateTime.UtcNow;

        var ts = baseDate.Value - utcDateTime;
        var delta = Math.Round (ts.TotalSeconds, 0);

        if (delta < -0.1) {
            // In the future
            delta = -delta;
            ts = -ts;

            if (delta < 60) {
                return ts.Seconds == 1 ? "in one second" : "in " + ts.Seconds + " seconds";
            }
            if (delta < 120) {
                return "in one minute";
            }
            if (delta < 3000) // 50 * 60
            {
                return "in " + ts.Minutes + " minutes";
            }
            if (delta < 5400) // 90 * 60
            {
                return "in one hour";
            }
            if (delta < 86400) // 24 * 60 * 60
            {
                return "in " + ts.Hours + " hours";
            }
            if (delta < 172800) // 48 * 60 * 60
            {
                return "in one day";
            }
            if (delta < 28 * 24 * 60 * 60) // 28 * 24 * 60 * 60
            {
                return "in " + ts.Days + " days";
            }
            if (delta < 12 * 29 * 24 * 60 * 60) // 12 * 29 * 24 * 60 * 60
            {
                var months = Convert.ToInt32 (Math.Floor ((double) ts.Days / 30));
                return "in " + ((months <= 1) ? "one month" : months + " months");
            } else {
                var years = Convert.ToInt32 (Math.Floor ((double) ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }
        if (delta < 0.1)
            return "now";
        if (delta < 60) {
            return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
        }
        if (delta < 120) {
            return "one minute ago";
        }
        if (delta < 3000) // 50 * 60
        {
            return ts.Minutes + " minutes ago";
        }
        if (delta < 5400) // 90 * 60
        {
            return "one hour ago";
        }
        if (delta < 86400) // 24 * 60 * 60
        {
            return ts.Hours + " hours ago";
        }
        if (delta < 172800) // 48 * 60 * 60
        {
            return "one day ago";
        }
        if (delta < 29 * 24 * 60 * 60) // 29 * 24 * 60 * 60
        {
            return ts.Days + " days ago";
        }
        if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
        {
            var months = Convert.ToInt32 (Math.Floor ((double) ts.Days / 30));
            return months <= 1 ? "one month ago" : months + " months ago";
        } else {
            var years = Convert.ToInt32 (Math.Floor ((double) ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }
    }
    #endregion

}