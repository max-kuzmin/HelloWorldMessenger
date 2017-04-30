using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using Xamarin.UITest.Android;

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
        }

        [Test]
        public void LoginError1()
        {
            app.Tap("SingInButton");
            app.WaitForElement("Wrong login");
        }

        [Test]
        public void LoginError2()
        {
            app.EnterText("LoginField", Helpers.GetRandStr());
            app.Tap("SingInButton");
            app.WaitForElement("Wrong login");
        }

        [Test]
        public void LoginError3()
        {
            app.EnterText("LoginField", Helpers.GetRandStr());
            app.EnterText("PassField", Helpers.GetRandStr());
            app.Tap("SingInButton");
            app.WaitForElement("Wrong login");
        }

        [Test]
        public void LoginError4()
        {
            app.EnterText("PassField", Helpers.GetRandStr());
            app.Tap("SingInButton");
            app.WaitForElement("Wrong login");
        }

        [Test]
        public void LoginGood()
        {
            app.EnterText("LoginField", Helpers.Login);
            app.EnterText("PassField", Helpers.Pass);
            app.Tap("SingInButton");
            app.WaitForElement("DialogsList");
        }


    }
}

