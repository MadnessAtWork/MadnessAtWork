namespace MegsCookieDatabase
{
    partial class HomeScreen
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
            this.historyButton = new System.Windows.Forms.Button();
            this.newOrderButton = new System.Windows.Forms.Button();
            this.graphButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // historyButton
            // 
            this.historyButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.historyButton.BackColor = System.Drawing.SystemColors.ControlDark;
            this.historyButton.Location = new System.Drawing.Point(12, 12);
            this.historyButton.Name = "historyButton";
            this.historyButton.Size = new System.Drawing.Size(258, 51);
            this.historyButton.TabIndex = 0;
            this.historyButton.Text = "History";
            this.historyButton.UseVisualStyleBackColor = false;
            this.historyButton.Click += new System.EventHandler(this.historyButton_Click);
            // 
            // newOrderButton
            // 
            this.newOrderButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.newOrderButton.BackColor = System.Drawing.SystemColors.ControlDark;
            this.newOrderButton.Location = new System.Drawing.Point(276, 12);
            this.newOrderButton.Name = "newOrderButton";
            this.newOrderButton.Size = new System.Drawing.Size(248, 51);
            this.newOrderButton.TabIndex = 1;
            this.newOrderButton.Text = "New Order";
            this.newOrderButton.UseVisualStyleBackColor = false;
            this.newOrderButton.Click += new System.EventHandler(this.newOrderButton_Click);
            // 
            // graphButton
            // 
            this.graphButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.graphButton.BackColor = System.Drawing.SystemColors.ControlDark;
            this.graphButton.Location = new System.Drawing.Point(530, 12);
            this.graphButton.Name = "graphButton";
            this.graphButton.Size = new System.Drawing.Size(258, 51);
            this.graphButton.TabIndex = 2;
            this.graphButton.Text = "Data Graphs";
            this.graphButton.UseVisualStyleBackColor = false;
            this.graphButton.Click += new System.EventHandler(this.graphButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(152, 167);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(478, 69);
            this.label1.TabIndex = 3;
            this.label1.Text = "Welcome Megan";
            // 
            // HomeScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.graphButton);
            this.Controls.Add(this.newOrderButton);
            this.Controls.Add(this.historyButton);
            this.Name = "HomeScreen";
            this.Text = "HomeScreen";
            this.Load += new System.EventHandler(this.HomeScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button historyButton;
        private System.Windows.Forms.Button newOrderButton;
        private System.Windows.Forms.Button graphButton;
        private System.Windows.Forms.Label label1;
    }
}