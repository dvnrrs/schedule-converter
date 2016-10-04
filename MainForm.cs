using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
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
			var recorders = new Dictionary<string, string>();

			try
			{
				recorders = ConfigFiles.ReadRecorders();
			}

			catch (Exception)
			{
				MessageBox.Show(this,
					"The list of recorder names could not be read from the configuration file. " +
					"Please create a file called \"recorders.txt\" in the application's installation " +
					"directory (" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
					") with lines of the form:\n\nValue Location Name = Panopto Recorder Name\n\n" +
					"The conversion will continue with no recorder name remapping.",
					"No recorder name mapping found",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
			}

			try
			{
				var builder = new OleDbConnectionStringBuilder();
				builder.DataSource = _inputTextBox.Text;
				builder.Provider = "Microsoft.ACE.OLEDB.12.0";
				builder["Extended Properties"] = "Excel 12.0; HDR=YES";

				var rowsByFile = new Dictionary<string, List<string>>();
				int validSheets = 0;

				using (var connection = new OleDbConnection(builder.ConnectionString))
				{
					connection.Open();

					var sheets = new List<string>(GetSheetNames(connection));

					for (int i = 0; i < sheets.Count; ++i)
					{
						var data = new DataTable();
						new OleDbDataAdapter("SELECT * FROM [" + sheets[0] + "]", connection).Fill(data);

						if (data.Columns["Course"] == null ||
							data.Columns["Session Name"] == null ||
							data.Columns["Location"] == null ||
							data.Columns["Duration"] == null ||
							data.Columns["Start Date/Time"] == null ||
							data.Columns["Presenters"] == null)
						{
							continue;
						}

						++validSheets;

						foreach (var row in data.AsEnumerable())
						{
							var courseStr = row.Field<string>("Course");
							var sessionNameStr = row.Field<string>("Session Name");
							var locationStr = row.Field<string>("Location");
							var durationStr = row.Field<string>("Duration");
							var startDateTimeStr = row.Field<string>("Start Date/Time");
							var presentersStr = row.Field<string>("Presenters");

							var startDate = DateTime.ParseExact(row.Field<string>("Start Date/Time"),
								"MM/dd/yyyy 'at' hh:mm tt",
								CultureInfo.InvariantCulture,
								DateTimeStyles.AssumeLocal);
							var duration = TimeSpan.ParseExact(row.Field<string>("Duration"),
								"h' hours and 'm' mins'",
								CultureInfo.InvariantCulture,
								TimeSpanStyles.None).Add(TimeSpan.FromMinutes(5));
							var folder = string.Format("{0} ({1} {2:0000})",
								row.Field<string>("Course"),
								_monthNames[startDate.Month],
								startDate.Year);

							var recorder = recorders.ContainsKey(locationStr) ?
								recorders[locationStr] : locationStr;

							var key = startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
							var filename = _outputTextBox.Text;
							filename = Path.Combine(
								Path.GetDirectoryName(filename),
								string.Format("{0} ({1}){2}",
									Path.GetFileNameWithoutExtension(filename),
									key,
									Path.GetExtension(filename)));

							if (!rowsByFile.ContainsKey(filename)) rowsByFile[filename] = new List<string>();
							rowsByFile[filename].Add(string.Join(",",
								sessionNameStr,
								recorder,
								startDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture),
								startDate.ToString("h:mm tt", CultureInfo.InvariantCulture),
								(startDate.Add(duration)).ToString("h:mm tt", CultureInfo.InvariantCulture),
								presentersStr,
								folder));
						}

					}
				}

				if (validSheets == 0)
				{
					MessageBox.Show(this, "No worksheets with a full set of schedule columns were " +
						"found in the input spreadsheet. Please ensure that the spreadsheet contains " +
						"at least one sheet that has columns named Course, Session Name, Location, " +
						"Duration, Start Date/Time and Presenters.",
						"No valid data found",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning);
					return;
				}

				foreach (var filename in rowsByFile.Keys)
				{
					if (!File.Exists(filename)) continue;

					if (MessageBox.Show(this,
						"One or more output files exist. Do you want to overwrite them?",
						"Confirm overwrite files",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Warning,
						MessageBoxDefaultButton.Button2) != DialogResult.Yes)
						return;

					break;
				}

				foreach (var entry in rowsByFile)
				{
					using (var file = new FileStream(entry.Key, FileMode.Create, FileAccess.Write))
					using (var writer = new StreamWriter(file, Encoding.UTF8))
					{
						foreach (var row in entry.Value)
						{
							writer.WriteLine(row);
						}
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

			foreach (DataRow row in schema.Rows)
			{
				yield return row["TABLE_NAME"].ToString();
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
