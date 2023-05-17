using Template.Backend.Data.Utilities;

namespace Template.Backend.Api.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
