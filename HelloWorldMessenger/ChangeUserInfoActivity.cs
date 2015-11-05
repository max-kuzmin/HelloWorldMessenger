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
using Java.Lang;
using Android.Graphics;
using System.IO;
using Android.Provider;

namespace HelloWorldMessenger
{
    [Activity(Theme = "@android:style/Theme.Holo.Light.NoActionBar", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class ChangeUserInfoActivity : Activity
    {
        ImageView uploadImgView = null;
        Bitmap imgNew = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ChangeUserInfo);

            Button saveButton = FindViewById<Button>(Resource.Id.SaveButton);
            saveButton.Click += SaveButton_Click;

            uploadImgView = FindViewById<ImageView>(Resource.Id.AvatarView);
            uploadImgView.Click += UploadImgButton_Click;


            AsyncGetAvatar task = new AsyncGetAvatar(FindViewById<ImageView>(Resource.Id.AvatarView), HelpersAPI.MyLogin);
            task.Execute();
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
                        //сохраняем новую аву
                        if (imgNew != null) HelpersAPI.PutImageToAPI(imgNew);

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