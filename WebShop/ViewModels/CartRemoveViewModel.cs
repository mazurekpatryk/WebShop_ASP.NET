using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.ViewModels
{
    public class CartRemoveViewModel
    {
        public decimal CartTotal
        {
            get; set;
        }
        public int CartItemsCount
        {
            get; set;
        }
        public int RemovedItemCount
        {
            get; set;
        }
        public int RemoveItemId
        {
            get; set;
        }

    }
}