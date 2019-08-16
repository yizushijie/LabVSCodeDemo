using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Harry.LabTools.LabControlPlus;
using System.Reflection;

namespace Harry.LabTools.LabComm
{
    public partial class CCommBaseControl : UserControl
    {
        #region 变量定义

		/// <summary>
		/// 使用的端口
		/// </summary>
        private CCommBase defaultCCOMM = null;

		/// <summary>
		/// 是否显示端口配置参数
		/// </summary>
		private bool defaultIsShowCommParam=false;
		
        #endregion

        #region 属性定义

        /// <summary>
        /// 通讯端口对象
        /// </summary>
        public virtual CCommBase mCCOMM
        {
            get
            {
                return this.defaultCCOMM;
            }
            set
            {
                if (this.defaultCCOMM==null)
                {
                    this.defaultCCOMM = new CCommBase();
                }
                this.defaultCCOMM = value;
            }
        }

        /// <summary>
        /// 初始化端口的按钮
        /// </summary>
        public virtual Button mButton
        {
            get
            {
                return this.button_COMM;
            }
            set
            {
                this.button_COMM = value;
            }
        }

        /// <summary>
        /// 端口操作状态指示
        /// </summary>
        public virtual PictureBox mPictureBox
        {
            get
            {
                return this.pictureBox_COMM;
            }
            set
            {
                this.pictureBox_COMM = value;
            }
        }
		
		/// <summary>
		/// 是否显示端口配置参数
		/// </summary>
		public virtual bool mIsShowCommParam
		{
			get
			{
				return this.defaultIsShowCommParam;
			}
			set
			{
				this.defaultIsShowCommParam=value;
			}
		}

		/// <summary>
		/// 通讯端口名称
		/// </summary>
		public virtual string mCCommName
		{
			get
			{
				return this.comboBox_COMM.Text;
			}
		}

		/// <summary>
		/// 消息显示的控件
		/// </summary>
		public virtual RichTextBox mCCommRichTextBox
		{
			get
			{
				if (this.defaultCCOMM!=null)
				{
					return this.defaultCCOMM.mCCommRichTextBox;
				}
				else
				{
					return  null;
				}
			}
			set
			{
				if (this.defaultCCOMM!=null)
				{
					this.defaultCCOMM.mCCommRichTextBox=value;
				}
			}
		}

        #endregion

        #region 构造函数

		/// <summary>
		/// 
		/// </summary>
		public  CCommBaseControl()
		{
			 InitializeComponent();
			 this.StartupInit(null);
			this.defaultIsShowCommParam=true;
		}

