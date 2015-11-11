using System;
using System.Json;
using Android.App;
using Android.Content;
using System.Net;
using System.IO;
using Android.Net;
using Android.Graphics;

namespace HelloWorldMessenger
{
    public static class HelpersAPI
    {

        //static string server = "http://169.254.80.80/HelloWorldAPI/";
        //static string CookieDomain = "169.254.80.80";

        static string server = "http://api-maxgsomgsom.rhcloud.com/";
        static string CookieDomain = "api-maxgsomgsom.rhcloud.com";

        static bool testMode = false;

        static string myLogin = "";
        static bool online = true;

        static int updInterval = 10000;
        static int timeout = 5000;


        public static bool NeedCheckInBackground { get; set; }

        static bool authOnlineChecked = false;

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

        public static int UpdInterval
        {
            get
            {
                return updInterval;
            }
        }

        public static JsonValue RequestToAPI(string param)
        {
            JsonValue json = null;

            try
            {
                //проверка на подключение к сети
                ConnectivityManager conMgr = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
                NetworkInfo netInfo = conMgr.ActiveNetworkInfo;
                if (!testMode && (netInfo == null || !netInfo.IsConnected)) throw new Exception();

                //запрос к апи
                System.Uri address = new System.Uri(new System.Uri(Server), param);
                
                HttpWebRequest req = new HttpWebRequest(address);
                req.CookieContainer = GetCookieFromSetting();
                req.Timeout = timeout;
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

                authOnlineChecked = false;


            }

            return json;

        }



        public static Bitmap GetImageFromAPI(string login=null)
        {
            Bitmap img = null;

            try
            {
                //проверка на подключение к сети
                ConnectivityManager conMgr = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
                NetworkInfo netInfo = conMgr.ActiveNetworkInfo;
                if (!testMode && (netInfo == null || !netInfo.IsConnected)) throw new Exception();

                string param = "avatar/show";
                if (login != null) param += "?login=" + login;

                //запрос к апи
                System.Uri address = new System.Uri(new System.Uri(Server), param);

                HttpWebRequest req = new HttpWebRequest(address);
                req.CookieContainer = GetCookieFromSetting();
                req.Timeout = timeout;
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                try
                {
                    img = BitmapFactory.DecodeStream(res.GetResponseStream());
                }
                catch { }

                res.Close();

                online = true;
            }
            catch
            {
                //если ошибка запроса - нет инета
                online = false;
                authOnlineChecked = false;

            }

            return img;

        }






        public static JsonValue PutImageToAPI(Bitmap img, string login = null)
        {
            JsonValue json = null;

            try
            {
                //проверка на подключение к сети
                ConnectivityManager conMgr = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
                NetworkInfo netInfo = conMgr.ActiveNetworkInfo;
                if (!testMode && (netInfo == null || !netInfo.IsConnected)) throw new Exception();

                //путь
                string param = "avatar/upload";
                System.Uri address = new System.Uri(new System.Uri(Server), param);

                //параметры запроса
                HttpWebRequest req = new HttpWebRequest(address);
                req.CookieContainer = GetCookieFromSetting();
                req.Timeout = timeout;
                req.Method = "POST";
                req.ContentType = "image/jpeg";

                //получаем и конвертируем изображение
                MemoryStream memory = new MemoryStream();
                
                img.Compress(Bitmap.CompressFormat.Jpeg, 90, memory);
                byte[] imgBytes = memory.ToArray();
                BinaryWriter writer = new BinaryWriter(req.GetRequestStream());
                writer.Write(imgBytes);
                
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream());

                string resText = reader.ReadToEnd();
                json = JsonValue.Parse(resText);

                res.Close();

                online = true;
            }
            catch
            {
                //если ошибка запроса - нет инета
                online = false;
                authOnlineChecked = false;


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
                    //проверка на подключение к сети
                    ConnectivityManager conMgr = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
                    NetworkInfo netInfo = conMgr.ActiveNetworkInfo;
                    if (!testMode && (netInfo == null || !netInfo.IsConnected)) throw new Exception();

                    //запрос
                    string param = "login?login=" + login + "&pass=" + pass;

                    System.Uri address = new System.Uri(new System.Uri(Server), param);
                    HttpWebRequest req = new HttpWebRequest(address);
                    req.Timeout = timeout;
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
                authOnlineChecked = false;
            }

            return false;
        }



        public static void LogOut()
        {
            ISharedPreferences prefs = Application.Context.GetSharedPreferences("Setting", FileCreationMode.Private);
            ISharedPreferencesEditor prefEditor = prefs.Edit();
            authOnlineChecked = false;
            prefEditor.Clear();
            prefEditor.Commit();
        }

        //проверка на авторизацию в апи
        public static bool AuthCheckAPI()
        {
            //коннектимся к инету только 1 раз для проверка авторизации
            if (!authOnlineChecked)
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
                    authOnlineChecked = true;
                    return true;
                }

                myLogin = prefs.GetString("login", "");
                return false;
            }
            return online;
        }
    }
}