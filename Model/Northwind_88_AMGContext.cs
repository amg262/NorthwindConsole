using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Web;


namespace NorthwindConsole.Model
{
    public partial class Northwind_88_AMGContext : DbContext
    {
        public Northwind_88_AMGContext()
        {
        }

        public Northwind_88_AMGContext(DbContextOptions<Northwind_88_AMGContext> options)
            : base(options)
        {
        }

        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config")
            .GetCurrentClassLogger();

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<EmployeeTerritories> EmployeeTerritories { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Shippers> Shippers { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<Territories> Territories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
                optionsBuilder.UseSqlServer(@config["NorthwindContext:ConnectionString"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.Address).HasMaxLength(60);

                entity.Property(e => e.City).HasMaxLength(25);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.Country).HasMaxLength(15);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(24);

                entity.Property(e => e.Phone).HasMaxLength(24);

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.Region).HasMaxLength(15);
            });

            modelBuilder.Entity<EmployeeTerritories>(entity =>
            {
                entity.HasKey(e => new {e.EmployeeId, e.TerritoryId})
                    .IsClustered(false);

                entity.Property(e => e.TerritoryId).HasMaxLength(20);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeTerritories)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeTerritories_Employees");

                entity.HasOne(d => d.Territory)
                    .WithMany(p => p.EmployeeTerritories)
                    .HasForeignKey(d => d.TerritoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeTerritories_Territories");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.Property(e => e.Address).HasMaxLength(60);

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.City).HasMaxLength(15);

                entity.Property(e => e.Country).HasMaxLength(15);

                entity.Property(e => e.Extension).HasMaxLength(4);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.HireDate).HasColumnType("datetime");

