using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Models
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public UserRole()
        {
            User = default!;
            Role = default!;
        }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
