using Template.Backend.Data.Utilities;

namespace Template.Backend.IntegrationTest.FakeServices
{
    public class FakeDateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
