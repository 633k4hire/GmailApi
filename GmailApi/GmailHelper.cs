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

namespace GmailApi
{
    class GmailHelper
    {

        public static string[] _Scopes = { GmailService.Scope.GmailSend };
        public static string _ApplicationName = "MyApp";
        public UserCredential _Credential;
        public GmailService _Service;
        public GmailHelper(string base64Credentials, string appname="MyAwp")
        {
            _ApplicationName = appname;
            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(base64Credentials)))
            {
                string credPath = "token.json";
                _Credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    _Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            } //new FileDataStore(credPath, true)).Result;

            _Service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _Credential,
                ApplicationName = _ApplicationName,
            });
        }

        public bool Send(string[] recipeints, string subject, string htmlBody)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string recips = "To: ";
                foreach (var r in recipeints)
                {
                    recips += r + ",";
                }
                recips = recips.TrimEnd(',');

                sb.AppendLine(recips);
                sb.AppendLine("Subject: " + subject);
                sb.AppendLine("Content-Type: text/html; charset=us-ascii");
                sb.AppendLine();//add second terminator
                sb.AppendLine(htmlBody); //html

                //string plainText1 = "To: metzger.matthew@gmail.com\r\n" +
                //               "Subject: test 2\r\n" +
                //               "Content-Type: text/html; charset=us-ascii\r\n\r\n" +
                //               "<h1>Body Test </h1>";
                string plainText = sb.ToString();

                var newMsg = new Google.Apis.Gmail.v1.Data.Message();
                newMsg.Raw = Base64UrlEncode(plainText.ToString());
                _Service.Users.Messages.Send(newMsg, "me").Execute();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        public class MemoryDataStore : IDataStore
        {
            public Task ClearAsync()
            {
                throw new NotImplementedException();
            }

            public Task DeleteAsync<T>(string key)
            {
                throw new NotImplementedException();
            }

            public Task<T> GetAsync<T>(string key)
            {
                throw new NotImplementedException();
            }

            public Task StoreAsync<T>(string key, T value)
            {
                throw new NotImplementedException();
            }
        }
    }
}
