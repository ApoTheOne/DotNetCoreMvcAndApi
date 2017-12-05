using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vyayaam.Data.Entities;

namespace Vyayaam.Data
{
    public class VyayaamRepository : IVyayaamRepository
    {
        private readonly VyayaamContext context;
        private readonly ILogger<VyayaamRepository> logger;

        public VyayaamRepository(VyayaamContext context, ILogger<VyayaamRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                logger.LogInformation("GetAllProducts was called.");
                return context.Products
                    .OrderBy(p => p.Title)
                    .ToList();
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to get all products: {ex}");
                throw;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return context.Products
                .Where(p => p.Category == category)
                .ToList();
        }

        public bool SaveAll()
        {
            return context.SaveChanges() > 0;
            /* context.SaveChanges() => returns number of saved rows */
        }
    }
}
