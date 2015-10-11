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
    [Activity(Label = "UserInfoActivity", Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
    public class UserInfoActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.UserInfo);

            Button addDialog = FindViewById<Button>(Resource.Id.CreateDialogButton);
            addDialog.Click += AddDialog_Click;
        }

        private void AddDialog_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            

            if (HelpersAPI.Online)
            {

                string login = FindViewById<EditText>(Resource.Id.LoginField).Text;

                string param = "dialog/add?login=" + login;

                JsonValue result = HelpersAPI.RequestToAPI(param);

                if (result.ContainsKey("status") && result["status"] == "true")
                {

                    StartActivity(new Intent(this, typeof(DialogsActivity)));

                }
            }

            //Toast t = Toast.MakeText(this, Resource.String.RegError, ToastLength.Long);
            //t.SetGravity(GravityFlags.Center, 0, 0);
            //t.Show();
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (!HelpersAPI.AuthCheckAPI())
            {
                StartActivity(new Intent(this, typeof(SingInActivity)));
                return;
            }
            //else
            //{

            //    string param = "user/show?login" + login; //логин из предыдущего активити
            //    JsonValue userData = HelpersAPI.RequestToAPI(param);

            //    if (userData.ContainsKey("login"))
            //    {


            //        EditText login = FindViewById<EditText>(Resource.Id.LoginField);
            //        EditText name = FindViewById<EditText>(Resource.Id.NameField);
            //        EditText info = FindViewById<EditText>(Resource.Id.InfoField);

            //        //назначаем пол€

            //    }
            //}

        }
    }
}