using Template.Backend.Data.Utilities;

namespace Template.Backend.IntegrationTest.FakeServices
{
    public class FakeUserService : ICurrentUserService
    {
        public string? UserId => "TestUser@mail.com";
    }
}
