using Harry.LabTools.LabControlPlus;
using Harry.LabTools.LabGenFunc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

namespace Harry.LabTools.LabComm
{
	public partial class CCommSerial
	{
		#region 变量定义

		#endregion

		#region 属性定义

		#endregion

		#region 构造函数

		#endregion

		#region 公有函数
		/// <summary>
		/// 初始化设备
		/// </summary>
		/// <param name="cbb"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public override int Init(ComboBox cbb, RichTextBox msg = null)
		{
			int _return = -1;
			if (this.GetSerialPortName() == true)
			{
				if (cbb != null)
				{
					//---异步调用
					if (cbb.InvokeRequired)
					{
						cbb.BeginInvoke((EventHandler)
								 //cbb.Invoke((EventHandler)
								 (delegate
								 {
									 cbb.Items.Clear();
									 for (int i = 0; i < this.defaSerialIndexMemu.Count; i++)
									 {
										 cbb.Items.Add("COM" + this.defaSerialIndexMemu[i].ToString());
									 }
									 cbb.SelectedIndex = 0;
								 }));
					}
					else
					{
						cbb.Items.Clear();
						for (int i = 0; i < this.defaSerialIndexMemu.Count; i++)
						{
							cbb.Items.Add("COM" + this.defaSerialIndexMemu[i].ToString());
						}
						cbb.SelectedIndex = 0;
					}
					//---获取设备的驱动信息
					this.defaultSerialInfo = this.defaultSerialInfoMemu[0];
					//---获取设备的名称信息
					this.Name = "COM" + this.defaSerialIndexMemu[0].ToString();
				}
			}
			else
			{
				if (cbb != null)
				{
					//---异步调用
					if (cbb.InvokeRequired)
					{
						cbb.BeginInvoke((EventHandler)
								 //cbb.Invoke((EventHandler)
								 (delegate
								 {
									 cbb.Items.Clear();
									 cbb.SelectedIndex = -1;
								 }));
					}
					else
					{
						cbb.Items.Clear();
						cbb.SelectedIndex = -1;
					}
				}
			}


          	this.mCCommComBox = cbb;
            this.mCCommRichTextBox = msg;

            //---添加端口监控函数
            this.AddWatcherCommEvent();
            return _return;
		}

		/// <summary>
		/// 刷新设备
		/// </summary>
		/// <param name="cbb"></param>
		/// <param name="msg"></param>
		public override int RefreshDevice(ComboBox cbb, RichTextBox msg = null)
		{
			return this.Init(cbb,msg);
		}

