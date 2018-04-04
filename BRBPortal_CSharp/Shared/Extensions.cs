using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;

namespace BRBPortal_CSharp
{
    #region DataRow
    public static class DataRowExtensions
    {
        public static Nullable<T> GetValueOfNullableType<T>(this DataRow row, string columnName)
            where T : struct
        {
            if (row == null || row.IsNull(columnName))
            {
                return null;
            }
            else
            {
                if (typeof(T).IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), row[columnName].ToString());
                }
                else
                {
                    return (T)Convert.ChangeType(row[columnName], typeof(T));
                }
            }
        }

        public static T GetValueOfType<T>(this DataRow row, string columnName)
        {
            if (row == null || row.IsNull(columnName))
            {
                return default(T);
            }
            else
            {
                if (typeof(T).IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), row[columnName].ToString());
                }
                else
                {
                    return (T)Convert.ChangeType(row[columnName], typeof(T));
                }
            }
        }

        public static bool IsNullValue(this DataRow row, string columnName)
        {
            return (row == null || row.IsNull(columnName));
        }
    }
    #endregion

    #region DateTime
    public static class DateTimeExtensions
    {
        public static long ConvertToUnix(this DateTime datetime)
        {
            DateTime origin = new DateTime(1970, 1, 1);
            TimeSpan diff = datetime - origin;

            return (long)Math.Floor(diff.TotalSeconds);
        }

        public static string AsShortDateFormat(this DateTime datetime)
        {
            return datetime.ToString("MM/dd/yyyy");
        }

        public static string AsDatePickerFormat(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd");
        }
    }
    #endregion

    #region GridViewRow
    public static class GridViewRowExtensions
    {
        public static int GetColumnIndexByName(this GridViewRow row, string columnName)
        {
            int columnIndex = 0;

            foreach (DataControlFieldCell cell in row.Cells)
            {
                if (cell.ContainingField is BoundField)
                {
                    if (((BoundField)cell.ContainingField).DataField.Equals(columnName))
                    {
                        break;
                    }
                }
                columnIndex++; // keep adding 1 while we don't have the correct name
            }

            return columnIndex;
        }
    }
    #endregion

    #region Collection
    public static class CollectionExtensions
    {
        public static string GetStringValue(this Dictionary<string, string> fields, string key, string defaultValue = "")
        {
            return fields.ContainsKey(key) ? fields[key] : defaultValue;
        }

        public static string ToDelimitedString(this Dictionary<string, string> fields, string delimiter = "::")
        {
            return string.Join(delimiter, fields);
        }
    }
    #endregion

    #region Enum
    // SEE: http://www.andrescottwilson.com/tag/c-net-enum-extensions
    public static class EnumExtensions
    {
        public static int ToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }

        public static bool HasNumber(this string input)
        {
            return input.Any(char.IsDigit);
        }

        public static string ToIntString(this Enum enumValue)
        {
            return enumValue.ToInt().ToString();
        }
    }

    public static class EnumHelper<T>
    {
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();

            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }

            return enumValues;
        }

        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        private static string lookupResource(Type resourceManagerProvider, string resourceKey)
        {
            foreach (PropertyInfo staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (staticProperty.PropertyType == typeof(System.Resources.ResourceManager))
                {
                    System.Resources.ResourceManager resourceManager = (System.Resources.ResourceManager)staticProperty.GetValue(null, null);
                    return resourceManager.GetString(resourceKey);
                }
            }

            return resourceKey; // Fallback with the key name
        }

        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes[0].ResourceType != null)
            {
                return lookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);
            }

            if (descriptionAttributes == null)
            {
                return string.Empty;
            }
            else
            {
                return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
            }
        }
    }
    #endregion

    #region Long
    public static class LongExtensions
    {
        public static DateTime ConvertUnixToDateTime(this long value)
        {
            try
            {
                DateTime origin = new DateTime(1970, 1, 1);

                return origin.AddSeconds(value);
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }
    }
    #endregion

    #region String
    public static class StringExtensions
    {
        public static T? ConvertToNullableType<T>(this string s) where T : struct
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }

            return (T)Convert.ChangeType(s, typeof(T));
        }

        public static T ConvertToType<T>(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return default(T);
            }

            return (T)Convert.ChangeType(s, typeof(T));
        }

        public static String EscapeXMLChars(this string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");

            return str;
        }

        public static string UnescapeXMLChars(this string str)
        {
            str = str.Replace("&amp;", "&");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");

            return str;
        }
    }
    #endregion
}