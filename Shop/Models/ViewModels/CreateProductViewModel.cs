using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models.ViewModels
{
    public class CreateProductViewModel
    {
        [Required(ErrorMessage = "Please enter Name of product.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Invalid Name Lenght")]
        public string Name { get; set; }

        [DisplayName("Category")]
        public int? CategoryId { get; set; }

        [Required(ErrorMessage = "Please enter price of product.")]
        public double Price { get; set; }

        public string Description { get; set; }
        
        public HttpPostedFileBase Thumbnail { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public Product Product { get; set; }
    }
}