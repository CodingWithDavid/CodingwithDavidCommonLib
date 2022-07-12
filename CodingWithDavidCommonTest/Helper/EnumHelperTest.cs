    public enum Statuses
    {
        Unknown = 0,
        New = 1,
        Processed = 2,
        Error = 3,
        Completed = 4,
        Queued = 5,
        Uploaded = 6,
        [System.ComponentModel.Description("Marked for Deletion")]
        MarkForDeleted = 7,
        [System.ComponentModel.Description("Ready for Download")]
        ReadyForDownload = 8,
    }

    public class EnumHelperTest
    {
        [Fact]
        public void GetDescriptionStringPositiveTest()
        {
            string result = EnumParse.GetDescription(Statuses.MarkForDeleted);
            Assert.True(result.Match("Marked for Deletion"));
        }

        [Fact]
        public void GetDefaultStringPositiveTest()
        {
            string result = EnumParse.GetDescription(Statuses.Queued);
            Assert.True(result.Match("Queued"));
        }
    }
