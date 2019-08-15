using Harry.LabTools.LabComm;
using System;
using System.Windows.Forms;

namespace LabTestForm
{
	public partial class Form1 : Form
	{

		private CCommBase usedComm = null;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			this.usedComm = new CCommSerial();
            this.cCommBaseControl1.Init(this.usedComm,this.cRichTextBoxEx1);
		}
	}
}
