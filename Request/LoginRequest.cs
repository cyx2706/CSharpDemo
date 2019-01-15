using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcDemo.Request
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "请输入用户名")]
        [StringLength(10, ErrorMessage = "用户名长度不能超过10位")]
        public string Username { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [StringLength(50,MinimumLength = 6,ErrorMessage ="密码长度不能低于6位,不超过50位")]
        public string Password { get; set; }
    }
}