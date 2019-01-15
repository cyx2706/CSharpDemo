using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MvcDemo.Dao;
using MvcDemo.Exception;
using MvcDemo.Utils;

namespace MvcDemo.Services
{
    public class UserService
    {
        public void login(string name,string pwd)
        {
            // 密码进行md5加密
            pwd = MD5Util.encode(pwd);
            UserDao dao = new UserDao();
            DataRowCollection rows = dao.FindUserByNameAndPwd(name, pwd);
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

                //TODO 将信息存入session中
            }
            // rows
            // Session 
        }

        public void logout()
        {
            //待完成
        }
    }
}