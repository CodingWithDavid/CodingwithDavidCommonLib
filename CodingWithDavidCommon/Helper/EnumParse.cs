
using System.ComponentModel;
using System.Reflection;


public static class EnumParse
{
    /// <summary>
    /// Parses a string into the Enum defined
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns>Null or Enum</returns>
    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    /// <summary>
    /// Finds an enum value based on an integer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <returns>Enum value or a blank string</returns>
    public static string ParseEnum<T>(int id)
    {
        string result = "";
        T inEnum = (T)Enum.ToObject(typeof(T), id);
        if (inEnum != null)
        {
            result = inEnum.ToString()!;

            if (result.Match(id.ToString()))
            {
                result = "";
            }
        }
        return result;
    }

    /// <summary>
    /// Returns the defined description of the enum
    /// </summary>
    /// <param name="value"></param>
    /// <returns>The defined description or an empty string</returns>
    public static string GetDescription(Enum value)
    {
        if (value != null)
        {
            FieldInfo field = value.GetType().GetField(value.ToString())!;
            return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is not DescriptionAttribute attribute ? value.ToString() : attribute.Description;
        }
        return "";
    }
}
