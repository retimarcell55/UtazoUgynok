namespace TravellingSalesmen.AlgorithmParameters
{
    partial class GeneticAlgorithmParams
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
            this.GenerationsLabel = new System.Windows.Forms.Label();
            this.PopulationSizeLabel = new System.Windows.Forms.Label();
            this.FistChildMutateLabel = new System.Windows.Forms.Label();
            this.SecondChildMutateLabel = new System.Windows.Forms.Label();
            this.MutationProbabilityLabel = new System.Windows.Forms.Label();
            this.WeakParentRateLabel = new System.Windows.Forms.Label();
            this.Submit = new System.Windows.Forms.Button();
            this.GenerationsTextBox = new System.Windows.Forms.TextBox();
            this.PopulationSizeTextBox = new System.Windows.Forms.TextBox();
            this.MutationProbabilityTextBox = new System.Windows.Forms.TextBox();
            this.WeakParentRateTextBox = new System.Windows.Forms.TextBox();
            this.FirstChildMutateCheckBox = new System.Windows.Forms.CheckBox();
            this.SecondCildMutateCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // GenerationsLabel
            // 
            this.GenerationsLabel.AutoSize = true;
            this.GenerationsLabel.Location = new System.Drawing.Point(16, 25);
            this.GenerationsLabel.Name = "GenerationsLabel";
            this.GenerationsLabel.Size = new System.Drawing.Size(64, 13);
            this.GenerationsLabel.TabIndex = 0;
            this.GenerationsLabel.Text = "Generations";
            this.GenerationsLabel.Click += new System.EventHandler(this.GenerationsLabel_Click);
            // 
            // PopulationSizeLabel
            // 
            this.PopulationSizeLabel.AutoSize = true;
            this.PopulationSizeLabel.Location = new System.Drawing.Point(16, 66);
            this.PopulationSizeLabel.Name = "PopulationSizeLabel";
            this.PopulationSizeLabel.Size = new System.Drawing.Size(78, 13);
            this.PopulationSizeLabel.TabIndex = 1;
            this.PopulationSizeLabel.Text = "Population size";
            this.PopulationSizeLabel.Click += new System.EventHandler(this.PopulationSizeLabel_Click);
            // 
            // FistChildMutateLabel
            // 
            this.FistChildMutateLabel.AutoSize = true;
            this.FistChildMutateLabel.Location = new System.Drawing.Point(16, 110);
            this.FistChildMutateLabel.Name = "FistChildMutateLabel";
            this.FistChildMutateLabel.Size = new System.Drawing.Size(140, 13);
            this.FistChildMutateLabel.TabIndex = 2;
            this.FistChildMutateLabel.Text = "First child mutate (true/false)";
            this.FistChildMutateLabel.Click += new System.EventHandler(this.FistChildMutateLabel_Click);
            // 
            // SecondChildMutateLabel
            // 
            this.SecondChildMutateLabel.AutoSize = true;
            this.SecondChildMutateLabel.Location = new System.Drawing.Point(16, 149);
            this.SecondChildMutateLabel.Name = "SecondChildMutateLabel";
            this.SecondChildMutateLabel.Size = new System.Drawing.Size(158, 13);
            this.SecondChildMutateLabel.TabIndex = 3;
            this.SecondChildMutateLabel.Text = "Second child mutate (true/false)";
            this.SecondChildMutateLabel.Click += new System.EventHandler(this.SecondChildMutateLabel_Click);
            // 
            // MutationProbabilityLabel
            // 
            this.MutationProbabilityLabel.AutoSize = true;
            this.MutationProbabilityLabel.Location = new System.Drawing.Point(16, 191);
            this.MutationProbabilityLabel.Name = "MutationProbabilityLabel";
            this.MutationProbabilityLabel.Size = new System.Drawing.Size(98, 13);
            this.MutationProbabilityLabel.TabIndex = 4;
            this.MutationProbabilityLabel.Text = "Mutation probability";
            this.MutationProbabilityLabel.Click += new System.EventHandler(this.MutationProbabilityLabel_Click);
            // 
            // WeakParentRateLabel
            // 
            this.WeakParentRateLabel.AutoSize = true;
            this.WeakParentRateLabel.Location = new System.Drawing.Point(16, 229);
            this.WeakParentRateLabel.Name = "WeakParentRateLabel";
            this.WeakParentRateLabel.Size = new System.Drawing.Size(91, 13);
            this.WeakParentRateLabel.TabIndex = 5;
            this.WeakParentRateLabel.Text = "Weak Parent rate";
            this.WeakParentRateLabel.Click += new System.EventHandler(this.WeakParentRateLabel_Click);
            // 
            // Submit
            // 
            this.Submit.Location = new System.Drawing.Point(81, 278);
            this.Submit.Name = "Submit";
            this.Submit.Size = new System.Drawing.Size(132, 42);
            this.Submit.TabIndex = 6;
            this.Submit.Text = "Submit";
            this.Submit.UseVisualStyleBackColor = true;
            this.Submit.Click += new System.EventHandler(this.Submit_Click);
            // 
            // GenerationsTextBox
            // 
            this.GenerationsTextBox.Location = new System.Drawing.Point(185, 22);
            this.GenerationsTextBox.Name = "GenerationsTextBox";
            this.GenerationsTextBox.Size = new System.Drawing.Size(100, 20);
            this.GenerationsTextBox.TabIndex = 7;
            // 
            // PopulationSizeTextBox
            // 
            this.PopulationSizeTextBox.Location = new System.Drawing.Point(185, 63);
            this.PopulationSizeTextBox.Name = "PopulationSizeTextBox";
            this.PopulationSizeTextBox.Size = new System.Drawing.Size(100, 20);
            this.PopulationSizeTextBox.TabIndex = 8;
            // 
            // MutationProbabilityTextBox
            // 
            this.MutationProbabilityTextBox.Location = new System.Drawing.Point(185, 188);
            this.MutationProbabilityTextBox.Name = "MutationProbabilityTextBox";
            this.MutationProbabilityTextBox.Size = new System.Drawing.Size(100, 20);
            this.MutationProbabilityTextBox.TabIndex = 11;
            // 
            // WeakParentRateTextBox
            // 
            this.WeakParentRateTextBox.Location = new System.Drawing.Point(185, 226);
            this.WeakParentRateTextBox.Name = "WeakParentRateTextBox";
            this.WeakParentRateTextBox.Size = new System.Drawing.Size(100, 20);
            this.WeakParentRateTextBox.TabIndex = 12;
            // 
            // FirstChildMutateCheckBox
            // 
            this.FirstChildMutateCheckBox.AutoSize = true;
            this.FirstChildMutateCheckBox.Location = new System.Drawing.Point(185, 109);
            this.FirstChildMutateCheckBox.Name = "FirstChildMutateCheckBox";
            this.FirstChildMutateCheckBox.Size = new System.Drawing.Size(59, 17);
            this.FirstChildMutateCheckBox.TabIndex = 13;
            this.FirstChildMutateCheckBox.Text = "Mutate";
            this.FirstChildMutateCheckBox.UseVisualStyleBackColor = true;
            // 
            // SecondCildMutateCheckBox
            // 
            this.SecondCildMutateCheckBox.AutoSize = true;
            this.SecondCildMutateCheckBox.Location = new System.Drawing.Point(185, 145);
            this.SecondCildMutateCheckBox.Name = "SecondCildMutateCheckBox";
            this.SecondCildMutateCheckBox.Size = new System.Drawing.Size(59, 17);
            this.SecondCildMutateCheckBox.TabIndex = 14;
            this.SecondCildMutateCheckBox.Text = "Mutate";
            this.SecondCildMutateCheckBox.UseVisualStyleBackColor = true;
            // 
            // GeneticAlgorithmParams
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 332);
            this.Controls.Add(this.SecondCildMutateCheckBox);
            this.Controls.Add(this.FirstChildMutateCheckBox);
            this.Controls.Add(this.WeakParentRateTextBox);
            this.Controls.Add(this.MutationProbabilityTextBox);
            this.Controls.Add(this.PopulationSizeTextBox);
            this.Controls.Add(this.GenerationsTextBox);
            this.Controls.Add(this.Submit);
            this.Controls.Add(this.WeakParentRateLabel);
            this.Controls.Add(this.MutationProbabilityLabel);
            this.Controls.Add(this.SecondChildMutateLabel);
            this.Controls.Add(this.FistChildMutateLabel);
            this.Controls.Add(this.PopulationSizeLabel);
            this.Controls.Add(this.GenerationsLabel);
            this.Name = "GeneticAlgorithmParams";
            this.Text = "GeneticAlgorithmParams";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label GenerationsLabel;
        private System.Windows.Forms.Label PopulationSizeLabel;
        private System.Windows.Forms.Label FistChildMutateLabel;
        private System.Windows.Forms.Label SecondChildMutateLabel;
        private System.Windows.Forms.Label MutationProbabilityLabel;
        private System.Windows.Forms.Label WeakParentRateLabel;
        private System.Windows.Forms.Button Submit;
        private System.Windows.Forms.TextBox GenerationsTextBox;
        private System.Windows.Forms.TextBox PopulationSizeTextBox;
        private System.Windows.Forms.TextBox MutationProbabilityTextBox;
        private System.Windows.Forms.TextBox WeakParentRateTextBox;
        private System.Windows.Forms.CheckBox FirstChildMutateCheckBox;
        private System.Windows.Forms.CheckBox SecondCildMutateCheckBox;
    }
}