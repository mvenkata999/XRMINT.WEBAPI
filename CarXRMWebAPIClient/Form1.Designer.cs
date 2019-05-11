namespace CarXRMWebAPIClient
{
    partial class Form1
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
            this.bttnGetCustomerList = new System.Windows.Forms.Button();
            this.richTxtBxResp = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // bttnGetCustomerList
            // 
            this.bttnGetCustomerList.Location = new System.Drawing.Point(175, 72);
            this.bttnGetCustomerList.Name = "bttnGetCustomerList";
            this.bttnGetCustomerList.Size = new System.Drawing.Size(105, 23);
            this.bttnGetCustomerList.TabIndex = 0;
            this.bttnGetCustomerList.Text = "GetCustomerList";
            this.bttnGetCustomerList.UseVisualStyleBackColor = true;
            this.bttnGetCustomerList.Click += new System.EventHandler(this.bttnGetCustomerList_Click);
            // 
            // richTxtBxResp
            // 
            this.richTxtBxResp.Location = new System.Drawing.Point(56, 119);
            this.richTxtBxResp.Name = "richTxtBxResp";
            this.richTxtBxResp.Size = new System.Drawing.Size(393, 98);
            this.richTxtBxResp.TabIndex = 1;
            this.richTxtBxResp.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 278);
            this.Controls.Add(this.richTxtBxResp);
            this.Controls.Add(this.bttnGetCustomerList);
            this.Name = "Form1";
            this.Text = "CarXRMWebAPI Client";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bttnGetCustomerList;
        private System.Windows.Forms.RichTextBox richTxtBxResp;
    }
}

