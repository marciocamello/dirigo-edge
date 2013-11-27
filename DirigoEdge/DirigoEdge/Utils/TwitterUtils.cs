using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Security;


namespace DirigoEdge.Utils
{
    public static class TwitterUtils
    {

        public static List<Tweet> GetTweets(int count)
        {
            var username = ""; // WebConfigurationManager.AppSettings["twitterUsername"];
            var authResponse = Authenticate();

            //Empty list if the authentication is invalid
            if (authResponse == null) return new List<Tweet>();

            var timelineFormat = "https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={0}&include_rts=1&exclude_replies=1&count={1}";
            var timelineUrl = String.Format(timelineFormat, username, count);
            var timeLineRequest = (HttpWebRequest) WebRequest.Create(timelineUrl);
            var timelineHeaderFormat = "{0} {1}";
            timeLineRequest.Headers.Add("Authorization",
                String.Format(timelineHeaderFormat, authResponse.token_type, authResponse.access_token));
            timeLineRequest.Method = "Get";
            var timeLineResponse = timeLineRequest.GetResponse();
            var timeLineJson = String.Empty;
            using (authResponse.authResponse)
            {
                using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                {
                    timeLineJson = reader.ReadToEnd();
                }
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            var tweet = js.Deserialize<List<Tweet>>(timeLineJson);

            return tweet;
        }

        private static TwitAuthenticateResponse Authenticate()
        {
            var oAuthConsumerKey = "";    //WebConfigurationManager.AppSettings["oAuthConsumerKey"];
            var oAuthConsumerSecret = ""; //WebConfigurationManager.AppSettings["oAuthConsumerSecret"];
            var oAuthUrl = "https://api.twitter.com/oauth2/token"; //WebConfigurationManager.AppSettings["oAuthUrl"];


            var authHeaderFormat = "Basic {0}";

            var authHeader = String.Format(authHeaderFormat,
                Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(oAuthConsumerKey) + ":" +
                                                              Uri.EscapeDataString((oAuthConsumerSecret)))
                    ));

            var postBody = "grant_type=client_credentials";

            HttpWebRequest authRequest = (HttpWebRequest) WebRequest.Create(oAuthUrl);
            authRequest.Headers.Add("Authorization", authHeader);
            authRequest.Method = "POST";
            authRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            authRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (Stream stream = authRequest.GetRequestStream())
            {
                byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
                stream.Write(content, 0, content.Length);
            }

            authRequest.Headers.Add("Accept-Encoding", "gzip");

            try
            {
                WebResponse authResponse = authRequest.GetResponse();
                TwitAuthenticateResponse twitAuthResponse;

                using (authResponse)
                {
                    using (var reader = new StreamReader(authResponse.GetResponseStream()))
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        var objectText = reader.ReadToEnd();
                        twitAuthResponse = js.Deserialize<TwitAuthenticateResponse>(objectText);
                    }
                }

                twitAuthResponse.authResponse = authResponse;
                return twitAuthResponse;
            }
            catch (WebException e)
            {
                return null;
            }
        }

        public class TwitAuthenticateResponse
        {
            public String token_type { get; set; }
            public String access_token { get; set; }
            public WebResponse authResponse { get; set; }
        }

        public class Tweet
        {
            public String created_at { get; set; }
            public String id_str { get; set; }
            public String text { get; set; }
        }
    }
}