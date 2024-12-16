using Microsoft.AspNetCore.Identity;

namespace BhoomiGlobalAPI.Entities
{
    public class User:IdentityUser
    {
        public string FName { get; set; }
        public string LName { get; set; }
    }
}
