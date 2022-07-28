﻿namespace CToolingType
{
    partial class fData
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
            this.panelControl = new System.Windows.Forms.Panel();
            this.labToolTypeDesc = new System.Windows.Forms.Label();
            this.editToolTypeDesc = new System.Windows.Forms.TextBox();
            this.labelToolingNo = new System.Windows.Forms.Label();
            this.editToolTypeNo = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panelControl.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl
            // 
            this.panelControl.AutoScroll = true;
            this.panelControl.BackColor = System.Drawing.SystemColors.Control;
            this.panelControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelControl.Controls.Add(this.labToolTypeDesc);
            this.panelControl.Controls.Add(this.editToolTypeDesc);
            this.panelControl.Controls.Add(this.labelToolingNo);
            this.panelControl.Controls.Add(this.editToolTypeNo);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl.Location = new System.Drawing.Point(0, 0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(319, 136);
            this.panelControl.TabIndex = 22;
            // 
            // labToolTypeDesc
            // 
            this.labToolTypeDesc.AutoSize = true;
            this.labToolTypeDesc.Location = new System.Drawing.Point(18, 59);
            this.labToolTypeDesc.Name = "labToolTypeDesc";
            this.labToolTypeDesc.Size = new System.Drawing.Size(58, 12);
            this.labToolTypeDesc.TabIndex = 23;
            this.labToolTypeDesc.Text = "Description";
            // 
            // editToolTypeDesc
            // 
            this.editToolTypeDesc.BackColor = System.Drawing.Color.White;
            this.editToolTypeDesc.Location = new System.Drawing.Point(128, 56);
            this.editToolTypeDesc.Name = "editToolTypeDesc";
            this.editToolTypeDesc.Size = new System.Drawing.Size(172, 22);
            this.editToolTypeDesc.TabIndex = 24;
            // 
            // labelToolingNo
            // 
            this.labelToolingNo.AutoSize = true;
            this.labelToolingNo.Location = new System.Drawing.Point(17, 20);
            this.labelToolingNo.Name = "labelToolingNo";
            this.labelToolingNo.Size = new System.Drawing.Size(86, 12);
            this.labelToolingNo.TabIndex = 0;
            this.labelToolingNo.Text = "Tooling Type No";
            // 
            // editToolTypeNo
            // 
            this.editToolTypeNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.editToolTypeNo.Location = new System.Drawing.Point(128, 17);
            this.editToolTypeNo.Name = "editToolTypeNo";
            this.editToolTypeNo.Size = new System.Drawing.Size(172, 22);
            this.editToolTypeNo.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 136);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(319, 39);
            this.panel2.TabIndex = 21;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(177, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(63, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // fData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 175);
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.panel2);
            this.Name = "fData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fData";
            this.Load += new System.EventHandler(this.fData_Load);
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Label labelToolingNo;
        private System.Windows.Forms.TextBox editToolTypeNo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label labToolTypeDesc;
        private System.Windows.Forms.TextBox editToolTypeDesc;

    }
}