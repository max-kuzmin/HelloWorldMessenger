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
    [Activity(MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
    public class SingInActivity : Activity
    {


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SingIn);

            //обработчики
            Button singInButton = FindViewById<Button>(Resource.Id.SingInButton);
            singInButton.Click += SingInButton_Click;

            Button registerButton = FindViewById<Button>(Resource.Id.RegisterButton);
            registerButton.Click += RegisterButton_Click;


        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            if (HelpersAPI.Online)
                StartActivity(new Intent(this, typeof(RegisterActivity)));
        }


        protected override void OnStart()
        {
            base.OnStart();

            if (HelpersAPI.AuthCheckAPI())
            {
                StartActivity(new Intent(this, typeof(DialogsActivity)));
                return;
            }

            FindViewById<EditText>(Resource.Id.LoginField).Text = HelpersAPI.MyLogin;

        }

        private void SingInButton_Click(object sender, EventArgs e)
        {
            string login = FindViewById<EditText>(Resource.Id.LoginField).Text;
            string pass = FindViewById<EditText>(Resource.Id.PassField).Text;

            //логин, если онлайн и верный пароль
            if (HelpersAPI.Online && HelpersAPI.SinginToAPI(login, pass))
            {
                StartActivity(new Intent(this, typeof(DialogsActivity)));
                return;
            }
            //логин, если оффлайн и логин совпадает с сохраненным
            else if (!HelpersAPI.Online && login == HelpersAPI.MyLogin)
            {
                StartActivity(new Intent(this, typeof(DialogsActivity)));
                return;
            }


            //если неверный логин и пароль
            Toast t = Toast.MakeText(this, Resource.String.LoginPassError, ToastLength.Long);
            t.SetGravity(GravityFlags.Center,0,0);
            t.Show();
        }
    }
}

