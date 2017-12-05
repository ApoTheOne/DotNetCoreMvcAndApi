using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vyayaam.Data.Entities;

namespace Vyayaam.Data
{
    public class VyayaamSeeder
    {
        private readonly VyayaamContext context;
        private readonly IHostingEnvironment hosting;

        public VyayaamSeeder(VyayaamContext context, IHostingEnvironment hosting)
        {
            this.context = context;
            this.hosting = hosting;
        }

        public void Seed()
        {
            context.Database.EnsureCreated();

            if(!context.Products.Any())
            {
                //Need to Seed
                string filePath = Path.Combine(hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                context.Products.AddRange(products);

                var order = new Order()
                {
                    OrderDate = DateTime.Now,
                    OrderNumber = "IN00001",
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    }
                };

                context.Orders.Add(order);

                context.SaveChanges();
            }
        }
    }
}
