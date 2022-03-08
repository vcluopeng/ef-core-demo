using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EfCoreDemo.Core.Entities
{
    public class User
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        [StringLength(EfCoreDemoConsts.StringLength20), Required]
        public string FirstName { get; set; }
        /// <summary>
        /// 姓氏
        /// </summary>
        [StringLength(EfCoreDemoConsts.StringLength20), Required]
        public string LastName { get; set; }
        /// <summary>
        /// 全名
        /// </summary>
        [StringLength(EfCoreDemoConsts.StringLength50), Required]
        public string FullName { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [StringLength(EfCoreDemoConsts.StringLength50), Required]
        public string UserName { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [StringLength(EfCoreDemoConsts.StringLength50), Required]
        public string Email { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [StringLength(EfCoreDemoConsts.StringLength20)]
        public string Phone { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(EfCoreDemoConsts.StringLength50)]
        public string NickName { get; set; }
        public User()
        {

        }
    }
}