		/// <summary>
		/// 设备插入
		/// </summary>
		/// <param name="cbb"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public override int InsertDevice()
		{
			int _return = 0;
			int portIndex = -1;
			//---获取当前设备存在的通信端口
			List<byte> addNames = this.GetSerialPortIndex(SerialPort.GetPortNames());
			//---检查设备端口
			if ((addNames == null) || (addNames.Count == 0))
			{
				if (this.defaSerialIndexMemu!=null)
				{
					this.defaSerialIndexMemu.Clear();
				}
				if (this.defaultSerialInfoMemu!=null)
				{
					this.defaultSerialInfoMemu.Clear();
				}
				if (this.mCCommComBox != null)
				{
					//---异步调用
					if (this.mCCommComBox.InvokeRequired)
					{
						this.mCCommComBox.BeginInvoke((EventHandler)
								 //cbb.Invoke((EventHandler)
								 (delegate
								 {
									 this.mCCommComBox.Items.Clear();
									 this.mCCommComBox.SelectedIndex = -1;
								 }));
					}
					else
					{
						this.mCCommComBox.Items.Clear();
						this.mCCommComBox.SelectedIndex = -1;
					}
				}
                _return = 1;
                this.defaultSerialMsg = "获取设备失败，请插入设备！\r\n";
			}
			else
			{
				
				int i = 0;
                this.defaultSerialMsg = "";
                //---遍历是哪个设备插入
                for (i = 0; i < addNames.Count; i++)
				{
					if ((this.defaSerialIndexMemu != null) && (this.defaSerialIndexMemu.Count > 0))
					{
						//---查询是哪个设备插入
						portIndex = this.defaSerialIndexMemu.IndexOf(addNames[i]);
					}
					//---UI显示插入的设备
					if (portIndex < 0)
					{
                        this.defaultSerialMsg += "COM" + addNames[i].ToString() + "设备插入！\r\n";
					}
				}

				portIndex = -1;

				List<byte> addDevice = new List<byte>();

				//---获取当前选择的端口
				if (this.mCCommComBox != null)
				{
					//---异步调用
					if (this.mCCommComBox.InvokeRequired)
					{
						this.mCCommComBox.Invoke((EventHandler)
									 (delegate
									 {
										 portIndex = this.mCCommComBox.SelectedIndex;
									 }));
					}
					else
					{
						portIndex = this.mCCommComBox.SelectedIndex;
					}
				}

				if ((this.defaSerialIndexMemu.Count != 0) && (portIndex >= 0))
				{
					portIndex = this.defaSerialIndexMemu[portIndex];
				}

  				this.defaSerialIndexMemu = new List<byte>();
				this.defaSerialIndexMemu.AddRange(addNames.ToArray());

				if (this.defaSerialIndexMemu.Count != 0)
				{
					if (portIndex < 0)
					{
						portIndex = 0;
					}
					else
					{
						portIndex = this.defaSerialIndexMemu.IndexOf((byte)portIndex);
					}

				}

				if (this.mCCommComBox != null)
				{
					//---异步调用
					if (this.mCCommComBox.InvokeRequired)
					{
						this.mCCommComBox.BeginInvoke((EventHandler)
								 //cbb.Invoke((EventHandler)
								 (delegate
								 {
									 this.mCCommComBox.Items.Clear();
									 for (i = 0; i < this.defaSerialIndexMemu.Count; i++)
									 {
										 this.mCCommComBox.Items.Add("COM" + this.defaSerialIndexMemu[i].ToString());
									 }
									 this.mCCommComBox.SelectedIndex = portIndex;
								 }));
					}
					else
					{
						this.mCCommComBox.Items.Clear();
						for (i = 0; i < this.defaSerialIndexMemu.Count; i++)
						{
							this.mCCommComBox.Items.Add("COM" + this.defaSerialIndexMemu[i].ToString());
						}
						this.mCCommComBox.SelectedIndex = portIndex;
					}
				}
			}
			//---获取驱动信息
			this.defaultSerialInfoMemu = this.GetSerialPortInfo(SystemHardware.GetSerialPort());
			if ((this.defaultSerialInfoMemu!=null)&&(this.defaultSerialInfoMemu.Count>0)&&(this.defaultSerialInfoMemu.Count> portIndex))
			{
				this.defaultSerialInfo = this.defaultSerialInfoMemu[portIndex];
			}

            if (this.mCCommRichTextBox != null)
            {
                CRichTextBoxPlus.AppendTextInfoTopWithDataTime(this.mCCommRichTextBox, this.defaultSerialMsg,(_return==0?Color.Black:Color.Red), false);
            }
            return _return;
		}

