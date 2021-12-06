using System;
using System.Collections.Generic;

namespace NorthwindConsole.Model
{
    public partial class EmployeeTerritories
    {
        public int EmployeeId { get; set; }
        public string TerritoryId { get; set; }


        public override string ToString()
        {
            return $"Emp Id: {EmployeeId} Terr Id: {TerritoryId}";
        }

        public virtual Employees Employee { get; set; }
        public virtual Territories Territory { get; set; }
    }
}