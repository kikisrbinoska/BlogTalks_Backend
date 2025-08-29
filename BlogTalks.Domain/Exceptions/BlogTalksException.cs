using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Domain.Exceptions
{
    namespace BlogTalks.Domain.Exceptions
    {
        public class BlogTalksException : Exception
        {
            public HttpStatusCode StatusCode { get; set; }

            
            public BlogTalksException() { }

            public BlogTalksException(string message) : base(message) { }

            public BlogTalksException(string message, Exception innerException) : base(message, innerException) { }

            public BlogTalksException(string message, int statusCode) : base(message)
            {
                StatusCode = (HttpStatusCode)statusCode;
            }

            public BlogTalksException(string message, HttpStatusCode statusCode) : base(message)
            {
                StatusCode = statusCode;
            }

           
        }
    }
}
