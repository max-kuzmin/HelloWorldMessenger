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

using System.Security.Cryptography;

namespace HelloWorldMessenger
{
    [Activity(Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
    public class SearchUsersActivity : Activity
    {

        UsersAdapter adapter = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SearchUsers);

            Button search = FindViewById<Button>(Resource.Id.SearchButton);
            search.Click += Search_Click;

        }

        //выводим список найденных пользователей
        private void Search_Click(object sender, EventArgs e)
        {
            if (HelpersAPI.Online)
            {
                ListView usersList = FindViewById<ListView>(Resource.Id.SearchUsersList);
                usersList.ItemClick += UsersList_ItemClick;

                List<UserData> items = new List<UserData>();

                string query = FindViewById<EditText>(Resource.Id.SearchField).Text;

                if (query.Length >0)
                {
                    JsonValue jsonItems = HelpersAPI.RequestToAPI("user/search?query=" + query);

                    if (jsonItems.JsonType == JsonType.Array || !jsonItems.ContainsKey("status"))
                    {
                        foreach (JsonValue item in jsonItems)
                        {
                            items.Add(new UserData(item["name"], item["login"], item["info"]));
                        }

                    }

                    adapter = new UsersAdapter(this, items);
                    usersList.Adapter = adapter; 
                }
            }
            else 
                Toast.MakeText(this, Resource.String.NoInternet, ToastLength.Long).Show();
        }

        private void UsersList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //переход на страницу пользователя
            if (HelpersAPI.Online)
            {
                string login = ((UserData)adapter.GetItem(e.Position)).Login;
                Intent intent = new Intent(this, typeof(UserInfoActivity));
                intent.PutExtra("login", login);
                intent.PutExtra("isAddDialog", true);
                StartActivity(intent);
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
        }
    }








    public class UsersAdapter : BaseAdapter<UserData>
    {

        Context ctx;
        List<UserData> list;




        public UsersAdapter(Context context, IEnumerable<UserData> items)
        {
            ctx = context;
            list = new List<UserData>(items);

        }

        public override UserData this[int position]
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
            return list.ElementAt(position).Handle.ToInt64();
            
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = View.Inflate(ctx, Resource.Layout.UserListItem, null);
            }

            view.FindViewById<TextView>(Resource.Id.UserNameListItem).Text = list.ElementAt(position).Name;
            view.FindViewById<TextView>(Resource.Id.UserLoginListItem).Text = list.ElementAt(position).Login;

            return view;
        }

    }


    public class UserData : Java.Lang.Object
    {
        public string Name = "";
        public string Login = "";
        public string Info = "";

        public UserData(string name, string login, string info)
        {
            Name = name;
            Login = login;
            Info = info;
        }
    }
}