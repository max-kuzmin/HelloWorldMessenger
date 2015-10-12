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
    [Activity(Label = "MessagesActivity")]
    public class MessagesActivity : Activity
    {

        long dialog_id = 0;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Messages);

            dialog_id = bundle.GetLong("dialog_id");
        }


        protected override void OnStart()
        {
            base.OnStart();


            if (!HelpersAPI.AuthCheckAPI())
            {
                StartActivity(new Intent(this, typeof(SingInActivity)));
            }


            Button send = FindViewById<Button>(Resource.Id.SendMessageButton);
            send.Click += Send_Click;






            //����� ���������
            ListView messages = FindViewById<ListView>(Resource.Id.MessagesList);
            messages.ItemClick += Messages_ItemClick;

            List<MessageData> items = new List<MessageData>();

            JsonValue jsonItems = HelpersAPI.RequestToAPI("message/show?dialog_id="+dialog_id);

            if (jsonItems.JsonType == JsonType.Array || !jsonItems.ContainsKey("status"))
            {
                foreach (JsonValue item in jsonItems)
                {
                    items.Add(new MessageData(item["message_id"], item["text"], item["login"], item["time"]));
                }

            }

            messages.Adapter = new MessagesAdapter(this, items);


        }

        private void Send_Click(object sender, EventArgs e)
        {

            EditText messageText = FindViewById<EditText>(Resource.Id.MessageField);
            string param = "message/add?dialog_id=" + dialog_id + "&text=" + messageText.Text;
            JsonValue result = HelpersAPI.RequestToAPI(param);
            if (result.ContainsKey("status") && result["status"] == "true")
            {

            }
            //��������� ������ ����
            
        }

        private void Messages_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //���� �� ����������
        }
    }



    public class MessagesAdapter : BaseAdapter<MessageData>
    {

        Context ctx;
        List<MessageData> list;



        public void AddItems(MessageData[] item)
        {
            //add items to show


            throw new NotImplementedException();
        }


        public MessagesAdapter(Context context, IEnumerable<MessageData> items)
        {
            ctx = context;
            list = new List<MessageData>(items);

            //���������� �� �������
            list.Sort((MessageData x, MessageData y) => {

                if (x.Time > y.Time) return 1;
                else if (x.Time < y.Time) return -1;
                else return 0;

            });
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
                view.FindViewById<LinearLayout>(Resource.Id.MessageListItem).SetGravity(GravityFlags.Right);
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