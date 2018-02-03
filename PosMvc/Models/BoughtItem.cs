using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PosMvc.Models
{
    public class BoughtItem
    {
        public int Id { get; set; }
        public Guid CustomerGuid { get; set; }
        public int Quantity { get; set; }
        public Item Item { get; set; }
        public int ItemId { get; set; }
    }
}