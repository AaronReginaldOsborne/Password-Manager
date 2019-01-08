using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Project_1_XML_WPF_Password
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Field is required.")
                : ValidationResult.ValidResult;
        }
    }

    public class NoWhiteSpaceValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Regex regex = new Regex(@"\S*");
            if (value == null || value.Equals(""))
                return new ValidationResult(false, "Field is required.");
            Match match = regex.Match(value.ToString());
            if (match.Length == value.ToString().Length && match.Success)
            {
                return new ValidationResult(true, "");
            }
            else
            {
                return new ValidationResult(false, "No White Space.");
            }
        }
    }

    public class NotEmptyDateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime time;
            if (!DateTime.TryParse((value ?? "").ToString(),
                CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
                out time)) return new ValidationResult(false, "Invalid date");

            return time.Date > DateTime.Now.Date
                ? new ValidationResult(false, "Past date required")
                : ValidationResult.ValidResult;
        }
    }
}
