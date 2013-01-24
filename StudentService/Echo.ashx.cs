using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace StudentService.Views.Home
{
    /// <summary>
    /// Summary description for Echo
    /// </summary>
    public class Echo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/xml";
            
            using (var reader = new StreamReader(context.Request.InputStream))
            {
                context.Response.Write(reader.ReadToEnd());
            }
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}