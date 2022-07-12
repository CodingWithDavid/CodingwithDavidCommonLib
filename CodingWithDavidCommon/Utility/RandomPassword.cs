using System.Security.Cryptography;

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
        if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
            return null;

        char[][] charGroups = new char[][]
        {
                PASSWORD_CHARS_LCASE.ToCharArray(),
                PASSWORD_CHARS_UCASE.ToCharArray(),
                PASSWORD_CHARS_NUMERIC.ToCharArray(),
                PASSWORD_CHARS_SPECIAL.ToCharArray()
        };

        int[] charsLeftInGroup = new int[charGroups.Length];

        for (int i = 0; i < charsLeftInGroup.Length; i++)
            charsLeftInGroup[i] = charGroups[i].Length;

        int[] leftGroupsOrder = new int[charGroups.Length];

        for (int i = 0; i < leftGroupsOrder.Length; i++)
            leftGroupsOrder[i] = i;

        byte[] randomBytes = new byte[4];

#pragma warning disable SYSLIB0023 // Type or member is obsolete
        RNGCryptoServiceProvider rng = new();
#pragma warning restore SYSLIB0023 // Type or member is obsolete
        rng.GetBytes(randomBytes);

        int seed = (randomBytes[0] & 0x7f) << 24 |
                    randomBytes[1] << 16 |
                    randomBytes[2] << 8 |
                    randomBytes[3];

        Random random = new(seed);

        char[]? password;
        if (minLength < maxLength)
            password = new char[random.Next(minLength, maxLength + 1)];
        else
            password = new char[minLength];

        int nextCharIdx;
        int nextGroupIdx;
        int nextLeftGroupsOrderIdx;
        int lastCharIdx;

        int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

        for (int i = 0; i < password.Length; i++)
        {
            if (lastLeftGroupsOrderIdx == 0)
                nextLeftGroupsOrderIdx = 0;
            else
                nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx);

            nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

            lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

            if (lastCharIdx == 0)
                nextCharIdx = 0;
            else
                nextCharIdx = random.Next(0, lastCharIdx + 1);

            password[i] = charGroups[nextGroupIdx][nextCharIdx];

            if (lastCharIdx == 0)
                charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
            else
            {
                if (lastCharIdx != nextCharIdx)
                {
                    (charGroups[nextGroupIdx][nextCharIdx], charGroups[nextGroupIdx][lastCharIdx]) = (charGroups[nextGroupIdx][lastCharIdx], charGroups[nextGroupIdx][nextCharIdx]);
                }
                charsLeftInGroup[nextGroupIdx]--;
            }

            if (lastLeftGroupsOrderIdx == 0)
                lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            else
            {
                if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                {
                    (leftGroupsOrder[nextLeftGroupsOrderIdx], leftGroupsOrder[lastLeftGroupsOrderIdx]) = (leftGroupsOrder[lastLeftGroupsOrderIdx], leftGroupsOrder[nextLeftGroupsOrderIdx]);
                }
                lastLeftGroupsOrderIdx--;
            }
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
        if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
            return null;

        char[][] charGroups = new char[][]
        {
                PASSWORD_CHARS_UCASE.ToCharArray(),
                PASSWORD_CHARS_NUMERIC.ToCharArray()
        };

        int[] charsLeftInGroup = new int[charGroups.Length];

        for (int i = 0; i < charsLeftInGroup.Length; i++)
            charsLeftInGroup[i] = charGroups[i].Length;

        int[] leftGroupsOrder = new int[charGroups.Length];

        for (int i = 0; i < leftGroupsOrder.Length; i++)
            leftGroupsOrder[i] = i;

        byte[] randomBytes = new byte[4];

#pragma warning disable SYSLIB0023 // Type or member is obsolete
        RNGCryptoServiceProvider rng = new();
#pragma warning restore SYSLIB0023 // Type or member is obsolete
        rng.GetBytes(randomBytes);

        int seed = (randomBytes[0] & 0x7f) << 24 |
                    randomBytes[1] << 16 |
                    randomBytes[2] << 8 |
                    randomBytes[3];

        Random random = new(seed);

        char[]? password;
        if (minLength < maxLength)
            password = new char[random.Next(minLength, maxLength + 1)];
        else
            password = new char[minLength];

        int nextCharIdx;

        int nextGroupIdx;

        int nextLeftGroupsOrderIdx;

        int lastCharIdx;

        int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

        for (int i = 0; i < password.Length; i++)
        {
            if (lastLeftGroupsOrderIdx == 0)
                nextLeftGroupsOrderIdx = 0;
            else
                nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx);

            nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

            lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

            if (lastCharIdx == 0)
                nextCharIdx = 0;
            else
                nextCharIdx = random.Next(0, lastCharIdx + 1);

            password[i] = charGroups[nextGroupIdx][nextCharIdx];

            if (lastCharIdx == 0)
                charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
            else
            {
                if (lastCharIdx != nextCharIdx)
                {
                    (charGroups[nextGroupIdx][nextCharIdx], charGroups[nextGroupIdx][lastCharIdx]) = (charGroups[nextGroupIdx][lastCharIdx], charGroups[nextGroupIdx][nextCharIdx]);
                }
                charsLeftInGroup[nextGroupIdx]--;
            }

            if (lastLeftGroupsOrderIdx == 0)
                lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            else
            {
                if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                {
                    (leftGroupsOrder[nextLeftGroupsOrderIdx], leftGroupsOrder[lastLeftGroupsOrderIdx]) = (leftGroupsOrder[lastLeftGroupsOrderIdx], leftGroupsOrder[nextLeftGroupsOrderIdx]);
                }
                lastLeftGroupsOrderIdx--;
            }
        }
        return new string(password);
    }
}