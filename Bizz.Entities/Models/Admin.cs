using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bizz.Entities.Models
{
    public class Admin
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public string ContactName { get; set; }
            public string Email { get; set; }
            public string Mobile { get; set; }
            public string Address { get; set; }
            public string Code { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public string Password { get; set; }
            public decimal Wallet { get; set; }
            public string ImageUrl { get; set; }
            public string Website { get; set; }
            public string DashboardUrl { get; set; }
        
    }
}
