using System.Security.Cryptography;

public class ModernizationTest
{
    [Fact]
    public void ZipStrRoundTripsText()
    {
        const string input = "The quick brown fox jumps over the lazy dog.";

        string compressed = StringHelper.ZipStr(input);

        Assert.NotEqual(input, compressed);
        Assert.Equal(input, StringHelper.Unzip(compressed));
    }

    [Fact]
    public void PageResultReturnsRequestedPage()
    {
        int[] source = Enumerable.Range(1, 10).ToArray();

        IQueryable<int> result = RecordSorterHelper.PageResult(source.AsQueryable(), 2, 3);

        Assert.Equal(new[] { 4, 5, 6 }, result.ToArray());
    }

    [Fact]
    public void ForceFileCopyReportsSuccess()
    {
        string folder = CreateTempFolder();
        string source = Path.Combine(folder, "source.txt");
        string destination = Path.Combine(folder, "destination.txt");

        try
        {
            File.WriteAllText(source, "copy me");

            Assert.True(FileFolderHelper.ForceFileCopy(source, destination));
            Assert.Equal("copy me", File.ReadAllText(destination));
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    [Fact]
    public void ForceFileMoveReportsSuccess()
    {
        string folder = CreateTempFolder();
        string source = Path.Combine(folder, "source.txt");
        string destination = Path.Combine(folder, "destination.txt");

        try
        {
            File.WriteAllText(source, "move me");

            Assert.True(FileFolderHelper.ForceFileMove(source, destination));
            Assert.False(File.Exists(source));
            Assert.Equal("move me", File.ReadAllText(destination));
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    [Fact]
    public void EncryptionRoundTripsWithExplicitPassword()
    {
        string folder = CreateTempFolder();
        string source = Path.Combine(folder, "source.txt");
        string encrypted = Path.Combine(folder, "source.bin");
        string decrypted = Path.Combine(folder, "decrypted.txt");

        try
        {
            File.WriteAllText(source, "confidential content");

            FileFolderHelper.EncryptFile(source, encrypted, "correct horse battery staple");
            FileFolderHelper.DecryptFile(encrypted, decrypted, "correct horse battery staple");

            Assert.Equal(File.ReadAllText(source), File.ReadAllText(decrypted));
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    [Fact]
    public void EncryptionRejectsWrongPassword()
    {
        string folder = CreateTempFolder();
        string source = Path.Combine(folder, "source.txt");
        string encrypted = Path.Combine(folder, "source.bin");
        string decrypted = Path.Combine(folder, "decrypted.txt");

        try
        {
            File.WriteAllText(source, "confidential content");
            FileFolderHelper.EncryptFile(source, encrypted, "correct password");

            Assert.Throws<AuthenticationTagMismatchException>(() =>
                FileFolderHelper.DecryptFile(encrypted, decrypted, "wrong password"));
        }
        finally
        {
            Directory.Delete(folder, true);
        }
    }

    [Fact]
    public void GeneratedPasswordContainsAllRequiredCharacterGroups()
    {
        string password = RandomPassword.Generate(16, 16)!;

        Assert.Contains(password, char.IsLower);
        Assert.Contains(password, char.IsUpper);
        Assert.Contains(password, char.IsDigit);
        Assert.Contains(password, c => "*$-+?_&=!%{}/".Contains(c));
    }

    [Fact]
    public void GeneratedPasswordWithoutSpecialCharactersUsesOnlyAllowedGroups()
    {
        string password = RandomPassword.GenerateNoSpecialCharacators(16, 16)!;

        Assert.Contains(password, char.IsUpper);
        Assert.Contains(password, char.IsDigit);
        Assert.DoesNotContain(password, c => !char.IsUpper(c) && !char.IsDigit(c));
    }

    [Fact]
    public void CsvWriterAndReaderRoundTripQuotedValues()
    {
        using MemoryStream stream = new();
        using (ReadWriteCsvUtility.CsvFileWriter writer = new(stream, leaveOpen: true))
        {
            writer.WriteRow(new ReadWriteCsvUtility.CsvRow { "one", "two, with comma", "quote \"value\"" });
        }

        stream.Position = 0;
        using ReadWriteCsvUtility.CsvFileReader reader = new(stream);
        ReadWriteCsvUtility.CsvRow row = new();

        Assert.True(reader.ReadRow(row));
        Assert.Equal(new[] { "one", "two, with comma", "quote \"value\"" }, row);
    }

    private static string CreateTempFolder()
    {
        string folder = Path.Combine(Path.GetTempPath(), "CodingWithDavidCommon-" + Guid.NewGuid());
        Directory.CreateDirectory(folder);
        return folder;
    }
}
