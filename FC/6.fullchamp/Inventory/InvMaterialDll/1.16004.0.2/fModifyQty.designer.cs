﻿namespace InvMaterialdll
{
    partial class fModifyQty
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該公開 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.labQty = new System.Windows.Forms.Label();
            this.editQty = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lablReelNo = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labQty
            // 
            this.labQty.AutoSize = true;
            this.labQty.BackColor = System.Drawing.Color.Transparent;
            this.labQty.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labQty.Location = new System.Drawing.Point(8, 53);
            this.labQty.Name = "labQty";
            this.labQty.Size = new System.Drawing.Size(57, 15);
            this.labQty.TabIndex = 0;
            this.labQty.Text = "Unit Qty";
            // 
            // editQty
            // 
            this.editQty.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.editQty.Location = new System.Drawing.Point(83, 50);
            this.editQty.Name = "editQty";
            this.editQty.Size = new System.Drawing.Size(118, 25);
            this.editQty.TabIndex = 1;
            this.editQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editQty_KeyPress);
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSave.Location = new System.Drawing.Point(100, 81);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lablReelNo
            // 
            this.lablReelNo.BackColor = System.Drawing.Color.Transparent;
            this.lablReelNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lablReelNo.Font = new System.Drawing.Font("新細明體", 12F);
            this.lablReelNo.ForeColor = System.Drawing.Color.Maroon;
            this.lablReelNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lablReelNo.Location = new System.Drawing.Point(83, 18);
            this.lablReelNo.Name = "lablReelNo";
            this.lablReelNo.Size = new System.Drawing.Size(200, 25);
            this.lablReelNo.TabIndex = 68;
            this.lablReelNo.Text = "N/A";
            this.lablReelNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.BackColor = System.Drawing.Color.Transparent;
            this.label52.Font = new System.Drawing.Font("新細明體", 12F);
            this.label52.ForeColor = System.Drawing.Color.Black;
            this.label52.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label52.Location = new System.Drawing.Point(8, 22);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(59, 16);
            this.label52.TabIndex = 67;
            this.label52.Text = "Reel No";
            // 
            // fModifyQty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 133);
            this.Controls.Add(this.lablReelNo);
            this.Controls.Add(this.label52);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.editQty);
            this.Controls.Add(this.labQty);
            this.Name = "fModifyQty";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Modify Unit Qty";
            this.Load += new System.EventHandler(this.fModifyQty_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labQty;
        private System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.TextBox editQty;
        private System.Windows.Forms.Label lablReelNo;
        private System.Windows.Forms.Label label52;
    }
}