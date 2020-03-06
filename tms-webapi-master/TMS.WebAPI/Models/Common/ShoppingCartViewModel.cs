using System;

namespace TMS.Web.Models
{
    [Serializable]
    public class ShoppingCartViewModel
    {
        public int ProductId { set; get; }
        public int Quantity { set; get; }
    }
}