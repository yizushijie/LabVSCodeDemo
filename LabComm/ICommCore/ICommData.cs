using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harry.LabTools.LabComm
{
	/// <summary>
	/// CRC校验方式
	/// </summary>
	public enum COMM_CRC : byte
	{
		CRC_NONE = 0,           //---无校验方式
		CRC_CHECKSUM = 1,           //---检验和
		CRC_CRC8 = 2,           //---CRC8校验
		CRC_CRC16 = 3,          //---CRC16校验
		CRC_CRC32 = 4,          //---CRC32校验
	};

	/// <summary>
	/// 
	/// </summary>
	public class CCommData
	{
		#region 变量定义

		/// <summary>
		/// 数据
		/// </summary>
		public List<byte> mByte = null;

		/// <summary>
		/// CRC的结果
		/// </summary>
		public UInt32 mCRCResult = 0;

		/// <summary>
		/// 数据的长度
		/// </summary>
		public int mLength = 0;

		/// <summary>
		/// CRC的方式
		/// </summary>
		public COMM_CRC mCRCMode = COMM_CRC.CRC_NONE;

		/// <summary>
		/// 父命令
		/// </summary>
		public byte mParentCMD = 0;

		/// <summary>
		/// 子命令
		/// </summary>
		public byte mChildCMD = 0;

		/// <summary>
		/// 数据发送的长度
		/// </summary>
		public int mSize = 64;

		/// <summary>
		/// 数据报头信息
		/// </summary>
		public int mID = 0x00;

		/// <summary>
		/// 结果标志位
		/// </summary>
		public int mFlagResult = -1;

		#endregion

		#region 属性定义

		#endregion

		#region 构造函数

		/// <summary>
		/// 构造函数
		/// </summary>
		public CCommData()
		{

		}

		/// <summary>
		/// 构造函数
		/// </summary>
		public CCommData(int val,bool isID=true)
		{
			if (isID)
			{
				this.mID = val;
			}
			else
			{
				this.mSize = val;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="size"></param>
		public CCommData(int id,int size)
		{
			this.mID = id;
			this.mSize = size;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="size"></param>
		/// <param name="crcMode"></param>
		public CCommData(int id, int size,COMM_CRC crcMode)
		{
			this.mID = id;
			this.mSize = size;
			this.mCRCMode = crcMode;
		}

		#endregion

		#region 析构函数

		/// <summary>
		/// 析构函数
		/// </summary>
		~CCommData()
		{
			//---垃圾回收
			GC.SuppressFinalize(this);
		}

		#endregion

		#region 公有函数

		#endregion

		#region 私有函数

		#endregion

		#region 事件函数

		#endregion
	}

	/// <summary>
	/// 通讯接口的数据函数
	/// </summary>
	interface ICommData
	{
		#region 属性定义

		/// <summary>
		/// 接收数据
		/// </summary>
		CCommData ReceData
		{
			get;
			set;
		}


		/// <summary>
		/// 发送数据
		/// </summary>
		CCommData SendData
		{
			get;
			set;
		}

		/// <summary>
		/// 接收校验通过
		/// </summary>
		bool ReceCheckPass
		{
			get;
		}

		#endregion
		

		#region 函数定义

		/// <summary>
		/// 分析接收数据
		/// </summary>
		/// <returns></returns>
		bool AnalyseReceData(byte[] cmd);

		/// <summary>
		/// 解析发送数据
		/// </summary>
		/// <param name="cmd">发送命令</param>
		/// <returns></returns>
		bool AnalyseSendData(byte[]cmd);

		#endregion



	}
}
