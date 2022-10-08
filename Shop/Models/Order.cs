using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public int OrderStatusId { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        [MaxLength(10)]
        public string PostCode { get; set; }

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        [Required]
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual ICollection<ProductOrder> Products { get; set; }
    }
}