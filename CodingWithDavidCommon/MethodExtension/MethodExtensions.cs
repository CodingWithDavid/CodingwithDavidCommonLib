using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CodingWithDavid.Common;

/// <summary>Provides domain-specific extension methods for strings and objects.</summary>
public static class MethodExtensions
{
    /// <summary>Compares two strings using the current culture without case sensitivity.</summary>
    /// <param name="str">The source string.</param>
    /// <param name="match">The string to compare.</param>
    /// <returns><see langword="true"/> when the strings match.</returns>
    public static bool Match(this string? str, string? match)
    {
        return string.Equals(str, match, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>Converts a string to title case using the current culture.</summary>
    /// <param name="str">The source string.</param>
    /// <returns>The title-cased string.</returns>
    public static string MixCase(this string str)
    {
        TextInfo textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(str.ToLower());
    }

    /// <summary>Appends a dated user note to an existing note.</summary>
    /// <param name="note">The existing note.</param>
    /// <param name="userName">The user adding the note.</param>
    /// <param name="newNote">The note text to append.</param>
    /// <returns>The combined note text.</returns>
    public static string AppendNote(this string? note, string userName, string newNote)
    {
        StringBuilder sb = new(note ?? string.Empty);
        if (sb.Length != 0)
            sb.Append(Environment.NewLine);

        sb.Append(userName);
        sb.Append(": ");
        sb.Append(newNote);
        sb.Append(" - ");
        sb.Append(DateTime.Now.ToString());
        return sb.ToString();
    }

    /// <summary>Removes non-ASCII characters after normalizing smart quotes.</summary>
    /// <param name="str">The source string.</param>
    /// <returns>The ASCII-only string.</returns>
    public static string MakeAsciiOnly(this string? str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        string fields = str.Replace('”', '"').Replace('’', '\'').Replace('“', '"');
        return Regex.Replace(fields, @"[^\x20-\x7F]", "");
    }

    /// <summary>Replaces backslashes with forward slashes.</summary>
    /// <param name="str">The source URL or path.</param>
    /// <returns>The normalized string.</returns>
    public static string FixUrlSlash(this string? str)
    {
        return string.IsNullOrEmpty(str) ? str ?? string.Empty : str.Replace('\\', '/');
    }

    /// <summary>Converts common textual true values such as true, yes, and 1 to a Boolean.</summary>
    /// <param name="str">The source string.</param>
    /// <returns><see langword="true"/> when the value represents true.</returns>
    public static bool ToBool(this string? str)
    {
        return str?.ToLowerInvariant() switch
        {
            "true" or "yes" or "1" => true,
            _ => false
        };
    }

    /// <summary>Places single quotes around a non-empty string.</summary>
    /// <param name="target">The source string.</param>
    /// <returns>The quoted string.</returns>
    public static string SingleQuote(this string target)
    {
        return string.IsNullOrEmpty(target) ? target : $"'{target}'";
    }

    /// <summary>Places double quotes around a non-empty string.</summary>
    /// <param name="target">The source string.</param>
    /// <returns>The quoted string.</returns>
    public static string DoubleQuote(this string target)
    {
        return string.IsNullOrEmpty(target) ? target : $"\"{target}\"";
    }

    /// <summary>Parses a date or returns a supplied fallback value.</summary>
    /// <param name="valueToConvert">The value to parse.</param>
    /// <param name="defaultVal">The fallback value.</param>
    /// <returns>The parsed date or fallback.</returns>
    public static DateTime GetSafeDate(this string valueToConvert, DateTime defaultVal)
    {
        return DateTime.TryParse(valueToConvert, out DateTime returnVal) ? returnVal : defaultVal;
    }

    /// <summary>Parses a decimal or returns -1 when parsing fails.</summary>
    /// <param name="valueToConvert">The value to parse.</param>
    /// <returns>The parsed decimal or -1.</returns>
    public static decimal GetSafeDecimal(this string valueToConvert)
    {
        return decimal.TryParse(valueToConvert, out decimal returnVal) ? returnVal : -1;
    }

    /// <summary>Returns readable property names and values for an object.</summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="objectInstance">The object to inspect.</param>
    /// <returns>A pipe-delimited property summary.</returns>
    public static string GetPropertiesInfo<T>(this object objectInstance)
    {
        Type parentObjType = typeof(T);
        StringBuilder sb = new();
        foreach (PropertyInfo prop in parentObjType.GetProperties())
        {
            if (sb.Length > 0)
                sb.Append('|');

            sb.Append(prop.Name);
            sb.Append(": ");
            sb.Append(prop.GetValue(objectInstance, null));
        }
        return sb.ToString();
    }
}
