
public class FileFolderHelperTest
{
    [Fact]
    public void ForceFileCopyBadOldFileNameTest()
    {
        var UUT = FileFolderHelper.ForceFileMove("", "Test");

        Assert.False(UUT);
    }

    [Fact]
    public void ForceFileCopyBadNewFileNameTest()
    {
        var UUT = FileFolderHelper.ForceFileMove("Test", "");

        Assert.False(UUT);
    }

    [Fact]
    public void ForceFileCopyFileNotFoundTest()
    {
        var UUT = FileFolderHelper.ForceFileMove("Test", "Test");

        Assert.False(UUT);
    }

    [Fact]
    public void ForceFileMoveBadOldFileNameTest()
    {
        var UUT = FileFolderHelper.ForceFileMove("", "Test");

        Assert.False(UUT);
    }

    [Fact]
    public void ForceFileForceMoveBadNewFileNameTest()
    {
        var UUT = FileFolderHelper.ForceFileMove("Test", "");
        Assert.False(UUT);
    }

    [Fact]
    public void ForceFileForceMoveFileNotFoundTest()
    {
        var UUT = FileFolderHelper.ForceFileMove("Test", "Test");
        Assert.False(UUT);
    }

    [Fact]
    public void RenameFileWithGuidBlankNameTest()
    {
        var UUT = FileFolderHelper.RenameFileWithGuid("");
        Assert.Equal("", UUT);
    }

    [Fact]
    public void RenameFileWithGuidNullNameTest()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var UUT = FileFolderHelper.RenameFileWithGuid(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Equal("", UUT);
    }

    [Fact]
    public void RenameFileWithGuidInvalidTest()
    {
        var UUT = FileFolderHelper.RenameFileWithGuid("Test");
        Assert.Equal("", UUT);
    }

    [Fact]
    public void CleanFolderDirectoryNullTest()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var UUT = FileFolderHelper.CleanFolder(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.False(UUT);
    }

    [Fact]
    public void CleanFolderDirectoryInvalidTest()
    {
        var UUT = FileFolderHelper.CleanFolder("Test");
        Assert.False(UUT);
    }

    [Fact]
    public void CleanFolderAndRemoveDirectoryNullTest()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var UUT = FileFolderHelper.CleanFolder(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.False(UUT);
    }

    [Fact]
    public void CleanFolderAndRemoveDirectoryInvalidTest()
    {
        var UUT = FileFolderHelper.CleanFolder("Test");
        Assert.False(UUT);
    }


    [Fact]
    public void GetLatestFolderDirectoryNullTest()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var UUT = FileFolderHelper.GetLatestFolder(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Equal("", UUT);
    }

    [Fact]
    public void GetLatestFolderDirectoryInvalidTest()
    {
        var UUT = FileFolderHelper.GetLatestFolder("Test");
        Assert.Equal("", UUT);
    }
}
