using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Models
{
    public class Role : IdentityRole<Guid>
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }

    public enum Roles
    {
        SuperAdmin,
        Admin,
        Report,
        Normal
    }
}
