namespace ScheduleConverter
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
			this._mainTable = new System.Windows.Forms.TableLayoutPanel();
			this._inputLabel = new System.Windows.Forms.Label();
			this._inputTextBox = new System.Windows.Forms.TextBox();
			this._inputBrowseButton = new System.Windows.Forms.Button();
			this._outputLabel = new System.Windows.Forms.Label();
			this._outputTextBox = new System.Windows.Forms.TextBox();
			this._outputBrowseButton = new System.Windows.Forms.Button();
			this._buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
			this._convertButton = new System.Windows.Forms.Button();
			this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this._mainTable.SuspendLayout();
			this._buttonPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _mainTable
			// 
			this._mainTable.AutoSize = true;
			this._mainTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._mainTable.ColumnCount = 3;
			this._mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._mainTable.Controls.Add(this._inputLabel, 0, 0);
			this._mainTable.Controls.Add(this._inputTextBox, 1, 0);
			this._mainTable.Controls.Add(this._inputBrowseButton, 2, 0);
			this._mainTable.Controls.Add(this._outputLabel, 0, 1);
			this._mainTable.Controls.Add(this._outputTextBox, 1, 1);
			this._mainTable.Controls.Add(this._outputBrowseButton, 2, 1);
			this._mainTable.Controls.Add(this._buttonPanel, 0, 2);
			this._mainTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this._mainTable.Location = new System.Drawing.Point(0, 0);
			this._mainTable.Margin = new System.Windows.Forms.Padding(4);
			this._mainTable.Name = "_mainTable";
			this._mainTable.Padding = new System.Windows.Forms.Padding(20, 18, 20, 18);
			this._mainTable.RowCount = 3;
			this._mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._mainTable.Size = new System.Drawing.Size(886, 187);
			this._mainTable.TabIndex = 0;
			// 
			// _inputLabel
			// 
			this._inputLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._inputLabel.AutoSize = true;
			this._inputLabel.Location = new System.Drawing.Point(20, 29);
			this._inputLabel.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
			this._inputLabel.Name = "_inputLabel";
			this._inputLabel.Size = new System.Drawing.Size(232, 17);
			this._inputLabel.TabIndex = 0;
			this._inputLabel.Text = "eValue spreadsheet input file (xlsx):";
			// 
			// _inputTextBox
			// 
			this._inputTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._inputTextBox.Location = new System.Drawing.Point(259, 26);
			this._inputTextBox.Margin = new System.Windows.Forms.Padding(7, 6, 13, 6);
			this._inputTextBox.Name = "_inputTextBox";
			this._inputTextBox.Size = new System.Drawing.Size(399, 22);
			this._inputTextBox.TabIndex = 2;
			this._inputTextBox.TextChanged += new System.EventHandler(this.OnTextBoxTextChanged);
			// 
			// _inputBrowseButton
			// 
			this._inputBrowseButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._inputBrowseButton.AutoSize = true;
			this._inputBrowseButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._inputBrowseButton.Location = new System.Drawing.Point(671, 24);
			this._inputBrowseButton.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
			this._inputBrowseButton.Name = "_inputBrowseButton";
			this._inputBrowseButton.Size = new System.Drawing.Size(76, 27);
			this._inputBrowseButton.TabIndex = 4;
			this._inputBrowseButton.Text = "Browse...";
			this._inputBrowseButton.UseVisualStyleBackColor = true;
			this._inputBrowseButton.Click += new System.EventHandler(this.OnInputBrowseButtonClick);
			// 
			// _outputLabel
			// 
			this._outputLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._outputLabel.AutoSize = true;
			this._outputLabel.Location = new System.Drawing.Point(20, 68);
			this._outputLabel.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
			this._outputLabel.Name = "_outputLabel";
			this._outputLabel.Size = new System.Drawing.Size(205, 17);
			this._outputLabel.TabIndex = 1;
			this._outputLabel.Text = "Output filename template (csv):";
			// 
			// _outputTextBox
			// 
			this._outputTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._outputTextBox.Location = new System.Drawing.Point(259, 65);
			this._outputTextBox.Margin = new System.Windows.Forms.Padding(7, 6, 13, 6);
			this._outputTextBox.Name = "_outputTextBox";
			this._outputTextBox.Size = new System.Drawing.Size(399, 22);
			this._outputTextBox.TabIndex = 3;
			this._outputTextBox.TextChanged += new System.EventHandler(this.OnTextBoxTextChanged);
			// 
			// _outputBrowseButton
			// 
			this._outputBrowseButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._outputBrowseButton.AutoSize = true;
			this._outputBrowseButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._outputBrowseButton.Location = new System.Drawing.Point(671, 63);
			this._outputBrowseButton.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
			this._outputBrowseButton.Name = "_outputBrowseButton";
			this._outputBrowseButton.Size = new System.Drawing.Size(76, 27);
			this._outputBrowseButton.TabIndex = 5;
			this._outputBrowseButton.Text = "Browse...";
			this._outputBrowseButton.UseVisualStyleBackColor = true;
			this._outputBrowseButton.Click += new System.EventHandler(this.OnOutputBrowseButtonClick);
			// 
			// _buttonPanel
			// 
			this._buttonPanel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonPanel.AutoSize = true;
			this._buttonPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._mainTable.SetColumnSpan(this._buttonPanel, 3);
			this._buttonPanel.Controls.Add(this._convertButton);
			this._buttonPanel.Location = new System.Drawing.Point(766, 131);
			this._buttonPanel.Margin = new System.Windows.Forms.Padding(0, 25, 0, 0);
			this._buttonPanel.Name = "_buttonPanel";
			this._buttonPanel.Size = new System.Drawing.Size(100, 28);
			this._buttonPanel.TabIndex = 6;
			// 
			// _convertButton
			// 
			this._convertButton.Enabled = false;
			this._convertButton.Location = new System.Drawing.Point(0, 0);
			this._convertButton.Margin = new System.Windows.Forms.Padding(0);
			this._convertButton.Name = "_convertButton";
			this._convertButton.Size = new System.Drawing.Size(100, 28);
			this._convertButton.TabIndex = 0;
			this._convertButton.Text = "&Convert";
			this._convertButton.UseVisualStyleBackColor = true;
			this._convertButton.Click += new System.EventHandler(this.OnConvertButtonClick);
			// 
			// _openFileDialog
			// 
			this._openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
			this._openFileDialog.Title = "Select XLSX input file";
			// 
			// _saveFileDialog
			// 
			this._saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
			this._saveFileDialog.Title = "Select CSV output file";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(886, 187);
			this.Controls.Add(this._mainTable);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Schedule Converter";
			this._mainTable.ResumeLayout(false);
			this._mainTable.PerformLayout();
			this._buttonPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _mainTable;
		private System.Windows.Forms.Button _outputBrowseButton;
		private System.Windows.Forms.TextBox _outputTextBox;
		private System.Windows.Forms.Label _inputLabel;
		private System.Windows.Forms.Label _outputLabel;
		private System.Windows.Forms.TextBox _inputTextBox;
		private System.Windows.Forms.Button _inputBrowseButton;
		private System.Windows.Forms.FlowLayoutPanel _buttonPanel;
		private System.Windows.Forms.Button _convertButton;
		private System.Windows.Forms.OpenFileDialog _openFileDialog;
		private System.Windows.Forms.SaveFileDialog _saveFileDialog;
	}
}

