using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Json;
using System.IO;

namespace HelloWorldMessenger
{
    [Activity(MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.NoActionBar")]
    public class SingInActivity : Activity
    {


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SingIn);


            Button singInButton = FindViewById<Button>(Resource.Id.SingInButton);
            singInButton.Click += SingInButton_Click;



        }



        protected override void OnStart()
        {
            base.OnStart();

            JsonValue json = Helpers.RequestToAPI("login/check");

            if (json.ContainsKey("login"))
            {
                StartActivity(new Intent(this, typeof(DialogsActivity)));
                return;
            }

            
        }

        private void SingInButton_Click(object sender, EventArgs e)
        {
            string login = FindViewById<EditText>(Resource.Id.LoginField).Text;
            string pass = FindViewById<EditText>(Resource.Id.PassField).Text;

            if (login.Length>=1 && pass.Length>=1)
            {
                string param = "login?login=" + login + "&pass=" + pass;

                Uri address = new Uri(new Uri(Helpers.Server), param);
                HttpWebRequest req = new HttpWebRequest(address);
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(res.GetResponseStream());
                string resText = reader.ReadToEnd();
                JsonValue json = JsonValue.Parse(resText);

                reader.Close();
                res.Close();



                if (json.ContainsKey("status") && json["status"] == "true")
                {

                    ISharedPreferences prefs = Application.Context.GetSharedPreferences("Setting", FileCreationMode.Private);
                    ISharedPreferencesEditor prefEditor = prefs.Edit();

                    string cookie = res.Headers.Get("Set-Cookie").Split('=', ';')[1];

                    prefEditor.PutString("PHPSESSID", cookie);
                    prefEditor.Commit();





                    StartActivity(new Intent(this, typeof(DialogsActivity)));
                    return;
                }

            }

            Toast t = Toast.MakeText(this, Resource.String.LoginPassError, ToastLength.Long);
            t.SetGravity(GravityFlags.Center,0,0);
            t.Show();
        }
    }
}

