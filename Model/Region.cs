using System;
using System.Collections.Generic;

namespace NorthwindConsole.Model
{
    public partial class Region
    {
        public Region()
        {
            Territories = new HashSet<Territories>();
        }

        public override string ToString()
        {
            return $"Reg Id: {RegionId} Desc: {RegionDescription}";
        }

        public int RegionId { get; set; }
        public string RegionDescription { get; set; }

        public virtual ICollection<Territories> Territories { get; set; }
    }
}