using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Json;

namespace HelloWorldMessenger
{
    [Activity(Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
    public class RegisterActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Register);

            Button registerButton = FindViewById<Button>(Resource.Id.RegisterButton);
            registerButton.Click += RegisterButton_Click;
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            if (HelpersAPI.Online)
            {
                //регистрируемся в апи
                string login = FindViewById<EditText>(Resource.Id.LoginField).Text;
                string pass = FindViewById<EditText>(Resource.Id.PassField).Text;
                string name = FindViewById<EditText>(Resource.Id.NameField).Text;
                string info = FindViewById<EditText>(Resource.Id.InfoField).Text;

                if (login.Length > 0 && pass.Length > 0 && name.Length > 0 && info.Length > 0)
                {
                    string param = "register?login=" + login + "&pass=" + pass + "&name=" + name + "&info=" + info;

                    JsonValue result = HelpersAPI.RequestToAPI(param);

                    //логинимся
                    if (result.ContainsKey("status") && result["status"] == "true")
                    {
                        if (HelpersAPI.SinginToAPI(login, pass))
                        {
                            StartActivity(new Intent(this, typeof(DialogsActivity)));
                        }
                        else
                        {
                            StartActivity(new Intent(this, typeof(SingInActivity)));
                        }
                        return;

                    }
                }

                //при ошибке регистрации вывод сообщения
                Toast t = Toast.MakeText(this, Resource.String.RegError, ToastLength.Long);
                t.SetGravity(GravityFlags.Center, 0, 0);
                t.Show(); 
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (HelpersAPI.AuthCheckAPI())
            {
                StartActivity(new Intent(this, typeof(DialogsActivity)));
                return;
            }

        }
    }
}