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
using Android.Net;

namespace HelloWorldMessenger
{
    public class HelpersAPI
    {

        //doto картинки аватарки, проверка отсутвия инета
        //разлогивание, удаление диалога, изменение имени диалога, изменение своих данных, красивое время

        static string server = "http://169.254.80.80/HelloWorldAPI/";
        static string CookieDomain = "169.254.80.80";

        //static string server = "http://gsomgsom.icycoolhosting.com/HelloWorldAPI/";
        //static string CookieDomain = "gsomgsom.icycoolhosting.com";

        static string myLogin = "";
        static bool online = true;

        public static bool Online
        {
            get
            {
                return online;
            }

        }

        public static string Server
        {
            get
            {
                return server;
            }

        }

        public static string MyLogin
        {
            get
            {
                return myLogin;
            }

        }

        public static JsonValue RequestToAPI(string param)
        {
            JsonValue json = null;

            try
            {
                //проверка на подключение к сети /////////////////////////////////
                ConnectivityManager conMgr = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
                NetworkInfo netInfo = conMgr.ActiveNetworkInfo;
                //if (netInfo == null || !netInfo.IsConnected) throw new Exception();

                //запрос к апи
                System.Uri address = new System.Uri(new System.Uri(Server), param);
                HttpWebRequest req = new HttpWebRequest(address);
                req.CookieContainer = GetCookieFromSetting();
                req.Timeout = 1000;
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream());

                string resText = reader.ReadToEnd();

                json = JsonValue.Parse(resText);

                reader.Close();
                res.Close();

                online = true;
            }
            catch 
            {
                //если ошибка запроса - нет инета
                json = JsonValue.Parse("{\"status\":\"offline\"}");
                online = false;

                ISharedPreferences prefs = Application.Context.GetSharedPreferences("Setting", FileCreationMode.Private);

                Toast.MakeText(Application.Context, Resource.String.NoInternet, ToastLength.Long).Show();
            }

            return json;

        }



        //получаем куки из хранилища
        public static CookieContainer GetCookieFromSetting()
        {
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("Setting", FileCreationMode.Private);
            string cookie = prefs.GetString("PHPSESSID", "");
            CookieContainer container = new CookieContainer();

            if (cookie != "")
            {
                Cookie result = new Cookie("PHPSESSID", cookie) { Domain = CookieDomain };
                container.Add(result);
            }


            return container;
        }


        //преобразование времени
        public static DateTime FromUnixTime(long seconds)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            date = date.AddSeconds(seconds).ToLocalTime();

            return date;
        }



        //авторизация в апи
        public static bool SinginToAPI(string login, string pass)
        {

            try {

                if (login.Length > 0 && pass.Length > 0)
                {

                    //запрос
                    string param = "login?login=" + login + "&pass=" + pass;

                    System.Uri address = new System.Uri(new System.Uri(Server), param);
                    HttpWebRequest req = new HttpWebRequest(address);
                    req.Timeout = 1000;
                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                    StreamReader reader = new StreamReader(res.GetResponseStream());
                    string resText = reader.ReadToEnd();
                    JsonValue json = JsonValue.Parse(resText);

                    reader.Close();
                    res.Close();

                    online = true;

                    //сохранение куки в храналище
                    if (json.ContainsKey("status") && json["status"] == "true")
                    {

                        ISharedPreferences prefs = Application.Context.GetSharedPreferences("Setting", FileCreationMode.Private);
                        ISharedPreferencesEditor prefEditor = prefs.Edit();

                        string cookie = res.Headers.Get("Set-Cookie").Split('=', ';')[1];

                        prefEditor.PutString("PHPSESSID", cookie);
                        prefEditor.Commit();

                        return true;
                    }

                }

                
            }
            catch
            {
                //если ошибка - нет интернета
                online = false;
                Toast.MakeText(Application.Context, Resource.String.NoInternet, ToastLength.Long).Show();
            }

            return false;
        }



        public static void LogOut()
        {
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("Setting", FileCreationMode.Private);
            ISharedPreferencesEditor prefEditor = prefs.Edit();
            prefEditor.Clear();
            prefEditor.Commit();
        }

        //проверка на авторизацию в апи
        public static bool AuthCheckAPI()
        {
            JsonValue json = RequestToAPI("login/check");

            ISharedPreferences prefs = Application.Context.GetSharedPreferences("Setting", FileCreationMode.Private);
            ISharedPreferencesEditor prefEditor = prefs.Edit();

            if (json.ContainsKey("login"))
            {
                myLogin = json["login"];
                //сохраняем логин
                prefEditor.PutString("login", myLogin);
                prefEditor.Commit();
                return true;
            }

            myLogin = prefs.GetString("login", "");
            return false;
        }
    }
}