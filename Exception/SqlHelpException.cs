using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcDemo.Exception
{
    public class SqlHelpException : ApplicationException
    {
        public SqlHelpException(string message) : base(message)
        {
        }
    }
}