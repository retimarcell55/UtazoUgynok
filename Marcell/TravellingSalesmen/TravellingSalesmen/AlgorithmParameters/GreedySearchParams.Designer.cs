namespace TravellingSalesmen.AlgorithmParameters
{
    partial class GreedySearchParams
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
            this.PatienceNumberLabel = new System.Windows.Forms.Label();
            this.NumberOfRunsLabel = new System.Windows.Forms.Label();
            this.MaxRouteLengthPerAgentLabel = new System.Windows.Forms.Label();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.PatienceNumberTextBox = new System.Windows.Forms.TextBox();
            this.NumerOfRunsTextBox = new System.Windows.Forms.TextBox();
            this.MaxRouteLengthPerAgentTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // PatienceNumberLabel
            // 
            this.PatienceNumberLabel.AutoSize = true;
            this.PatienceNumberLabel.Location = new System.Drawing.Point(12, 21);
            this.PatienceNumberLabel.Name = "PatienceNumberLabel";
            this.PatienceNumberLabel.Size = new System.Drawing.Size(87, 13);
            this.PatienceNumberLabel.TabIndex = 0;
            this.PatienceNumberLabel.Text = "Patience number";
            this.PatienceNumberLabel.Click += new System.EventHandler(this.PatienceNumberLabel_Click);
            // 
            // NumberOfRunsLabel
            // 
            this.NumberOfRunsLabel.AutoSize = true;
            this.NumberOfRunsLabel.Location = new System.Drawing.Point(12, 82);
            this.NumberOfRunsLabel.Name = "NumberOfRunsLabel";
            this.NumberOfRunsLabel.Size = new System.Drawing.Size(79, 13);
            this.NumberOfRunsLabel.TabIndex = 1;
            this.NumberOfRunsLabel.Text = "Number of runs";
            this.NumberOfRunsLabel.Click += new System.EventHandler(this.NumberOfRunsLabel_Click);
            // 
            // MaxRouteLengthPerAgentLabel
            // 
            this.MaxRouteLengthPerAgentLabel.AutoSize = true;
            this.MaxRouteLengthPerAgentLabel.Location = new System.Drawing.Point(12, 151);
            this.MaxRouteLengthPerAgentLabel.Name = "MaxRouteLengthPerAgentLabel";
            this.MaxRouteLengthPerAgentLabel.Size = new System.Drawing.Size(134, 13);
            this.MaxRouteLengthPerAgentLabel.TabIndex = 2;
            this.MaxRouteLengthPerAgentLabel.Text = "Max route length per agent";
            this.MaxRouteLengthPerAgentLabel.Click += new System.EventHandler(this.MaxRouteLengthPerAgentLabel_Click);
            // 
            // SubmitButton
            // 
            this.SubmitButton.Location = new System.Drawing.Point(68, 220);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(132, 31);
            this.SubmitButton.TabIndex = 3;
            this.SubmitButton.Text = "Submit";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // PatienceNumberTextBox
            // 
            this.PatienceNumberTextBox.Location = new System.Drawing.Point(157, 21);
            this.PatienceNumberTextBox.Name = "PatienceNumberTextBox";
            this.PatienceNumberTextBox.Size = new System.Drawing.Size(100, 20);
            this.PatienceNumberTextBox.TabIndex = 4;
            // 
            // NumerOfRunsTextBox
            // 
            this.NumerOfRunsTextBox.Location = new System.Drawing.Point(157, 82);
            this.NumerOfRunsTextBox.Name = "NumerOfRunsTextBox";
            this.NumerOfRunsTextBox.Size = new System.Drawing.Size(100, 20);
            this.NumerOfRunsTextBox.TabIndex = 5;
            // 
            // MaxRouteLengthPerAgentTextBox
            // 
            this.MaxRouteLengthPerAgentTextBox.Location = new System.Drawing.Point(157, 148);
            this.MaxRouteLengthPerAgentTextBox.Name = "MaxRouteLengthPerAgentTextBox";
            this.MaxRouteLengthPerAgentTextBox.Size = new System.Drawing.Size(100, 20);
            this.MaxRouteLengthPerAgentTextBox.TabIndex = 6;
            // 
            // GreedySearchParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 263);
            this.Controls.Add(this.MaxRouteLengthPerAgentTextBox);
            this.Controls.Add(this.NumerOfRunsTextBox);
            this.Controls.Add(this.PatienceNumberTextBox);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.MaxRouteLengthPerAgentLabel);
            this.Controls.Add(this.NumberOfRunsLabel);
            this.Controls.Add(this.PatienceNumberLabel);
            this.Name = "GreedySearchParams";
            this.Text = "GreedySearchParams";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PatienceNumberLabel;
        private System.Windows.Forms.Label NumberOfRunsLabel;
        private System.Windows.Forms.Label MaxRouteLengthPerAgentLabel;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.TextBox PatienceNumberTextBox;
        private System.Windows.Forms.TextBox NumerOfRunsTextBox;
        private System.Windows.Forms.TextBox MaxRouteLengthPerAgentTextBox;
    }
}