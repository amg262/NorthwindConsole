using System;
using System.Collections.Generic;

namespace NorthwindConsole.Model
{
    public partial class Products
    {
        public Products()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        public virtual Categories Category { get; set; }
        public virtual Suppliers Supplier { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }


        public override string ToString()
        {
            return
                $"Prod ID: {ProductId} Prod Name: {ProductName}  Supp ID: {SupplierId}  Cat ID: {CategoryId}  QtyPerUnit: {QuantityPerUnit}  Unit Price: {UnitPrice}  Units In Stock: {UnitsInStock}  Units on Order: {UnitsOnOrder}  Reorder Level: {ReorderLevel}   Discontinued: {Discontinued}\n";
        }
    }
}