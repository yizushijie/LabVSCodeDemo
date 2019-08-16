using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Harry.LabTools.LabComm
{
	public partial class CCommBase : ICommParam
	{

		#region 公共属性

		/// <summary>
		/// 
		/// </summary>
		public virtual string Name
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual int Index
		{
			get
			{
				return -1;
			}
			set
			{
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual string Info
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual int Timeout
		{
			get
			{
				return 200;
			}
			set
			{
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual bool IsMultiAddr
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual bool IsMultiCMD
		{
			get
			{
				return false;
			}
			set
			{
			}
		}


		/// <summary>
		/// 端口是否打开
		/// </summary>
		public virtual bool IsOpen
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 消息信息
		/// </summary>
		public virtual string LogMessage
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// 通讯状态
		/// </summary>
		public virtual COMM_STATE COMMSTATE
		{
			get
			{
				return COMM_STATE.STATE_IDLE;
			}
		}

		/// <summary>
		/// 使用的时间
		/// </summary>
		public virtual TimeSpan UsedTime
		{
			get
			{
				return new TimeSpan();
			}
		}

        /// <summary>
        /// 设备连接状态
        /// </summary>
        public virtual bool IsConnected
        {
            get
            {
                return false;
            }
        }

		/// <summary>
		/// 设备是否发生变化,TRUE---发生变化，FALSE---未变化
		/// </summary>
		public virtual bool IsChanged
		{
			get
			{
				return false;
			}
			set
			{

			}
		}
        #endregion

        #region 串口属性

        /// <summary>
        /// 串行参数
        /// </summary>
        public virtual CCommSerialParam mSerialParam
		{
			get
			{
				return null;
			}
			set
			{

			}
		}

		#endregion

		#region USB属性

		/// <summary>
		/// USB属性
		/// </summary>
		public virtual CCommUSBParam mUSBParam
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		#endregion

		#region 函数定义

		/// <summary>
		/// 初始化串口参数
		/// </summary>
		/// <param name="serialParam"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int Init(CCommSerialParam serialParam, RichTextBox msg = null)
		{
			return -1;
		}

		/// <summary>
		/// 初始化USB参数
		/// </summary>
		/// <param name="uSBParam"></param>
		/// <param name="msg"></param>
		/// <returns></returns>
		public virtual int Init(CCommUSBParam usbParam, RichTextBox msg = null)
		{
			return -1;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="serialParam"></param>
		/// <param name="uSBParam"></param>
		public virtual void  AnalyseParam( CCommSerialParam serialParam,CCommUSBParam usbParam)
		{
			if ((serialParam!=null)&&(this.mSerialParam!=null))
			{
				this.mSerialParam.mName		=serialParam.mName		;
				this.mSerialParam.mBaudRate	=serialParam.mBaudRate	;
				this.mSerialParam.mStopBits	=serialParam.mStopBits	;
				this.mSerialParam.mDataBits	=serialParam.mDataBits	;
				this.mSerialParam.mParity	=serialParam.mParity	;
			}
			if ((usbParam!=null)&&(this.mUSBParam!=null))
			{
				this.mUSBParam.mVID=mUSBParam.mVID;
				this.mUSBParam.mPID=mUSBParam.mPID;
			}
		}

		#endregion

	}
}
