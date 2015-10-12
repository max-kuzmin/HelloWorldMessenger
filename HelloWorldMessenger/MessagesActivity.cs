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
    [Activity(Theme = "@android:style/Theme.Holo.Light")]
    public class MessagesActivity : Activity
    {

        long dialog_id = 0;

        MessagesAdapter adapter = null;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Messages);
            this.SetTitle(Resource.String.Messages);

            dialog_id = Intent.GetLongExtra("dialog_id", 0);
        }


        protected override void OnStart()
        {
            base.OnStart();


            if (!HelpersAPI.AuthCheckAPI())
            {
                StartActivity(new Intent(this, typeof(SingInActivity)));
                return;
            }


            Button send = FindViewById<Button>(Resource.Id.SendMessageButton);
            send.Click += Send_Click;






            //вывод сообщений
            ListView messages = FindViewById<ListView>(Resource.Id.MessagesList);
            messages.ItemClick += Messages_ItemClick;

            List<MessageData> items = new List<MessageData>();
            string param = "message/show?dialog_id=" + dialog_id+"&time=0";
            JsonValue jsonItems = HelpersAPI.RequestToAPI(param);

            if (jsonItems.JsonType == JsonType.Array || !jsonItems.ContainsKey("status"))
            {
                foreach (JsonValue item in jsonItems)
                {
                    items.Add(new MessageData(item["message_id"], item["text"], item["login"], item["time"]));
                }

            }
            adapter = new MessagesAdapter(this, items);
            messages.Adapter = adapter;
            messages.SetSelection(messages.Adapter.Count - 1);


        }

        private void Send_Click(object sender, EventArgs e)
        {

            EditText messageText = FindViewById<EditText>(Resource.Id.MessageField);
            string param = "message/add?dialog_id=" + dialog_id + "&text=" + messageText.Text;
            messageText.Text = "";
            JsonValue result = HelpersAPI.RequestToAPI(param);
            if (result.ContainsKey("status") && result["status"] == "true")
            {
                
                List<MessageData> items = new List<MessageData>();
                long lastTime = ((MessageData)adapter.GetItem(adapter.Count - 1)).Time;
                string param2 = "message/show?dialog_id=" + dialog_id + "&time="+ lastTime;
                JsonValue jsonItems = HelpersAPI.RequestToAPI(param2);
                if (jsonItems.JsonType == JsonType.Array || !jsonItems.ContainsKey("status"))
                {
                    foreach (JsonValue item in jsonItems)
                    {
                        items.Add(new MessageData(item["message_id"], item["text"], item["login"], item["time"]));
                    }
                    adapter.AddItems(items);

                    ListView messages = FindViewById<ListView>(Resource.Id.MessagesList);
                    messages.SetSelection(messages.Adapter.Count - 1);
                }
            }
            
        }

        private void Messages_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //клик на сообщении
        }
    }



    public class MessagesAdapter : BaseAdapter<MessageData>
    {

        Context ctx;
        List<MessageData> list;



        public void AddItems(List<MessageData> items)
        {
            items.Sort(Comparator);
            list.AddRange(items);
            this.NotifyDataSetChanged();

        }


        public MessagesAdapter(Context context, IEnumerable<MessageData> items)
        {
            ctx = context;
            list = new List<MessageData>(items);

            //сортировка по времени
            list.Sort(Comparator);
        }

        private static int Comparator(MessageData x, MessageData y)
        {
            if (x.Time > y.Time) return 1;
            else if (x.Time < y.Time) return -1;
            else return 0;
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
            View view = convertView;
            if (view == null)
            {
                view = View.Inflate(ctx, Resource.Layout.MessageListItem, null);
            }

            view.FindViewById<TextView>(Resource.Id.MessageTextListItem).Text = list.ElementAt(position).Text;

            DateTime date = HelpersAPI.FromUnixTime(list.ElementAt(position).Time);

            view.FindViewById<TextView>(Resource.Id.MessageTimeListItem).Text = date.ToShortDateString() + " " + date.ToLongTimeString();

            if (HelpersAPI.MyLogin == list.ElementAt(position).Login)
            {
                view.FindViewById<TextView>(Resource.Id.MessageTextListItem).Gravity = GravityFlags.Right;
                view.FindViewById<TextView>(Resource.Id.MessageTimeListItem).Gravity = GravityFlags.Right;
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
    }
}