using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bizz.Entities.Models;

public class CustomerResponse<T>
{
    public bool Status { get; set; }
    public string RespText { get; set; }
    public int Count { get; set; }  
    public List<T> Data { get; set; }
}
public class Customer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string Location { get; set; }
        public string AadharNo { get; set; }
        public string CompanyName { get; set; }
        public string GstNo { get; set; }
        public string PanNo { get; set; }
        public string RegistrationType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }

