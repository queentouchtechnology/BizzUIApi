using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bizz.Entities.Models
{
    public class Procedure
    {

    }

    public class SetMasterLoginModel
    {
        public string Response { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public string UserType { get; set; }
    }
}
