using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiJieBeiComponents.Domains
{
    public class Games
    {
        public int Id { get; set; }
       
        [ Display(Name = "主场")]
        public string ZhuChang { get; set; }
        [Display(Name = "客场")]
        public string KeChang { get; set; }
        [Display(Name = "主场缩写")]
        public string ZhuChangSuoXie { get; set; }
        [Display(Name = "客场缩写")]
        public string KeChangSuoXie { get; set; }
        [Display(Name = "主场得分")]
        public int ZhuChangScore { get; set; }
        [Display(Name = "客场得分")]
        public int KeChangScore { get; set; }
        [Display(Name = "比赛结果")]
        public GameOrderStatus Status { get; set; }
        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }
        [Display(Name = "比赛时间")]
        public DateTime StartTime { get; set; }
        public virtual List<GameOrders> GameOrders { get; set; }
    }

}
