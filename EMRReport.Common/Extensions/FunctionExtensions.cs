using EMRReport.Common.Models.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EMRReport.Common.Extensions
{
    public static class FunctionExtensions
    {
        public static DateTime? ConvertStringToDateTime(this string dateTime)
        {
            try
            {
                //14/08/2016 18:23
                string[] dsplit = dateTime.Split('/');
                int d = 0; int m = 0; int y = 0; int H = 0; int M = 0;
                int.TryParse(dsplit[0], out d);
                int.TryParse(dsplit[1], out m);
                string[] ysplit = dsplit[2].Split(' ');
                int.TryParse(ysplit[0], out y);
                if (ysplit.Length > 1)
                {
                    string[] Hsplit = ysplit[1].Split(':');
                    if (Hsplit.Length > 1)
                    {
                        int.TryParse(Hsplit[0], out H);
                        int.TryParse(Hsplit[1], out M);
                    }
                }
                int s = 0;
                return new DateTime(y, m, d, H, M, s);
            }
            catch
            {
                return null;
            }
        }
        public static DateTime? ConvertStringToDate(this string dateTime)
        {
            try
            {
                //14/08/2016
                string[] dsplit = dateTime.Split('/');
                int d = 0; int m = 0; int y = 0;
                int.TryParse(dsplit[0], out d);
                int.TryParse(dsplit[1], out m);
                string[] ysplit = dsplit[2].Split(' ');
                int.TryParse(ysplit[0], out y);
                int s = 0;
                return new DateTime(y, m, d);
            }
            catch
            {
                return null;
            }
        }
        public static int ConvertDateStringToDays(this string datetime)
        {
            DateTime? dfb = datetime.ConvertStringToDate();
            DateTime now = DateTime.Now;
            return dfb.HasValue ? (int)Math.Floor((decimal)(now - dfb.Value).TotalDays) : 0;
        }
        public static Tuple<DateTime?, DateTime?> ConvertDateRangeStringToDateTimes(this string dateRange)
        {
            DateTime? from = null;
            DateTime? to = null;
            try
            {
                string[] dataRange = dateRange.Split('-');
                from = dataRange[0].ConvertStringToDateTime();
                to = dataRange[1].ConvertStringToDateTime();
            }
            catch
            {
                return null;
            }
            return Tuple.Create(from, to);
        }
        public static string ConvertDateTimeToString(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                StringBuilder sb = new StringBuilder();
                int d = dateTime.Value.Day;
                int m = dateTime.Value.Month;
                int y = dateTime.Value.Year;
                int h = dateTime.Value.Hour;
                int M = dateTime.Value.Minute;
                if (y > 100)
                    return sb.Append(d).Append("/").Append(m).Append("/").Append(y).Append(" ").Append(h).Append(":").Append(M).ToString();
            }
            return "";
        }
        public static string ConvertDateTimeToString(this DateTime dateTime)
        {
            StringBuilder sb = new StringBuilder();
            int d = dateTime.Day;
            int m = dateTime.Month;
            int y = dateTime.Year;
            int h = dateTime.Hour;
            int M = dateTime.Minute;
            if (y > 100)
                return sb.Append(d).Append("/").Append(m).Append("/").Append(y).Append(" ").Append(h).Append(":").Append(M).ToString();
            else
                return "";
        }
        public static string ConvertValidatorInputToCommaSepertedString(this string Codes)
        {
            Codes = Codes == null ? null : Codes.TrimEnd(',').TrimStart(',').ToString();
            if (!string.IsNullOrEmpty(Codes))
                Codes = string.Join(",", Codes.Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray());
            return !string.IsNullOrEmpty(Codes) ? Codes : "";
        }
        public static bool CheckIsDOSValidation(this DateTime EncounterDate, DateTime? DateOfService)
        {
            bool IsDOS = false;
            if (DateOfService.HasValue)
                return EncounterDate >= DateOfService.Value ? true : false;
            return IsDOS;
        }
        public static int GetDate(this DateTime date)
        {
            return date.DayOfYear;
        }
        public static int GetWeek(this DateTime date)
        {
            return (int)Math.Ceiling((decimal)date.DayOfYear / 7);
        }
        public static int GetMonth(this DateTime date)
        {
            return date.Month;
        }
        public static int GetYear(this DateTime date)
        {
            return date.Year;
        }
        public static string ValidateXMLFiles(this List<IFormFile> XMLfiles)
        {
            if (XMLfiles == null || XMLfiles.Count == 0)
                return "File Not Found";
            else if (XMLfiles.Where(x => Path.GetExtension(x.FileName).ToLower() != ".xml").Count() > 0)
                return "Only XML Files Supported Not";
            else
                return "";
        }
        public static string Append(this string message1, string message2, string message3)
        {
            StringBuilder sb = new StringBuilder();
            return sb.Append(message1).Append(message2).Append(message3).ToString();
        }
        public static string Append(this string message1, string message2, string message3, string message4)
        {
            StringBuilder sb = new StringBuilder();
            return sb.Append(message1).Append(message2).Append(message3).Append(message4).ToString();
        }
        public static string ConvertDateToPathString(this DateTime dateTime)
        {
            StringBuilder sb = new StringBuilder();
            int d = dateTime.Day;
            int m = dateTime.Month;
            int y = dateTime.Year;
            if (y > 100)
                return sb.Append(y).Append("-").Append(m).Append("-").Append(d).ToString();
            else
                return "";
        }
        private static string LimitCommaSeperatedCodesToChars(string Codes, int StringLength)
        {
            StringBuilder sbCodes = new StringBuilder(10);
            if (!string.IsNullOrEmpty(Codes) && Codes.Length > StringLength - 1)
            {
                sbCodes.Length = 0;
                Codes = Codes.Substring(0, StringLength - 5);
                int idx = Codes.LastIndexOf(';');
                return sbCodes.Append(Codes.Substring(0, idx)).Append(";..").ToString();
            }
            else
                return Codes;
        }
        public static string LimitCommaSeperatedCodesTo4000Chars(this string Codes)
        {
            return LimitCommaSeperatedCodesToChars(Codes, 4000);
        }
        public static string LimitCommaSeperatedCodesTo1000Chars(this string Codes)
        {
            return LimitCommaSeperatedCodesToChars(Codes, 1000);
        }

        public static List<UserMenuDto> MenuToDtoList(this List<Tuple<string, string>> menuList)
        {
            var groupList = menuList.Select(x => x.Item1).Distinct().ToArray();
            return groupList.Select
            (x => new UserMenuDto
            {
                GroupName = x,
                ControlList = menuList.Where(y => y.Item1 == x).Select(x => x.Item2).ToArray()
            }).ToList();
        }
        public static bool ConvertStringToBool(this string Status)
        {
            if (!string.IsNullOrEmpty(Status))
                return Status.ToLower() == "active" || Status.ToLower() == "yes" || Status.ToLower() == "true" ? true : false;
            else
                return false;
        }
        public static int ConvertStringToInt(this string strint)
        {
            if (!string.IsNullOrEmpty(strint))
                return Convert.ToInt32(strint);
            else
                return 0;
        }
        public static string ConvertBoolYesString(this bool Status)
        {
            return Status ? "yes" : "no";
        }
        public static string ConvertBoolToActiveString(this bool Status)
        {
            return Status ? "Active" : "InActive";

        }
    }
}