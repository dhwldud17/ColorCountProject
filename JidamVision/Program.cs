using JidamVision.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JidamVision
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //#LOGFORM#2 log4net 설정 파일을 읽어들임
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));

            SLogger.Write("Logger initialized!", SLogger.LogType.Info);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
