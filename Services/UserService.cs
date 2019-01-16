using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MvcDemo.Dao;
using MvcDemo.Entities;
using MvcDemo.Exception;
using MvcDemo.Utils;

namespace MvcDemo.Services
{
    public class UserService
    {
        public User login(string name,string pwd)
        {
            string trimName = name.Trim().Replace("\r\n","");
            // 密码进行md5加密
            string encodePwd = MD5Util.encode(pwd);
            UserDao dao = new UserDao();
            DataRowCollection rows = dao.FindUserByNameAndPwd(trimName, encodePwd);
            int len = rows.Count;
            // 找不到即表示输入有误
            if (len < 1)
            {
                throw new LoginException("账号或密码错误");
            }
            // 出现重复的账号及密码,则提示异常
            if (len > 1)
            {
                throw new LoginException("该账号存在异常");
            }

            foreach(DataRow user in rows)
            {
                // 检验用户状态
                if (user["user_status"].ToString().Equals("0"))
                {
                    throw new LoginException("该账号已被冻结,不可登录");
                }

                User usr = new User();
                usr.Id = user["user_id"].ToString();
                usr.Name = user["user_name"].ToString();
                usr.Nick = user["user_nick"].ToString();
                usr.Status = (bool) user["user_status"];
                return usr;
            }
            return null;
        }

        public void logout()
        {
            //待完成
        }
    }
}