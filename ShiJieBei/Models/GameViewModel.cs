using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShiJieBeiComponents.Domains;

namespace ShiJieBei.Models
{
    public class GameViewModel
    {
        public User User { get; set; }
        public List<Games> Games { get; set; }
    }
}