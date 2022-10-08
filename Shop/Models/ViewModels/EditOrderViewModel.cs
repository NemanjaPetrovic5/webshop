using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop.Models.ViewModels
{
    public class EditOrderViewModel
    {
        public Order Order { get; set; }

        public IEnumerable<OrderStatus> OrderStatuss { get; set; }

    }
}