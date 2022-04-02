using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Models
{
    public class User : IdentityUser<Guid>
    {
        public User()
        {
            RefreshTokens = new HashSet<RefreshToken>();
            //PurchaseOrderRequests = new HashSet<PurchaseOrder>();
            //PurchaseOrderApprovals = new HashSet<PurchaseOrder>();
            //GRNReceives = new HashSet<GoodsReceivedNote>();
            //GRNApprovals = new HashSet<GoodsReceivedNote>();
            UserRoles = new HashSet<UserRole>();
            FirstName = default!;
            LastName = default!;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        //public virtual ICollection<PurchaseOrder> PurchaseOrderRequests { get; set; }
        //public virtual ICollection<PurchaseOrder> PurchaseOrderApprovals { get; set; }
        //public virtual ICollection<GoodsReceivedNote> GRNReceives { get; set; }
        //public virtual ICollection<GoodsReceivedNote> GRNApprovals { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
