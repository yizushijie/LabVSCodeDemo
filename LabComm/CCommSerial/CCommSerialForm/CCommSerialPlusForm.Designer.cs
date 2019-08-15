namespace Harry.LabTools.LabComm
{
    partial class CCommSerialPlusForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.cCommSerialPort = new Harry.LabTools.LabComm.CCommSerialPlusControl();
			this.SuspendLayout();
			// 
			// cCommSerialPort
			// 
			this.cCommSerialPort.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cCommSerialPort.Location = new System.Drawing.Point(0, 0);
			this.cCommSerialPort.MaximumSize = new System.Drawing.Size(151, 189);
			this.cCommSerialPort.mCCOMM = null;
			this.cCommSerialPort.MinimumSize = new System.Drawing.Size(151, 189);
			this.cCommSerialPort.mIsShowCommParam = true;
			this.cCommSerialPort.Name = "cCommSerialPort";
			this.cCommSerialPort.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
			this.cCommSerialPort.Size = new System.Drawing.Size(151, 189);
			this.cCommSerialPort.TabIndex = 0;
			// 
			// CCommSerialPlusForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(156, 194);
			this.Controls.Add(this.cCommSerialPort);
			this.DoubleBuffered = true;
			this.Name = "CCommSerialPlusForm";
			this.Text = "CCommSerialPlusForm";
			this.ResumeLayout(false);

        }

        #endregion

        private CCommSerialPlusControl cCommSerialPort;
    }
}