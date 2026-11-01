using System.Text.RegularExpressions;

namespace CodingWithDavid.Common;

/// <summary>Provides phone number normalization helpers.</summary>
public static class PhoneNumberHelper
{

    /// <summary>
    /// Clean phone number
    /// </summary>
    /// <param name="phoneNumber">Number to clean</param>
    public static string CleanPhoneNumber(string phoneNumber)
    {
        //Create string pattern
        string pattern = @"[\!\@\#\$\%\\^\&\*()\-\\_\+\=//\\\[\]\.\,\:\;//""\'\?\<\>\|\`\~]";

        //Create RegEx object
        Regex regex = new(pattern);

        //Clean string
        string result = regex.Replace(phoneNumber, "");

        result = result.Replace(" ", "");

        return result;
    }
}
