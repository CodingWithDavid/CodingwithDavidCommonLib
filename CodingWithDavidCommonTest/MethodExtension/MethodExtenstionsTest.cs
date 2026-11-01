public class MethodExtenstionsTest
{
    [Fact]
    public void EnumFromAnIntegerTest()
    {
        string result = EnumParse.ParseEnum<Test>(2);
        Assert.True(result.Match("two"));
    }

    [Fact]
    public void EnumFromAnIntegerEmptyReturnTest()
    {
        string result = EnumParse.ParseEnum<Test>(20);
        Assert.True(string.IsNullOrEmpty(result));
    }

    [Fact]
    public void EnumFromAStringTest()
    {
        Assert.Equal(Test.one, EnumParse.ParseEnum<Test>("one"));
    }

    [Fact]
    public void EnumFromAnEmptyStringThrowsTest()
    {
        Assert.Throws<ArgumentException>(() => EnumParse.ParseEnum<Test>(""));
    }
}

public enum Test
{
    one = 1,
    two = 2,
    three = 3
}
