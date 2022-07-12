using System.Text.Json;

public static class JsonHelper
{
    /// <summary>
    /// Provides a generic Deserializer for a Json string to an object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input">Json string</param>
    /// <returns>T or a blank T</returns>
    public static T Deserialize<T>(string input) where T : class
    {
        if (input.IsNotEmpty())
        {
            return JsonSerializer.Deserialize<T>(input)!;
        }
        return (T)Activator.CreateInstance(typeof(T))!;
    }

    /// <summary>
    /// Provides a generic Serializer of an object T to a string
    /// </summary>
    /// <typeparam name="T">Object</typeparam>
    /// <param name="input">Object to serialize</param>
    /// <returns>Json string</returns>
    public static string Serialize<T>(T input) where T : class
    {
        string result = "";

        if (input != null)
        {
            result = JsonSerializer.Serialize(input);
        }
        return result;
    }
}
