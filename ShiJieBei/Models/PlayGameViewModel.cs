using ShiJieBeiComponents.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShiJieBei.Models
{
    public class PlayGameViewModel
    {
        public User User { get; set; }
        public Games Game { get; set; }
        public List<GameOrders> GameOrders { get; set; }
    }
}