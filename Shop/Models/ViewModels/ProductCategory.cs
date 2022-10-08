using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models.ViewModels
{
    public class ProductCategory
    {
        //This hold the selected value in post action
        public string SelectedCategory { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Product> SearchResults { get; set; } = new List<Product>();
    }
}