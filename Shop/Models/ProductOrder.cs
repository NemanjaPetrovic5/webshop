using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class ProductOrder
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [DisplayName("Quantity")]
        [Required]
        public int Count { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public double Subtotal { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}