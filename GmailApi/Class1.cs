using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GmailQuickstart
{
    class Program
    { 

        static void Main(string[] args)
        {
            var bytes = File.ReadAllBytes("credentials2.json");
            var baseString = Convert.ToBase64String(bytes);

            GmailApi.GmailHelper gh = new GmailApi.GmailHelper(baseString);

            gh.Send(new string[] { "metzger.matthew@gmail.com" }, "Api Test", "My Body");
           
            Console.Read();
        }
        
    }
}