		/// <summary>
		/// 设备移除
		/// </summary>
		/// <param name="cbb"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public override int RemoveDevice()
		{
			int _return = 0;
			//---获取当前设备存在的通信端口
			List<byte> addNames = this.GetSerialPortIndex(SerialPort.GetPortNames());
			if ((addNames == null) || (addNames.Count == 0))
			{
				this.defaSerialIndexMemu = new List<byte>();
				if (this.mCCommComBox != null)
				{
					//---异步调用
					if (this.mCCommComBox.InvokeRequired)
					{
						this.mCCommComBox.BeginInvoke((EventHandler)
								 //cbb.Invoke((EventHandler)
								 (delegate
								 {
									 this.mCCommComBox.Items.Clear();
                                     this.mCCommComBox.Text = string.Empty;
                                     this.mCCommComBox.SelectedIndex = -1;
								 }));
					}
					else
					{
						this.mCCommComBox.Items.Clear();
                        this.mCCommComBox.Text = string.Empty;
                        this.mCCommComBox.SelectedIndex = -1;
					}
				}
                _return = 1;
                this.defaultSerialMsg = "获取设备失败，请插入设备！\r\n";
			}
			else
			{
				int portIndex = -1;
				int i = 0;
                this.defaultSerialMsg = "";
                //---遍历是哪个设备移除
                for (i = 0; i < this.defaSerialIndexMemu.Count; i++)
				{
					//---查询是哪个设备移除
					portIndex = addNames.IndexOf(this.defaSerialIndexMemu[i]);
					//---UI显示插入的设备
					if (portIndex < 0)
					{
                        //---判断是不是当前设备
						if (this.defaSerialIndexMemu[i] == this.Index)
						{
							this.Name = "";
                            if (this.defaultConnected ==true)
                            {
                                if (this.defaultSerialPort!=null)
                                {
                                    //---注销事件接收函数
                                    this.defaultSerialPort.DataReceived -= new SerialDataReceivedEventHandler(this.EventDataReceivedHandler);
                                    //---释放资源
                                    this.defaultSerialPort.Dispose();
                                }
                            }
						}
                        _return = 2;
                        this.defaultSerialMsg += "COM" + this.defaSerialIndexMemu[i].ToString() + "设备移除！\r\n";
					}
				}

				portIndex = -1;

				List<byte> addDevice = new List<byte>();

				//---获取当前使用的设备列表
				if (this.mCCommComBox != null)
				{
					//---异步调用
					if (this.mCCommComBox.InvokeRequired)
					{
						this.mCCommComBox.Invoke((EventHandler)
									 (delegate
									 {
										 portIndex = this.mCCommComBox.SelectedIndex;
									 }));
					}
					else
					{
						portIndex = this.mCCommComBox.SelectedIndex;
					}
				}

				if ((this.defaSerialIndexMemu.Count != 0) && (portIndex >= 0))
				{
					portIndex = this.defaSerialIndexMemu[portIndex];
				}

				if (this.defaSerialIndexMemu==null)
				{
					this.defaSerialIndexMemu = new List<byte>();
				}
				else
				{
					this.defaSerialIndexMemu.Clear();
				}
				this.defaSerialIndexMemu.AddRange(addNames.ToArray());

				if (this.defaSerialIndexMemu.Count != 0)
				{
					if (portIndex < 0)
					{
                        //---默认选择第一个设备
						portIndex = 0;
					}
					else
					{
						portIndex = this.defaSerialIndexMemu.IndexOf((byte)portIndex);
						if (portIndex < 0)
						{
                            //---默认选择第一个设备
                            portIndex = 0;
						}
					}
				}
                //---刷新设备
                if (this.mCCommComBox != null)
				{
					//---异步调用
					if (this.mCCommComBox.InvokeRequired)
					{
						this.mCCommComBox.BeginInvoke((EventHandler)
								 //cbb.Invoke((EventHandler)
								 (delegate
								 {
									 this.mCCommComBox.Items.Clear();
									 for (i = 0; i < this.defaSerialIndexMemu.Count; i++)
									 {
										 this.mCCommComBox.Items.Add("COM" + this.defaSerialIndexMemu[i].ToString());
									 }
									 this.mCCommComBox.SelectedIndex = portIndex;
								 }));
					}
					else
					{
						this.mCCommComBox.Items.Clear();
						for (i = 0; i < this.defaSerialIndexMemu.Count; i++)
						{
							this.mCCommComBox.Items.Add("COM" + this.defaSerialIndexMemu[i].ToString());
						}
						this.mCCommComBox.SelectedIndex = portIndex;
					}
				}
			}
			//---判断设备是否为空
			if (this.defaSerialIndexMemu.Count == 0)
			{
				//---释放端口
				if ((this.defaultSerialPort != null) && (this.Index != 0))
				{
					//---端口状态，空闲
					this.defaultSerialSTATE = COMM_STATE.STATE_IDLE;
					//---关闭端口
					this.defaultSerialPort.Close();
                    //---释放串口资源
                    this.defaultSerialPort.Dispose();
					this.Name = string.Empty;
				}
			}
            if (this.mCCommRichTextBox != null)
            {
                CRichTextBoxPlus.AppendTextInfoTopWithDataTime(this.mCCommRichTextBox, this.defaultSerialMsg, (_return == 0 ? Color.Black : Color.Red), false);
            }
            return _return;
		}

