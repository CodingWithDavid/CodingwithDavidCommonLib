public class PasswordHelperTest
{
    [Fact]
    public void ComplexPasswordPositiveTest()
    {
        string password = "abAB!@12";
        Assert.True(PasswordHelper.IsPasswordComplex(password, 8));
    }

    [Fact]
    public void ComplexPasswordToShortTest()
    {
        string password = "aA!@12";
        Assert.False(PasswordHelper.IsPasswordComplex(password, 8));
    }

    [Fact]
    public void ComplexPasswordNoNumbersTest()
    {
        string password = "abAB!@aa";
        Assert.False(PasswordHelper.IsPasswordComplex(password, 8));
    }

    [Fact]
    public void ComplexPasswordNoSpecialTest()
    {
        string password = "abABaa12";
        Assert.False(PasswordHelper.IsPasswordComplex(password, 8));
    }

    [Fact]
    public void ComplexPasswordNoUpperCaseTest()
    {
        string password = "abab!@12";
        Assert.False(PasswordHelper.IsPasswordComplex(password, 8));
    }

    [Fact]
    public void ComplexPasswordNoLowerTest()
    {
        string password = "ABAB!@12";
        Assert.False(PasswordHelper.IsPasswordComplex(password, 8));
    }
}