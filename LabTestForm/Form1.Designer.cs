using System;

namespace LabTestForm
{
	partial class Form1
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			GC.SuppressFinalize(this.usedComm);
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.cRichTextBoxEx1 = new Harry.LabTools.LabControlPlus.CRichTextBoxEx();
            this.cCommBaseControl1 = new Harry.LabTools.LabComm.CCommBaseControl();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(72, 356);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 0;
            // 
            // cRichTextBoxEx1
            // 
            this.cRichTextBoxEx1.Location = new System.Drawing.Point(371, 70);
            this.cRichTextBoxEx1.Name = "cRichTextBoxEx1";
            this.cRichTextBoxEx1.Size = new System.Drawing.Size(305, 293);
            this.cRichTextBoxEx1.TabIndex = 1;
            this.cRichTextBoxEx1.Text = "";
            // 
            // cCommBaseControl1
            // 
            this.cCommBaseControl1.Location = new System.Drawing.Point(12, 12);
            this.cCommBaseControl1.Name = "cCommBaseControl1";
            this.cCommBaseControl1.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.cCommBaseControl1.Size = new System.Drawing.Size(262, 56);
            this.cCommBaseControl1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cCommBaseControl1);
            this.Controls.Add(this.cRichTextBoxEx1);
            this.Controls.Add(this.comboBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox comboBox1;
        private Harry.LabTools.LabControlPlus.CRichTextBoxEx cRichTextBoxEx1;
        private Harry.LabTools.LabComm.CCommBaseControl cCommBaseControl1;
    }
}