		/// <summary>
		/// 向设备写入命令
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public override int WriteCmdToDevice(string cmd, RichTextBox msg = null)
		{
			int _return = -1;
			if ((this.defaultSerialPort != null) && (this.defaultSerialPort.IsOpen))
			{
				//---等待发送完成
				while (this.defaultSerialPort.BytesToWrite > 0)
				{
					//---响应窗体函数
					Application.DoEvents();
				}
				//---发送数据
				this.defaultSerialPort.WriteLine(cmd);
				_return = 0;
			}
			else
			{
				this.defaultSerialMsg = "端口：" + this.Name + "初始化异常！\r\n";
			}
			if ((msg != null) && (_return != 0))
			{
				CRichTextBoxPlus.AppendTextInfoTopWithDataTime(msg, this.defaultSerialMsg, Color.Red, false);
			}
			return _return;
		}

		/// <summary>
		/// 向设备写入命令
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public override int WriteCmdToDevice(byte[] cmd, RichTextBox msg = null)
		{
			int _return = -1;
			if (this.AnalyseSendData(cmd)==true)
			{
				if ((this.defaultSerialPort!=null)&&(this.defaultSerialPort.IsOpen))
				{
					//---等待发送完成
					while (this.defaultSerialPort.BytesToWrite > 0)
					{
						//---响应窗体函数
						Application.DoEvents();
					}
					//---发送数据
					this.defaultSerialPort.Write(this.defaultSerialSendData.mByte.ToArray(), 0, this.defaultSerialSendData.mByte.Count);
					_return = 0;
				}
				else
				{
					this.defaultSerialMsg = "端口：" + this.Name + "初始化异常！\r\n";
				}
			}
			if ((msg!=null)&&(_return!=0))
			{
				CRichTextBoxPlus.AppendTextInfoTopWithDataTime(msg, this.defaultSerialMsg, Color.Red, false);
			}
			return _return;
		}

		/// <summary>
		/// 从设备中读取命令
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="timeout"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public override int ReadCmdFromDevice(ref string cmd, int timeout = 200, RichTextBox msg = null)
		{
			//---获取开始时间标签
			DateTime nowTime = DateTime.Now;
			int _return = -1;
			if ((this.defaultSerialPort != null) && (this.defaultSerialPort.IsOpen))
			{
				CGenFuncDelay.GenFuncDelayms(timeout);
				if (this.defaultSerialPort.BytesToRead > 0)
				{
					cmd = this.defaultSerialPort.ReadExisting();
					_return = 0;
				}
				else
				{
					this.defaultSerialMsg = "未收到响应数据！\r\n";
				}
			}
			else
			{
				this.defaultSerialMsg = "端口：" + this.Name + "初始化异常！\r\n";
			}
			if ((msg != null) && (_return != 0))
			{
				CRichTextBoxPlus.AppendTextInfoTopWithDataTime(msg, this.defaultSerialMsg, Color.Red, false);
			}
			this.defaultSerialTimeout = timeout;
			//---结束时间
			this.defaultSerialUsedTime = DateTime.Now - nowTime;
			return _return;
		}

		/// <summary>
		/// 从设备中读取命令
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="timeout"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public override int ReadCmdFromDevice(ref byte[] cmd, int timeout = 200, RichTextBox msg = null)
		{
			//---获取开始时间标签
			DateTime nowTime = DateTime.Now;
			int _return = -1;
			if (this.defaultSerialReceData.mSize>250)
			{
				_return = this.Analyse16BitsData(timeout);
			}
			else
			{
				_return = this.Analyse8BitsData(timeout);
			}
			if (_return==0)
			{
				cmd = new byte[this.defaultSerialReceData.mByte.Count];
				this.defaultSerialReceData.mByte.CopyTo(cmd);
			}
			if ((msg != null) && (_return != 0))
			{
				CRichTextBoxPlus.AppendTextInfoTopWithDataTime(msg, this.defaultSerialMsg, Color.Red, false);
			}
			this.defaultSerialTimeout = timeout;
			//---结束时间
			this.defaultSerialUsedTime = DateTime.Now - nowTime;
			return _return;
		}

