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
    public class UserInfoActivity : Activity
    {

        string userLogin = "";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.UserInfo);

            Button addDialog = FindViewById<Button>(Resource.Id.CreateDialogButton);
            addDialog.Click += AddDialog_Click;

            userLogin = Intent.GetStringExtra("login");
        }

        //���������� ��������
        private void AddDialog_Click(object sender, EventArgs e)
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
        }


        protected override void OnStart()
        {
            base.OnStart();

            if (!HelpersAPI.AuthCheckAPI())
            {
                StartActivity(new Intent(this, typeof(SingInActivity)));
                return;
            }


            //����� ���� �� ���
            if (HelpersAPI.Online)
            {
                string param = "user/show?login=" + userLogin;
                JsonValue userData = HelpersAPI.RequestToAPI(param);

                if (userData.ContainsKey("login"))
                {

                    EditText login = FindViewById<EditText>(Resource.Id.LoginField);
                    EditText name = FindViewById<EditText>(Resource.Id.NameField);
                    EditText info = FindViewById<EditText>(Resource.Id.InfoField);

                    login.Text = userData["login"];
                    name.Text = userData["name"];
                    info.Text = userData["info"];

                }
            }
        }
    }
}