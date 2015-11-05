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
using Java.Util;

namespace HelloWorldMessenger
{
    [Activity(Theme = "@android:style/Theme.Holo.Light", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MessagesActivity : Activity
    {

        long dialog_id = 0;
        string dialogName = "";
        Timer t;

        ListView messages = null;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);



            SetContentView(Resource.Layout.Messages);
            SetTitle(Resource.String.Messages);

            dialog_id = Intent.GetLongExtra("dialog_id", 0);
            dialogName = Intent.GetStringExtra("dialogName");


            ActionBar.Title = dialogName;
        }















        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.MessagesMenu, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        //обработка клика на пункте меню сверху
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.LogOutMenuButton)
            {
                HelpersAPI.LogOut();
                StartActivity(new Intent(this, typeof(SingInActivity)));
            }
            else if (item.ItemId == Resource.Id.RenameDialogMenuButton)
            {
                RenameDialog dialog = new RenameDialog();
                dialog.OnDismiss += Dialog_DismissEvent;
                dialog.Show(FragmentManager, "RenameDialog");
            }
            else if (item.ItemId == Resource.Id.DeleteDialogMenuButton)
            {
                JsonValue result = HelpersAPI.RequestToAPI("dialog/delete?dialog_id=" + dialog_id);
                if (result.ContainsKey("status") && result["status"] == "true")
                {
                    StartActivity(new Intent(this, typeof(DialogsActivity)));
                }
            }
            else if (item.ItemId == Resource.Id.ChangeMyInfoButton)
            {
                if (HelpersAPI.Online)
                {
                    StartActivity(new Intent(this, typeof(ChangeUserInfoActivity)));
                }
            }

