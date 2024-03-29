﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Harry.LabTools.LabWinAPI
{
	public partial class CWin32API
	{
		public const int SWP_NOOWNERZORDER = 0x200;
		public const int SWP_NOREDRAW = 0x8;
		public const int SWP_NOZORDER = 0x4;
		public const int SWP_SHOWWINDOW = 0x0040;
		public const int WS_EX_MDICHILD = 0x40;
		public const int SWP_FRAMECHANGED = 0x20;
		public const int SWP_NOACTIVATE = 0x10;
		public const int SWP_ASYNCWINDOWPOS = 0x4000;
		public const int SWP_NOMOVE = 0x2;
		public const int SWP_NOSIZE = 0x1;
		public const int GWL_STYLE = (-16);
		public const int WS_VISIBLE = 0x10000000;
		public const int WM_CLOSE = 0x10;
		public const int WS_CHILD = 0x40000000;

		//{隐藏, 并且任务栏也没有最小化图标}
		public const int SW_HIDE = 0;

		//{用最近的大小和位置显示, 激活}
		public const int SW_SHOWNORMAL = 1;

		//{同 SW_SHOWNORMAL}
		public const int SW_NORMAL = 1;

		//{最小化, 激活}
		public const int SW_SHOWMINIMIZED = 2;

		//{最大化, 激活}
		public const int SW_SHOWMAXIMIZED = 3;

		//{同 SW_SHOWMAXIMIZED}
		public const int SW_MAXIMIZE = 3;

		//{用最近的大小和位置显示, 不激活}
		public const int SW_SHOWNOACTIVATE = 4;

		//{同 SW_SHOWNORMAL}
		public const int SW_SHOW = 5;

		//{最小化, 不激活}
		public const int SW_MINIMIZE = 6;

		//{同 SW_MINIMIZE}
		public const int SW_SHOWMINNOACTIVE = 7;

		//{同 SW_SHOWNOACTIVATE}
		public const int SW_SHOWNA = 8;

		//{同 SW_SHOWNORMAL}
		public const int SW_RESTORE = 9;

		//{同 SW_SHOWNORMAL}
		public const int SW_SHOWDEFAULT = 10;

		//{同 SW_SHOWNORMAL}    
		public const int SW_MAX = 10;


		[DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
		public static extern long GetWindowThreadProcessId(long hWnd, long lpdwProcessId);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
		public static extern long GetWindowLong(IntPtr hwnd, int nIndex);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="nIndex"></param>
		/// <param name="dwNewLong"></param>
		/// <returns></returns>
		public static IntPtr SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong)
		{
			if (IntPtr.Size == 4)
			{
				return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
			}
			return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
		}

		[DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
		public static extern IntPtr SetWindowLongPtr32(HandleRef hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
		public static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern long SetWindowPos(IntPtr hwnd, long hWndInsertAfter, long x, long y, long cx, long cy, long wFlags);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

		[DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
		public static extern bool PostMessage(IntPtr hwnd, uint Msg, uint wParam, uint lParam);

		[DllImport("Kernel32.dll")]
		public extern static int FormatMessage(int flag, ref IntPtr source, int msgid, int langid, ref string buf, int size, ref IntPtr args);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetParent(IntPtr hwnd);

		[DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
		public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		/// <summary>
		/// 获取系统错误信息描述
		/// </summary>
		/// <param name="errCode">系统错误码</param>
		/// <returns></returns>
		public static string GetLastError()
		{
			var errCode = Marshal.GetLastWin32Error();
			IntPtr tempptr = IntPtr.Zero;
			string msg = null;
			FormatMessage(0x1300, ref tempptr, errCode, 0, ref msg, 255, ref tempptr);
			return msg;
		}

		/// <summary>
		/// 获取系统错误信息描述
		/// </summary>
		/// <param name="errCode">系统错误码</param>
		/// <returns></returns>
		public static string GetLastErrorString(int errCode)
		{
			IntPtr tempptr = IntPtr.Zero;
			string msg = null;
			FormatMessage(0x1300, ref tempptr, errCode, 0, ref msg, 255, ref tempptr);
			return msg;
		}

		/// <summary>
		/// 获取鼠标的位置
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out Point pt);

		[StructLayout(LayoutKind.Sequential)]
		struct CURSORINFO
		{
			public int cbSize;
			public int flags;
			public IntPtr hCursor;
			public Point ptScreenPos;
		}
		[DllImport("user32.dll")]
		static extern bool GetCursorInfo(out CURSORINFO pci);
		private const int CURSOR_SHOWING = 0x00000001;

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static Cursor GetMouseCursor()
		{
			CURSORINFO vCurosrInfo;
			vCurosrInfo.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
            GetCursorInfo(out vCurosrInfo);
			Cursor vCursor = new Cursor(vCurosrInfo.hCursor);
			return vCursor;
		}

	}
}
