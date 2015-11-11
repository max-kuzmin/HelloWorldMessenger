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
using Android.Graphics;

namespace HelloWorldMessenger
{
    //асинхронное получение сообщений из апи
    public class AsyncGetMessagesFromAPI : AsyncTask
    {
        Context ctx = null;
        ListView messages = null;
        List<MessageData> items;
        long dialog_id = 0;
        long lastTime = 0;

        public AsyncGetMessagesFromAPI(Context context, ListView messages, long dialog_id)
        {
            ctx = context;
            this.messages = messages;
            items = new List<MessageData>();
            this.dialog_id = dialog_id;

            if (messages.Adapter.Count > 0) lastTime = ((MessageData)messages.Adapter.GetItem(messages.Adapter.Count - 1)).Time;
        }


        protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
        {

            if (HelpersAPI.AuthCheckAPI())
            {

                //запрос на сервер за новыми сообщени€ми
                List<MessageData> itemsFromAPI = new List<MessageData>();

                string param = "message/show?dialog_id=" + dialog_id + "&time=" + lastTime;
                JsonValue jsonItems = HelpersAPI.RequestToAPI(param);

                if (jsonItems.JsonType == JsonType.Array || !jsonItems.ContainsKey("status"))
                {
                    foreach (JsonValue item in jsonItems)
                    {
                        itemsFromAPI.Add(new MessageData(item["message_id"], item["text"], item["login"], item["time"]));
                    }

                }

                //добавл€ем сообщени€ с сервера в Ѕƒ
                HelpersDB.PutMessages(itemsFromAPI, dialog_id);
                items.AddRange(itemsFromAPI);
            }


            return null;


        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);

            if (items.Count > 0 && HelpersAPI.Online)
            {
                (messages.Adapter as MessagesAdapter).AddItems(items);
                messages.SetSelection(messages.Adapter.Count - 1);
            }
        }
    }

    //получение авы асинхронно
    public class AsyncGetAvatar : AsyncTask
    {

        ImageView imgView = null;
        string login = "";
        Bitmap img = null;

        public AsyncGetAvatar(ImageView imgView, string login = null)
        {
            this.imgView = imgView;
            this.login = login;
        }

        protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
        {
            if (HelpersAPI.Online)
            {
                img = HelpersAPI.GetImageFromAPI(login);
            }


            return null;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);

            if (img != null && HelpersAPI.Online)
            {
                imgView.SetImageBitmap(img);
            }
        }
    }

    //асинхронный запрос к апи
    public class AsyncGetDialogsFromAPI : AsyncTask
    {

        List<DialogData> items;

        Context ctx = null;
        ListView dialogs = null;

        public AsyncGetDialogsFromAPI(Context context, ListView dialogs)
        {
            ctx = context;
            this.dialogs = dialogs;
            items = new List<DialogData>();
        }


        protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
        {

            if (HelpersAPI.AuthCheckAPI())
            {

                //получение диалогов из апи
                JsonValue jsonItems2 = HelpersAPI.RequestToAPI("dialog/show");

                if (jsonItems2.JsonType == JsonType.Array || !jsonItems2.ContainsKey("status"))
                {
                    foreach (JsonValue item in jsonItems2)
                    {
                        items.Add(new DialogData(item["dialog_id"], item["name"], item["users"], item["time"], item["new"] == 1));
                    }

                    //очистка списка диалогов и добавление диалогов из апи
                    HelpersDB.DeleteDialogs(HelpersAPI.MyLogin);
                    HelpersDB.PutDialogs(items, HelpersAPI.MyLogin);

                }
            }

            return null;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);

            if (HelpersAPI.Online && items.Count>0)
                dialogs.Adapter = new DialogsAdapter(ctx, items);

        }
    }
}