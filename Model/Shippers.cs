using System;
using System.Collections.Generic;

namespace NorthwindConsole.Model
{
    public partial class Shippers
    {
        public Shippers()
        {
            Orders = new HashSet<Orders>();
        }

        public override string ToString()
        {
            return $"Ship Id: {ShipperId} Company Name: {CompanyName} Phone: {Phone}";
        }

        public int ShipperId { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}