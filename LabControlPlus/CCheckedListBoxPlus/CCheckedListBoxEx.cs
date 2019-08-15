using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Harry.LabTools.LabControlPlus
{
	/// <summary>
	///
	/// </summary>
	public class CCheckedListBoxEx : CheckedListBox
	{
		#region 绘制函数

		/// <summary>
		/// 重新绘制选中是的背景为透明色
		/// </summary>
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			//Color.Black --- 为字体颜色
			DrawItemEventArgs e2 = new DrawItemEventArgs(e.Graphics, e.Font, new Rectangle(e.Bounds.Location, e.Bounds.Size), e.Index, (e.State & DrawItemState.Focus) == DrawItemState.Focus ? DrawItemState.Focus : DrawItemState.None, Color.Black, this.BackColor);
			base.OnDrawItem(e2);
		}

		#endregion 绘制函数
	}
}
