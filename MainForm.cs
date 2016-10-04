using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
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
				builder["Extended Properties"] = "Excel 12.0; HDR=YES";

				using (var connection = new OleDbConnection(builder.ConnectionString))
				{
					connection.Open();

					var sheets = new List<string>(GetSheetNames(connection));
					if (sheets.Count != 1) throw new InvalidDataException(
						"The Excel document has more than one worksheet; please " +
						"ensure the input file contains only a single sheet!");

					new OleDbDataAdapter("SELECT * FROM [" + sheets[0] + "]", connection).Fill(data);
				}

				using (var file = new FileStream(_outputTextBox.Text, FileMode.Create, FileAccess.Write))
				using (var writer = new StreamWriter(file, Encoding.UTF8))
				{
					foreach (var row in data.AsEnumerable())
					{
						var sessionName = row.Field<string>(3);
						var remoteRecorder = GetRemoteRecorder(row.Field<string>(4));
						var startDate = DateTime.ParseExact(row.Field<string>(6),
							"MM/dd/yyyy 'at' hh:mm tt",
							CultureInfo.InvariantCulture,
							DateTimeStyles.AssumeLocal);
						var duration = TimeSpan.ParseExact(row.Field<string>(5),
							"h' hours and 'm' mins'",
							CultureInfo.InvariantCulture,
							TimeSpanStyles.None);
						var presenter = row.Field<string>(8);
						var folder = string.Format("{0} ({1} {2:0000})",
							row.Field<string>(2),
							_monthNames[startDate.Month],
							startDate.Year);

						writer.WriteLine(string.Join(",",
							sessionName,
							remoteRecorder,
							startDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture),
							startDate.ToString("h:mm tt", CultureInfo.InvariantCulture),
							(startDate.Add(duration)).ToString("h:mm tt", CultureInfo.InvariantCulture),
							presenter,
							folder));
					}
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

		private static string GetRemoteRecorder(string location)
		{
			switch (location)
			{
				case "Classroom 1": return "SK_CR1_CAPTURE";
				case "Classroom 2": return "SK_CR2a_CAPTURE";
				case "Multipurpose Lab": return "SK_MPL_CAPTURE";
				case "Upper Auditorium": return "SK_AUDU_CAPTURE";
				case "Lower Auditorium": return "SK_AUDL_CAPTURE";
				case "Upper & Lower Auditorium": return "SK_aUDU_CAPTURE";
				default: return "XXX";
			}
		}

		private static string[] _monthNames = new[]
		{
			"",
			"Jan", "Feb", "Mar",
			"Apr", "May", "June",
			"July", "Aug", "Sept",
			"Oct", "Nov", "Dec"
		};
	}
}
