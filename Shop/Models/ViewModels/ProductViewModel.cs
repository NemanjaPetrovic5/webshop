using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models.ViewModels
{
	public class ProductViewModel
	{
        public Product Product { get; set; }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        public int? CategoryId { get; set; }

        [DisplayName("New thumbnail")]
		public HttpPostedFileBase Thumbnail { get; set; }
        public IEnumerable<Category> Categories { get; set; }

	}
}