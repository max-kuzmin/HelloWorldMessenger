using System;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace HelloWorldMessenger
{
    [Activity(Theme = "@android:style/Theme.Holo.Light", MainLauncher = false)]
    public class DialogsActivity : Activity
    {

        ListView dialogs = null;
        Timer t = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Dialogs);

            this.SetTitle(Resource.String.Dialogs);
            
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.DialogsMenu, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        //обработка клика на пункте меню сверху
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (HelpersAPI.Online)
            {
                if (item.ItemId == Resource.Id.AddDialogButton)
                {
                    StartActivity(new Intent(this, typeof(SearchUsersActivity)));
                }
            }
            else
                Toast.MakeText(this, Resource.String.NoInternet, ToastLength.Long).Show();

            return base.OnOptionsItemSelected(item);
        }



        protected override void OnStart()
        {
            base.OnStart();

            dialogs = FindViewById<ListView>(Resource.Id.DialogsList);
            dialogs.ItemClick += Dialogs_ItemClick;

            if (HelpersAPI.MyLogin == "")
            {
                //если не залогинился - возврат к авторизации
                StartActivity(new Intent(this, typeof(SingInActivity)));
                return;
            }

            HelpersAPI.NeedCheckInBackground = false;

            //запоняем диалоги из базы
            dialogs.Adapter = new DialogsAdapter(this, HelpersDB.GetDialogs(HelpersAPI.MyLogin));


            //проверка диалогов каждые 10 сек
            t = new Timer();
            t.ScheduleAtFixedRate(new UpdDialogsTimerTask(this, dialogs), 0, HelpersAPI.UpdInterval);

        }



        //обработка клика на диалоге
        private void Dialogs_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            long dialog_id = ((DialogData)dialogs.Adapter.GetItem(e.Position)).Id;
            string dialogName = ((DialogData)dialogs.Adapter.GetItem(e.Position)).Name;
            Intent intent = new Intent(this, typeof(MessagesActivity));
            intent.AddFlags(ActivityFlags.NoHistory);
            intent.PutExtra("dialog_id", dialog_id);
            intent.PutExtra("dialogName", dialogName);
            StartActivity(intent);
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (t != null) t.Cancel();
            HelpersAPI.NeedCheckInBackground = true;
        }
    }


    public class DialogsAdapter : BaseAdapter<DialogData>
    {

        Context ctx;
        List<DialogData> list;



        public DialogsAdapter(Context context, IEnumerable<DialogData> items)
        {
            ctx = context;
            list = new List<DialogData>(items);
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

                //задаем данные в лэйауте
                view.FindViewById<TextView>(Resource.Id.DialogNameListItem).Text = list.ElementAt(position).Name;
                view.FindViewById<TextView>(Resource.Id.DialogMembersListItem).Text = list.ElementAt(position).Members;

                DateTime date = HelpersAPI.FromUnixTime(list.ElementAt(position).Time);

                view.FindViewById<TextView>(Resource.Id.DialogTimeListItem).Text = date.ToString("HH:mm:ss dd.MM");

                if (list.ElementAt(position).IsNew)
                {
                    view.FindViewById<LinearLayout>(Resource.Id.DialogItemBackground).SetBackgroundColor(Color.LightGray);
                    view.FindViewById<TextView>(Resource.Id.DialogTimeListItem).SetTextColor(Color.Black);
                }
            

            return view;
        }

    }


    public class DialogData: Java.Lang.Object
    {
        public string Name = "";
        public string Members = "";
        public long Id = 0;
        public long Time = 0;
        public bool IsNew = false;

        public DialogData(long id, string name, string members, long time, bool isNew)
        {
            Name = name;
            Members = members;
            Id = id;
            Time = time;
            IsNew = isNew;
            
        }


        public static int Comparator(DialogData x, DialogData y)  {

                if (x.Time > y.Time) return -1;
                else if (x.Time<y.Time) return 1;
                else return 0;

            }
}


    

    //событие таймера для обновления списка диалогов
    public class UpdDialogsTimerTask : TimerTask
    {

        Context ctx = null;
        ListView dialogs = null;

        public UpdDialogsTimerTask(Context context, ListView dialogs)
        {
            ctx = context;
            this.dialogs = dialogs;
        }


        public override void Run()
        {

            //запускаем обращение к апи в отдельном потоке
            AsyncGetDialogsFromAPI async = new AsyncGetDialogsFromAPI(ctx, dialogs);
            async.Execute();
        }
    }
}