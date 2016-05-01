using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace MainIrcFeed
{
    public static class Images
    {
        public static string A { get; private set; }
        public static string B { get; private set; }
        public static string Z { get; private set; }
        public static string L { get; private set; }
        public static string R { get; private set; }

        public static string Cu { get; private set; }
        public static string Cd { get; private set; }
        public static string Cr { get; private set; }
        public static string Cl { get; private set; }

        public static string Du { get; private set; }
        public static string Dd { get; private set; }
        public static string Dr { get; private set; }
        public static string Dl { get; private set; }
        public static string Start { get; private set;}

        static Images()
        {
            A = _getFullPath("a");
            B = _getFullPath("b");
            Z = _getFullPath("z");
            L = _getFullPath("l");
            R = _getFullPath("r");
            Cu = _getFullPath("cup");
            Cd = _getFullPath("cdown");
            Cr = _getFullPath("cright");
            Cl = _getFullPath("cleft");
            Du = _getFullPath("dup");
            Dd = _getFullPath("ddown");
            Dr = _getFullPath("dright");
            Dl = _getFullPath("Dleft");
            Start = _getFullPath("start");

        }

        private static string _getFullPath(string file)
        {
            var outPutDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

            return System.IO.Path.Combine(outPutDirectory, "images\\" + file + ".png");
        }
    }
}
