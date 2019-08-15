using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Harry.LabTools.LabComm
{ 
    public partial class CCommSerialPlusForm : CCommBaseForm
    {
        #region 变量定义
		
        #endregion

        #region 属性定义

		/// <summary>
		/// 
		/// </summary>
		public override CCommSerialParam mCCommSrialParam
		{
			get
			{
				return new CCommSerialParam(this.cCommSerialPort.mCCommName,this.cCommSerialPort.mBaudRate,this.cCommSerialPort.mStopBits,this.cCommSerialPort.mDataBits,this.cCommSerialPort.mParity);
			}
		}
        
        #endregion

        #region 构造函数

		/// <summary>
		/// 
		/// </summary>
		public CCommSerialPlusForm():base()
		{
			 InitializeComponent();
		}

        /// <summary>
        /// 
        /// </summary>
        public CCommSerialPlusForm(bool isLimitedSize=false)
        {
            InitializeComponent();
            //---判断是否限定最小尺寸
            if (isLimitedSize)
            {
                //---限制窗体的大小
                this.MinimumSize = this.Size;
                this.MaximumSize = this.Size;
            }          
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="isLimitedSize"></param>
		public CCommSerialPlusForm( CCommBase cComm,string text=null,bool isLimitedSize = false)
		{
			InitializeComponent();

			if ((text!="")&&(text!=string.Empty))
			{
				this.cCommSerialPort.mButton.Text=text;
			}

			//---判断是否限定最小尺寸
			if (isLimitedSize)
			{
				//---限制窗体的大小
				this.MinimumSize = this.Size;
				this.MaximumSize = this.Size;
			}
			this.Init( cComm);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="isLimitedSize"></param>
		public CCommSerialPlusForm(ComboBox cbb, CCommBase cComm,string text=null,bool isLimitedSize = false)
		{
			InitializeComponent();

			if ((text!="")&&(text!=string.Empty))
			{
				this.cCommSerialPort.mButton.Text=text;
			}

			//---判断是否限定最小尺寸
			if (isLimitedSize)
			{
				//---限制窗体的大小
				this.MinimumSize = this.Size;
				this.MaximumSize = this.Size;
			}
			this.Init(cbb, cComm);
		}

		#endregion

		#region 析构函数

		/// <summary>
		/// 
		/// </summary>
		~CCommSerialPlusForm()
        {
            this.FreeResource();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void FreeResource()
        {
            base.FreeResource();
            if (this.cCommSerialPort!=null)
            {
                GC.SuppressFinalize(this.cCommSerialPort);
            }
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 公有函数

		
        #endregion

        #region 保护函数

        #endregion

        #region 私有函数

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cComm"></param>
		private void Init( CCommBase cComm)
		{
			if (cComm!=null)
			{
				this.cCommSerialPort.RemoveComboBoxMouseDownClick();
				this.cCommSerialPort.RemoveButtonClick();

				//---加载按钮事件
				this.cCommSerialPort.mButton.Click+=new EventHandler(this.ShowParamDialog_Click);
			}
			else
			{
				this.DialogResult=DialogResult.None;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cbb"></param>
		/// <param name="cComm"></param>
		private void Init(ComboBox cbb, CCommBase cComm)
		{
			if (cComm!=null)
			{
				this.cCommSerialPort.RemoveComboBoxMouseDownClick();
				this.cCommSerialPort.RemoveButtonClick();
				this.cCommSerialPort.RefreshCOMM(cbb);

				//---加载按钮事件
				this.cCommSerialPort.mButton.Click+=new EventHandler(this.ShowParamDialog_Click);		
				//---波特率
				this.cCommSerialPort.AnalyseBaudRate(cComm.mSerialParam.mBaudRate);
				//---数据位
				this.cCommSerialPort.AnalyseDataBits(cComm.mSerialParam.mDataBits);
				//---停止位
				this.cCommSerialPort.AnalyseStopBits(cComm.mSerialParam.mStopBits);
				//---校验位
				this.cCommSerialPort.AnalyseParity(cComm.mSerialParam.mParity);
			}
			else
			{
				this.DialogResult = DialogResult.None;
			}
		}
	
        #endregion

        #region 事件函数

        /// <summary>
        /// 按键点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void ShowParamDialog_Click(object sender, System.EventArgs e)
        {
            //---返回操作完成状态
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        #endregion
    }
}
