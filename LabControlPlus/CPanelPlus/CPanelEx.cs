using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Harry.LabTools.LabWinAPI;
using Harry.LabTools.LabGenFunc;

namespace Harry.LabTools.LabControlPlus
{

	/// <summary>
	/// 使用本控件是嵌套外部exe应用，不支持计算器
	/// </summary>
	public partial class CPanelEx : Panel
	{

		#region 属性定义

		/// <summary>
		/// 
		/// </summary>
		private readonly ManualResetEvent _eventDone = new ManualResetEvent(false);

		/// <summary>
		/// 
		/// </summary>
		private Process defaulProcess = null;

		/// <summary>
		/// 
		/// </summary>
		private IntPtr defaultEmbededWindowHandle=(IntPtr)0;

		#endregion

		#region 构造函数
		public CPanelEx()
		{
			InitializeComponent();
		}


		#endregion

		#region 重载函数

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnHandleDestroyed(EventArgs e)
		{
			this.Stop();
			base.OnHandleDestroyed(e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			if (defaulProcess != null)
			{
				CWin32API.MoveWindow(this.defaulProcess.MainWindowHandle, 0, 0, this.Width, this.Height, true);
			}

			base.OnResize(e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSizeChanged(EventArgs e)
		{
			this.Invalidate();
			base.OnSizeChanged(e);
		}

		#endregion

		#region 私有函数定义

		/// <summary>
		/// 启动外进程
		/// </summary>
		/// <param name="appPath"></param>
		/// <returns></returns>
		private Process Start(string appPath)
		{
			if (!File.Exists(appPath))
			{
				return null;
			}
			ProcessStartInfo info = new ProcessStartInfo(appPath)
			{
				//UseShellExecute = true,
				//CreateNoWindow = true,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				RedirectStandardInput= true,
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Minimized
			};
			Process process = Process.Start(info);

			//if ((process==null)||(process.HasExited==true))
			//{
			//	///process = new Process();
			//	//process.StartInfo.FileName = appPath;
			//	process.Kill();
			//}

			return process;
		}

		/// <summary>
		/// 关闭外进程
		/// </summary>
		/// <param name="process"></param>
		private void Kill(Process process)
		{
			if ((process != null) && (!process.HasExited))
			{
				process.Kill();
			}
		}


		/// <summary>
		/// 将外进程嵌入到当前程序
		/// </summary>
		/// <param name="process"></param>
		private bool EmbeddedProcess(Process process)
		{
			//---是否嵌入成功标志，用作返回值
			bool isEmbedSuccess = false;
			//---外进程句柄
			IntPtr processHwnd = process.MainWindowHandle;
			//---容器句柄
			IntPtr panelHwnd = this.Handle;

			if (processHwnd != (IntPtr)0 && panelHwnd != (IntPtr)0)
			{
				//---把本窗口句柄与目标窗口句柄关联起来
				int setTime = 0;
				while (!isEmbedSuccess && setTime < 10)
				{
					isEmbedSuccess = (CWin32API.SetParent(processHwnd, panelHwnd) != 0);
					CGenFuncDelay.GenFuncDelayms(150);
					//Thread.Sleep(100);
					setTime++;
				}
				//---设置初始尺寸和位置
				CWin32API.MoveWindow(this.defaulProcess.MainWindowHandle, 0, 0, this.Width, this.Height, true);
				// Remove border and whatnot               
				//---移除边框和右上角的最大，最小和关闭功能
				CWin32API.SetWindowLong(new HandleRef(this, this.defaulProcess.MainWindowHandle), CWin32API.GWL_STYLE, CWin32API.WS_VISIBLE);
			}

			if (isEmbedSuccess)
			{
				this.defaultEmbededWindowHandle = this.defaulProcess.MainWindowHandle;
			}

			return isEmbedSuccess;
		}


		

		#endregion

		#region 公有函数定义

		/// <summary>
		/// 
		/// </summary>
		/// <param name="processPath"></param>
		/// <returns></returns>
		public bool EmbeddedProcess(string processPath)
		{
			bool isStartAndEmbedSuccess = false;
			this._eventDone.Reset();

			//---启动进程
			this.defaulProcess = this.Start(processPath);
			
			if (this.defaulProcess == null)
			{
				return false;
			}

			//---等待新进程完成它的初始化并等待用户输入
			this.defaulProcess.WaitForInputIdle();

			//---确保可获取到句柄
			Thread thread = new Thread(new ThreadStart(() =>
			{
				while (true)
				{
					if (this.defaulProcess.MainWindowHandle != (IntPtr)0)
					{
						this._eventDone.Set();
						break;
					}
					//---
					CGenFuncDelay.GenFuncDelayms(20);
					//Thread.Sleep(10);
				}
			}));
			thread.Start();

			//---嵌入进程
			if (this._eventDone.WaitOne(10000))
			{
				isStartAndEmbedSuccess = this.EmbeddedProcess(defaulProcess);
				if (!isStartAndEmbedSuccess)
				{
					this.Kill(this.defaulProcess);
				}
			}
			return isStartAndEmbedSuccess;
		}

		
		/// <summary>
		/// 关闭进程
		/// </summary>
		public void Stop()
		{
			this.Kill(this.defaulProcess);
		}


		/// <summary>
		/// 嵌入已经存在的进程
		/// </summary>
		/// <param name="process"></param>
		/// <returns></returns>
		public bool EmbeddedExistedProcess(Process process)
		{
			this.defaulProcess = process;

			return this.EmbeddedProcess(process);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="strProcessesByName"></param>
		public  void Kill(string strProcessesByName)//关闭线程
		{
			foreach (Process p in Process.GetProcesses())
			{
				if (p.ProcessName.ToUpper().Contains(strProcessesByName))
				{
					try
					{
						p.Kill();
						p.WaitForExit(); // possibly with a timeout
					}
					catch (Win32Exception e)
					{
						MessageBox.Show(e.Message.ToString());   // process was terminating or can't be terminated - deal with it
					}
					catch (InvalidOperationException e)
					{
						MessageBox.Show(e.Message.ToString()); // process has already exited - might be able to let this one go
					}
				}
			}
		}

		#endregion

	}
}
