namespace prosnap
{
    partial class ChooseMethod
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
            this.btnBox = new System.Windows.Forms.RadioButton();
            this.btnQuadTree = new System.Windows.Forms.RadioButton();
            this.btnHorizontal = new System.Windows.Forms.RadioButton();
            this.btnVertical = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.btnencrypt = new System.Windows.Forms.Button();
            this.btnsend = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBox
            // 
            this.btnBox.AutoSize = true;
            this.btnBox.Location = new System.Drawing.Point(32, 181);
            this.btnBox.Name = "btnBox";
            this.btnBox.Size = new System.Drawing.Size(43, 17);
            this.btnBox.TabIndex = 8;
            this.btnBox.TabStop = true;
            this.btnBox.Text = "Box";
            this.btnBox.UseVisualStyleBackColor = true;
            this.btnBox.Click += new System.EventHandler(this.btnBox_Click);
            // 
            // btnQuadTree
            // 
            this.btnQuadTree.AutoSize = true;
            this.btnQuadTree.Location = new System.Drawing.Point(32, 144);
            this.btnQuadTree.Name = "btnQuadTree";
            this.btnQuadTree.Size = new System.Drawing.Size(76, 17);
            this.btnQuadTree.TabIndex = 7;
            this.btnQuadTree.TabStop = true;
            this.btnQuadTree.Text = "Quad Tree";
            this.btnQuadTree.UseVisualStyleBackColor = true;
            this.btnQuadTree.Click += new System.EventHandler(this.btnQuadTree_Click);
            // 
            // btnHorizontal
            // 
            this.btnHorizontal.AutoSize = true;
            this.btnHorizontal.Location = new System.Drawing.Point(32, 110);
            this.btnHorizontal.Name = "btnHorizontal";
            this.btnHorizontal.Size = new System.Drawing.Size(72, 17);
            this.btnHorizontal.TabIndex = 6;
            this.btnHorizontal.TabStop = true;
            this.btnHorizontal.Text = "Horizontal";
            this.btnHorizontal.UseVisualStyleBackColor = true;
            this.btnHorizontal.Click += new System.EventHandler(this.btnHorizontal_Click);
            // 
            // btnVertical
            // 
            this.btnVertical.AutoSize = true;
            this.btnVertical.Location = new System.Drawing.Point(32, 76);
            this.btnVertical.Name = "btnVertical";
            this.btnVertical.Size = new System.Drawing.Size(60, 17);
            this.btnVertical.TabIndex = 5;
            this.btnVertical.TabStop = true;
            this.btnVertical.Text = "Vertical";
            this.btnVertical.UseVisualStyleBackColor = true;
            this.btnVertical.CheckedChanged += new System.EventHandler(this.btnVertical_CheckedChanged);
            this.btnVertical.Click += new System.EventHandler(this.btnVertical_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(114, 41);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(453, 351);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(32, 46);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(56, 17);
            this.radioButton1.TabIndex = 12;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "normal";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Visible = false;
            // 
            // btnencrypt
            // 
            this.btnencrypt.Location = new System.Drawing.Point(29, 221);
            this.btnencrypt.Name = "btnencrypt";
            this.btnencrypt.Size = new System.Drawing.Size(75, 23);
            this.btnencrypt.TabIndex = 13;
            this.btnencrypt.Text = "Encrypt";
            this.btnencrypt.UseVisualStyleBackColor = true;
            this.btnencrypt.Click += new System.EventHandler(this.btnencrypt_Click);
            // 
            // btnsend
            // 
            this.btnsend.Location = new System.Drawing.Point(29, 298);
            this.btnsend.Name = "btnsend";
            this.btnsend.Size = new System.Drawing.Size(75, 23);
            this.btnsend.TabIndex = 14;
            this.btnsend.Text = "Send";
            this.btnsend.UseVisualStyleBackColor = true;
            this.btnsend.Click += new System.EventHandler(this.btnsend_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(29, 259);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Zip Files";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Image = global::prosnap.Properties.Resources.Close_box_red;
            this.button2.Location = new System.Drawing.Point(525, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(41, 23);
            this.button2.TabIndex = 16;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ChooseMethod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LavenderBlush;
            this.ClientSize = new System.Drawing.Size(596, 404);
            this.ControlBox = false;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnsend);
            this.Controls.Add(this.btnencrypt);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnBox);
            this.Controls.Add(this.btnQuadTree);
            this.Controls.Add(this.btnHorizontal);
            this.Controls.Add(this.btnVertical);
            this.Name = "ChooseMethod";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose Method";
            this.Load += new System.EventHandler(this.ChooseMethod_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton btnBox;
        private System.Windows.Forms.RadioButton btnQuadTree;
        private System.Windows.Forms.RadioButton btnHorizontal;
        private System.Windows.Forms.RadioButton btnVertical;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button btnencrypt;
        private System.Windows.Forms.Button btnsend;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;

    }
}