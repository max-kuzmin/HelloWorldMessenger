using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;
using System.IO;
using Android.Net;
using Java.Net;

namespace HelloWorldMessenger
{
    public class HelpersAPI
    {

        //doto �������� �������� � � ����������, ������� ���������� �������������
        //userinfo userlistitem ��������

        //static string server = "http://169.254.80.80/HelloWorldAPI/";
        //static string CookieDomain = "169.254.80.80";

        static string server = "http://api-maxgsomgsom.rhcloud.com/";
        static string CookieDomain = "api-maxgsomgsom.rhcloud.com";

        static string myLogin = "";
        static bool online = true;

        static int updInterval = 10000;


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
                //�������� �� ����������� � ����
                ConnectivityManager conMgr = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
                NetworkInfo netInfo = conMgr.ActiveNetworkInfo;
                if (netInfo == null || !netInfo.IsConnected) throw new Exception();

                //������ � ���
                //param = URLEncoder.Encode(param);
                //param = param.Replace("%2F", "/").Replace("%3F", "?").Replace("%3D", "=").Replace("%26", "&").Replace("%2523", "#");
                System.Uri address = new System.Uri(new System.Uri(Server), param);
                
                HttpWebRequest req = new HttpWebRequest(address);
                req.CookieContainer = GetCookieFromSetting();
                req.Timeout = 60000;
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
                //���� ������ ������� - ��� �����
                json = JsonValue.Parse("{\"status\":\"offline\"}");
                online = false;

                authOnlineChecked = false;

                ISharedPreferences prefs = Application.Context.GetSharedPreferences("Setting", FileCreationMode.Private);

                Toast.MakeText(Application.Context, Resource.String.NoInternet, ToastLength.Long).Show();
            }

            return json;

        }



        //�������� ���� �� ���������
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


        //�������������� �������
        public static DateTime FromUnixTime(long seconds)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            date = date.AddSeconds(seconds).ToLocalTime();
            return date;
        }



        //����������� � ���
        public static bool SinginToAPI(string login, string pass)
        {

            try {

                if (login.Length > 0 && pass.Length > 0)
                {
                    //�������� �� ����������� � ����
                    ConnectivityManager conMgr = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
                    NetworkInfo netInfo = conMgr.ActiveNetworkInfo;
                    if (netInfo == null || !netInfo.IsConnected) throw new Exception();

                    //������
                    string param = "login?login=" + login + "&pass=" + pass;

                    System.Uri address = new System.Uri(new System.Uri(Server), param);
                    HttpWebRequest req = new HttpWebRequest(address);
                    req.Timeout = 60000;
                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                    StreamReader reader = new StreamReader(res.GetResponseStream());
                    string resText = reader.ReadToEnd();
                    JsonValue json = JsonValue.Parse(resText);

                    reader.Close();
                    res.Close();

                    online = true;

                    //���������� ���� � ���������
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
                //���� ������ - ��� ���������
                online = false;
                authOnlineChecked = false;
                Toast.MakeText(Application.Context, Resource.String.NoInternet, ToastLength.Long).Show();
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

        //�������� �� ����������� � ���
        public static bool AuthCheckAPI()
        {
            //����������� � ����� ������ 1 ��� ��� �������� �����������
            if (!authOnlineChecked)
            {
                JsonValue json = RequestToAPI("login/check");

                ISharedPreferences prefs = Application.Context.GetSharedPreferences("Setting", FileCreationMode.Private);
                ISharedPreferencesEditor prefEditor = prefs.Edit();

                if (json.ContainsKey("login"))
                {
                    myLogin = json["login"];
                    //��������� �����
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