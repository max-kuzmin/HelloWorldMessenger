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
using System.Timers;

namespace HelloWorldMessenger
{

    [Service]
    public class NewMessagesService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        Timer t;
        

        public override void OnCreate()
        {
            base.OnCreate();

            t = new Timer(HelpersAPI.UpdInterval);
            t.Elapsed += T_Elapsed;
            t.Start();
            
        }



        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {

            if (HelpersAPI.AuthCheckAPI() && HelpersAPI.NeedCheckInBackground)
            {
                JsonValue result = HelpersAPI.RequestToAPI("dialog/check");

                if (result.JsonType == JsonType.Array || result.ContainsKey("status") == false)
                {
                    NotificationManager nMgr = (NotificationManager)GetSystemService(NotificationService);

                    foreach (JsonValue item in result)
                    {
                        string text = item["name"];
                        Notification notification = new Notification(Resource.Drawable.Icon, text);

                        Intent intent = new Intent(this, typeof(MessagesActivity));
                        intent.PutExtra("dialog_id", (long)item["dialog_id"]);
                        intent.PutExtra("dialogName", (string)item["name"]);
                        intent.AddFlags(ActivityFlags.NoHistory);
                        PendingIntent pending = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.CancelCurrent);

                        //notification.Vibrate = new long[] { 100 };
                        notification.SetLatestEventInfo(this, GetString(Resource.String.HasNewMessages), text, pending);
                        notification.Flags = NotificationFlags.AutoCancel;
                        nMgr.Notify(item["dialog_id"], notification);
                        
                    }


                    //NotificationManager nMgr = (NotificationManager)GetSystemService(NotificationService);
                    //Notification notification = new Notification(Resource.Drawable.Icon, "Цена: " + intent.GetStringExtra("price"));
                    //notification.ContentIntent = PendingIntent.GetActivity(context, 0, new Intent(context, typeof(MainActivity)), 0);
                    //notification.SetLatestEventInfo(context, "", "Цена: " + intent.GetStringExtra("price"), PendingIntent.GetActivity(context, 0, new Intent(context, typeof(MainActivity)), 0));
                    //nMgr.Notify(0, notification);

                    
                }
            }

        }
    }
}