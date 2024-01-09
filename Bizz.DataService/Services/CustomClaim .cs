using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bizz.DataService.Services
{
    public class CustomClaim : Claim
    {
        public string CustomField1 { get; set; }
        public int CustomField2 { get; set; }

        public CustomClaim(string type, string value, string customField1, int customField2)
            : base(type, value)
        {
            CustomField1 = customField1;
            CustomField2 = customField2;
        }
    }
}
