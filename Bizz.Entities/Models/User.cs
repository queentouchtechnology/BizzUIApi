using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bizz.Entities.Models
{
    public class User
    {

        public int Id { get; set; }
        public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public int OrgId { get; set; }
            public string Role { get; set; }
            public string DefaultStatus { get; set; }
            public bool IsActive { get; set; }
            public string Mobile { get; set; }
            public string Address { get; set; }
            public string Code { get; set; }
            public int ReportToId { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public decimal UserWallet { get; set; }
            public string UserImageUrl { get; set; }
            public string DashboardUrl { get; set; }
            public string Website { get; set; }
        
    }
}
