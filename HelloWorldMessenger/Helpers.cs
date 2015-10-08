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
    class Helpers
    {
        public static JsonValue RequestToAPI(string server, string param)
        {
            Uri address = new Uri(new Uri(server), param);
            HttpWebRequest req = new HttpWebRequest(address);

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            StreamReader reader = new StreamReader(res.GetResponseStream());


            string resText = reader.ReadToEnd();

            JsonValue json = JsonValue.Parse(resText);

            reader.Close();
            res.Close();

            return json;

        }
    }
}