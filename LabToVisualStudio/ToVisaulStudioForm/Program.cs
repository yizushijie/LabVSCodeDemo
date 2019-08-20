using System;
using System.Linq;
using System.Windows.Forms;

namespace Harry.LabTools.LabToVisualStudio
{
	static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
		static void Main(string[] arg)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			if (arg.Count()>0)
			{
                string str = null;
                for (int i = 0; i < arg.Length; i++)
                {
                    str += arg[i].ToString();
                }
                //MessageBox.Show(str);
                Application.Run(new ToVisualStudioForm(str));
			}
			else
			{
				Application.Run(new ToVisualStudioForm());
			}
			
		}
	}
}
