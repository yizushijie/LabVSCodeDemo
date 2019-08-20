using Harry.LabTools.LabGenForm;
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
    public partial class CCommBaseForm : FloatPopupBaseForm
    {
        #region 变量定义

        #endregion

        #region 属性定义
		/// <summary>
		/// 
		/// </summary>
		public virtual CCommSerialParam mCCommSrialParam
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual CCommUSBParam mCCommUSBParam
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual bool mCCommChanged
		{
			get
			{
				return false;
			}
		}


		#endregion

		#region 构造函数

		/// <summary>
		/// 
		/// </summary>
		public CCommBaseForm()
        {
            InitializeComponent();
        }

        #endregion

        #region 析构函数

        /// <summary>
        /// 
        /// </summary>
        ~CCommBaseForm()
        {
            this.FreeResource();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void FreeResource()
        {
			GC.SuppressFinalize(this);
        }

        #endregion

        #region 公有函数

		
        #endregion

        #region 保护函数

        #endregion

        #region 私有函数

        #endregion

        #region 事件函数

		/// <summary>
		/// 
		/// </summary>
		public virtual void ParamShowDialog_Click(object sender, System.EventArgs e)
        {
            //---返回操作完成状态
            this.DialogResult = System.Windows.Forms.DialogResult.None;
        }

		#endregion
    }
}
