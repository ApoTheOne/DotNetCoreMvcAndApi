using System.ComponentModel.DataAnnotations;

namespace Vyayaam.ViewModels
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public int ProductId { get; set; }
        /* AutoMapper will add following property mappings 
         * even without having to add them in CreateMap in Mappings Profile class.
         * All we did was, just added Product prefix to these newly added properties
        */
        public string ProductCategory { get; set; }
        public string ProductSize { get; set; }
        public string ProductTitle { get; set; }
        public string ProductArtId { get; set; }
    }
}