namespace SSM
{
    partial class UnknownAppIdDialog
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
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.Ok = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Thumbnail = new System.Windows.Forms.PictureBox();
            this.MainInstructionLabel = new System.Windows.Forms.Label();
            this.DetailLabel = new System.Windows.Forms.Label();
            this.GameNameInput = new System.Windows.Forms.TextBox();
            this.BottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Thumbnail)).BeginInit();
            this.SuspendLayout();
            // 
            // BottomPanel
            // 
            this.BottomPanel.BackColor = System.Drawing.SystemColors.Control;
            this.BottomPanel.Controls.Add(this.Ok);
            this.BottomPanel.Controls.Add(this.Cancel);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomPanel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BottomPanel.Location = new System.Drawing.Point(0, 97);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(544, 42);
            this.BottomPanel.TabIndex = 3;
            // 
            // Ok
            // 
            this.Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Ok.Enabled = false;
            this.Ok.Location = new System.Drawing.Point(376, 7);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(75, 23);
            this.Ok.TabIndex = 0;
            this.Ok.Text = "OK";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(457, 7);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Thumbnail
            // 
            this.Thumbnail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Thumbnail.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Thumbnail.Location = new System.Drawing.Point(12, 12);
            this.Thumbnail.Name = "Thumbnail";
            this.Thumbnail.Size = new System.Drawing.Size(142, 80);
            this.Thumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Thumbnail.TabIndex = 1;
            this.Thumbnail.TabStop = false;
            this.Thumbnail.Click += new System.EventHandler(this.Thumbnail_Click);
            // 
            // MainInstructionLabel
            // 
            this.MainInstructionLabel.AutoSize = true;
            this.MainInstructionLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainInstructionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
            this.MainInstructionLabel.Location = new System.Drawing.Point(160, 12);
            this.MainInstructionLabel.Name = "MainInstructionLabel";
            this.MainInstructionLabel.Size = new System.Drawing.Size(187, 20);
            this.MainInstructionLabel.TabIndex = 0;
            this.MainInstructionLabel.Text = "Unknown screenshot found";
            // 
            // DetailLabel
            // 
            this.DetailLabel.AutoSize = true;
            this.DetailLabel.Location = new System.Drawing.Point(160, 41);
            this.DetailLabel.Name = "DetailLabel";
            this.DetailLabel.Size = new System.Drawing.Size(327, 13);
            this.DetailLabel.TabIndex = 1;
            this.DetailLabel.Text = "Please enter the name of the game featured in the screenshot:";
            // 
            // GameNameInput
            // 
            this.GameNameInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GameNameInput.Location = new System.Drawing.Point(164, 57);
            this.GameNameInput.Name = "GameNameInput";
            this.GameNameInput.Size = new System.Drawing.Size(368, 22);
            this.GameNameInput.TabIndex = 2;
            this.GameNameInput.TextChanged += new System.EventHandler(this.GameNameInput_TextChanged);
            // 
            // UnknownAppIdDialog
            // 
            this.AcceptButton = this.Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(544, 139);
            this.Controls.Add(this.GameNameInput);
            this.Controls.Add(this.DetailLabel);
            this.Controls.Add(this.MainInstructionLabel);
            this.Controls.Add(this.Thumbnail);
            this.Controls.Add(this.BottomPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UnknownAppIdDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Steam Screenshot Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UnknownAppIdDialog_FormClosed);
            this.Load += new System.EventHandler(this.UnknownAppIdDialog_Load);
            this.BottomPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Thumbnail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.PictureBox Thumbnail;
        private System.Windows.Forms.Label MainInstructionLabel;
        private System.Windows.Forms.Label DetailLabel;
        private System.Windows.Forms.TextBox GameNameInput;
    }
}