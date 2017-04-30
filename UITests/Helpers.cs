using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Xamarin.UITest.Android;

namespace UITests
{
    static public class Helpers
    {
        static public string LoginString = "max1";
        static public string PassString = "111";

        static public string GetRandStr()
        {
            var temp = DateTime.Now.Ticks.ToString();
            return temp.Substring(temp.Length - 11, 10);
        }

        static public void Login(AndroidApp app)
        {
            app.WaitForElement("LoginField");
            app.EnterText("LoginField", LoginString);
            app.EnterText("PassField", PassString);
            app.DismissKeyboard();
            app.Tap("SingInButton");
            app.WaitForElement("DialogsList");
        }
    }
}
