public class MethodExtenstionsTest
{
    [Fact]
    public void ToBinaryStringZeroTest()
    {
        int number = 0;
        string result = number.ToBinaryString();
        Assert.Equal("0", result);
    }

    [Fact]
    public void ToBinaryStringPositiveTest()
    {
        int number = 3;
        string result = number.ToBinaryString(7);
        Assert.Equal("0000011", result);
    }

    [Fact]
    public void ToBinaryStringMaxTest()
    {
        int number = 127;
        string result = number.ToBinaryString(7);
        Assert.Equal("1111111", result);
    }

    [Fact]
    public void ToBinaryStringOverMaxTest()
    {
        int number = 127;
        string result = number.ToBinaryString(3);
        Assert.Equal("1111111", result);
    }


    [Fact]
    public void ToBinaryIntPositiveTest()
    {
        string str = "0000011";
        int result = str.ToBinaryInt();

        Assert.Equal(3, result);
    }

    [Fact]
    public void ToBinaryIntZeroTest()
    {
        string str = "00000";
        int result = str.ToBinaryInt();

        Assert.Equal(0, result);
    }

    [Fact]
    public void ToBinaryIntMaxTest()
    {
        string str = "1111111";
        int result = str.ToBinaryInt();

        Assert.Equal(127, result);
    }

    [Fact]
    public void IsADatePositiveTest()
    {
        string str = "01/01/2014";
        Assert.True(str.IsADate());
    }

    [Fact]
    public void IsADateNegitiveTest()
    {
        string str = "abcd";
        Assert.False(str.IsADate());
    }

    [Fact]
    public void IsADateBlankTest()
    {
        string str = "";
        Assert.False(str.IsADate());
    }

    [Fact]
    public void IsADateNullTest()
    {
        string? str = null;
#pragma warning disable CS8604 // Possible null reference argument.
        Assert.False(str.IsADate());
#pragma warning restore CS8604 // Possible null reference argument.
    }

    [Fact]
    public void IsADatePositiveDifferneetFormatTest()
    {
        string str = "2014/01/01";
        Assert.True(str.IsADate());
    }

    [Fact]
    public void ENumfromAIntegerTest()
    {
        string str = EnumParse.ParseEnum<Test>(2);
        Assert.True(str.Match("two"));
    }

    [Fact]
    public void ENumfromAIntegerEmptyReturnTest()
    {
        string str = EnumParse.ParseEnum<Test>(20);
        Assert.True(str.IsEmpty());
    }

    [Fact]
    public void ENumfromAStringTest()
    {
        Assert.Equal(Test.one, EnumParse.ParseEnum<Test>("one"));
    }

    [Fact]
    public void ENumfromAStringEmptyReturnTest()
    {
        try
        {
            int result = (int)EnumParse.ParseEnum<Test>("");
            Assert.True(false);
        }
        catch
        {
            Assert.True(true);
        }
    }

    [Fact]
    public void ToDecimalPositiveTest()
    {
        string str = "1.11";
        decimal result = str.ToDecimal();

        Assert.True(result == 1.11m);
    }

    [Fact]
    public void ToDecimalEmptyTest()
    {
        string str = "";
        decimal result = str.ToDecimal();

        Assert.True(result == 0m);
    }

    [Fact]
    public void ToDecimalZeroTest()
    {
        string str = "0";
        decimal result = str.ToDecimal();

        Assert.True(result == 0m);
    }

    [Fact]
    public void ToDecimalNegitiveTest()
    {
        string str = "-10";
        decimal result = str.ToDecimal();

        Assert.True(result == -10m);
    }

}

public enum Test
{
    one = 1,
    two = 2,
    three = 3
}

