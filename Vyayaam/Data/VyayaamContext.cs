using Microsoft.EntityFrameworkCore;
using Vyayaam.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vyayaam.Data
{
    public class VyayaamContext : DbContext
    {
        public VyayaamContext(DbContextOptions<VyayaamContext> options): base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        //public DbSet<OrderItem> OrderItems { get; set; }
    }
}
