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
    [Activity(MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SingIn);


            Button singInButton = FindViewById<Button>(Resource.Id.SingInButton);
            singInButton.Click += SingInButton_Click;



        }

        private void SingInButton_Click(object sender, EventArgs e)
        {
            string login = FindViewById<EditText>(Resource.Id.LoginField).Text;
            string pass = FindViewById<EditText>(Resource.Id.PassField).Text;

            if (login.Length>=4 && pass.Length>=4)
            {
                string param = "login?login=" + login + "&pass=" + pass;


                JsonValue res = Helpers.RequestToAPI(GetString(Resource.String.ServerAddress), param);

                if (res["status"] = "true")
                {
                    StartActivity(new Intent("DialogsActivity"));
                    return;
                }

            }

            Toast.MakeText(this, Resource.String.LoginPassError, ToastLength.Long);
        }
    }
}

