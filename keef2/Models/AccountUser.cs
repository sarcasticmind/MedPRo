using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace keef2.Models
{
    public class AccountUser : IdentityUser
    {
        public virtual List<Message> Messages { get; set; }
    }
}
