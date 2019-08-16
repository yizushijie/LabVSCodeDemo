using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Harry.LabTools.LabComm
{
	public partial class CCommSerial
	{
		#region 变量定义

		/// <summary>
		/// 使用的串口类
		/// </summary>
		private SerialPort defaultSerialPort = new SerialPort();

		/// <summary>
		/// 设备信息
		/// </summary>
		private string defaultSerialInfo = string.Empty;

		/// <summary>
		/// 超时时间，单位是ms
		/// </summary>
		private int defaultSerialTimeout = 200;

		/// <summary>
		/// 是不是复合命令
		/// </summary>
		private bool defaultSerialMultiCMD = false;

		/// <summary>
		/// 信息提示
		/// </summary>
		private string defaultSerialMsg = string.Empty;

		/// <summary>
		/// 工作状态
		/// </summary>
		private COMM_STATE defaultSerialSTATE = COMM_STATE.STATE_IDLE;

		/// <summary>
		/// 串口参数
		/// </summary>
		private CCommSerialParam defaultSerialParam = new CCommSerialParam();

		/// <summary>
		/// 使用的时间
		/// </summary>
		private TimeSpan defaultSerialUsedTime;


        /// <summary>
        /// 设备连接状态 true---连接，false---断开
        /// </summary>
        private bool defaultConnected = false;

		/// <summary>
		/// 设备是否发生变化,TRUE---发生变化，FALSE---未变化
		/// </summary>
		private bool defaultChanged = false;

        
        #endregion

        #region 公共属性

        /// <summary>
        /// 
        /// </summary>
        public override string Name
		{
			get
			{
				return this.defaultSerialPort.PortName;
			}
			set
			{
				if (this.defaultSerialPort.PortName!=value)
				{
					if (this.defaultSerialPort.IsOpen)
					{
						try
						{
							this.defaultSerialPort.Close();
						}
						catch (Exception)
						{
							//---清理资源
							this.defaultSerialPort.Dispose();
						}
						
					}
				}
				if ((value==string.Empty)||(value==string.Empty))
				{
					this.defaultSerialPort.PortName = "COM1";
				}
				else
				{
					this.defaultSerialPort.PortName = value;
				}
				this.defaultSerialParam.mName = this.defaultSerialPort.PortName;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override int Index
		{
			get
			{
				if ((this.Name=="")||(this.Name==string.Empty))
				{
					return 0;
				}
				else
				{
					return int.Parse(Regex.Replace(this.Name, @"[^\d]*", ""));
				}
			}
			set
			{
				this.Name = "COM" + value.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override string Info
		{
			get
			{
				return this.defaultSerialInfo;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override int Timeout
		{
			get
			{
				return this.defaultSerialTimeout;
			}
			set
			{
				this.defaultSerialTimeout = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool IsMultiAddr
		{
			get
			{
				return ((this.defaultSerialParam!=null)&&(defaultSerialParam.mAddrID>0));
			}
		}

		/// <summary>
		/// 是不是复合命令
		/// </summary>
		public override bool IsMultiCMD
		{
			get
			{
				return this.defaultSerialMultiCMD;
			}
			set
			{
				this.defaultSerialMultiCMD = value;
			}
		}


		/// <summary>
		/// 端口是否打开
		/// </summary>
		public override bool IsOpen
		{
			get
			{
				if (this.defaultSerialPort==null)
				{
					return false;
				}
				else
				{
					return this.defaultSerialPort.IsOpen;
				}
			}
		}

		/// <summary>
		/// 消息Log信息
		/// </summary>
		public override string LogMessage
		{
			get
			{
				return this.defaultSerialMsg;
			}
		}

		/// <summary>
		/// 通讯状态
		/// </summary>
		public override COMM_STATE COMMSTATE
		{
			get
			{
				return this.defaultSerialSTATE;
			}
		}


		/// <summary>
		/// 使用的时间
		/// </summary>
		public override TimeSpan UsedTime
		{
			get
			{
				return this.defaultSerialUsedTime;
			}
		}

        /// <summary>
        /// 设备连接状态
        /// </summary>
        public override bool IsConnected
        {
            get
            {
                return this.defaultConnected;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		public override bool IsChanged
		{
			get
			{
				return this.defaultChanged;
			}
			set
			{
				this.defaultChanged = value;
			}
		}



		#endregion

		#region 串口属性

		/// <summary>
		/// 串口配置信息
		/// </summary>
		public override CCommSerialParam mSerialParam
		{
			get
			{
				if (this.defaultSerialParam==null)
				{
					this.defaultSerialParam = new CCommSerialParam();
				}
				//this.defaultSerialParam.mName = this.defaultSerialPort.PortName;
				//this.defaultSerialParam.mBaudRate = this.defaultSerialPort.BaudRate.ToString();
				//this.defaultSerialParam.mDataBits = this.defaultSerialPort.DataBits.ToString();
				//this.defaultSerialParam.mStopBits = this.defaultSerialPort.StopBits.ToString();
				//if (this.defaultSerialPort.Parity == System.IO.Ports.Parity.Odd)
				//{
				//	this.defaultSerialParam.mParity = "ODD";
				//}
				//else if (this.defaultSerialPort.Parity == System.IO.Ports.Parity.Even)
				//{
				//	this.defaultSerialParam.mParity = "EVEN";
				//}
				//else
				//{
				//	this.defaultSerialParam.mParity =  "NONE";
				//}
				
				return this.defaultSerialParam;
			}
			set
			{
                if (this.defaultSerialParam!=null)
                {
                    if ((value != null)&&(this.defaultSerialParam.mName!=value.mName))
                    {
                        this.CloseDevice(this.defaultSerialParam.mName);
                    }
					if (value!=null)
					{
						this.defaultSerialParam.AnalyseParam(value.mName, value.mBaudRate, value.mStopBits, value.mDataBits, value.mParity);
						
						//---波特率
						if (this.defaultSerialPort.BaudRate != int.Parse(this.defaultSerialParam.mBaudRate))
						{
							this.defaultSerialPort.BaudRate = int.Parse(this.defaultSerialParam.mBaudRate);
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

						//---校验位
						if (this.defaultSerialPort.Parity != this.GetParityBits(this.defaultSerialParam.mParity))
						{
							this.defaultSerialPort.Parity = this.GetParityBits(this.defaultSerialParam.mParity);
						}
						
					}
                }
				else
				{
					this.defaultSerialParam=new CCommSerialParam(value.mName,value.mBaudRate,value.mStopBits,value.mDataBits,value.mParity);
				}
                if (value!=null)
                {
                    this.Name = value.mName;
                }
            }
		}
		#endregion

		#region 构造函数

		#endregion

		#region 公有函数

		/// <summary>
		/// 初始化串口参数
		/// </summary>
		/// <param name="serialParam"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public override int Init(CCommSerialParam serialParam, RichTextBox msg = null)
		{
			return -1;
		}

		#endregion

		#region 私有函数

		/// <summary>
		/// 获取校验位
		/// </summary>
		/// <param name="parityBits"></param>
		/// <returns></returns>
		private System.IO.Ports.Parity GetParityBits(string parityBits)
		{
			//---获取校验位
			System.IO.Ports.Parity _return;//= new Parity();

			//---奇校验
			if (parityBits.StartsWith("奇") || parityBits.StartsWith("Odd") || parityBits.StartsWith("ODD") || parityBits.StartsWith("odd") || parityBits.StartsWith("oDD"))
			{
				_return = System.IO.Ports.Parity.Odd;
			}

			//---偶校验
			else if (parityBits.StartsWith("偶") || parityBits.StartsWith("Even") || parityBits.StartsWith("EVEN") || parityBits.StartsWith("even") || parityBits.StartsWith("eVEN"))
			{
				_return = System.IO.Ports.Parity.Even;
			}

			//---无校验位
			else
			{
				_return = System.IO.Ports.Parity.None;
			}
			return _return;
		}

		/// <summary>
		/// 获取停止位
		/// </summary>
		/// <param name="stopBits"></param>
		/// <returns></returns>
		private StopBits GetStopBits(string stopBits)
		{
			//---获取校验位
			StopBits _return;// = new StopBits();
			try
			{
				double stopVal = Math.Truncate(Convert.ToDouble(stopBits));

				int temp = (int)(stopVal * 10);

				//---1位停止位
				if (temp == 10)
				{
					_return = System.IO.Ports.StopBits.One;
				}

				//---1.5位停止位
				else if (temp == 15)
				{
					_return = System.IO.Ports.StopBits.OnePointFive;
				}

				//---2位停止位
				else if (temp == 20)
				{
					_return = System.IO.Ports.StopBits.Two;
				}
				else
				{
					_return = System.IO.Ports.StopBits.None;
				}
			}
			catch
			{
				_return = System.IO.Ports.StopBits.None;
			}
			return _return;
		}

		#endregion

		#region 事件函数

		#endregion
	}
}