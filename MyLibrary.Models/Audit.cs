using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Models
{
    public class Audit : MyRestaurantObject
    {
        public Audit()
        {
            Username = default!;
            TableName = default!;
            Action = default!;
        }

        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Username { get; set; }
        public string TableName { get; set; }
        public string Action { get; set; }
        public string? KeyValues { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
    }
}
