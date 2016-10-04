using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ScheduleConverter
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void OnInputBrowseButtonClick(object sender, EventArgs e)
		{
			if (_openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				_inputTextBox.Text = _openFileDialog.FileName;

				if (_outputTextBox.Text.Length == 0)
				{
					_outputTextBox.Text = Path.Combine(
						Path.GetDirectoryName(_openFileDialog.FileName),
						Path.GetFileNameWithoutExtension(_openFileDialog.FileName) + ".csv");
				}
			}
		}

		private void OnOutputBrowseButtonClick(object sender, EventArgs e)
		{
			if (_saveFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				_outputTextBox.Text = _saveFileDialog.FileName;
			}
		}

		private void OnTextBoxTextChanged(object sender, EventArgs e)
		{
			_convertButton.Enabled = _inputTextBox.Text.Length > 0 &&
				_outputTextBox.Text.Length > 0;
		}

		private void OnConvertButtonClick(object sender, EventArgs e)
		{
			if (File.Exists(_outputTextBox.Text))
			{
				if (MessageBox.Show(this,
					"The selected output file exists. Do you want to overwrite it?",
					"Confirm overwrite file",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning,
					MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
			}

			try
			{
				var data = new DataTable();

				var builder = new OleDbConnectionStringBuilder();
				builder.DataSource = _inputTextBox.Text;
				builder.Provider = "Microsoft.ACE.OLEDB.12.0";
				builder["Extended Properties"] = "Excel 12.0; HDR=NO";

				using (var connection = new OleDbConnection(builder.ConnectionString))
				{
					connection.Open();

					var sheets = new List<string>(GetSheetNames(connection));
					if (sheets.Count != 1) throw new InvalidDataException(
						"The Excel document has more than one worksheet; please " +
						"ensure the input file contains only a single sheet!");

					new OleDbDataAdapter("SELECT * FROM [" + sheets[0] + "]", connection).Fill(data);
				}


				MessageBox.Show(this,
					"The conversion was successful.",
					"Conversion successful",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information);
			}

			catch (Exception ex)
			{
				MessageBox.Show(this,
					"The conversion failed:\n\n" + ex.Message,
					"Conversion failed",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
		}

		private static IEnumerable<string> GetSheetNames(OleDbConnection connection)
		{
			var schema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
			if (schema == null) throw new InvalidDataException("Failed to read Excel schema");
			foreach (DataRow row in schema.Rows) yield return row["TABLE_NAME"].ToString();
		}
	}
}
