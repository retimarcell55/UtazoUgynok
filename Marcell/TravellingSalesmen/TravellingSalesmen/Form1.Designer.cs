namespace TravellingSalesmen
{
    partial class MainForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ConfigurationName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SaveConfigurationButton = new System.Windows.Forms.Button();
            this.AgentPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.VertexCoordPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.LoadConfigurations = new System.Windows.Forms.Button();
            this.RunAlgorithm = new System.Windows.Forms.Button();
            this.AlgorithmComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ConfigurationsComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ActualResult = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.RunThrough = new System.Windows.Forms.Button();
            this.NextMove = new System.Windows.Forms.Button();
            this.Restart = new System.Windows.Forms.Button();
            this.visualizer = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.visualizer)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ConfigurationName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.SaveConfigurationButton);
            this.groupBox1.Controls.Add(this.AgentPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.VertexCoordPath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(22, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 339);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Save Parameters";
            // 
            // ConfigurationName
            // 
            this.ConfigurationName.Location = new System.Drawing.Point(19, 228);
            this.ConfigurationName.Name = "ConfigurationName";
            this.ConfigurationName.Size = new System.Drawing.Size(229, 20);
            this.ConfigurationName.TabIndex = 6;
            this.ConfigurationName.Text = "Enter name here";
            this.ConfigurationName.Enter += new System.EventHandler(this.ConfigurationName_Enter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 188);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Configuration name:";
            // 
            // SaveConfigurationButton
            // 
            this.SaveConfigurationButton.Location = new System.Drawing.Point(18, 285);
            this.SaveConfigurationButton.Name = "SaveConfigurationButton";
            this.SaveConfigurationButton.Size = new System.Drawing.Size(229, 34);
            this.SaveConfigurationButton.TabIndex = 4;
            this.SaveConfigurationButton.Text = "Save Configuration";
            this.SaveConfigurationButton.UseVisualStyleBackColor = true;
            this.SaveConfigurationButton.Click += new System.EventHandler(this.SaveConfigurationButton_Click);
            // 
            // AgentPath
            // 
            this.AgentPath.Location = new System.Drawing.Point(18, 137);
            this.AgentPath.Name = "AgentPath";
            this.AgentPath.Size = new System.Drawing.Size(229, 20);
            this.AgentPath.TabIndex = 3;
            this.AgentPath.Text = "Enter path here";
            this.AgentPath.Enter += new System.EventHandler(this.AgentPath_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Agent informations:";
            // 
            // VertexCoordPath
            // 
            this.VertexCoordPath.Location = new System.Drawing.Point(18, 58);
            this.VertexCoordPath.Name = "VertexCoordPath";
            this.VertexCoordPath.Size = new System.Drawing.Size(229, 20);
            this.VertexCoordPath.TabIndex = 1;
            this.VertexCoordPath.Text = "Enter path here";
            this.VertexCoordPath.Enter += new System.EventHandler(this.AdjacencyPath_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Graph vertex coordinates:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.LoadConfigurations);
            this.groupBox2.Controls.Add(this.RunAlgorithm);
            this.groupBox2.Controls.Add(this.AlgorithmComboBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.ConfigurationsComboBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(24, 441);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(264, 279);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Run Algorithm";
            // 
            // LoadConfigurations
            // 
            this.LoadConfigurations.Location = new System.Drawing.Point(139, 33);
            this.LoadConfigurations.Name = "LoadConfigurations";
            this.LoadConfigurations.Size = new System.Drawing.Size(107, 21);
            this.LoadConfigurations.TabIndex = 8;
            this.LoadConfigurations.Text = "Load";
            this.LoadConfigurations.UseVisualStyleBackColor = true;
            this.LoadConfigurations.Click += new System.EventHandler(this.LoadConfigurations_Click);
            // 
            // RunAlgorithm
            // 
            this.RunAlgorithm.Location = new System.Drawing.Point(17, 218);
            this.RunAlgorithm.Name = "RunAlgorithm";
            this.RunAlgorithm.Size = new System.Drawing.Size(229, 34);
            this.RunAlgorithm.TabIndex = 5;
            this.RunAlgorithm.Text = "Run Algorithm";
            this.RunAlgorithm.UseVisualStyleBackColor = true;
            this.RunAlgorithm.Click += new System.EventHandler(this.RunAlgorithm_Click);
            // 
            // AlgorithmComboBox
            // 
            this.AlgorithmComboBox.FormattingEnabled = true;
            this.AlgorithmComboBox.Items.AddRange(new object[] {
            "RandomSearch",
            "Christofides"});
            this.AlgorithmComboBox.Location = new System.Drawing.Point(17, 160);
            this.AlgorithmComboBox.Name = "AlgorithmComboBox";
            this.AlgorithmComboBox.Size = new System.Drawing.Size(229, 21);
            this.AlgorithmComboBox.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Select algorithm:";
            // 
            // ConfigurationsComboBox
            // 
            this.ConfigurationsComboBox.FormattingEnabled = true;
            this.ConfigurationsComboBox.Location = new System.Drawing.Point(17, 74);
            this.ConfigurationsComboBox.Name = "ConfigurationsComboBox";
            this.ConfigurationsComboBox.Size = new System.Drawing.Size(229, 21);
            this.ConfigurationsComboBox.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Select configuration:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ActualResult);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.RunThrough);
            this.groupBox3.Controls.Add(this.NextMove);
            this.groupBox3.Controls.Add(this.Restart);
            this.groupBox3.Controls.Add(this.visualizer);
            this.groupBox3.Location = new System.Drawing.Point(317, 26);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(932, 694);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Visualizer";
            // 
            // ActualResult
            // 
            this.ActualResult.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ActualResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ActualResult.Location = new System.Drawing.Point(765, 633);
            this.ActualResult.Name = "ActualResult";
            this.ActualResult.Size = new System.Drawing.Size(138, 33);
            this.ActualResult.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(686, 644);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Actual Result:";
            // 
            // RunThrough
            // 
            this.RunThrough.Location = new System.Drawing.Point(530, 633);
            this.RunThrough.Name = "RunThrough";
            this.RunThrough.Size = new System.Drawing.Size(141, 34);
            this.RunThrough.TabIndex = 7;
            this.RunThrough.Text = "Run through";
            this.RunThrough.UseVisualStyleBackColor = true;
            this.RunThrough.Click += new System.EventHandler(this.RunThrough_Click);
            // 
            // NextMove
            // 
            this.NextMove.Location = new System.Drawing.Point(278, 633);
            this.NextMove.Name = "NextMove";
            this.NextMove.Size = new System.Drawing.Size(141, 34);
            this.NextMove.TabIndex = 6;
            this.NextMove.Text = "Next Move";
            this.NextMove.UseVisualStyleBackColor = true;
            this.NextMove.Click += new System.EventHandler(this.NextMove_Click);
            // 
            // Restart
            // 
            this.Restart.Location = new System.Drawing.Point(26, 633);
            this.Restart.Name = "Restart";
            this.Restart.Size = new System.Drawing.Size(141, 34);
            this.Restart.TabIndex = 5;
            this.Restart.Text = "Restart";
            this.Restart.UseVisualStyleBackColor = true;
            this.Restart.Click += new System.EventHandler(this.Restart_Click);
            // 
            // visualizer
            // 
            this.visualizer.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.visualizer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.visualizer.Location = new System.Drawing.Point(26, 34);
            this.visualizer.Name = "visualizer";
            this.visualizer.Size = new System.Drawing.Size(877, 562);
            this.visualizer.TabIndex = 0;
            this.visualizer.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1277, 747);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "Travelling Salesmen Algorithm Visualizer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.visualizer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox VertexCoordPath;
        private System.Windows.Forms.Button SaveConfigurationButton;
        private System.Windows.Forms.TextBox AgentPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox ConfigurationsComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button RunAlgorithm;
        private System.Windows.Forms.ComboBox AlgorithmComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button RunThrough;
        private System.Windows.Forms.Button NextMove;
        private System.Windows.Forms.Button Restart;
        private System.Windows.Forms.PictureBox visualizer;
        private System.Windows.Forms.TextBox ConfigurationName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button LoadConfigurations;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label ActualResult;
    }
}