		/// <summary>
		/// 发送并读取响应
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="res"></param>
		/// <param name="timeout"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public override int SendCmdAndReadResponse(byte[] cmd, ref byte[] res, int timeout = 200, RichTextBox msg = null)
		{
			//---获取开始时间标签
			DateTime nowTime = DateTime.Now;
			int _return = this.WriteCmdToDevice(cmd, msg);
			if (_return==0)
			{
				_return = this.ReadCmdFromDevice(ref res, timeout, msg);
			}
			//---结束时间
			this.defaultSerialUsedTime = DateTime.Now - nowTime;
			return _return;
		}

		/// <summary>
		/// 发送并读取响应
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="res"></param>
		/// <param name="timeout"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public override int SendCmdAndReadResponse(string cmd, ref string res, int timeout = 200, RichTextBox msg = null)
		{
			//---获取开始时间标签
			DateTime nowTime = DateTime.Now;
			int _return = this.WriteCmdToDevice(cmd, msg);
			if (_return==0)
			{
				_return = ReadCmdFromDevice(ref res, timeout, msg);
			}
			//---结束时间
			this.defaultSerialUsedTime = DateTime.Now - nowTime;
			return _return;
		}

		/// <summary>
		/// 打开设备
		/// </summary>
		/// <returns></returns>
		public override int OpenDevice()
		{
			return this.OpenDevice(this.defaultSerialParam.mName,null);
		}

