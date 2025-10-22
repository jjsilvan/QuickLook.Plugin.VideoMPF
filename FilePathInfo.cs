using System;
using System.IO;
using System.Reflection;

namespace QuickLook.Plugin.VideoMPF 
{
    class FilePathInfo
    {
        public static string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        //public static string PathFile = AppDomain.CurrentDomain.BaseDirectory + @"\MediaPlayer";
        public static string PathFile = assemblyFolder + @"\MediaPlayer";
        public static string Play = PathFile + @"\Icons\Play.png";
        public static string Pause = PathFile + @"\Icons\Pause.png";
    }
}
