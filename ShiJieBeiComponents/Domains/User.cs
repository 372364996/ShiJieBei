using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShiJieBeiComponents.Domains
{
    public class User
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string UserGuid { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Sex { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        /// <summary>
        /// 头像图片网址
        /// </summary>
        public string HeadImg { get; set; }
        public string HeadImgShow { get { return String.IsNullOrEmpty(HeadImg) ? "headimgs/0" : HeadImg; } }
        /// <summary>
        /// 头像图片的Hash值。Hash值改变以后，需要重新下载头像
        /// </summary>
        public string HeadImgHash { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastImgTime { get; set; }
        public string Mobile { get; set; }

        /// <summary>
        /// 是否短信验证通过
        /// </summary>
        public bool IsMsgValid { get; set; }

        /// <summary>
        /// 是否赠送过点券
        /// </summary>
        public bool IsGiveVouchers { get; set; }

        /// <summary>
        /// 备注的昵称
        /// </summary>
        public string DescName { get; set; }
        /// <summary>
        /// 备注的头像
        /// </summary>
        public string DescHeadImg { get; set; }
        [StringLength(20)]
        public string TrueName { get; set; }
        public string WeiChat { get; set; }
        /// <summary>
        /// 我创建的合伙人Id
        /// </summary>
        public int? PartnerId { get; set; }
        /// <summary>
        /// 我的账户
        /// </summary>
        public virtual Account Account { get; set; }


    }



}