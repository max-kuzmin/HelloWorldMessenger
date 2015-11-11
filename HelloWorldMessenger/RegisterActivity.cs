using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Json;
using Android.Provider;
using Android.Graphics;

namespace HelloWorldMessenger
{
    [Activity(Theme = "@android:style/Theme.Holo.Light.NoActionBar", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class RegisterActivity : Activity
    {
        ImageView uploadImgView = null;
        Bitmap imgNew = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Register);

            Button registerButton = FindViewById<Button>(Resource.Id.RegisterButton);
            registerButton.Click += RegisterButton_Click;


            uploadImgView = FindViewById<ImageView>(Resource.Id.AvatarView);
            uploadImgView.Click += UploadImgButton_Click;
        }


        private void UploadImgButton_Click(object sender, EventArgs e)
        {
            Intent imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(imageIntent, GetString(Resource.String.SelectPicture)), 0);

        }


        //получение авы из файловой системы
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                imgNew = MediaStore.Images.Media.GetBitmap(ContentResolver, data.Data);
                imgNew = Bitmap.CreateScaledBitmap(imgNew, 100, 100, false);

                uploadImgView.SetImageBitmap(imgNew);
            }
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
                            if (imgNew != null) HelpersAPI.PutImageToAPI(imgNew);

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
            else
                Toast.MakeText(this, Resource.String.NoInternet, ToastLength.Long).Show();
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