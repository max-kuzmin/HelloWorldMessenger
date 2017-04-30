using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using Xamarin.UITest.Android;
using HelloWorldMessenger;

namespace UITests
{

    [TestFixture]
    public class LoginTests
    {
        AndroidApp app;

        [SetUp]
        public void Before()
        {
            app = ConfigureApp.Android.StartApp();
            app.WaitForElement("LoginField");
        }

        [Test]
        public void LoginError1()
        {
            app.Tap("SingInButton");
            app.WaitForElement("Wrong login or password");
        }

        [Test]
        public void LoginError2()
        {
            app.EnterText("LoginField", Helpers.GetRandStr());
            app.DismissKeyboard();
            app.Tap("SingInButton");
            app.WaitForElement("Wrong login or password");
        }

        [Test]
        public void LoginError3()
        {
            app.EnterText("LoginField", Helpers.GetRandStr());
            app.EnterText("PassField", Helpers.GetRandStr());
            app.DismissKeyboard();
            app.Tap("SingInButton");
            app.WaitForElement("Wrong login or password");
        }

        [Test]
        public void LoginError4()
        {
            app.EnterText("PassField", Helpers.GetRandStr());
            app.DismissKeyboard();
            app.Tap("SingInButton");
            app.WaitForElement("Wrong login or password");
        }

        [Test]
        public void LoginGood()
        {
            Helpers.Login(app);
        }

        [Test]
        public void Logout()
        {
            Helpers.Login(app);
            app.Tap(e => e.Marked("DialogsList").Child(0));

            app.Tap("ShowMessagesMenuButton");
            app.Tap("Log out");
            app.WaitForElement("LoginField");

        }


    }
}

