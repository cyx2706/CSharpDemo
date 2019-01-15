using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcDemo.Exception
{
    [Serializable]
    public class LoginException : ApplicationException
    {
        public LoginException(string message) : base(message)
        {
        }
    }
}