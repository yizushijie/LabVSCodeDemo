using Harry.LabTools.LabGenFunc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Harry.LabTools.LabHexEdit
{
	public partial  class CHexFile
	{
		#region 变量定义

		/// <summary>
		/// Hex数据行
		/// </summary>
		private List<CHexLine> defaultCHexLine = null;

		/// <summary>
		/// LOG信息
		/// </summary>
		private string defaultLogMessage = null;

		/// <summary>
		/// 数据是否有效
		/// </summary>
		private bool defaultIsOK = false;

		#endregion

		#region 属性定义

		/// <summary>
		/// 
		/// </summary>
		public virtual bool mIsOK
		{
			get
			{
				return this.defaultIsOK;
			}
		}

		/// <summary>
		/// Log信息
		/// </summary>
		public virtual string mLogMessage
		{
			get
			{
				return this.defaultLogMessage;
			}
		}

		/// <summary>
		/// 起始地址
		/// </summary>
		public virtual long mSTARTAddr
		{
			get
			{
				long _return = 0;
				byte[] buffer = null;
				if ((this.defaultCHexLine == null) || (this.defaultCHexLine.Count == 0))
				{
					this.defaultIsOK = false;
					_return = 0;
				}
				else
				{
					switch (this.defaultCHexLine[0].Type)
					{
						case HexType.DATA_RECORD:               //---文件信息首先填充0xFF，开始地址是0x00
							_return = this.defaultCHexLine[0].Addr;
							if (_return != 0)
							{
								_return = 0;
							}
							break;
						case HexType.END_OF_FILE_RECORD:
							this.defaultIsOK = false;
							break;
						case HexType.EXTEND_SEGMENT_ADDRESS_RECORD:
							buffer = new byte[2] { this.defaultCHexLine[0].InfoData[1], this.defaultCHexLine[0].InfoData[0] };
							_return = BitConverter.ToUInt16(buffer, 0);
							_return <<= 4;
							break;
						case HexType.START_SEGMENT_ADDRESS_RECORD:
							buffer = new byte[4] { this.defaultCHexLine[0].InfoData[3], this.defaultCHexLine[0].InfoData[2], this.defaultCHexLine[0].InfoData[1], this.defaultCHexLine[0].InfoData[0] };
							_return = BitConverter.ToUInt32(buffer, 0);
							break;
						case HexType.EXTEND_LINEAR_ADDRESS_RECORD:
							buffer = new byte[2] { this.defaultCHexLine[0].InfoData[1], this.defaultCHexLine[0].InfoData[0] };
							_return = BitConverter.ToUInt16(buffer, 0);
							_return <<= 16;
							break;
						case HexType.START_LINEAR_ADDRESS_RECORD:
							buffer = new byte[4] { this.defaultCHexLine[0].InfoData[3], this.defaultCHexLine[0].InfoData[2], this.defaultCHexLine[0].InfoData[1], this.defaultCHexLine[0].InfoData[0] };
							_return = BitConverter.ToUInt32(buffer, 0);
							break;
						default:
							this.defaultIsOK = false;
							break;
					}
				}
				return _return;
			}
		}

		/// <summary>
		/// 终止地址
		/// </summary>
		public virtual long mSTOPAddr
		{
			get
			{
				long _return = 0;
				byte[] buffer = null;
				if ((this.defaultCHexLine == null) || (this.defaultCHexLine.Count == 0))
				{
					_return = 0;
					this.defaultIsOK = false;
				}
				else
				{
					for (int i = 0; i < this.defaultCHexLine.Count; i++)
					{
						switch (this.defaultCHexLine[i].Type)
						{
							case HexType.DATA_RECORD:							//0
								_return = (_return & 0xFFFF0000) + (long) (this.defaultCHexLine[i].Addr + this.defaultCHexLine[i].Length);
								break;
							case HexType.END_OF_FILE_RECORD:					//1
								break;
							case HexType.EXTEND_SEGMENT_ADDRESS_RECORD:			//2
								buffer = new byte[2] { this.defaultCHexLine[i].InfoData[1], this.defaultCHexLine[i].InfoData[0]};
								_return = BitConverter.ToUInt16(buffer, 0);
								_return <<= 4;
								break;
							case HexType.START_SEGMENT_ADDRESS_RECORD:			//3
								buffer = new byte[4]
								{
									this.defaultCHexLine[i].InfoData[3], this.defaultCHexLine[i].InfoData[2], this.defaultCHexLine[i].InfoData[1],
									this.defaultCHexLine[i].InfoData[0]
								};
								//_return = BitConverter.ToUInt32(buffer, 0);
								break;
							case HexType.EXTEND_LINEAR_ADDRESS_RECORD:			//4
								buffer = new byte[2] { this.defaultCHexLine[i].InfoData[1], this.defaultCHexLine[i].InfoData[0]};
								_return = BitConverter.ToUInt16(buffer, 0);
								_return <<= 16;
								break;
							case HexType.START_LINEAR_ADDRESS_RECORD:			//5
								buffer = new byte[4]
								{
									this.defaultCHexLine[i].InfoData[3], this.defaultCHexLine[i].InfoData[2], this.defaultCHexLine[i].InfoData[1],
									this.defaultCHexLine[i].InfoData[0]
								};
								//_return = BitConverter.ToUInt32(buffer, 0);
								break;
							default:
								break;
						}
					}
				}
				return _return;
			}
		}

		#endregion

		#region 构造函数

		/// <summary>
		/// 构造函数
		/// </summary>
		public CHexFile()
		{

		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="hexPath"></param>
		public CHexFile(string hexPath)
		{
			this.defaultIsOK = this.AnalyseHexFile(hexPath);
		}

		#endregion

		#region 析构函数

		/// <summary>
		/// 
		/// </summary>
		~CHexFile()
		{
			if ((this.defaultCHexLine != null) || (this.defaultCHexLine.Count > 0))
			{
				GC.SuppressFinalize(this.defaultCHexLine);
			}

			GC.SuppressFinalize(this);
		}

		#endregion

		#region 公共函数

		/// <summary>
		/// 解析Hex文件数据
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public virtual bool AnalyseHexFile(string filePath)
		{
			//---检查文件是否存在
			if (!File.Exists(filePath))
			{
				this.defaultLogMessage = "Hex文件不存在!\r\n";
				return false;
			}
			try
			{
				using (StreamReader std = new StreamReader(filePath))
				{
					long i = 0;
					if ((this.defaultCHexLine == null) || (this.defaultCHexLine.Count == 0))
					{
						this.defaultCHexLine = new List<CHexLine>();
					}
					else
					{
						this.defaultCHexLine.Clear();
					}
					while (std.Peek() >= 0)
					{
						i++;
						//---读取的数据
						string readline = std.ReadLine();
						//---每行数据创建一个对象
						CHexLine readHexLine = new CHexLine(readline);
						//---判断数据的读取是否有效
						if (readHexLine.IsOK)
						{
							this.defaultCHexLine.Add(readHexLine);
						}
						else
						{
							this.defaultLogMessage = "第" + i.ToString() + "行" + "的数据解析错误!" + readHexLine.LogMessage + "\r\n";
							return false;
						}
					}
				}
			}
			catch
			{
				this.defaultLogMessage = "Hex文件解析错误!\r\n";
				return false;
			}
			return true;
		}

		/// <summary>
		/// 获取解析后的数据
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public virtual byte[] GetDataMap()
		{
			//---创建缓存区
			byte[] _return = new byte[this.mSTOPAddr];
			if (_return == null)
			{
				return null;
			}

			if ((this.defaultCHexLine==null)||(this.defaultCHexLine.Count==0)) 
			{
				return null;
			}
			//---用指定的数据填充缓存区
			CGenFuncMem.GenFuncMemset(ref _return, _return.Length, 0xFF);
			//---初始化数据的地址
			long baseAddr = 0;
			//---将解析后的数据填充到数据缓存区
			int i = 0;
			for (i = 0; i < this.defaultCHexLine.Count; i++)
			{
				byte[] buffer = null;
				//---数据类型的解析
				switch (this.defaultCHexLine[i].Type)
				{
					case HexType.DATA_RECORD:
						//---拷贝数据
						Array.Copy(this.defaultCHexLine[i].InfoData, 0, _return, (baseAddr + this.defaultCHexLine[i].Addr), this.defaultCHexLine[i].Length);
						break;

					case HexType.END_OF_FILE_RECORD:
						break;

					case HexType.EXTEND_SEGMENT_ADDRESS_RECORD:
						//---获取数据的地址
						buffer = new byte[2] { this.defaultCHexLine[i].InfoData[1], this.defaultCHexLine[i].InfoData[0] };
						baseAddr = BitConverter.ToUInt16(buffer, 0);
						baseAddr <<= 4;
						break;

					case HexType.START_SEGMENT_ADDRESS_RECORD:
						//---获取数据的地址
						buffer = new byte[4] { this.defaultCHexLine[i].InfoData[3], this.defaultCHexLine[i].InfoData[2], this.defaultCHexLine[i].InfoData[1], this.defaultCHexLine[i].InfoData[0] };
						baseAddr = BitConverter.ToUInt32(buffer, 0);
						baseAddr <<= 4;
						break;

					case HexType.EXTEND_LINEAR_ADDRESS_RECORD:
						//---获取数据的地址
						buffer = new byte[2] { this.defaultCHexLine[i].InfoData[1], this.defaultCHexLine[i].InfoData[0] };
						baseAddr = BitConverter.ToUInt16(buffer, 0);
						baseAddr <<= 16;
						break;

					case HexType.START_LINEAR_ADDRESS_RECORD:
						//---获取数据的地址
						buffer = new byte[4] { this.defaultCHexLine[i].InfoData[3], this.defaultCHexLine[i].InfoData[2], this.defaultCHexLine[i].InfoData[1], this.defaultCHexLine[i].InfoData[0] };
						baseAddr = BitConverter.ToUInt32(buffer, 0);
						break;

					default:
						return null;
				}
			}
			return _return;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public virtual byte[] GetDataMap(long length)
		{
			//---创建缓存区
			byte[] _return = new byte[length];
			if (_return == null)
			{
				return null;
			}

			return _return;
		}

		#endregion

		#region 保护函数

		#endregion

		#region 私有函数

		#endregion

		#region 事件函数

		#endregion

	}
}
