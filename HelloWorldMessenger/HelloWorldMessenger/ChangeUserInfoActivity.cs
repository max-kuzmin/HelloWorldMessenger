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
    public class ChangeUserInfoActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ChangeUserInfo);

            Button saveButton = FindViewById<Button>(Resource.Id.SaveButton);
            saveButton.Click += SaveButton_Click;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (HelpersAPI.Online)
            {
                //изменяем данные в апи
                string passNew = FindViewById<EditText>(Resource.Id.NewPassField).Text;
                string passOld = FindViewById<EditText>(Resource.Id.OldPassField).Text;
                string name = FindViewById<EditText>(Resource.Id.NameField).Text;
                string info = FindViewById<EditText>(Resource.Id.InfoField).Text;

                if (passOld.Length > 0 && passNew.Length > 0 && name.Length > 0 && info.Length > 0)
                {
                    string param = "user/change?oldpass=" + passOld + "&newpass=" + passNew + "&name=" + name + "&info=" + info;

                    JsonValue result = HelpersAPI.RequestToAPI(param);

                    if (result.ContainsKey("status") && result["status"] == "true")
                    {
                        StartActivity(new Intent(this, typeof(DialogsActivity)));
                    }
                }
                else
                {
                    //при ошибке вывод сообщения
                    Toast t = Toast.MakeText(this, Resource.String.SaveError, ToastLength.Long);
                    t.SetGravity(GravityFlags.Center, 0, 0);
                    t.Show();
                }
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (!HelpersAPI.AuthCheckAPI())
            {
                StartActivity(new Intent(this, typeof(SingInActivity)));
                return;
            }


            //вывод инфы из апи
            if (HelpersAPI.Online)
            {
                string param = "user/show?login=" + HelpersAPI.MyLogin;
                JsonValue userData = HelpersAPI.RequestToAPI(param);

                if (userData.ContainsKey("login"))
                {

                    EditText name = FindViewById<EditText>(Resource.Id.NameField);
                    EditText info = FindViewById<EditText>(Resource.Id.InfoField);

                    name.Text = userData["name"];
                    info.Text = userData["info"];

                }
            }

        }
    }
}