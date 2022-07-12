    public class EmailHelperTest
    {
        [Fact]
        public void TestGetDomainFromEmail()
        {
            string UUT = EmailHelper.EmailDomain("you@me.com");

            Assert.True(UUT.Match("me"));
        }

        [Fact]
        public void TestGetDomainFromEmailWithExtraDots()
        {
            string UUT = EmailHelper.EmailDomain("you.you2@Castle.me");

            Assert.True(UUT.Match("Castle"));
        }
    }
