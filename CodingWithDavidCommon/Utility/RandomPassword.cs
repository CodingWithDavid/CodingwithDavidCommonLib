using System.Security.Cryptography;

namespace CodingWithDavid.Common;

/// <summary>Generates cryptographically random passwords.</summary>
public class RandomPassword
{
    private const int DEFAULT_MIN_PASSWORD_LENGTH = 8;
    private const int DEFAULT_MAX_PASSWORD_LENGTH = 10;

    private const string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
    private const string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
    private const string PASSWORD_CHARS_NUMERIC = "23456789";
    private const string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";

    /// <summary>
    /// Function to generate a password of the default values
    /// </summary>
    /// <returns>string of new password</returns>
    public static string? Generate()
    {
        return Generate(DEFAULT_MIN_PASSWORD_LENGTH, DEFAULT_MAX_PASSWORD_LENGTH);
    }

    /// <summary>
    /// Generates a password with a dynamic length
    /// </summary>
    /// <param name="length">how long to make the password</param>
    /// <returns>new password</returns>
    public static string? Generate(int length)
    {
        return Generate(length, length);
    }

    /// <summary>
    /// Generates a password between a min and max length
    /// </summary>
    /// <param name="minLength">Minimal length allowed</param>
    /// <param name="maxLength">Maximum length allowed</param>
    /// <returns>New password</returns>
    public static string? Generate(int minLength, int maxLength)
    {
        if (minLength < 4 || maxLength < 4 || minLength > maxLength)
            return null;

        string[] groups = { PASSWORD_CHARS_LCASE, PASSWORD_CHARS_UCASE, PASSWORD_CHARS_NUMERIC, PASSWORD_CHARS_SPECIAL };
        int length = RandomNumberGenerator.GetInt32(minLength, maxLength + 1);
        char[] password = new char[length];

        for (int i = 0; i < groups.Length; i++)
            password[i] = groups[i][RandomNumberGenerator.GetInt32(groups[i].Length)];

        string allCharacters = string.Concat(groups);
        for (int i = groups.Length; i < password.Length; i++)
            password[i] = allCharacters[RandomNumberGenerator.GetInt32(allCharacters.Length)];

        for (int i = password.Length - 1; i > 0; i--)
        {
            int swapIndex = RandomNumberGenerator.GetInt32(i + 1);
            (password[i], password[swapIndex]) = (password[swapIndex], password[i]);
        }

        return new string(password);
    }

    /// <summary>
    /// Generates a password, but with out using any special characters
    /// </summary>
    /// <param name="minLength">Minimal length allowed</param>
    /// <param name="maxLength">Maximum length allowed</param>
    /// <returns>New password</returns>
    public static string? GenerateNoSpecialCharacators(int minLength, int maxLength)
    {
        if (minLength < 2 || maxLength < 2 || minLength > maxLength)
            return null;

        string[] groups = { PASSWORD_CHARS_UCASE, PASSWORD_CHARS_NUMERIC };
        int length = RandomNumberGenerator.GetInt32(minLength, maxLength + 1);
        char[] password = new char[length];

        for (int i = 0; i < groups.Length; i++)
            password[i] = groups[i][RandomNumberGenerator.GetInt32(groups[i].Length)];

        string allCharacters = string.Concat(groups);
        for (int i = groups.Length; i < password.Length; i++)
            password[i] = allCharacters[RandomNumberGenerator.GetInt32(allCharacters.Length)];

        for (int i = password.Length - 1; i > 0; i--)
        {
            int swapIndex = RandomNumberGenerator.GetInt32(i + 1);
            (password[i], password[swapIndex]) = (password[swapIndex], password[i]);
        }

        return new string(password);
    }
}