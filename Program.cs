using System;
using NLog.Web;
using System.IO;
using System.Linq;
using NorthwindConsole.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Channels;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace NorthwindConsole
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config")
            .GetCurrentClassLogger();

        static void Main(string[] args)
        {
            logger.Info("Program started");

            try
            {
                string choice;
                do
                {
                    /*
                     * Setting the bells and whistles. Colors, Timer, 
                     */

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Timer programTimer = new Timer();
                    programTimer.Start();


                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine($"Welcome to Andrew Gunn's Final Project");


                    Console.WriteLine($"You must choose which of 2 portals this program will enter.");
                    Console.WriteLine($"Enter the number of which portal to go initialize");
                    Console.WriteLine();
                    Console.WriteLine("1) Products");
                    Console.WriteLine("2) Categories");
                    Console.WriteLine("3) Shippers");
                    Console.WriteLine("4) Territories");
                    Console.WriteLine("5) Regions");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("");

                    try
                    {
                        Int32.TryParse(Console.ReadLine(), out int portal);


                        if (portal == 1)
                        {
                            var db = new Northwind_88_AMGContext();


                            // check for unique name`


                            Console.WriteLine("You enter the PRODUCT portal");
                            Console.WriteLine($"1) Add New Product");
                            Console.WriteLine("2) Edit Product");
                            Console.WriteLine("3) Display");
                            Console.WriteLine("4) Advanced Search");
                            Console.WriteLine("5) Delete");

                            Int32.TryParse(Console.ReadLine(), out int productPortal);

                            if (productPortal == 1)
                            {
                                Console.WriteLine("Name: ");
                                string name = Console.ReadLine();
                                Console.WriteLine("Supplier ID: ");
                                Int32.TryParse(Console.ReadLine(), out int supp);

                                Console.WriteLine("Cat ID: ");
                                Int32.TryParse(Console.ReadLine(), out int cat);

                                Console.WriteLine("Qty/Unit: ");
                                string qtyUnit = Console.ReadLine();

                                Console.WriteLine("Unit Price: ");
                                Decimal.TryParse(Console.ReadLine(), out decimal unitPrice);

                                Console.WriteLine("Unit Stock: ");
                                Int32.TryParse(Console.ReadLine(), out int unitStock);


                                // string unitStock = Console.ReadLine();

                                Console.WriteLine("Units Order: ");
                                Int32.TryParse(Console.ReadLine(), out int unitOrder);

                                Console.WriteLine("Reorder Level: ");
                                Int32.TryParse(Console.ReadLine(), out int reorder);

                                Console.WriteLine("Distoninued: ");
                                Boolean.TryParse(Console.ReadLine(), out bool discont);

                                string e = "";

                                var product = new Products
                                {
                                    ProductName = name, SupplierId = supp, CategoryId = cat, QuantityPerUnit = qtyUnit,
                                    UnitPrice = unitPrice, UnitsInStock = (short) unitStock,
                                    UnitsOnOrder = (short) unitOrder, ReorderLevel = (short) reorder,
                                    Discontinued = discont
                                };

                                db.AddProduct(product);
                            }
                            else if (productPortal == 2)
                            {
                                Console.WriteLine("Product ID input will select and overwrite values with new inputs");
                                Console.WriteLine("Edit Product with Id: ");
                                Int32.TryParse(Console.ReadLine(), out int prodId);

                                Console.WriteLine("Enter new name: ");
                                string name = Console.ReadLine();

                                Console.WriteLine("New Supplier ID: ");
                                Int32.TryParse(Console.ReadLine(), out int supp);

                                Console.WriteLine("New Cat ID: ");
                                Int32.TryParse(Console.ReadLine(), out int cat);

                                Console.WriteLine("New Qty/Unit: ");
                                string qtyUnit = Console.ReadLine();

                                Console.WriteLine("New Unit Price: ");
                                Decimal.TryParse(Console.ReadLine(), out decimal unitPrice);

                                Console.WriteLine("New Unit Stock: ");
                                Int32.TryParse(Console.ReadLine(), out int unitStock);

                                Console.WriteLine("New Units Order: ");
                                Int32.TryParse(Console.ReadLine(), out int unitOrder);

                                Console.WriteLine("New Reorder Level: ");
                                Int32.TryParse(Console.ReadLine(), out int reorder);

                                Console.WriteLine("New Distoninued: ");
                                Boolean.TryParse(Console.ReadLine(), out bool discont);

                                var product = new Products
                                {
                                    ProductName = name, SupplierId = supp, CategoryId = cat, QuantityPerUnit = qtyUnit,
                                    UnitPrice = unitPrice, UnitsInStock = (short) unitStock,
                                    UnitsOnOrder = (short) unitOrder, ReorderLevel = (short) reorder,
                                    Discontinued = discont
                                };

                                Products foundProd = db.GetProductById(prodId);

                                foundProd.ToString();
                                foundProd.ProductName = name;
                                foundProd.SupplierId = supp;
                                foundProd.CategoryId = cat;
                                foundProd.QuantityPerUnit = qtyUnit;
                                foundProd.UnitPrice = unitPrice;
                                foundProd.UnitsInStock = (short) unitStock;
                                foundProd.UnitsOnOrder = (short) unitOrder;
                                foundProd.ReorderLevel = (short) reorder;
                                foundProd.Discontinued = discont;

                                db.EditProduct(foundProd);

                                Products output = db.GetProductById(prodId);
                                //Products newProd = new Products() {ProductName = "New new"};
                                output.ToString();
                                Console.WriteLine();
                            }
                            else if (productPortal == 3)
                            {
                                Console.WriteLine("You entered Product Display Portal");
                                Console.WriteLine("1) Display All Products showing Product Name");
                                Console.WriteLine("2) Display All Products showing All Fields");
                                Console.WriteLine("3) Display Discontinued Products");
                                Console.WriteLine("4) Display Active Products");
                                Console.WriteLine("5) Display a Specific Product show All Fields");

                                Int32.TryParse(Console.ReadLine(), out int prodDisplayPortal);

                                if (prodDisplayPortal == 1)
                                {
                                    foreach (var prod in db.GetProducts())
                                    {
                                        Console.WriteLine(prod.ProductName + "\n");
                                    }
                                }
                                else if (prodDisplayPortal == 2)
                                {
                                    foreach (var prod in db.GetProducts())
                                    {
                                        if (prod.Discontinued == true)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Cyan;
                                        }

                                        Console.Write(prod.ToString() + "\n");
                                    }
                                }
                                else if (prodDisplayPortal == 3)
                                {
                                    foreach (var prod in db.GetDiscontinuedProds())
                                    {
                                        Console.Write(
                                            $"Id: {prod.ProductId} Name: {prod.ProductName} Discont: {prod.Discontinued}\n");
                                    }
                                }
                                else if (prodDisplayPortal == 4)
                                {
                                    foreach (var prod in db.GetActiveProds())
                                    {
                                        Console.Write(
                                            $"Id: {prod.ProductId} Name: {prod.ProductName} Discont: {prod.Discontinued}\n");
                                    }
                                }
                                else if (prodDisplayPortal == 5)
                                {
                                    Console.WriteLine("Show Product with Id: ");
                                    Int32.TryParse(Console.ReadLine(), out int id);

                                    var showProd = db.GetProductById(id);

                                    Console.Write($"{showProd.ToString()}\n");
                                }
                                else
                                {
                                }
                            }
                            else if (productPortal == 4)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;

                                Console.WriteLine("This is a very Advanced Search for Products and data");
                                Console.WriteLine(
                                    "If ANY of a products's fields EVEN CONTAINS let alone MATCHES, it will show.");
                                Console.WriteLine("This is the most in-depth and thorough search you could program.");
                                Console.WriteLine("For the sake of display, just the product Id, name will be shown.");
                                Console.WriteLine(
                                    "Its assumed you would understand the result you get rather than a blind search which would get very involved.");
                                Console.WriteLine(
                                    "But it will return all the entries youd need and make your informed selection after.");
                                Console.WriteLine("Search Query");
                                string query = Console.ReadLine();

                                var queryiedProds = db.QueryProducts(query);

                                var qls = new List<Categories>();

                                Console.ForegroundColor = ConsoleColor.Cyan;
                                foreach (var ql in queryiedProds)
                                {
                                    Console.Write($"Id: {ql.ProductId} Name: {ql.ProductName}\n");
                                }


                                Console.WriteLine("Search Successful!");

                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                            }
                            else if (productPortal == 5)
                            {
                                Console.WriteLine("Enter Prod Id to Delete");
                                Int32.TryParse(Console.ReadLine(), out int id);

                                var prod = db.GetProductById(id);
                                db.DeleteProduct(prod);
                                Console.WriteLine("Delete successful");
                            }
                            else
                            {
                                logger.Warn("Incorrect product portal");
                                Console.WriteLine("Incorrect product portal");
                            }
                        }
                        else if (portal == 2)
                        {
                            Console.WriteLine("You enter the CATEGORY portal");
                            Console.WriteLine($"1) Add New Category");
                            Console.WriteLine("2) Edit Category");
                            Console.WriteLine("3) Display");
                            Console.WriteLine("4) Advanced Search");
                            Console.WriteLine("5) Delete");


                            Int32.TryParse(Console.ReadLine(), out int catPortal);
                            var db = new Northwind_88_AMGContext();


                            if (catPortal == 1)
                            {
                                Console.WriteLine("Cat Name: ");
                                string name = Console.ReadLine();

                                Console.WriteLine("Cat Description: ");
                                string desc = Console.ReadLine();


                                string e = "";

                                var Category = new Categories()
                                {
                                    CategoryName = name, Description = desc
                                };

                                db.AddCategory(Category);
                            }
                            else if (catPortal == 2)
                            {
                                Console.WriteLine("Category ID input will select and overwrite values with new inputs");
                                Console.WriteLine("Edit Category with Id: ");
                                Int32.TryParse(Console.ReadLine(), out int catId);

                                Console.WriteLine("Enter new cat name: ");
                                string name = Console.ReadLine();

                                Console.WriteLine("Enter new cat desc: ");
                                string desc = Console.ReadLine();

                                var Category = new Categories()
                                {
                                    CategoryName = name, Description = desc
                                };

                                Categories foundCat = db.GetCategoryById(catId);

                                foundCat.ToString();
                                foundCat.CategoryName = name;
                                foundCat.CategoryId = catId;
                                foundCat.Description = desc;


                                db.EditCategory(foundCat);

                                Categories output = db.GetCategoryById(catId);
                                output.ToString();
                                Console.WriteLine();
                            }
                            else if (catPortal == 3)
                            {
                                Console.WriteLine("You entered Category Display Portal");
                                Console.WriteLine("1) Display All Categories showing Category Name");
                                Console.WriteLine("2) Display All Categories showing All Fields");
                                Console.WriteLine("5) Display a Specific Category");
                                Console.WriteLine("6) Display Categories + Related Product data");
                                Console.WriteLine("7) Display Product Data for Category Id");


                                Int32.TryParse(Console.ReadLine(), out int catDisplayPortal);

                                if (catDisplayPortal == 1)
                                {
                                    foreach (var cat in db.GetCategories())
                                    {
                                        Console.Write(cat.CategoryName + "\n");
                                    }
                                }
                                else if (catDisplayPortal == 2)
                                {
                                    foreach (var cat in db.GetCategories())
                                    {
                                        Console.Write(
                                            $"Id: {cat.CategoryId} Name: {cat.CategoryName} Desc: {cat.Description}\n");
                                    }
                                }
                                else if (catDisplayPortal == 3)
                                {
                                }
                                else if (catDisplayPortal == 4)
                                {
                                }
                                else if (catDisplayPortal == 5)
                                {
                                    Console.WriteLine("Show Category with Id: ");
                                    Int32.TryParse(Console.ReadLine(), out int id);

                                    var showCat = db.GetCategoryById(id);

                                    Console.Write(showCat.ToString() + "\n");
                                }
                                else if (catDisplayPortal == 6)
                                {
                                    db.GetOutputCategoryProductData();
                                }
                                else if (catDisplayPortal == 7)
                                {
                                    Console.WriteLine("Product Data for Cat Id of: ");
                                    Int32.TryParse(Console.ReadLine(), out int id);

                                    db.GetCategoryProductNameByCatId(id);
                                }
                                else
                                {
                                }
                            }
                            else if (catPortal == 4)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;

                                Console.WriteLine("This is a very Advanced Search for Categories and data");
                                Console.WriteLine(
                                    "If ANY of a category's fields EVEN CONTAINS let alone MATCHES, it will show.");
                                Console.WriteLine("This is the most in-depth and thorough search you could program.");
                                Console.WriteLine("For the sake of display, just the product Id, name will be shown.");
                                Console.WriteLine(
                                    "Its assumed you would understand the result you get rather than a blind search which would get very involved.");
                                Console.WriteLine(
                                    "But it will return all the entries youd need and make your informed selection after.");
                                Console.WriteLine("Search Query");
                                string query = Console.ReadLine();

                                var queriedCats = db.QueryCategories(query);

                                var qls = new List<Categories>();

                                foreach (var ql in queriedCats)
                                {
                                    qls.Add(ql);
                                    Console.Write($"{ql.CategoryName}");
                                }


                                Console.WriteLine("Search Successful!");

                                // foreach (var q in qls)
                                // {
                                //     Console.WriteLine($"{q.CategoryId} {q.CategoryName}");
                                // }
                            }
                            else if (catPortal == 5)
                            {
                                Console.WriteLine("Enter Cat Id to Delete");
                                Int32.TryParse(Console.ReadLine(), out int id);

                                var cat = db.GetCategoryById(id);
                                db.DeleteCategory(cat);
                                Console.WriteLine("Delete successful");
                            }
                            else
                            {
                                logger.Warn("Incorrect product portal");
                                Console.WriteLine("Incorrect product portal");
                            }
                        }
                        else
                        {
                            logger.Warn("Invalid portal");
                            Console.WriteLine("1, 2 are only valid portals to input");
                        }

                        Console.WriteLine("1) Display Categories");
                        Console.WriteLine("2) Add Category");
                        Console.WriteLine("3) Display Category and related products");
                        Console.WriteLine("4) Display all Categories and their related products");
                        //Console.WriteLine("5) Display All Category Data");
                        Console.WriteLine("\"q\" to quit");
                        choice = Console.ReadLine();
                        //Console.Clear();
                        logger.Info($"Option {choice} selected");


                        if (choice == "1")
                        {
                            var db = new Northwind_88_AMGContext();
                            var query = db.Categories.OrderBy(p => p.CategoryName);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{query.Count()} records returned");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.CategoryName} - {item.Description}");
                            }

                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (choice == "2")
                        {
                            Categories category = new Categories();
                            Console.WriteLine("Enter Category Name:");
                            category.CategoryName = Console.ReadLine();
                            Console.WriteLine("Enter the Category Description:");
                            category.Description = Console.ReadLine();

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
                        else if (choice == "3")
                        {
                            var db = new Northwind_88_AMGContext();
                            var query = db.Categories.OrderBy(p => p.CategoryId);

                            Console.WriteLine("Select the category whose products you want to display:");
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
                            }

                            Console.ForegroundColor = ConsoleColor.White;
                            int id = int.Parse(Console.ReadLine());
                            Console.Clear();
                            logger.Info($"CategoryId {id} selected");
                            Categories category = db.Categories.Include("Products")
                                .FirstOrDefault(c => c.CategoryId == id);
                            Console.WriteLine($"{category.CategoryName} - {category.Description}");
                            foreach (Products p in category.Products)
                            {
                                Console.Write(p.ProductName + "\n");
                            }
                        }
                        else if (choice == "4")
                        {
                            var db = new Northwind_88_AMGContext();
                            var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.CategoryName}");
                                foreach (Products p in item.Products)
                                {
                                    Console.WriteLine($"\t{p.ProductName}\n");
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        logger.Error(e);
                        throw e;
                    }

                    Console.WriteLine();
                } while (choice.ToLower() != "q");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            logger.Info("Program ended");
        }
    }
}