        /// <summary>
        /// 
        /// </summary>
        public CCommBaseControl(bool isShowCommParam=true)
        {
            InitializeComponent();
            this.StartupInit(null);
			this.defaultIsShowCommParam=isShowCommParam;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ccomm"></param>
        public CCommBaseControl(CCommBase ccomm, RichTextBox msg = null)
        {
            InitializeComponent();
            this.StartupInit(ccomm,msg);
        }

        #endregion

        #region 析构函数

        /// <summary>
        /// 
        /// </summary>
        ~CCommBaseControl()
        {
            this.FreeResource();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void FreeResource()
        {
            if (this.defaultCCOMM != null)
            {
                this.defaultCCOMM.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 公有函数

        /// <summary>
        /// 打开设备
        /// </summary>
        public virtual void OpenDevice()
        {
            if (this.comboBox_COMM.Items.Count>0)
            {
                if (this.defaultCCOMM!=null)
                {
                    if(this.defaultCCOMM.OpenDevice(this.comboBox_COMM.Text, this.defaultCCOMM.mCCommRichTextBox)==0)
                    {
                        this.button_COMM.Text = "关闭设备";
                        this.pictureBox_COMM.Image= Properties.Resources.open;
                        this.defaultCCOMM.EventCCommChange += this.EventDeviceChanged;
                    }
                }
            }
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        public virtual void CloseDevice()
        {
            if (this.defaultCCOMM!=null)
            {
                if (this.defaultCCOMM.IsAttached())
                {
                   if( this.defaultCCOMM.CloseDevice(this.defaultCCOMM.mCCommRichTextBox)==0)
                   {
                        this.button_COMM.Text = "打开设备";
                        this.pictureBox_COMM.Image = Properties.Resources.lost;
                        this.defaultCCOMM.EventCCommChange -= this.EventDeviceChanged;
                    }
                }
            }
        }

        /// <summary>
        /// 配置参数
        /// </summary>
        public virtual void ConfigParam()
        {
			
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public virtual void ErrMessage()
        {
            CMessageBoxPlus.Show(null, "设备操作异常！错误操作：" + this.button_COMM.Text, "错误提示");
        }

        /// <summary>
        /// 设备发生变化的时候
        /// </summary>
        public virtual void EventDeviceChanged()
        {
            if (this.defaultCCOMM!=null)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((EventHandler)
                                 (delegate
                                 {
                                     if ((this.comboBox_COMM.Text == "") || (this.comboBox_COMM.Text == string.Empty) || ((this.defaultCCOMM.Name != this.comboBox_COMM.Text)))
                                     {
										 //if (this.pictureBox_COMM.Image.Flags == Properties.Resources.open.Flags)
										 if (LabGenFunc.CGenFuncBitMap.GenFuncCompareBitMap((Bitmap)this.pictureBox_COMM.Image, Properties.Resources.open) == true)
										 {
                                             //---注销资源
                                             this.defaultCCOMM.Dispose();
                                             this.button_COMM.Text = "打开设备";
                                             this.pictureBox_COMM.Image = Properties.Resources.lost;
                                         }
                                     }
                                 }));
                }
                else
                {
                    if ((this.comboBox_COMM.Text == "") || (this.comboBox_COMM.Text == string.Empty)||((this.defaultCCOMM.Name != this.comboBox_COMM.Text)))
                    {
						//if (this.pictureBox_COMM.Image.Flags == Properties.Resources.open.Flags)
						if (LabGenFunc.CGenFuncBitMap.GenFuncCompareBitMap((Bitmap)this.pictureBox_COMM.Image, Properties.Resources.open) == true)
						{
                            //---注销资源
                            this.defaultCCOMM.Dispose();
                            this.button_COMM.Text = "打开设备";
                            this.pictureBox_COMM.Image = Properties.Resources.lost;
                        }
                    }
                }
            }
        }
		
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ccomm"></param>
        public virtual void Init(CCommBase ccomm = null,RichTextBox msg = null)
        {
            if (ccomm != null)
            {
                if (this.defaultCCOMM == null)
                {
                    this.defaultCCOMM = new CCommBase();
                }
                this.defaultCCOMM = ccomm;

                this.defaultCCOMM.Init(this.comboBox_COMM, msg);
            }
        }

		/// <summary>
		/// 移除端口控件的鼠标按下事件
		/// </summary>
		public virtual void RemoveComboBoxMouseDownClick()
		{
			LabGenFunc.CGenFuncEvent.GenFuncClearAllEvents(this.comboBox_COMM,"MouseDown");
		}

		/// <summary>
		/// 移除按键点击事件
		/// </summary>
		public virtual void RemoveButtonClick()
		{
			LabGenFunc.CGenFuncEvent.GenFuncClearAllEvents(this.button_COMM,"Click");
		}
		
		/// <summary>
		/// 刷新断口 
		/// </summary>
		/// <param name="cbb"></param>
		public virtual void RefreshCOMM(ComboBox cbb)
		{
			//---刷新设备存在的通信端口
			this.comboBox_COMM.Items.Clear();
			for (int i = 0; i < cbb.Items.Count; i++)
			{
				this.comboBox_COMM.Items.Add(cbb.Items[i]);
			}
			this.comboBox_COMM.SelectedIndex = cbb.SelectedIndex;
		}
		
		#endregion

		#region 保护函数

		#endregion

		#region 私有函数

		/// <summary>
		/// 启动初始化
		/// </summary>
		/// <param name="ccomm"></param>
		/// <param name="msg"></param>
		private void StartupInit(CCommBase ccomm=null, RichTextBox msg = null)
        {
            //---button 点击事件
            this.button_COMM.Click += new System.EventHandler(this.Button_Click);
            //---端口状态刷新事件
            this.pictureBox_COMM.Click += new EventHandler(this.PictureBox_Click);

			//---增加鼠标移动事件
			this.comboBox_COMM.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ComboBoxMouseDown_Click);

			if (ccomm!=null)
            {
                if (this.defaultCCOMM==null)
                {
                    this.defaultCCOMM = new CCommBase();
                }
                this.defaultCCOMM = ccomm;

                this.defaultCCOMM.Init(this.comboBox_COMM, msg);
            }
        }

		/// <summary>
		/// 配置通信端口的参数
		/// </summary>
		private void ConfigCOMMSerialPortParam()
		{
			if ((this.comboBox_COMM.Text != null) && (this.comboBox_COMM.Items.Count > 0))
			{
				CCommBaseForm p=null;
				//---检查对象类型
				if (this.defaultCCOMM.GetType()==typeof(CCommSerial))
				{
					p=new CCommSerialPlusForm(this.comboBox_COMM,this.defaultCCOMM,"配置设备" ,true );
				}
				else if(this.defaultCCOMM.GetType()==typeof(CCommSerial))
				{
					p=new CCommUSBPlusForm(); 
				}
				//---判断对象是否可用
				if (p!=null)
				{
					if (p.ShowDialog(this.comboBox_COMM, 0, this.comboBox_COMM.Height) == System.Windows.Forms.DialogResult.OK)
					{
						this.defaultCCOMM.AnalyseParam(p.mCCommSrialParam, p.mCCommUSBParam);
						//---检查端口是否发生变化
						if (p.mCCommChanged == true)
						{
							if (this.defaultCCOMM != null)
							{
								//---刷新设备
								this.defaultCCOMM.RefreshDevice(this.comboBox_COMM, this.defaultCCOMM.mCCommRichTextBox);
							}
						}
						if (this.defaultCCOMM.mCCommRichTextBox != null)
						{
							CRichTextBoxPlus.AppendTextInfoTopWithDataTime(this.defaultCCOMM.mCCommRichTextBox, "通信端口参数配置成功。\r\n", Color.Black, false);
						}
					}
					else
					{
						if (this.defaultCCOMM.mCCommRichTextBox != null)
						{
							CRichTextBoxPlus.AppendTextInfoTopWithDataTime(this.defaultCCOMM.mCCommRichTextBox, "通信端口参数配置失败。\r\n", Color.Red, false);
						}
					}
					//---释放资源
					p.FreeResource();
				}

			}
		}

		#endregion

		#region 事件函数

		/// <summary>
		/// 按钮点击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void Button_Click(object sender, System.EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;
            switch (btn.Name)
            {
                case "button_COMM":
                    if (btn.Text == "打开设备")
                    {
                        this.OpenDevice();
                    }
                    else if (btn.Text == "关闭设备")
                    {
                        this.CloseDevice();
                    }
                    else if (btn.Text == "配置设备")
                    {
                        this.ConfigParam();
                    }
                    else
                    {
                        this.ErrMessage();
                    }
                    break;

                default:
                    break;
            }
            btn.Enabled = true;
        }

        /// <summary>
		/// 图片点击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void PictureBox_Click(object sender, System.EventArgs e)
        {
            PictureBox ptb = (PictureBox)sender;
            ptb.Enabled = false;
            switch (ptb.Name)
            {
                case "pictureBox_COMM":
                    if ((this.button_COMM.Text == "打开设备")||(this.button_COMM.Text == "配置设备"))
                    {
                        if (this.defaultCCOMM != null)
                        {
                            //---刷新设备
                            this.defaultCCOMM.RefreshDevice(this.comboBox_COMM,this.defaultCCOMM.mCCommRichTextBox);
                        }
                    }
                    break;
                default:
                    break;
            }
            ptb.Enabled = true;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public virtual void ComboBoxMouseDown_Click(object sender, MouseEventArgs e)
		{
			ComboBox cbb = (ComboBox)sender;
			int index;
			switch (cbb.Name)
			{
				case "comboBox_COMM":
					//---判断鼠标按下的按键
					if ((e.Button == MouseButtons.Right) && (this.defaultIsShowCommParam == true))
					{
						if (LabGenFunc.CGenFuncBitMap.GenFuncCompareBitMap((Bitmap)this.pictureBox_COMM.Image, Properties.Resources.open)!=true)
						{
							//---鼠标右键配置通信端口的参数
							this.ConfigCOMMSerialPortParam();
							//---判断是否端口发生了变化
							if (((this.comboBox_COMM.Text != string.Empty) || this.comboBox_COMM.Text != "") && ((this.defaultCCOMM.Name != string.Empty) || (this.defaultCCOMM.Name != "")) && (this.defaultCCOMM.Name != this.comboBox_COMM.Text))
							{
								//---数据位
								index = this.comboBox_COMM.Items.IndexOf(this.defaultCCOMM.Name);
								if (index < 0)
								{
									this.comboBox_COMM.SelectedIndex = 0;
								}
								else
								{
									this.comboBox_COMM.SelectedIndex = index;
								}
							}
						}
					}
					break;
				default:
					break;
			}
		}

		#endregion
	}
}
