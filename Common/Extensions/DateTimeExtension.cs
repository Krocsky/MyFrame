using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// DateTime扩展
    /// </summary>
    public static class DateTimeExtension
    {
        public static string ToFriendlyDate(this DateTime dateTime, bool showTime = true, string dateFormat = null)
        {
            if (dateTime == DateTime.MinValue)
                return "-";

            if (string.IsNullOrEmpty(dateFormat))
                dateFormat = "yyyy-MM-dd";

            string timeFormat = "HH:mm";

            DateTime now = DateTime.Now;

            if (dateTime > now)
            {
                return dateTime.ToString(dateFormat + (showTime ? " " + timeFormat : ""));
            }

            TimeSpan intervalTime = now - dateTime;

            int intervalDays;

            if (now.Year == dateTime.Year)
                intervalDays = now.DayOfYear - dateTime.DayOfYear;
            else
                intervalDays = intervalTime.Days + 1;

            string result = "{0}";
            if (showTime)
                result = "{0}" + " " + dateTime.ToString(timeFormat);

            if (intervalDays > 7)
            {

                if (dateTime.Year == now.Year)
                {
                    return string.Format("{0}月{1}日{2}",
                                         dateTime.Month,
                                         dateTime.Day,
                                         "");
                }

                return dateTime.ToString(dateFormat);
            }

            if (intervalDays >= 3)
            {
                string timeScope = string.Format("{0}天前", intervalDays);
                return timeScope;
            }

            if (intervalDays == 2)
            {
                return string.Format(result, "前天");
            }

            if (intervalDays == 1)
            {
                return string.Format(result, "昨天");
            }

            if (intervalTime.Hours >= 1)
                return string.Format("{0}小时前", intervalTime.Hours);

            if (intervalTime.Minutes >= 1)
                return string.Format("{0}分钟前", intervalTime.Minutes);

            if (intervalTime.Seconds >= 1)
                return string.Format("{0}秒前", intervalTime.Seconds);

            return "刚刚";
        }

        /// <summary>
        /// 取今天星期几Plus：中文
        /// </summary>
        /// <returns></returns>
        public static string WeekDayOfDate(DateTime datetime)
        {
            int index = (int)datetime.DayOfWeek;
            string weekdayOfDate = "";
            switch (index)
            {
                case 0:
                    weekdayOfDate = "星期天";
                    break;
                case 1:
                    weekdayOfDate = "星期一";
                    break;
                case 2:
                    weekdayOfDate = "星期二";
                    break;
                case 3:
                    weekdayOfDate = "星期三";
                    break;
                case 4:
                    weekdayOfDate = "星期四";
                    break;
                case 5:
                    weekdayOfDate = "星期五";
                    break;
                case 6:
                    weekdayOfDate = "星期六";
                    break;
            }
            return weekdayOfDate;
        }
    }
}
