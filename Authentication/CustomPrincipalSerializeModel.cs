using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestHandler.Authentication
{
    public class CustomPrincipalSerializeModel
    {
        public int UserId { get; set; }
        public string[] roles { get; set; }
    }
}