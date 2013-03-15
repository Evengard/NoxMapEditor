using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net.Mail;
using System.IO;

namespace NoxShared
{
	/// <summary>
	/// Summary description for ExceptionDialog.
	/// </summary>
	public class ExceptionDialog : System.Windows.Forms.Form
	{
		private const string defaultFrom = "user@domain";
		private const string defaultTo = "MyEnemyMyFriend@hotmail.com";

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox boxEmailTo;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button buttonSend;
		private System.Windows.Forms.TextBox boxFrom;
		private System.Windows.Forms.TextBox boxMessage;
		private System.Windows.Forms.TextBox boxNotes;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonCancel;

		public ExceptionDialog(Exception ex)
		{
			InitializeComponent();

			//fill the text box
			ArrayList text = new ArrayList();
			text.Add("Version: " + Application.ProductVersion);
			text.Add("");
			text.Add(ex.Message);
			text.Add(ex.StackTrace);
			text.Add("");
			boxMessage.Lines = (string[]) text.ToArray(typeof(string));
			boxMessage.Select(boxMessage.Text.Length, 0);

			//use default email addresses
			boxFrom.Text = defaultFrom;
			boxEmailTo.Text = defaultTo;

			//save the message to disk
			StreamWriter wtr = new StreamWriter("CrashLog.txt");
			wtr.Write(ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n\r\n");
			if (ex.InnerException != null)
				wtr.Write(ex.InnerException.Message + "\r\n" + ex.InnerException.Source + "\r\n" + ex.InnerException.StackTrace + "\r\n\r\n");
			wtr.Close();
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ExceptionDialog));
			this.boxMessage = new System.Windows.Forms.TextBox();
			this.buttonSend = new System.Windows.Forms.Button();
			this.boxEmailTo = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.boxFrom = new System.Windows.Forms.TextBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.boxNotes = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// boxMessage
			// 
			this.boxMessage.AccessibleDescription = resources.GetString("boxMessage.AccessibleDescription");
			this.boxMessage.AccessibleName = resources.GetString("boxMessage.AccessibleName");
			this.boxMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("boxMessage.Anchor")));
			this.boxMessage.AutoSize = ((bool)(resources.GetObject("boxMessage.AutoSize")));
			this.boxMessage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("boxMessage.BackgroundImage")));
			this.boxMessage.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("boxMessage.Dock")));
			this.boxMessage.Enabled = ((bool)(resources.GetObject("boxMessage.Enabled")));
			this.boxMessage.Font = ((System.Drawing.Font)(resources.GetObject("boxMessage.Font")));
			this.boxMessage.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("boxMessage.ImeMode")));
			this.boxMessage.Location = ((System.Drawing.Point)(resources.GetObject("boxMessage.Location")));
			this.boxMessage.MaxLength = ((int)(resources.GetObject("boxMessage.MaxLength")));
			this.boxMessage.Multiline = ((bool)(resources.GetObject("boxMessage.Multiline")));
			this.boxMessage.Name = "boxMessage";
			this.boxMessage.PasswordChar = ((char)(resources.GetObject("boxMessage.PasswordChar")));
			this.boxMessage.ReadOnly = true;
			this.boxMessage.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("boxMessage.RightToLeft")));
			this.boxMessage.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("boxMessage.ScrollBars")));
			this.boxMessage.Size = ((System.Drawing.Size)(resources.GetObject("boxMessage.Size")));
			this.boxMessage.TabIndex = ((int)(resources.GetObject("boxMessage.TabIndex")));
			this.boxMessage.Text = resources.GetString("boxMessage.Text");
			this.boxMessage.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("boxMessage.TextAlign")));
			this.boxMessage.Visible = ((bool)(resources.GetObject("boxMessage.Visible")));
			this.boxMessage.WordWrap = ((bool)(resources.GetObject("boxMessage.WordWrap")));
			// 
			// buttonSend
			// 
			this.buttonSend.AccessibleDescription = resources.GetString("buttonSend.AccessibleDescription");
			this.buttonSend.AccessibleName = resources.GetString("buttonSend.AccessibleName");
			this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("buttonSend.Anchor")));
			this.buttonSend.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonSend.BackgroundImage")));
			this.buttonSend.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("buttonSend.Dock")));
			this.buttonSend.Enabled = ((bool)(resources.GetObject("buttonSend.Enabled")));
			this.buttonSend.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("buttonSend.FlatStyle")));
			this.buttonSend.Font = ((System.Drawing.Font)(resources.GetObject("buttonSend.Font")));
			this.buttonSend.Image = ((System.Drawing.Image)(resources.GetObject("buttonSend.Image")));
			this.buttonSend.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("buttonSend.ImageAlign")));
			this.buttonSend.ImageIndex = ((int)(resources.GetObject("buttonSend.ImageIndex")));
			this.buttonSend.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("buttonSend.ImeMode")));
			this.buttonSend.Location = ((System.Drawing.Point)(resources.GetObject("buttonSend.Location")));
			this.buttonSend.Name = "buttonSend";
			this.buttonSend.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("buttonSend.RightToLeft")));
			this.buttonSend.Size = ((System.Drawing.Size)(resources.GetObject("buttonSend.Size")));
			this.buttonSend.TabIndex = ((int)(resources.GetObject("buttonSend.TabIndex")));
			this.buttonSend.Text = resources.GetString("buttonSend.Text");
			this.buttonSend.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("buttonSend.TextAlign")));
			this.buttonSend.Visible = ((bool)(resources.GetObject("buttonSend.Visible")));
			this.buttonSend.Click += new System.EventHandler(this.button1_Click);
			// 
			// boxEmailTo
			// 
			this.boxEmailTo.AccessibleDescription = resources.GetString("boxEmailTo.AccessibleDescription");
			this.boxEmailTo.AccessibleName = resources.GetString("boxEmailTo.AccessibleName");
			this.boxEmailTo.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("boxEmailTo.Anchor")));
			this.boxEmailTo.AutoSize = ((bool)(resources.GetObject("boxEmailTo.AutoSize")));
			this.boxEmailTo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("boxEmailTo.BackgroundImage")));
			this.boxEmailTo.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("boxEmailTo.Dock")));
			this.boxEmailTo.Enabled = ((bool)(resources.GetObject("boxEmailTo.Enabled")));
			this.boxEmailTo.Font = ((System.Drawing.Font)(resources.GetObject("boxEmailTo.Font")));
			this.boxEmailTo.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("boxEmailTo.ImeMode")));
			this.boxEmailTo.Location = ((System.Drawing.Point)(resources.GetObject("boxEmailTo.Location")));
			this.boxEmailTo.MaxLength = ((int)(resources.GetObject("boxEmailTo.MaxLength")));
			this.boxEmailTo.Multiline = ((bool)(resources.GetObject("boxEmailTo.Multiline")));
			this.boxEmailTo.Name = "boxEmailTo";
			this.boxEmailTo.PasswordChar = ((char)(resources.GetObject("boxEmailTo.PasswordChar")));
			this.boxEmailTo.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("boxEmailTo.RightToLeft")));
			this.boxEmailTo.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("boxEmailTo.ScrollBars")));
			this.boxEmailTo.Size = ((System.Drawing.Size)(resources.GetObject("boxEmailTo.Size")));
			this.boxEmailTo.TabIndex = ((int)(resources.GetObject("boxEmailTo.TabIndex")));
			this.boxEmailTo.Text = resources.GetString("boxEmailTo.Text");
			this.boxEmailTo.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("boxEmailTo.TextAlign")));
			this.boxEmailTo.Visible = ((bool)(resources.GetObject("boxEmailTo.Visible")));
			this.boxEmailTo.WordWrap = ((bool)(resources.GetObject("boxEmailTo.WordWrap")));
			// 
			// label1
			// 
			this.label1.AccessibleDescription = resources.GetString("label1.AccessibleDescription");
			this.label1.AccessibleName = resources.GetString("label1.AccessibleName");
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label1.Anchor")));
			this.label1.AutoSize = ((bool)(resources.GetObject("label1.AutoSize")));
			this.label1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label1.Dock")));
			this.label1.Enabled = ((bool)(resources.GetObject("label1.Enabled")));
			this.label1.Font = ((System.Drawing.Font)(resources.GetObject("label1.Font")));
			this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
			this.label1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.ImageAlign")));
			this.label1.ImageIndex = ((int)(resources.GetObject("label1.ImageIndex")));
			this.label1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label1.ImeMode")));
			this.label1.Location = ((System.Drawing.Point)(resources.GetObject("label1.Location")));
			this.label1.Name = "label1";
			this.label1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label1.RightToLeft")));
			this.label1.Size = ((System.Drawing.Size)(resources.GetObject("label1.Size")));
			this.label1.TabIndex = ((int)(resources.GetObject("label1.TabIndex")));
			this.label1.Text = resources.GetString("label1.Text");
			this.label1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.TextAlign")));
			this.label1.Visible = ((bool)(resources.GetObject("label1.Visible")));
			// 
			// label3
			// 
			this.label3.AccessibleDescription = resources.GetString("label3.AccessibleDescription");
			this.label3.AccessibleName = resources.GetString("label3.AccessibleName");
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label3.Anchor")));
			this.label3.AutoSize = ((bool)(resources.GetObject("label3.AutoSize")));
			this.label3.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label3.Dock")));
			this.label3.Enabled = ((bool)(resources.GetObject("label3.Enabled")));
			this.label3.Font = ((System.Drawing.Font)(resources.GetObject("label3.Font")));
			this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
			this.label3.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label3.ImageAlign")));
			this.label3.ImageIndex = ((int)(resources.GetObject("label3.ImageIndex")));
			this.label3.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label3.ImeMode")));
			this.label3.Location = ((System.Drawing.Point)(resources.GetObject("label3.Location")));
			this.label3.Name = "label3";
			this.label3.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label3.RightToLeft")));
			this.label3.Size = ((System.Drawing.Size)(resources.GetObject("label3.Size")));
			this.label3.TabIndex = ((int)(resources.GetObject("label3.TabIndex")));
			this.label3.Text = resources.GetString("label3.Text");
			this.label3.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label3.TextAlign")));
			this.label3.Visible = ((bool)(resources.GetObject("label3.Visible")));
			// 
			// boxFrom
			// 
			this.boxFrom.AccessibleDescription = resources.GetString("boxFrom.AccessibleDescription");
			this.boxFrom.AccessibleName = resources.GetString("boxFrom.AccessibleName");
			this.boxFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("boxFrom.Anchor")));
			this.boxFrom.AutoSize = ((bool)(resources.GetObject("boxFrom.AutoSize")));
			this.boxFrom.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("boxFrom.BackgroundImage")));
			this.boxFrom.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("boxFrom.Dock")));
			this.boxFrom.Enabled = ((bool)(resources.GetObject("boxFrom.Enabled")));
			this.boxFrom.Font = ((System.Drawing.Font)(resources.GetObject("boxFrom.Font")));
			this.boxFrom.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("boxFrom.ImeMode")));
			this.boxFrom.Location = ((System.Drawing.Point)(resources.GetObject("boxFrom.Location")));
			this.boxFrom.MaxLength = ((int)(resources.GetObject("boxFrom.MaxLength")));
			this.boxFrom.Multiline = ((bool)(resources.GetObject("boxFrom.Multiline")));
			this.boxFrom.Name = "boxFrom";
			this.boxFrom.PasswordChar = ((char)(resources.GetObject("boxFrom.PasswordChar")));
			this.boxFrom.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("boxFrom.RightToLeft")));
			this.boxFrom.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("boxFrom.ScrollBars")));
			this.boxFrom.Size = ((System.Drawing.Size)(resources.GetObject("boxFrom.Size")));
			this.boxFrom.TabIndex = ((int)(resources.GetObject("boxFrom.TabIndex")));
			this.boxFrom.Text = resources.GetString("boxFrom.Text");
			this.boxFrom.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("boxFrom.TextAlign")));
			this.boxFrom.Visible = ((bool)(resources.GetObject("boxFrom.Visible")));
			this.boxFrom.WordWrap = ((bool)(resources.GetObject("boxFrom.WordWrap")));
			// 
			// buttonCancel
			// 
			this.buttonCancel.AccessibleDescription = resources.GetString("buttonCancel.AccessibleDescription");
			this.buttonCancel.AccessibleName = resources.GetString("buttonCancel.AccessibleName");
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("buttonCancel.Anchor")));
			this.buttonCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonCancel.BackgroundImage")));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("buttonCancel.Dock")));
			this.buttonCancel.Enabled = ((bool)(resources.GetObject("buttonCancel.Enabled")));
			this.buttonCancel.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("buttonCancel.FlatStyle")));
			this.buttonCancel.Font = ((System.Drawing.Font)(resources.GetObject("buttonCancel.Font")));
			this.buttonCancel.Image = ((System.Drawing.Image)(resources.GetObject("buttonCancel.Image")));
			this.buttonCancel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("buttonCancel.ImageAlign")));
			this.buttonCancel.ImageIndex = ((int)(resources.GetObject("buttonCancel.ImageIndex")));
			this.buttonCancel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("buttonCancel.ImeMode")));
			this.buttonCancel.Location = ((System.Drawing.Point)(resources.GetObject("buttonCancel.Location")));
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("buttonCancel.RightToLeft")));
			this.buttonCancel.Size = ((System.Drawing.Size)(resources.GetObject("buttonCancel.Size")));
			this.buttonCancel.TabIndex = ((int)(resources.GetObject("buttonCancel.TabIndex")));
			this.buttonCancel.Text = resources.GetString("buttonCancel.Text");
			this.buttonCancel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("buttonCancel.TextAlign")));
			this.buttonCancel.Visible = ((bool)(resources.GetObject("buttonCancel.Visible")));
			// 
			// boxNotes
			// 
			this.boxNotes.AccessibleDescription = resources.GetString("boxNotes.AccessibleDescription");
			this.boxNotes.AccessibleName = resources.GetString("boxNotes.AccessibleName");
			this.boxNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("boxNotes.Anchor")));
			this.boxNotes.AutoSize = ((bool)(resources.GetObject("boxNotes.AutoSize")));
			this.boxNotes.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("boxNotes.BackgroundImage")));
			this.boxNotes.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("boxNotes.Dock")));
			this.boxNotes.Enabled = ((bool)(resources.GetObject("boxNotes.Enabled")));
			this.boxNotes.Font = ((System.Drawing.Font)(resources.GetObject("boxNotes.Font")));
			this.boxNotes.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("boxNotes.ImeMode")));
			this.boxNotes.Location = ((System.Drawing.Point)(resources.GetObject("boxNotes.Location")));
			this.boxNotes.MaxLength = ((int)(resources.GetObject("boxNotes.MaxLength")));
			this.boxNotes.Multiline = ((bool)(resources.GetObject("boxNotes.Multiline")));
			this.boxNotes.Name = "boxNotes";
			this.boxNotes.PasswordChar = ((char)(resources.GetObject("boxNotes.PasswordChar")));
			this.boxNotes.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("boxNotes.RightToLeft")));
			this.boxNotes.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("boxNotes.ScrollBars")));
			this.boxNotes.Size = ((System.Drawing.Size)(resources.GetObject("boxNotes.Size")));
			this.boxNotes.TabIndex = ((int)(resources.GetObject("boxNotes.TabIndex")));
			this.boxNotes.Text = resources.GetString("boxNotes.Text");
			this.boxNotes.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("boxNotes.TextAlign")));
			this.boxNotes.Visible = ((bool)(resources.GetObject("boxNotes.Visible")));
			this.boxNotes.WordWrap = ((bool)(resources.GetObject("boxNotes.WordWrap")));
			// 
			// label2
			// 
			this.label2.AccessibleDescription = resources.GetString("label2.AccessibleDescription");
			this.label2.AccessibleName = resources.GetString("label2.AccessibleName");
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label2.Anchor")));
			this.label2.AutoSize = ((bool)(resources.GetObject("label2.AutoSize")));
			this.label2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label2.Dock")));
			this.label2.Enabled = ((bool)(resources.GetObject("label2.Enabled")));
			this.label2.Font = ((System.Drawing.Font)(resources.GetObject("label2.Font")));
			this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
			this.label2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.ImageAlign")));
			this.label2.ImageIndex = ((int)(resources.GetObject("label2.ImageIndex")));
			this.label2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label2.ImeMode")));
			this.label2.Location = ((System.Drawing.Point)(resources.GetObject("label2.Location")));
			this.label2.Name = "label2";
			this.label2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label2.RightToLeft")));
			this.label2.Size = ((System.Drawing.Size)(resources.GetObject("label2.Size")));
			this.label2.TabIndex = ((int)(resources.GetObject("label2.TabIndex")));
			this.label2.Text = resources.GetString("label2.Text");
			this.label2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.TextAlign")));
			this.label2.Visible = ((bool)(resources.GetObject("label2.Visible")));
			// 
			// ExceptionDialog
			// 
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.CancelButton = this.buttonCancel;
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.Add(this.label2);
			this.Controls.Add(this.boxNotes);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.boxFrom);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.boxEmailTo);
			this.Controls.Add(this.buttonSend);
			this.Controls.Add(this.boxMessage);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "ExceptionDialog";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			if (boxFrom.Text == defaultFrom || boxFrom.Text.Split('@').Length != 2)
			{
				MessageBox.Show("Please enter your email address.");
				return;
			}

			Hide();

			MailMessage msg = new MailMessage();
			msg.From = new MailAddress(boxFrom.Text);
			msg.To.Add(boxEmailTo.Text);
			msg.Subject = "NoxMapEditor Crash Report";
			msg.Body = boxMessage.Text + (boxNotes.Text == "" ? "" : "\n\nNotes:\n" + boxNotes.Text);

			bool sent = false;
			foreach (string server in DnsLib.DnsApi.GetMXRecords(boxEmailTo.Text.Split('@')[1]))
			{
                SmtpClient smtpClient = new SmtpClient(server);
				try
				{
                    smtpClient.Send(msg);
					sent = true;
					break;
				}
				catch (Exception) {}
			}
			if (!sent)
				MessageBox.Show("Couldn't send mail message.");
		}
	}
}
