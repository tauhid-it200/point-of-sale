using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PosMvc.Models
{
    public class Cart
    {
        public List<BoughtItem> BoughtItems { get; set; }
    }
}