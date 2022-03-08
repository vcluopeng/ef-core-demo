using System;
using System.Collections.Generic;
using System.Text;

namespace EfCoreDemo.Core.Dto
{
    public class SimpleUser
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名字
        /// </summary> 
        public string FirstName { get; set; }
        /// <summary>
        /// 姓氏
        /// </summary> 
        public string LastName { get; set; }
        /// <summary>
        /// 全名
        /// </summary> 
        public string FullName { get; set; }
        public SimpleUser()
        {

        }
        public SimpleUser(int id, string firstName, string lastName, string fullName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            FullName = fullName;
        }
    }
}
