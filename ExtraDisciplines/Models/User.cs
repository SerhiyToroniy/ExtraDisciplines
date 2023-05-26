using Microsoft.AspNetCore.Identity;

namespace ExtraDisciplines.Models
{
    public class User : IdentityUser
    {
        public int Score { get; set; }
    }
}
