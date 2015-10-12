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
using System.Collections;

namespace HelloWorldMessenger
{
    [Activity(Theme = "@android:style/Theme.Holo.Light", MainLauncher = false)]
    public class DialogsActivity : Activity
    {

        DialogsAdapter adapter = null;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Dialogs);


            this.SetTitle(Resource.String.Dialogs);
            
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.DialogsMenu, menu);

            //IMenuItem item = menu.FindItem(Resource.Id.AddDialogButton);
            //item.SetOnMenuItemClickListener(new CreateDialogClick(this, item.Handle));

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            if (item.ItemId == Resource.Id.AddDialogButton)
            {
                StartActivity(new Intent(this, typeof(SearchUsersActivity)));
            }

            return base.OnOptionsItemSelected(item);
        }





        protected override void OnStart()
        {
            base.OnStart();


            if (HelpersAPI.AuthCheckAPI())
            {


                ListView dialogs = FindViewById<ListView>(Resource.Id.DialogsList);
                dialogs.ItemClick += Dialogs_ItemClick;

                List<DialogData> items = new List<DialogData>();

                JsonValue jsonItems = HelpersAPI.RequestToAPI("dialog/show");

                if (jsonItems.JsonType == JsonType.Array || !jsonItems.ContainsKey("status"))
                {
                    foreach (JsonValue item in jsonItems)
                    {
                        items.Add(new DialogData(item["dialog_id"], item["name"], item["users"], item["time"]));
                    }

                }

                
                adapter = new DialogsAdapter(this, items);
                dialogs.Adapter = adapter;



            }
        }

        private void Dialogs_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            long dialog_id = ((DialogData)adapter.GetItem(e.Position)).Id;
            Intent intent = new Intent(this, typeof(MessagesActivity));
            intent.PutExtra("dialog_id", dialog_id);
            StartActivity(intent);
        }
    }

    public class DialogsAdapter : BaseAdapter<DialogData>
    {

        Context ctx;
        List<DialogData> list;



        public void AddItems(DialogData[] item)
        {
            //add items to show


            throw new NotImplementedException();
        }


        public DialogsAdapter(Context context, IEnumerable<DialogData> items)
        {
            ctx = context;
            list = new List<DialogData>(items);

            //сортировка по времени
            list.Sort(DialogData.Comparator);
        }

        public override DialogData this[int position]
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
                view = View.Inflate(ctx, Resource.Layout.DialogListItem, null);
            }

            view.FindViewById<TextView>(Resource.Id.DialogNameListItem).Text = list.ElementAt(position).Name;
            view.FindViewById<TextView>(Resource.Id.DialogMembersListItem).Text = list.ElementAt(position).Members;

            DateTime date = HelpersAPI.FromUnixTime(list.ElementAt(position).Time);

            view.FindViewById<TextView>(Resource.Id.DialogTimeListItem).Text = date.ToShortDateString() +" " + date.ToLongTimeString();



            return view;
        }

    }


    public class DialogData: Java.Lang.Object
    {
        public string Name = "";
        public string Members = "";
        public long Id = 0;
        public long Time = 0;

        public DialogData(long id, string name, string members, long time)
        {
            Name = name;
            Members = members;
            Id = id;
            Time = time;
        }


        public static int Comparator(DialogData x, DialogData y)  {

                if (x.Time > y.Time) return -1;
                else if (x.Time<y.Time) return 1;
                else return 0;

            }
}
}