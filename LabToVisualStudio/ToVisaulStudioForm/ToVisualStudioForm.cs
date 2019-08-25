using Harry.LabTools.LabControlPlus;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Harry.LabTools.LabToVisualStudio
{
	public partial class ToVisualStudioForm : Form
    {
		#region 变量定义

		#endregion

		#region 属性定义

		#endregion

		#region 构造函数

		/// <summary>
		/// 
		/// </summary>
		public ToVisualStudioForm()
		{
			InitializeComponent();
			this.Init();
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		public ToVisualStudioForm(string path)
		{
			InitializeComponent();
			this.TextBox_SrcProjectPath.Text = path;
			this.Init();
		}

		#endregion

		#region 公共函数


		/// <summary>
		/// 按键点击事件处理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void button_Click(object sender, EventArgs e)
		{
			Button bt = (Button)sender;
			bt.Enabled = false;
			bool _return = true;
			switch (bt.Text)
			{
				case "选择项目":
					this.TextBox_SrcProjectPath.Text = new CProjectFunc().AutoSelectSourceProject(this.comboBox_SrcProjectVersion.Text);
					if ((this.TextBox_SrcProjectPath.Text == string.Empty) || (this.TextBox_SrcProjectPath.Text == null))
					{
						_return = false;
					}
					break;

				case "工程转换":
					_return = new CProjectFunc().MakeVisualStudioProject(this.TextBox_SrcProjectPath.Text, this.comboBox_SrcProjectVersion.Text);
					if (_return)
					{
						_return = this.UsePreMakeToVisualStudio();
						if (_return)
						{
							if (this.checkBox_CloseApplication.Checked)
							{
								Application.Exit();
							}
						}
					}
					break;

				default:
					break;
			}
			bt.Enabled = true;
		}
		
		#endregion

		#region 私有函数

		/// <summary>
		/// 初始化设备
		/// </summary>
		private void Init()
		{
			this.comboBox_SrcProjectVersion.SelectedIndex = 0;
			this.comboBox_VisualStudioVersion.SelectedIndex = 0;
			this.button_SelectSourceProject.Click += new System.EventHandler(this.button_Click);
			this.button_ToVisualStudioProject.Click += new System.EventHandler(this.button_Click);

			//---通过加载文件的不同自适应当前文档
			if (this.TextBox_SrcProjectPath.Text != string.Empty)
			{
				if (this.TextBox_SrcProjectPath.Text.Contains("uvprojx") || this.TextBox_SrcProjectPath.Text.Contains("uvproj"))
				{
					if (this.comboBox_SrcProjectVersion.Text != "Keil")
					{
						this.comboBox_SrcProjectVersion.Text = "Keil";
						this.comboBox_SrcProjectVersion.SelectedIndex = this.comboBox_SrcProjectVersion.Items.IndexOf("Keil");
					}
				}
				else if (this.TextBox_SrcProjectPath.Text.Contains("ewp"))
				{
					if (this.comboBox_SrcProjectVersion.Text != "IAR")
					{
						this.comboBox_SrcProjectVersion.Text = "IAR";
						this.comboBox_SrcProjectVersion.SelectedIndex = this.comboBox_SrcProjectVersion.Items.IndexOf("IAR");
					}
				}
			}
			else
			{
				//CMessageBoxPlus.Show(this, "不支持的软件版本","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			//---限制窗体的大小
			this.MinimumSize = this.Size;
			this.MaximumSize = this.Size;

		}

		/// <summary>
		/// 使用PreMaKe创建VS项目
		/// </summary>
		/// <returns></returns>
		private bool UsePreMakeToVisualStudio()
		{
			//---要装换为vs工程的文件
			string toVSPath = null;
			//---vs项目的文件
			string vsSlnPath = null;
			//---获取文件工程
			string vsPath = null;
			//---文件名称
			string fileName = null;

			if (this.comboBox_SrcProjectVersion.Text == "IAR")
			{
				toVSPath = Path.GetDirectoryName(this.TextBox_SrcProjectPath.Text);

				//vsPath = Directory.GetParent(Path.GetDirectoryName(this.TextBox_SrcPath.Text)).FullName;
			}
			else if (this.comboBox_SrcProjectVersion.Text == "Keil")
			{
				toVSPath = Path.GetDirectoryName(this.TextBox_SrcProjectPath.Text);

				//vsPath = Directory.GetParent(Path.GetDirectoryName(this.TextBox_SrcPath.Text)).FullName;
			}
			else
			{
				return false;
			}
			//---解决方案路径
			vsSlnPath = Path.GetDirectoryName(toVSPath);
			//---工程路径
			vsPath = toVSPath.Split('\\')[vsSlnPath.Split('\\').Length];
			//---文件名称
			fileName = Path.ChangeExtension(this.TextBox_SrcProjectPath.Text, "vcxproj").Split('\\')[Path.ChangeExtension(this.TextBox_SrcProjectPath.Text, "vcxproj").Split('\\').Length - 1];
			////vsPath = "\"" + "--File=\"" + vsPath + "\\premake5.lua\" " + this.comboBox_VisualStudioVersion.Text + "\"";
			//---解决方案路劲
			vsSlnPath = this.TextBox_SrcProjectPath.Text.Replace("\\" + vsPath, "");
			//string[] arg = vsPath.Split(' ');
			//string tempPath = null;
			//if (arg.Length > 1)
			//{
			//	vsPath = string.Empty;
			//	for (int i = 0; i < arg.Length; i++)
			//	{
			//		tempPath = arg[i];
			//		if (i != (arg.Length - 1))
			//		{
			//			vsPath += (tempPath + " " + "\"");
			//		}
			//		else
			//		{
			//			vsPath += tempPath;
			//		}
			//	}
			//	//vsPath += "\"";
			//}

			//---启动进程
			Process proc = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = @"Resources\premake5.exe",
					//FileName = @"premake5.exe",

					//Arguments = "--File=\"" + Path.GetDirectoryName(this.TextBox_SrcProjectPath.Text) + "\\premake5.lua\" " + this.comboBox_VisualStudioVersion.Text,
					//Arguments = "--File="+ vsPath + "\\premake5.lua" + this.comboBox_VisualStudioVersion.Text,
					Arguments = "--File=\"" + toVSPath + "\\premake5.lua\" " + this.comboBox_VisualStudioVersion.Text,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					RedirectStandardInput = true,
					CreateNoWindow = true
				}
			};

			//---启动应用
			proc.Start();
			string makeOut = proc.StandardOutput.ReadToEnd();

			//---创建VS工程
			if (proc.ExitCode == 0)
			{
				CMessageBoxPlus.Show(this, makeOut, @"Make output");

				//---创建VS工程
				if (this.comboBox_VisualStudioVersion.Text.Contains("vs"))
				{
					//---移动vssln文件
					//---读取sln文件
					string str = Path.ChangeExtension(this.TextBox_SrcProjectPath.Text, "sln");
					//---读取数据
					StreamReader sr = new StreamReader(str, Encoding.UTF8);
					//---读取的内容
					string content = sr.ReadToEnd();
					sr.Close();
					content = content.Replace(fileName, vsPath + "\\" + fileName);

					//---修改正路径
					str = str.Replace("\\" + vsPath, "");
					//---写入文件
					StreamWriter sw = new StreamWriter(str, false, Encoding.UTF8);
					sw.Write(content);
					sw.Close();

					//---是否需要启动VS
					if (this.checkBox_OpenVSProject.Checked)
					{
						DialogResult dialogResult = CMessageBoxPlus.Show(this,"\tOpen " + this.comboBox_VisualStudioVersion.Text + " Project ?", this.Text, MessageBoxButtons.YesNo);
						if (dialogResult == DialogResult.Yes)
						{
							//ProcessStartInfo psi = new ProcessStartInfo(Path.ChangeExtension(this.TextBox_SrcProjectPath.Text, "sln"));
							ProcessStartInfo psi = new ProcessStartInfo(Path.ChangeExtension(vsSlnPath, "sln"));
							//ProcessStartInfo psi = new ProcessStartInfo(Path.ChangeExtension(vsPath, "sln"));
							//{
							//	UseShellExecute = true
							//};
							Process.Start(psi);
						}
					}
				}
			}
			else
			{
				CMessageBoxPlus.Show(this, makeOut, @"Make output", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return true;
		}

		#endregion

	}
}
