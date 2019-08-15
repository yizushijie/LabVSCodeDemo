using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Harry.LabTools.LabComm
{
    public partial class CCommSerialControl : CCommBaseControl
    {
        #region 变量定义

        #endregion

        #region 属性定义

        /// <summary>
        /// 端口属性
        /// </summary>
        public override CCommBase mCCOMM
        {
            get
            {
                return base.mCCOMM;
            }
            set
            {
                base.mCCOMM = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override Button mButton
        {
            get
            {
                return base.mButton;
            }
            set
            {
                base.mButton = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override PictureBox mPictureBox
        {
            get
            {
                return base.mPictureBox;
            }
            set
            {
                base.mPictureBox = value;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		public override bool mIsShowCommParam
		{
			get 
			{
				return	base.mIsShowCommParam;
			}
			set 
			{
				base.mIsShowCommParam = value;
			}
		}

		#endregion

		#region 构造函数

		/// <summary>
		/// 
		/// </summary>
		public CCommSerialControl()
        {
            InitializeComponent();

            //---限制窗体的大小
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
        }

        #endregion

        #region 析构函数

        ~CCommSerialControl()
        {
            this.FreeResource();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void FreeResource()
        {
            base.FreeResource();
        }

        #endregion

        #region 公有函数

        #endregion

        #region 保护函数

        #endregion

        #region 私有函数

        #endregion

        #region 事件函数

        #endregion
    }
}
