namespace GameClient
{
    partial class ClientForm
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
            this.outputRichTextBox = new System.Windows.Forms.RichTextBox();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.playerTabPlayerInfoRichTextBox = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.playerItemsTabRichTextBox = new System.Windows.Forms.RichTextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.playerSpellsTabRichTextBox = new System.Windows.Forms.RichTextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // outputRichTextBox
            // 
            this.outputRichTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.outputRichTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputRichTextBox.HideSelection = false;
            this.outputRichTextBox.Location = new System.Drawing.Point(12, 12);
            this.outputRichTextBox.Name = "outputRichTextBox";
            this.outputRichTextBox.ReadOnly = true;
            this.outputRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.outputRichTextBox.Size = new System.Drawing.Size(856, 525);
            this.outputRichTextBox.TabIndex = 0;
            this.outputRichTextBox.TabStop = false;
            this.outputRichTextBox.Text = "";
            // 
            // inputTextBox
            // 
            this.inputTextBox.Location = new System.Drawing.Point(13, 544);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(803, 20);
            this.inputTextBox.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(881, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(270, 396);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.playerTabPlayerInfoRichTextBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(262, 370);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Username";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // playerTabPlayerInfoRichTextBox
            // 
            this.playerTabPlayerInfoRichTextBox.Location = new System.Drawing.Point(4, 5);
            this.playerTabPlayerInfoRichTextBox.Name = "playerTabPlayerInfoRichTextBox";
            this.playerTabPlayerInfoRichTextBox.ReadOnly = true;
            this.playerTabPlayerInfoRichTextBox.Size = new System.Drawing.Size(252, 360);
            this.playerTabPlayerInfoRichTextBox.TabIndex = 2;
            this.playerTabPlayerInfoRichTextBox.Text = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.playerItemsTabRichTextBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(262, 370);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Items";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // playerItemsTabRichTextBox
            // 
            this.playerItemsTabRichTextBox.Location = new System.Drawing.Point(4, 5);
            this.playerItemsTabRichTextBox.Name = "playerItemsTabRichTextBox";
            this.playerItemsTabRichTextBox.ReadOnly = true;
            this.playerItemsTabRichTextBox.Size = new System.Drawing.Size(252, 360);
            this.playerItemsTabRichTextBox.TabIndex = 1;
            this.playerItemsTabRichTextBox.Text = "";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.playerSpellsTabRichTextBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(262, 370);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Spells";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // playerSpellsTabRichTextBox
            // 
            this.playerSpellsTabRichTextBox.Location = new System.Drawing.Point(4, 5);
            this.playerSpellsTabRichTextBox.Name = "playerSpellsTabRichTextBox";
            this.playerSpellsTabRichTextBox.ReadOnly = true;
            this.playerSpellsTabRichTextBox.Size = new System.Drawing.Size(252, 360);
            this.playerSpellsTabRichTextBox.TabIndex = 0;
            this.playerSpellsTabRichTextBox.Text = "";
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(822, 542);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(46, 23);
            this.sendButton.TabIndex = 3;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // ClientForm
            // 
            this.AcceptButton = this.sendButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1163, 580);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.inputTextBox);
            this.Controls.Add(this.outputRichTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ClientForm";
            this.Text = "GAMENAMEHERE";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button sendButton;
        public System.Windows.Forms.RichTextBox outputRichTextBox;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox playerItemsTabRichTextBox;
        private System.Windows.Forms.RichTextBox playerSpellsTabRichTextBox;
        private System.Windows.Forms.RichTextBox playerTabPlayerInfoRichTextBox;
    }
}