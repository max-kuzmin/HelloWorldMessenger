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

namespace HelloWorldMessenger
{
    [Activity(Label = "SearchUsersActivity")]
    public class SearchUsersActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SearchUsers);

            // Create your application here
        }
    }
}