
namespace Chat_Client
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.NameInputBox = new System.Windows.Forms.TextBox();
            this.ChatName = new System.Windows.Forms.Label();
            this.InputMsgBox = new System.Windows.Forms.TextBox();
            this.BtnLogin = new System.Windows.Forms.Button();
            this.OutputMsgBox = new System.Windows.Forms.TextBox();
            this.BtnSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NameInputBox
            // 
            this.NameInputBox.Location = new System.Drawing.Point(412, 12);
            this.NameInputBox.Name = "NameInputBox";
            this.NameInputBox.Size = new System.Drawing.Size(423, 21);
            this.NameInputBox.TabIndex = 0;
            // 
            // ChatName
            // 
            this.ChatName.AutoSize = true;
            this.ChatName.Font = new System.Drawing.Font("Noto Sans CJK KR Regular", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ChatName.Location = new System.Drawing.Point(331, 13);
            this.ChatName.Name = "ChatName";
            this.ChatName.Size = new System.Drawing.Size(75, 19);
            this.ChatName.TabIndex = 1;
            this.ChatName.Text = "Chat Name";
            this.ChatName.UseMnemonic = false;
            this.ChatName.Click += new System.EventHandler(this.label1_Click);
            // 
            // InputMsgBox
            // 
            this.InputMsgBox.Location = new System.Drawing.Point(12, 374);
            this.InputMsgBox.Name = "InputMsgBox";
            this.InputMsgBox.Size = new System.Drawing.Size(823, 21);
            this.InputMsgBox.TabIndex = 2;
            // 
            // BtnLogin
            // 
            this.BtnLogin.Font = new System.Drawing.Font("Noto Sans CJK KR Regular", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnLogin.Location = new System.Drawing.Point(651, 40);
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.Size = new System.Drawing.Size(184, 42);
            this.BtnLogin.TabIndex = 3;
            this.BtnLogin.Text = "Connect to server";
            this.BtnLogin.UseVisualStyleBackColor = true;
            this.BtnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // OutputMsgBox
            // 
            this.OutputMsgBox.Location = new System.Drawing.Point(12, 88);
            this.OutputMsgBox.Multiline = true;
            this.OutputMsgBox.Name = "OutputMsgBox";
            this.OutputMsgBox.ReadOnly = true;
            this.OutputMsgBox.Size = new System.Drawing.Size(823, 280);
            this.OutputMsgBox.TabIndex = 4;
            // 
            // BtnSend
            // 
            this.BtnSend.Font = new System.Drawing.Font("Noto Sans CJK KR Regular", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnSend.Location = new System.Drawing.Point(642, 405);
            this.BtnSend.Name = "BtnSend";
            this.BtnSend.Size = new System.Drawing.Size(193, 50);
            this.BtnSend.TabIndex = 5;
            this.BtnSend.Text = "Send Message";
            this.BtnSend.UseVisualStyleBackColor = true;
            this.BtnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 467);
            this.Controls.Add(this.BtnSend);
            this.Controls.Add(this.OutputMsgBox);
            this.Controls.Add(this.BtnLogin);
            this.Controls.Add(this.InputMsgBox);
            this.Controls.Add(this.ChatName);
            this.Controls.Add(this.NameInputBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NameInputBox;
        private System.Windows.Forms.Label ChatName;
        private System.Windows.Forms.TextBox InputMsgBox;
        private System.Windows.Forms.Button BtnLogin;
        private System.Windows.Forms.TextBox OutputMsgBox;
        private System.Windows.Forms.Button BtnSend;
    }
}

