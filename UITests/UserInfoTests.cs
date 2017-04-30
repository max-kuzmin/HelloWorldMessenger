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
    class UserInfoTests
    {
        AndroidApp app;

        [SetUp]
        public void Before()
        {
            app = ConfigureApp.Android.StartApp();
            Helpers.Login(app);
            app.Tap(e => e.Marked("DialogsList").Child(0));
            app.WaitForElement("MessagesList");

        }

        [Test]
        public void GoToUserInfo()
        {
            app.Tap(e => e.Marked("MessagesList").Child(1));
            app.WaitForElement("AvatarView");
        }

        [Test]
        public void ChangeMyInfoError1()
        {
            app.Tap("ShowMessagesMenuButton");
            app.Tap("Change my info");
            app.WaitForElement("AvatarView");
            app.Tap("SaveButton");
            app.WaitForElement("Save error");
        }

        [Test]
        public void ChangeMyInfoError2()
        {
            app.Tap("ShowMessagesMenuButton");
            app.Tap("Change my info");
            app.WaitForElement("AvatarView");
            app.EnterText("OldPassField", Helpers.PassString);
            app.DismissKeyboard();
            app.Tap("SaveButton");
            app.WaitForElement("Save error");
        }

        [Test]
        public void ChangeMyInfoError3()
        {
            app.Tap("ShowMessagesMenuButton");
            app.Tap("Change my info");
            app.WaitForElement("AvatarView");
            app.EnterText("OldPassField", Helpers.PassString);
            app.EnterText("NewPassField", Helpers.PassString);
            app.ClearText("NameField");
            app.DismissKeyboard();
            app.Tap("SaveButton");
            app.WaitForElement("Save error");
        }

        [Test]
        public void ChangeMyInfoError4()
        {
            app.Tap("ShowMessagesMenuButton");
            app.Tap("Change my info");
            app.WaitForElement("AvatarView");
            app.EnterText("OldPassField", Helpers.PassString);
            app.EnterText("NewPassField", Helpers.PassString);
            app.ClearText("InfoField");
            app.DismissKeyboard();
            app.Tap("SaveButton");
            app.WaitForElement("Save error");
        }

        [Test]
        public void ChangeMyInfoError5()
        {
            app.Tap("ShowMessagesMenuButton");
            app.Tap("Change my info");
            app.WaitForElement("AvatarView");
            app.EnterText("NewPassField", Helpers.PassString);
            app.DismissKeyboard();
            app.Tap("SaveButton");
            app.WaitForElement("Save error");
        }

        [Test]
        public void ChangeMyInfoError6()
        {
            app.Tap("ShowMessagesMenuButton");
            app.Tap("Change my info");
            app.WaitForElement("AvatarView");
            app.EnterText("OldPassField", "12");
            app.EnterText("NewPassField", Helpers.PassString);
            app.DismissKeyboard();
            app.Tap("SaveButton");
            app.WaitForElement("Save error");
        }

        [Test]
        public void ChangeMyInfoError7()
        {
            app.Tap("ShowMessagesMenuButton");
            app.Tap("Change my info");
            app.WaitForElement("AvatarView");
            app.EnterText("OldPassField", Helpers.PassString);
            app.EnterText("NewPassField", "12");
            app.DismissKeyboard();
            app.Tap("SaveButton");
            app.WaitForElement("Save error");
        }


        [Test]
        public void ChangeMyInfoGood()
        {
            app.Tap("ShowMessagesMenuButton");
            app.Tap("Change my info");
            app.WaitForElement("AvatarView");
            app.EnterText("OldPassField", Helpers.PassString);
            app.EnterText("NewPassField", Helpers.PassString);
            app.DismissKeyboard();
            app.Tap("SaveButton");
            app.WaitForElement("DialogsList");
        }

    }
}
