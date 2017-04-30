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
    class DialogsTests
    {

        AndroidApp app;
        SearchTests searchTests;

        [SetUp]
        public void Before()
        {
            app = ConfigureApp.Android.StartApp();
            searchTests = new SearchTests(app);
            Helpers.Login(app);
        }

        
        public void CreateDialog()
        {
            searchTests.Search1();

            app.Tap(e => e.Marked("SearchUsersList").Child(0));
            app.WaitForElement("CreateDialogButton");
            app.Tap("CreateDialogButton");
            app.WaitForElement("max1-ann");
        }

        [Test]
        public void CreateDeleteDialog()
        {
            CreateDialog();

            int count = app.Query(e => e.Marked("DialogsList").Child()).Length;

            app.Tap(e => e.Marked("DialogsList").Child(0));
            app.WaitForElement("ShowMessagesMenuButton");
            app.Tap("ShowMessagesMenuButton");
            app.Tap("Delete dialog");
            app.WaitForElement("DialogsList");

            app.WaitFor(() => (count - 1) == app.Query(e => e.Marked("DialogsList").Child()).Length);
        }

        [Test]
        public void CreateRenameDialog()
        {
            CreateDialog();
            app.Tap(e => e.Marked("DialogsList").Child(0));
            app.WaitForElement("ShowMessagesMenuButton");
            app.Tap("ShowMessagesMenuButton");
            app.Tap("Rename dialog");

            string str = Helpers.GetRandStr();
            app.EnterText("EditName", "Dialog " + str);
            app.Tap("OkButton");
            app.WaitForElement("Dialog " + str);

            app.Tap("ShowMessagesMenuButton");
            app.Tap("Delete dialog");
        }


        [Test]
        public void CreateRenameWrongDialog()
        {
            CreateDialog();
            app.Tap(e => e.Marked("DialogsList").Child(0));
            app.WaitForElement("ShowMessagesMenuButton");
            app.Tap("ShowMessagesMenuButton");
            app.Tap("Rename dialog");

            app.Tap("OkButton");
            app.WaitForElement("max1-ann");

            app.Tap("ShowMessagesMenuButton");
            app.Tap("Delete dialog");
        }


        [Test]
        public void SendMessage()
        {
            app.Tap(e => e.Marked("DialogsList").Child(0));
            string message = "Message " + Helpers.GetRandStr();
            app.EnterText("MessageField", message);
            app.Tap("SendMessageButton");
            app.DismissKeyboard();
            app.WaitForElement(message);
        }

        [Test]
        public void SendEmptyMessage()
        {
            app.Tap(e => e.Marked("DialogsList").Child(0));
            app.WaitForElement("MessagesList");
            int count = app.Query(e => e.Marked("MessagesList").Child()).Length;

            app.Tap("SendMessageButton");
            try
            {
                app.WaitForElement(Helpers.GetRandStr());
                Assert.Fail();
            }
            catch
            {
                Assert.IsTrue(count == app.Query(e => e.Marked("MessagesList").Child()).Length);
            }
        }

    }
}