            return base.OnOptionsItemSelected(item);
        }

        private void Dialog_DismissEvent(object sender, EventArgs e)
        {
            RenameDialog dialog = (sender as RenameDialog);
            if (dialog.Text != "")
            {
                string param = "dialog/rename?dialog_id=" + dialog_id + "&name=" + dialog.Text;
                JsonValue result = HelpersAPI.RequestToAPI(param);
                if (result.ContainsKey("status") && result["status"] == "true")
                {
                    ActionBar.Title = dialog.Text;
                }
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            //проверка на онлайн и авторизацию
            if (!HelpersAPI.AuthCheckAPI() && HelpersAPI.MyLogin == "")
            {
                StartActivity(new Intent(this, typeof(SingInActivity)));
                return;
            }

            HelpersAPI.NeedCheckInBackground = false;

            //обработчики
            Button send = FindViewById<Button>(Resource.Id.SendMessageButton);
            send.Click += Send_Click;

            messages = FindViewById<ListView>(Resource.Id.MessagesList);
            messages.ItemClick += Messages_ItemClick;


            //получаем из БД сообщения
            long lastTime = 0;
            List<MessageData> items = new List<MessageData>();
            List<MessageData> messagesFromDB = HelpersDB.GetMessages(dialog_id);
            if (messagesFromDB.Count > 0)
            {
                lastTime = messagesFromDB[messagesFromDB.Count - 1].Time;
                items.AddRange(messagesFromDB);
            }
            messages.Adapter = new MessagesAdapter(this, items);
            messages.SetSelection(messages.Adapter.Count - 1);


            //проверка сообщений каждые 10 сек
            t = new Timer();
            t.ScheduleAtFixedRate(new UpdMessagesTimerTask(this, messages, dialog_id), 0, HelpersAPI.UpdInterval);



        }


        private void Send_Click(object sender, EventArgs e)
        {
            if (HelpersAPI.Online)
            {
                //постим сообщение на сервер
                EditText messageText = FindViewById<EditText>(Resource.Id.MessageField);
                string param = "message/add?dialog_id=" + dialog_id + "&text=" + messageText.Text;
                JsonValue result = HelpersAPI.RequestToAPI(param);

                //получаем новые сообщения с сервера
                if (result.ContainsKey("status") && result["status"] == "true")
                {
                    messageText.Text = "";
                    //асинхронно проверяем новые сообщения - не надо так, начинаю лезть баги
                    //AsyncGetMessagesFromAPI async = new AsyncGetMessagesFromAPI(this, messages, dialog_id);
                    //async.Execute();
                }
            }

        }

        private void Messages_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //клик на сообщении
            if (HelpersAPI.Online)
            {
                string login = ((MessageData)messages.Adapter.GetItem(e.Position)).Login;
                Intent intent = new Intent(this, typeof(UserInfoActivity));
                intent.PutExtra("login", login);
                intent.PutExtra("isAddDialog", false);
                StartActivity(intent);
            }
        }


        protected override void OnPause()
        {
            base.OnPause();
            HelpersAPI.NeedCheckInBackground = true;
            t.Cancel();
        }
    }



    public class MessagesAdapter : BaseAdapter<MessageData>
    {

        Context ctx;
        List<MessageData> list;



        public void AddItems(List<MessageData> items)
        {
            items.Sort(MessageData.Comparator);
            list.AddRange(items);
            this.NotifyDataSetChanged();

        }


        public MessagesAdapter(Context context, IEnumerable<MessageData> items)
        {
            ctx = context;
            list = new List<MessageData>(items);

            list.Sort(MessageData.Comparator);
        }


        public override MessageData this[int position]
        {
            get
            {
                return list.ElementAt(position);
            }
        }

        public override int Count
        {
            get
            {
                return list.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return list.ElementAt(position);
        }

        public override long GetItemId(int position)
        {
            return list.ElementAt(position).Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //извлекаем лэйаут
            View view = convertView;
            if (view == null)
            {
                view = View.Inflate(ctx, Resource.Layout.MessageListItem, null);

            }

            //присваиваем элементам значения
            view.FindViewById<TextView>(Resource.Id.MessageTextListItem).Text = list.ElementAt(position).Text;

            DateTime date = HelpersAPI.FromUnixTime(list.ElementAt(position).Time);

            view.FindViewById<TextView>(Resource.Id.MessageLoginListItem).Text = list.ElementAt(position).Login + " on " + date.ToString("HH:mm:ss dd.MM");

            //настраиваем вид
            if (HelpersAPI.MyLogin == list.ElementAt(position).Login)
            {
                view.FindViewById<TextView>(Resource.Id.MessageTextListItem).Gravity = GravityFlags.Right;
                view.FindViewById<TextView>(Resource.Id.MessageLoginListItem).Gravity = GravityFlags.Right;
                view.FindViewById<LinearLayout>(Resource.Id.MessageBackgroundLayout).SetGravity(GravityFlags.Right);

                LinearLayout l = view.FindViewById<LinearLayout>(Resource.Id.MessageListItem);
                l.SetPadding(100, l.PaddingTop, 3, l.PaddingBottom);
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.MessageTextListItem).Gravity = GravityFlags.Left;
                view.FindViewById<TextView>(Resource.Id.MessageLoginListItem).Gravity = GravityFlags.Left;
                view.FindViewById<LinearLayout>(Resource.Id.MessageBackgroundLayout).SetGravity(GravityFlags.Left);

                LinearLayout l = view.FindViewById<LinearLayout>(Resource.Id.MessageListItem);
                l.SetPadding(3, l.PaddingTop, 100, l.PaddingBottom);
            }


            return view;
        }

    }


    public class MessageData : Java.Lang.Object
    {
        public string Text = "";
        public string Login = "";
        public long Id = 0;
        public long Time = 0;

        public MessageData(long id, string text, string login, long time)
        {
            Text = text;
            Login = login;
            Id = id;
            Time = time;
        }

        public static int Comparator(MessageData x, MessageData y)
        {
            if (x.Time > y.Time) return 1;
            else if (x.Time < y.Time) return -1;
            else return 0;
        }
    }


    public class RenameDialog : DialogFragment
    {
        View v;
        string text = "";
        public string Text
        {
            get
            {
                return text;
            }

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Dialog.SetTitle(Resource.String.EnterName);
            v = inflater.Inflate(Resource.Layout.RenameDialog, container, false);
            Button ok = v.FindViewById<Button>(Resource.Id.OkButton);
            ok.Click += Ok_Click;
            return v;
        }


        private void Ok_Click(object sender, EventArgs e)
        {
            EditText newName = v.FindViewById<EditText>(Resource.Id.EditName);
            text = newName.Text;
            Dialog.Dismiss();
            OnDismiss(this, new EventArgs());
        }


        new public event EventHandler<EventArgs> OnDismiss;
    }




    //событие для таймера обновления сообщений
    public class UpdMessagesTimerTask : TimerTask
    {

        Context ctx = null;
        ListView messages = null;
        long dialog_id = 0;

        public UpdMessagesTimerTask(Context context, ListView messages, long dialog_id)
        {
            ctx = context;
            this.messages = messages;
            this.dialog_id = dialog_id;
        }


        public override void Run()
        {
            AsyncGetMessagesFromAPI async = new AsyncGetMessagesFromAPI(ctx, messages, dialog_id);
            async.Execute();
        }
    }
}