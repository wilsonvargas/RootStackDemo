using System;
using System.Collections.Generic;
using System.Text;

namespace RootStack.Demo.Exceptions
{
    public class HttpServiceException : Exception
    {
        public int StatusCode { get; set; }
        public string Content { get; set; }
    }
}
