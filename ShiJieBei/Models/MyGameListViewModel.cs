using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShiJieBeiComponents.Domains;

namespace ShiJieBei.Models
{
    public class MyGameListViewModel
    {
        public string Number { get; set; }
        public string CreateTime { get; set; }
        public GameOrderStatus GameOrderStatus { get; set; }
        public string  KeChang { get; set; }
        public string ZhuChang { get; set; }
        public string StartTime { get; set; }
    }
}