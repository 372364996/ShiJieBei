using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiJieBeiComponents.Domains
{
    public class GameOrders
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public string Number { get; set; }
        public GameOrderStatus GameOrderStatus { get; set; }
        public  int GameCount { get; set; }
        public bool? IsWin { get; set; }
        public DateTime CreateTime { get; set; }
        public virtual Games Game { get; set; }
        public virtual User User { get; set; }
    }

    public enum GameOrderStatus
    {
        Win, Ping, Lose
    }
}
