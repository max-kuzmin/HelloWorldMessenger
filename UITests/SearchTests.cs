using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using Xamarin.UITest.Android;
using HelloWorldMessenger;
using System.Resources;

namespace UITests
{
    class SearchTests
    {

        AndroidApp app;


        public SearchTests() { }

        public SearchTests(AndroidApp app)
        {
            this.app = app;
        }

        [SetUp]
        public void Before()
        {
            app = ConfigureApp.Android.StartApp();
            Helpers.Login(app);
        }

        [Test]
        public void Search1()
        {
            app.Tap("AddDialogButton");
            app.WaitForElement("SearchField");
            app.EnterText("SearchField", "ann");
            app.Tap("SearchButton");
            app.WaitForElement("Ann K.");
            Assert.IsTrue(app.Query(e => e.Marked("SearchUsersList").Child()).Length == 1);
        }

        [Test]
        public void Search2()
        {
            app.Tap("AddDialogButton");
            app.WaitForElement("SearchField");
            app.EnterText("SearchField", "Maxim");
            app.Tap("SearchButton");
            app.WaitForElement("Maxim K.");
            Assert.IsTrue(app.Query(e => e.Marked("SearchUsersList").Child()).Length == 1);
        }

        [Test]
        public void Search3()
        {
            app.Tap("AddDialogButton");
            app.WaitForElement("SearchField");
            app.EnterText("SearchField", "");
            app.Tap("SearchButton");
            Assert.IsTrue(app.Query(e=> e.Marked("SearchUsersList").Child()).Length == 0);
        }

        [Test]
        public void Search4()
        {
            app.Tap("AddDialogButton");
            app.WaitForElement("SearchField");
            app.EnterText("SearchField", "girl");
            app.Tap("SearchButton");
            Assert.IsTrue(app.Query(e => e.Marked("SearchUsersList").Child()).Length == 2);
        }
    }
}
