using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models.ViewModels
{
    public class CreateOrderViewModel
    {
        [DisplayName("First name")]
        [Required(ErrorMessage = "Enter your first name.")]
        [StringLength(50, ErrorMessage = "First Name must be less than 50 characters.")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        [Required(ErrorMessage = "Enter your last name.")]
        [StringLength(50, ErrorMessage = "Last Name must be less than 50 characters.")]
        public string LastName { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "Enter your city.")]
        [StringLength(100, ErrorMessage = "City Name must be less than 100 characters.")]
        public string City { get; set; }

        [DisplayName("Address")]
        [Required(ErrorMessage = "Enter your Address.")]
        [StringLength(100, ErrorMessage = "Address must be less than 100 characters.")]
        public string Address { get; set; }

        [DisplayName("Post code")]
        [Required(ErrorMessage = "Enter your Post Code.")]
        [StringLength(10, ErrorMessage = "Post Code must have less than 10 characters.")]
        public string PostCode { get; set; }

        [DisplayName("Phone number")]
        [Required(ErrorMessage = "Enter your Phone number.")]
        [StringLength(20, ErrorMessage = "Phone number be less than 20 characters.")]
        public string Phone { get; set; }

     
    }
}