using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiJieBeiComponents.Domains
{
    public class Manager
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
        public ManagerStatus Status { get; set; }
        public int? UserId { get; set; }
    }

    public enum ManagerStatus
    {
        Normal,
        Disabled,
        Deleted
    }
}
