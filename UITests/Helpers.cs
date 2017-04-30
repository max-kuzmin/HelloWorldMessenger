using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITests
{
    static public class Helpers
    {
        static public string Login = "max1";
        static public string Pass = "111";

        static public string GetRandStr()
        {
            var temp = DateTime.Now.Ticks.ToString();
            return temp.Substring(temp.Length - 11, 10);
        }

    }
}
