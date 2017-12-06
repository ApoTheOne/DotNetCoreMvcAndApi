using System.Collections.Generic;
using Vyayaam.Data.Entities;

namespace Vyayaam.Data
{
    public interface IVyayaamRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);
        
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int id);

        bool SaveAll();
        void AddEntity(object order);
    }
}