                entity.Property(e => e.HomePhone).HasMaxLength(24);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.Region).HasMaxLength(15);

                entity.Property(e => e.Title).HasMaxLength(30);

                entity.Property(e => e.TitleOfCourtesy).HasMaxLength(25);

                entity.HasOne(d => d.ReportsToNavigation)
                    .WithMany(p => p.InverseReportsToNavigation)
                    .HasForeignKey(d => d.ReportsTo)
                    .HasConstraintName("FK_Employees_Employees");
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.Property(e => e.Discount).HasColumnType("decimal(5, 3)");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

                entity.Property(e => e.UnitPrice).HasColumnType("money");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Products");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.Property(e => e.Freight)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.RequiredDate).HasColumnType("datetime");

                entity.Property(e => e.ShipAddress).HasMaxLength(60);

                entity.Property(e => e.ShipCity).HasMaxLength(15);

                entity.Property(e => e.ShipCountry).HasMaxLength(15);

                entity.Property(e => e.ShipName).HasMaxLength(40);

                entity.Property(e => e.ShipPostalCode).HasMaxLength(10);

                entity.Property(e => e.ShipRegion).HasMaxLength(15);

                entity.Property(e => e.ShippedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Orders_Customers");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Orders_Employees");

                entity.HasOne(d => d.ShipViaNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShipVia)
                    .HasConstraintName("FK_Orders_Shippers");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.QuantityPerUnit).HasMaxLength(20);

                entity.Property(e => e.ReorderLevel).HasDefaultValueSql("((0))");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UnitsInStock).HasDefaultValueSql("((0))");

                entity.Property(e => e.UnitsOnOrder).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Products_Categories");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_Products_Suppliers");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.HasKey(e => e.RegionId)
                    .IsClustered(false);

                entity.Property(e => e.RegionId).ValueGeneratedNever();

                entity.Property(e => e.RegionDescription)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Shippers>(entity =>
            {
                entity.HasKey(e => e.ShipperId);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.Phone).HasMaxLength(24);
            });

            modelBuilder.Entity<Suppliers>(entity =>
            {
                entity.HasKey(e => e.SupplierId);

                entity.Property(e => e.Address).HasMaxLength(60);

                entity.Property(e => e.City).HasMaxLength(15);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.ContactName).HasMaxLength(30);

                entity.Property(e => e.ContactTitle).HasMaxLength(30);

                entity.Property(e => e.Country).HasMaxLength(15);

                entity.Property(e => e.Fax).HasMaxLength(24);

                entity.Property(e => e.Phone).HasMaxLength(24);

                entity.Property(e => e.PostalCode).HasMaxLength(10);

                entity.Property(e => e.Region).HasMaxLength(15);
            });

            modelBuilder.Entity<Territories>(entity =>
            {
                entity.HasKey(e => e.TerritoryId)
                    .IsClustered(false);

                entity.Property(e => e.TerritoryId).HasMaxLength(20);

                entity.Property(e => e.TerritoryDescription)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Territories)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Territories_Region");
            });

            OnModelCreatingPartial(modelBuilder);
        }


        public List<Products> GetProducts()
        {
            return Products.ToList();
        }

        public void AddProduct(Products prod)
        {
            ValidationContext context = new ValidationContext(prod, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(prod, context, results, true);
            if (isValid)
            {
                var db = new Northwind_88_AMGContext();
                // check for unique name`
                if (db.Products.Any(c => c.ProductName == prod.ProductName))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("Name exists", new string[] {"ProductName"}));
                }
                else
                {
                    logger.Info("Validation passed");
                    // TODO: save category to db

                    Products.Add(prod);
                    SaveChanges();
                    logger.Info($"{prod.ToString()} added");
                }
            }

            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }

        public List<Products> QueryProducts(string query)
        {
            var foundProds = new List<Products>();

            var allProds = Products.ToList();
            Int32.TryParse(query, out int prodId);
            Int32.TryParse(query, out int suppId);
            Int32.TryParse(query, out int catId);
            //Int32.TryParse(query, out int qtyUnit);
            Decimal.TryParse(query, out decimal unitPrice);
            Int32.TryParse(query, out int unitStock);
            Int32.TryParse(query, out int unitsOrder);
            Int32.TryParse(query, out int reorder);
            Boolean.TryParse(query, out bool distcontinued);

            foreach (var prod in allProds.Where(p => p.ProductId == prodId)) foundProds.Add(prod);
            foreach (var prod in allProds.Where(p => p.ProductName.Contains(query))) foundProds.Add(prod);
            foreach (var prod in allProds.Where(p => p.CategoryId == catId)) foundProds.Add(prod);
            foreach (var prod in allProds.Where(p => p.SupplierId == suppId)) foundProds.Add(prod);
            foreach (var prod in allProds.Where(p => p.QuantityPerUnit.Contains(query))) foundProds.Add(prod);
            foreach (var prod in allProds.Where(p => p.UnitPrice == unitPrice)) foundProds.Add(prod);
            foreach (var prod in allProds.Where(p => p.UnitsInStock == unitStock)) foundProds.Add(prod);
            foreach (var prod in allProds.Where(p => p.UnitsOnOrder == unitsOrder)) foundProds.Add(prod);
            foreach (var prod in allProds.Where(p => p.ReorderLevel == reorder)) foundProds.Add(prod);
            foreach (var prod in allProds.Where(p => p.Discontinued == distcontinued)) foundProds.Add(prod);


            return foundProds;
        }


        public void DeleteProduct(Products prod)
        {
            var pointer = prod;
            Products.Remove(prod);
            SaveChanges();
            logger.Info($"{pointer.ToString()} deleted");
        }

        public void EditProduct(Products prod)
        {
            ValidationContext context = new ValidationContext(prod, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(prod, context, results, true);
            if (isValid)
            {
                var db = new Northwind_88_AMGContext();
                // check for unique name`
                if (db.Products.Any(c => c.ProductName == prod.ProductName))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("Name exists", new string[] {"ProductName"}));
                }
                else
                {
                    logger.Info("Validation passed");
                    // TODO: save category to db

                    Products.Update(prod);
                    SaveChanges();
                    logger.Info($"{prod.ToString()} editted");
                }
            }

            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }

        public Products GetProductById(int prodId)
        {
            var prod = new Products() {ProductId = prodId};
            return prod;
        }

        public List<Products> GetProductInListById(Products prod)
        {
            var list = new List<Products>(Products.Where(p => p.ProductId == prod.ProductId));
            return list;
        }

        public List<Products> GetDiscontinuedProds()
        {
            var list = new List<Products>(Products.Where(p => p.Discontinued == true));
            return list;
        }

        public List<Products> GetActiveProds()
        {
            var list = new List<Products>(Products.Where(p => p.Discontinued == false));
            return list;
        }

        public List<Categories> GetCategories()
        {
            return Categories.ToList();
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public List<Categories> QueryCategories(string query)
        {
            var foundCats = new List<Categories>();

            List<Categories> allCats = Categories.ToList();
            Int32.TryParse(query, out int catId);
            Int32.TryParse(query, out int prodId);

            foreach (Categories cat in allCats.Where(c => c.CategoryId == catId)) foundCats.Add(cat);
            foreach (Categories cat in allCats.Where(c => c.CategoryName.Contains(query))) foundCats.Add(cat);
            foreach (Categories cat in allCats.Where(c => c.Description.Contains(query))) foundCats.Add(cat);


            return foundCats;
        }


        public List<Categories> GetCategoryInListById(Categories cat)
        {
            var list = new List<Categories>(Categories.Where(c => c.CategoryId == cat.CategoryId));
            return list;
        }

        public Categories GetCategoryById(int catId)
        {
            var cat = new Categories() {CategoryId = catId};
            return cat;
        }

        public void DeleteCategory(Categories cat)
        {
            var pointer = cat;
            Categories.Remove(cat);
            logger.Info($"{cat.ToString()} deleted");
            SaveChanges();
        }

        public void AddCategory(Categories category)
        {
            ValidationContext context = new ValidationContext(category, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(category, context, results, true);
            if (isValid)
            {
                var db = new Northwind_88_AMGContext();
                // check for unique name`
                if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("Name exists", new string[] {"CategoryName"}));
                }
                else
                {
                    logger.Info("Validation passed");
                    // TODO: save category to db

                    Categories.Add(category);
                    SaveChanges();
                    logger.Info($"{category.ToString()} added");
                }
            }

            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }

        public void EditCategory(Categories category)
        {
            ValidationContext context = new ValidationContext(category, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(category, context, results, true);
            if (isValid)
            {
                var db = new Northwind_88_AMGContext();
                // check for unique name`
                if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("Name exists", new string[] {"CategoryName"}));
                }
                else
                {
                    logger.Info("Validation passed");
                    // TODO: save category to db

                    Categories.Update(category);
                    SaveChanges();
                    logger.Info($"{category.ToString()} editted");
                }
            }

            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }

        public void GetOutputCategoryProductData()
        {
            var prods = new List<Products>();
            //var cats = new List<Categories>();

            prods = this.GetProducts();
            var cats = this.GetCategories();

            foreach (var cat in cats)
            {
                Console.ForegroundColor = ConsoleColor.Blue;

                Console.WriteLine($"Cat Id: {cat.CategoryId} Cat Name: {cat.CategoryName} Cat Desc: {cat.Description}");

                foreach (var prod in cat.Products)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine($"Product Data");
                    Console.Write(prod.ToString() + "\n");
                }

                Console.ForegroundColor = ConsoleColor.DarkCyan;
            }
        }

        public void GetCategoryProductDataByCatId(int catId)
        {
            var cat = this.GetCategoryById(catId);

            Console.WriteLine($"Product Data for: {cat.CategoryName}");

            foreach (var prod in cat.Products)
            {
                Console.Write($"{prod.ToString()}\n");
            }
        }

        public void GetCategoryProductNameByCatId(int catId)
        {
            var cats = this.GetCategories();

            var prods = this.GetProducts();

            foreach (var cat in cats)
            {
                if (cat.CategoryId == catId)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Products in {cat.CategoryName}");

                    foreach (var prod in prods)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        if (prod.CategoryId == catId)
                        {
                            Console.Write($"{prod.ProductName}\n");
                        }
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
        }


        /*
         * TEMPLATE FOR EACH OF THE EASY CLASSES
         */
        public void AddRegion(Region reg)
        {
            ValidationContext context = new ValidationContext(reg, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(reg, context, results, true);
            if (isValid)
            {
                var db = new Northwind_88_AMGContext();
                // check for unique name`
                if (db.Region.Any(c => c.RegionDescription == reg.RegionDescription))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("Desc exists", new string[] {"RegionDesc"}));
                }
                else
                {
                    logger.Info("Validation passed");
                    // TODO: save category to db

                    Region.Add(reg);
                    SaveChanges();
                    logger.Info($"{reg.ToString()} added");
                }
            }

            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }

        public List<Region> GetRegions()
        {
            return Region.ToList();
        }


        public void DeleteRegion(Region reg)
        {
            var pointer = reg;
            Region.Remove(reg);
            logger.Info($"{reg.RegionId} removed");
            SaveChanges();
        }

        public Region GetRegionById(int id)
        {
            Region reg = new Region() {RegionId = id};
            return reg;
        }

        public void EditRegion(Region reg)
        {
            ValidationContext context = new ValidationContext(reg, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(reg, context, results, true);
            if (isValid)
            {
                var db = new Northwind_88_AMGContext();
                // check for unique name`
                if (db.Region.Any(c => c.RegionDescription == reg.RegionDescription))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("Desc exists", new string[] {"RegionDesc"}));
                }
                else
                {
                    logger.Info("Validation passed");
                    // TODO: save category to db

                    Region.Update(reg);
                    SaveChanges();
                    logger.Info($"{reg.RegionId} updated");
                }
            }

            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }


        /*
         * TEMPLATE FOR EACH OF THE EASY CLASSES
         */
        public void AddTerritory(Territories ter)
        {
            ValidationContext context = new ValidationContext(ter, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(ter, context, results, true);
            if (isValid)
            {
                var db = new Northwind_88_AMGContext();
                // check for unique name`
                if (db.Territories.Any(c => c.TerritoryDescription == ter.TerritoryDescription))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("Desc exists", new string[] {"TerrDesc"}));
                }
                else
                {
                    logger.Info("Validation passed");
                    // TODO: save category to db
                    Territories.Add(ter);
                    SaveChanges();
                    logger.Info($"{ter.ToString()} added");
                }
            }

            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }

        public List<Territories> GetTerritories()
        {
            return Territories.ToList();
        }


        public void DeleteTerritory(Territories ter)
        {
            var pointer = ter;
            Territories.Remove(ter);
            logger.Info($"{ter.TerritoryId} removed");
            SaveChanges();
        }

        public Territories GetTerritoryById(string id)
        {
            Territories ter = new Territories() {TerritoryId = id};
            return ter;
        }

        public void EditTerritory(Territories ter)
        {
            ValidationContext context = new ValidationContext(ter, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(ter, context, results, true);
            if (isValid)
            {
                var db = new Northwind_88_AMGContext();
                // check for unique name`
                if (db.Territories.Any(c => c.TerritoryDescription == ter.TerritoryDescription))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("Desc exists", new string[] {"TerrDesc"}));
                }
                else
                {
                    logger.Info("Validation passed");
                    // TODO: save category to db
                    Territories.Update(ter);
                    SaveChanges();
                    logger.Info($"{ter.TerritoryId} updated");
                }
            }

            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }


        /*
         * TEMPLATE FOR EACH OF THE EASY CLASSES
         */
        public void AddShipper(Shippers ship)
        {
            ValidationContext context = new ValidationContext(ship, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(ship, context, results, true);
            if (isValid)
            {
                var db = new Northwind_88_AMGContext();
                // check for unique name`
                if (db.Shippers.Any(c => c.CompanyName == ship.CompanyName))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("Name exists", new string[] {"CompanyName"}));
                }

                if (db.Shippers.Any(c => c.Phone == ship.Phone))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("phone exists", new string[] {"Phone"}));
                }
                else
                {
                    logger.Info("Validation passed");
                    // TODO: save category to db
                    Shippers.Add(ship);
                    SaveChanges();
                    logger.Info($"{ship.ShipperId} add");
                }
            }

            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }

        public List<Shippers> GetShippers()
        {
            return Shippers.ToList();
        }


        public void DeleteShipper(Shippers ship)
        {
            var pointer = ship;
            Shippers.Remove(ship);
            logger.Info($"{ship.ShipperId} removed");
            SaveChanges();
        }

        public Shippers GetShipperById(int id)
        {
            Shippers ship = new Shippers() {ShipperId = id};
            return ship;
        }

        public void EditShipper(Shippers ship)
        {
            ValidationContext context = new ValidationContext(ship, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(ship, context, results, true);
            if (isValid)
            {
                var db = new Northwind_88_AMGContext();
                // check for unique name`
                if (db.Shippers.Any(c => c.CompanyName == ship.CompanyName))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("name exists", new string[] {"CompanyName"}));
                }

                if (db.Shippers.Any(c => c.Phone == ship.Phone))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("phone exists", new string[] {"Phone"}));
                }
                else
                {
                    logger.Info("Validation passed");
                    // TODO: save category to db

                    Shippers.Update(ship);
                    SaveChanges();
                    logger.Info($"{ship.ShipperId} updated");
                }
            }

            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }


        /*
         * TEMPLATE FOR EACH OF THE EASY CLASSES
         */
        public void AddEt(EmployeeTerritories et)
        {
            ValidationContext context = new ValidationContext(et, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(et, context, results, true);
            if (isValid)
            {
                var db = new Northwind_88_AMGContext();
                // check for unique name`
                if (db.EmployeeTerritories.Any(c => c.EmployeeId == et.EmployeeId) &&
                    db.EmployeeTerritories.Any(c => c.TerritoryId == et.TerritoryId))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("IDs exists", new string[] {"EmpIdTerId"}));
                }
                else
                {
                    logger.Info("Validation passed");
                    // TODO: save category to db

                    EmployeeTerritories.Add(et);
                    SaveChanges();
                    logger.Info($"{et.EmployeeId} {et.TerritoryId} updated");
                }
            }

            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }

        public List<EmployeeTerritories> GetEt()
        {
            return EmployeeTerritories.ToList();
        }


        public void DeleteEt(EmployeeTerritories et)
        {
            var pointer = et;
            EmployeeTerritories.Remove(et);
            logger.Info($"{et.EmployeeId} {et.TerritoryId} removed");
            SaveChanges();
        }

        public EmployeeTerritories GetEtnById(int eId, string tId)
        {
            EmployeeTerritories et = new EmployeeTerritories() {EmployeeId = eId, TerritoryId = tId};
            return et;
        }

        public void EditEt(EmployeeTerritories et)
        {
            ValidationContext context = new ValidationContext(et, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(et, context, results, true);
            if (isValid)
            {
                var db = new Northwind_88_AMGContext();
                // check for unique name`
                if (db.EmployeeTerritories.Any(c => c.EmployeeId == et.EmployeeId) &&
                    db.EmployeeTerritories.Any(c => c.TerritoryId == et.TerritoryId))
                {
                    // generate validation error
                    isValid = false;
                    results.Add(new ValidationResult("IDs exists", new string[] {"EmpIdTerId"}));
                }
                else
                {
                    logger.Info("Validation passed");
                    // TODO: save category to db

                    EmployeeTerritories.Update(et);
                    SaveChanges();
                    logger.Info($"{et.EmployeeId} {et.TerritoryId} editted");
                }
            }

            if (!isValid)
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }
    }
}