using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bizz.Entities.Models
{
    public class OrganizationModel
    {
        public int Id { get; set; }
		[Required(ErrorMessage = "Organization name is required.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Contact name is required.")]
		public string ContactName { get; set; }

		[Required(ErrorMessage = "Email is required.")]
		public string Email { get; set; }
        public int Mid { get; set; }
		[Required(ErrorMessage = "Mobile Number is required.")]
		public string Mobile { get; set; }
        public string Address { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Decimal OrgWallet { get; set; }
        public string OrgImageUrl { get; set; }
        public string OrgPass { get; set; }
        public int MstrId { get; set; }
        public string TokenId { get; set; }
        public string OrgWebsite { get; set; }
        public string APIToken { get; set; }
        public string DashboardUrl { get; set; }
    }
}
