using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;
using System.IO;

namespace HelloWorldMessenger
{
    public class Helpers
    {

        public static string Server = "http://169.254.80.80/HelloWorldAPI/";
        static string CookieDomain = "169.254.80.80";


        static bool online = true;

        public static bool Online
        {
            get
            {
                return online;
            }

        }

        public static JsonValue RequestToAPI(string param)
        {
            Uri address = new Uri(new Uri(Server), param);
            HttpWebRequest req = new HttpWebRequest(address);

            req.CookieContainer = GetCookieFromSetting();

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            StreamReader reader = new StreamReader(res.GetResponseStream());


            string resText = reader.ReadToEnd();

            JsonValue json = JsonValue.Parse(resText);

            reader.Close();
            res.Close();

            return json;

        }




        public static CookieContainer GetCookieFromSetting()
        {
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("Setting", FileCreationMode.Private);

            string cookie = prefs.GetString("PHPSESSID", "");

            CookieContainer container = new CookieContainer();

            if (cookie!="")
            {
                Cookie result = new Cookie("PHPSESSID", cookie) { Domain = CookieDomain };
                container.Add(result);
            }


            return container;
        }


        public static DateTime FromUnixTime(long seconds)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            date = date.AddSeconds(seconds).ToLocalTime();

            return date;
        }
    }
}