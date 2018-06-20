using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShiJieBeiComponents.Domains
{
    public class User
    {
        [Display(Name ="用户ID")]
        public int Id { get; set; }
        [StringLength(100)]
        public string UserGuid { get; set; }
        [Display(Name = "用户昵称")]
        public string Name { get; set; }
        public string Password { get; set; }
        [Display(Name = "用户邮箱")]
        public string Email { get; set; }
        [Display(Name = "用户钱包")]
        public string Wallet { get; set; }
        public string RetrievePassWordCode { get; set; }
        public int Sex { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        /// <summary>
        /// 头像图片网址
        /// </summary>
        [Display(Name = "用户头像")]
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
        /// 邮箱激活码
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 是否邮箱验证通过
        /// </summary>
        [Display(Name = "邮箱是否验证")]
        public bool IsEmailValid { get; set; }

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

        public virtual List<GameOrders> GameOrders { get; set; }
    }



}