		/// <summary>
		/// 打开设备
		/// </summary>
		/// <param name="argName"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public override int OpenDevice(string argName, RichTextBox msg = null)
		{
			int _return = 0;
			if (((argName != string.Empty) && (argName != null) && (argName != "") && (argName.StartsWith("COM"))))
			{
				//---判断串口类是否初始化
				if (this.defaultSerialPort == null)
				{
					this.defaultSerialPort = new SerialPort();
				}
				//---判断当前端口是否可用
				if (this.defaultSerialPort.IsOpen)
				{
					//---等待端口事件处理完成
					while (!((this.defaultSerialSTATE == COMM_STATE.STATE_IDLE) || (this.defaultSerialSTATE == COMM_STATE.STATE_ERROR)))
					{
						Application.DoEvents();
					}
					//---端口状态，空闲
					this.defaultSerialSTATE = COMM_STATE.STATE_IDLE;
					//---关闭端口
					this.defaultSerialPort.Close();
				}
				//---判断端口状态
				if (this.defaultSerialPort.IsOpen == false)
				{
					//---获取端口名称
					if (this.defaultSerialPort.PortName != argName)
					{
						this.defaultSerialPort.PortName = argName;
					}

					
					//---查空操作
					if (this.defaultSerialParam == null)
					{
						this.defaultSerialParam = new CCommSerialParam();
					}
					//---使用的设备端口
					this.Name = argName;

					//---波特率
					if (this.defaultSerialPort.BaudRate != int.Parse(this.defaultSerialParam.mBaudRate))
					{
						this.defaultSerialPort.BaudRate = int.Parse(this.defaultSerialParam.mBaudRate);
					}

					//---校验位
					if (this.defaultSerialPort.Parity != this.GetParityBits(this.defaultSerialParam.mParity))
					{
						this.defaultSerialPort.Parity = this.GetParityBits(this.defaultSerialParam.mParity);
					}

					//---停止位
					if (this.defaultSerialPort.StopBits != this.GetStopBits(this.defaultSerialParam.mStopBits))
					{
						this.defaultSerialPort.StopBits = this.GetStopBits(this.defaultSerialParam.mStopBits);
					}

					//---数据位
					if (this.defaultSerialPort.DataBits != int.Parse(this.defaultSerialParam.mDataBits))
					{
						this.defaultSerialPort.DataBits = int.Parse(this.defaultSerialParam.mDataBits);
					}
					try
					{
						//---打开端口
						this.defaultSerialPort.Open();
						//---判断端口打开是否成功
						if (this.defaultSerialPort.IsOpen == false)
						{
							//---端口状态，错误
							this.defaultSerialSTATE = COMM_STATE.STATE_ERROR;
							this.defaultSerialMsg = "端口：" + this.Name + "打开失败!\r\n";
							_return = 2;
						}
						else
						{
                            this.defaultConnected = true;
                            this.defaultSerialMsg = "端口：" + this.Name + "打开成功!\r\n";
							//---注册事件接收函数
							this.defaultSerialPort.DataReceived += new SerialDataReceivedEventHandler(this.EventDataReceivedHandler);
							_return = 0;
						}
					}
					catch 
					{
						this.defaultSerialMsg = "端口：" + this.Name + "打开异常!\r\n";
						_return = 3;
					}
					
				}
				else
				{
					//---端口状态，错误
					this.defaultSerialSTATE = COMM_STATE.STATE_ERROR;
					this.defaultSerialMsg = "端口：" + argName + "初始化失败!\r\n";
					_return = 4;
				}
			}
			if (_return > 0)
			{
				//---消息插件弹出
				CMessageBoxPlus.Show(this.mCCommForm, this.defaultSerialMsg + "\r\n" + "错误号：" + _return.ToString() + "\r\n", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else if (_return < 0)
			{
				CMessageBoxPlus.Show(this.mCCommForm, "端口名称不合法!\r\n", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
				_return = 5;
			}
			else
			{
				//---消息显示
				if (msg != null)
				{
					CRichTextBoxPlus.AppendTextInfoTopWithDataTime(msg, this.defaultSerialMsg, _return == 0 ? Color.Black : Color.Red, false);
				}
			}
			return _return;
		
		}

		/// <summary>
		/// 打开设备
		/// </summary>
		/// <param name="argIndex"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public override int OpenDevice(int argIndex, RichTextBox msg = null)
		{
			this.Name = "COM" + argIndex.ToString();
			return this.OpenDevice(this.Name, msg); 
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="argSerialParam"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public override int OpenDevice(CCommSerialParam argSerialParam, RichTextBox msg = null)
		{
			if (this.defaultSerialParam==null)
			{
				this.defaultSerialParam = new CCommSerialParam();
			}
			this.Name = argSerialParam.mName;
			this.defaultSerialParam.mBaudRate = argSerialParam.mBaudRate;
			this.defaultSerialParam.mParity = argSerialParam.mParity;
			this.defaultSerialParam.mDataBits = argSerialParam.mDataBits;
			this.defaultSerialParam.mStopBits = argSerialParam.mStopBits;
			this.defaultSerialParam.mAddrID = argSerialParam.mAddrID;
			return this.OpenDevice(this.Name,msg);
		}

		/// <summary>
		/// 关闭设备
		/// </summary>
		/// <returns></returns>
		public override int CloseDevice()
		{
			return this.CloseDevice(null);
		}
		/// <summary>
		/// 关闭设备
		/// </summary>
		/// <returns></returns>
		public override int CloseDevice(RichTextBox msg = null)
		{
            int _return = -1;
            if ((this.defaultSerialPort!=null)&&(this.defaultSerialPort.IsOpen))
            {
                try
                {
                    this.defaultSerialPort.Close();
                    if (this.defaultSerialPort.IsOpen==false)
                    {
                        this.defaultConnected = false;
                        _return = 0;
                        this.defaultSerialMsg = "端口:" + this.Name.ToString() + "关闭成功!\r\n";
                        //---注销事件接收函数
                        this.defaultSerialPort.DataReceived -= new SerialDataReceivedEventHandler(this.EventDataReceivedHandler);
                        //---释放端口使用的资源
                        this.defaultSerialPort.Dispose();
                    }
                    else
                    {
                        this.defaultSerialMsg = "端口:" + this.Name.ToString() + "关闭失败!\r\n";
                        _return = 1;
                    }
                }
                catch 
                {
                    this.defaultSerialMsg = "端口:" + this.Name.ToString() + "关闭异常!\r\n";
                    _return = 2;
                }
            }
            else
            {
                this.defaultSerialMsg = "端口资源已释放!\r\n";
                _return = 3;
            }
			//---消息显示
            if (msg != null)
            {
                CRichTextBoxPlus.AppendTextInfoTopWithDataTime(msg, this.defaultSerialMsg, _return == 0 ? Color.Black : Color.Red, false);
            }
            return _return;
		}

		/// <summary>
		/// 关闭设备
		/// </summary>
		/// <param name="argName"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public override int CloseDevice(string argName, RichTextBox msg = null)
		{
            int _return = -1;
            if (this.defaultSerialPort == null)
            {
                this.defaultSerialMsg = "端口资源已释放!\r\n";
                _return = 1;
            }
            else if(this.defaultSerialPort.PortName!=argName)
            {
                this.defaultSerialMsg = "释放端口名称不匹配!\r\n";
                _return = 2;
            }
            else if(this.defaultSerialPort.IsOpen==false)
            {
                this.defaultSerialMsg = "端口:"+argName+"已关闭!\r\n";
                this.defaultSerialPort.Dispose();
                _return = 3;
            }
            else
            {
                try
                {
                    this.defaultSerialPort.Close();
                    if (this.defaultSerialPort.IsOpen == false)
                    {
                        this.defaultConnected = false;
                        _return = 0;
                        this.defaultSerialMsg = "端口:" + this.Name.ToString() + "关闭成功!\r\n";
                        //---注销事件接收函数
                        this.defaultSerialPort.DataReceived -= new SerialDataReceivedEventHandler(this.EventDataReceivedHandler);
                        //---释放端口使用的资源
                        this.defaultSerialPort.Dispose();
                    }
                    else
                    {
                        this.defaultSerialMsg = "端口:" + this.Name.ToString() + "关闭失败!\r\n";
                        _return = 4;
                    }
                }
                catch
                {
                    this.defaultSerialMsg = "端口:" + this.Name.ToString() + "关闭异常!\r\n";
                    _return = 5;
                }
            }
            //---消息显示
            if (msg != null)
            {
                CRichTextBoxPlus.AppendTextInfoTopWithDataTime(msg, this.defaultSerialMsg, _return == 0 ? Color.Black : Color.Red, false);
            }
            return _return;
        }

		/// <summary>
		/// 关闭设备
		/// </summary>
		/// <param name="argIndex"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public override int CloseDevice(int argIndex, RichTextBox msg = null)
		{
            string argName = "COM" + argIndex.ToString();
            return this.CloseDevice(argName, msg);
		}

		/// <summary>
		/// 设备是否处于连接状态
		/// </summary>
		/// <returns></returns>
		public override bool IsAttached()
		{
            if (this.defaultSerialPort == null)
            {
                return false;
            }
            else
            {
                return this.defaultSerialPort.IsOpen;
            }
		}

		/// <summary>
		/// 设备是否处于连接状态
		/// </summary>
		/// <param name="argName"></param>
		/// <returns></returns>
		public override bool IsAttached(string argName)
		{
            if (this.defaultSerialPort == null)
            {
                return false;
            }
            else if(this.defaultSerialPort.PortName==argName)
            {
                return this.defaultSerialPort.IsOpen;
            }
            else
            {
                return false;
            }
        }

		/// <summary>
		/// 设备是否处于连接状态
		/// </summary>
		/// <param name="argIndex"></param>
		/// <returns></returns>
		public override bool IsAttached(int argIndex)
		{
            string argName = "COM" + argIndex.ToString();
            return this.IsAttached(argName);
        }

		#endregion
		
		#region 私有函数

		#endregion

		#region 事件函数

		#endregion
	}
}
