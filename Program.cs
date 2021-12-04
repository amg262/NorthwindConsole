using System;
using NLog.Web;
using System.IO;
using System.Linq;
using NorthwindConsole.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                    Console.WriteLine($"1. Products   |   2. Categories");

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
                                        Console.WriteLine(prod.ProductName);
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

                                        Console.WriteLine(prod.ToString());
                                    }
                                }
                                else if (prodDisplayPortal == 3)
                                {
                                    foreach (var prod in db.GetDiscontinuedProds())
                                    {
                                        Console.WriteLine(prod.ProductName);
                                        Console.WriteLine(prod.Discontinued);
                                    }
                                }
                            }
                            else
                            {
                                logger.Warn("Incorrect product portal");
                                Console.WriteLine("Incorrect product portal");
                            }
                        }
                        else if (portal == 2)
                        {
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
                        Console.WriteLine("\"q\" to quit");
                        choice = Console.ReadLine();
                        Console.Clear();
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
                                Console.WriteLine(p.ProductName);
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
                                    Console.WriteLine($"\t{p.ProductName}");
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