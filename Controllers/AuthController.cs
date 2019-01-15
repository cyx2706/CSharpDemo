using MvcDemo.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using MvcDemo.Utils;
using MvcDemo.Services;
using MvcDemo.Exception;

namespace MvcDemo.Controllers
{
    /**
     * @author 陈颖旭
     * 
     * @description 
     * 权限验证控制器,主要用于登录及注册操作
     * 登录成功后会生成Session
     */
    public class AuthController : Controller
    {
        /**
         * @author 陈颖旭
         * @method post
         * @param username 用户名(非空,长度不超过10)
         * @param password 密码(非空,长度在6-50之间)
         * 
         * @description 登录验证控制器 输入账号密码,然后到数据库查询数据
         */
        [HttpPost]
        [ValidateInput(true)]
        public JsonResult Login(LoginRequest loginRequest)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            // 验证模型
            if (ModelState.IsValid)
            {
                UserService service = new UserService();
                try
                {
                    service.login(loginRequest.Username, loginRequest.Password);
                    map.Add("status", true);
                }
                catch (LoginException e)
                {
                    map.Add("info", e.Message);
                    map.Add("status", false);
                }
            }
            else
            {
                // 返回错误码,方便前端操作
                map.Add("status", false);
                //获取所有错误的Key
                List<string> Keys = ModelState.Keys.ToList();
                //获取每一个key对应的ModelStateDictionary
                foreach (var key in Keys)
                {
                    List<string> sb = new List<string>();
                    var errors = ModelState[key].Errors.ToList();
                    //获取当前字段的所有错误描述
                    foreach (var error in errors)
                    {
                        sb.Add(error.ErrorMessage);
                    }
                    map.Add(key, sb);
                }
            }
            // 返回数据
            JsonResult res = new JsonResult();
            res.Data = map;
            return res;
        }
    }
}