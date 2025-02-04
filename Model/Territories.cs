﻿using System;
using System.Collections.Generic;

namespace NorthwindConsole.Model
{
    public partial class Territories
    {
        public Territories()
        {
            EmployeeTerritories = new HashSet<EmployeeTerritories>();
        }

        public override string ToString()
        {
            return $"Terr Id: {TerritoryId} Desc: {TerritoryDescription} Region Id: {RegionId}";
        }

        public string TerritoryId { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionId { get; set; }

        public virtual Region Region { get; set; }
        public virtual ICollection<EmployeeTerritories> EmployeeTerritories { get; set; }
    }
}