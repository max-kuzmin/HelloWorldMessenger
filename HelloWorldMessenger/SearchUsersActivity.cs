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
    [Activity(Label = "SearchUsersActivity", Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
    public class SearchUsersActivity : Activity
    {

        UsersAdapter adapter = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SearchUsers);

            // Create your application here

            Button search = FindViewById<Button>(Resource.Id.SearchButton);
            search.Click += Search_Click;

        }

        private void Search_Click(object sender, EventArgs e)
        {
            if (HelpersAPI.Online)
            {
                ListView usersList = FindViewById<ListView>(Resource.Id.SearchUsersList);
                usersList.ItemClick += UsersList_ItemClick;

                List<User> items = new List<User>();

                string query = FindViewById<EditText>(Resource.Id.SearchField).Text;

                JsonValue jsonItems = HelpersAPI.RequestToAPI("user/search?query=" + query);

                if (jsonItems.JsonType == JsonType.Array || !jsonItems.ContainsKey("status"))
                {
                    foreach (JsonValue item in jsonItems)
                    {
                        items.Add(new User(item["name"], item["login"], item["info"], null));
                    }

                }

                adapter = new UsersAdapter(this, items);
                usersList.Adapter = adapter;
            }
        }

        private void UsersList_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //переход на страницу пользователя


            string login = ((User)adapter.GetItem(e.Position)).Login;
            Intent intent = new Intent(this, typeof(UserInfoActivity));
            intent.PutExtra("login", login);
            StartActivity(intent);
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








    public class UsersAdapter : BaseAdapter<User>
    {

        Context ctx;
        List<User> list;




        public UsersAdapter(Context context, IEnumerable<User> items)
        {
            ctx = context;
            list = new List<User>(items);

        }

        public override User this[int position]
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


    public class User : Java.Lang.Object
    {
        public string Name = "";
        public string Login = "";
        public string Info = "";
        public byte[] Avatar = null;

        public User(string name, string login, string info, byte[] avatar)
        {
            Name = name;
            Login = login;
            Info = info;
            Avatar = avatar;
        }
    }
}