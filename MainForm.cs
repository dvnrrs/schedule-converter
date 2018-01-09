using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
						Path.GetFileNameWithoutExtension(_openFileDialog.FileName) + ".converted.csv");
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
				var rows = new List<string>();

				using (FileStream inputFile = new FileStream(_inputTextBox.Text, FileMode.Open, FileAccess.Read))
				using (FileStream outputFile = new FileStream(_outputTextBox.Text, FileMode.Create, FileAccess.Write))
				using (StreamReader inputReader = new StreamReader(inputFile, Encoding.UTF8))
				using (StreamWriter outputWriter = new StreamWriter(outputFile, new UTF8Encoding(false)))
				using (CsvReader inputCsvReader = new CsvReader(inputReader))
				using (CsvWriter outputCsvWriter = new CsvWriter(outputWriter))
				{
					if (!inputCsvReader.Read() || !inputCsvReader.ReadHeader())
						throw new Exception("Failed to read CSV header row");

					int eventNameField = inputCsvReader.GetFieldIndex("Event Name", 0, true);
					int eventDescriptionField = inputCsvReader.GetFieldIndex("Event Description", 0, true);
					int roomField = inputCsvReader.GetFieldIndex("Room", 0, true);
					int startDateField = inputCsvReader.GetFieldIndex("Start Date", 0, true);
					int startTimeField = inputCsvReader.GetFieldIndex("Start Time", 0, true);
					int endDateField = inputCsvReader.GetFieldIndex("End Date", 0, true);
					int endTimeField = inputCsvReader.GetFieldIndex("End Time", 0, true);

					if (eventNameField < 0 ||
						eventDescriptionField < 0 ||
						roomField < 0 ||
						startDateField < 0 ||
						startTimeField < 0 ||
						endDateField < 0 ||
						endTimeField < 0)
					{
						throw new Exception("Input CSV is missing required columns. The required columns are:\n\n" +
							"Event Name\n" +
							"Event Description\n" +
							"Room\n" +
							"Start Date\n" +
							"Start Time\n" +
							"End Date\n" +
							"End Time\n");
					}

					while (inputCsvReader.Read())
					{
						string eventName = inputCsvReader.GetField(eventNameField);
						string eventDescription = inputCsvReader.GetField(eventDescriptionField);
						string room = inputCsvReader.GetField(roomField);
						string startDate = inputCsvReader.GetField(startDateField);
						string startTime = inputCsvReader.GetField(startTimeField);
						string endDate = inputCsvReader.GetField(endDateField);
						string endTime = inputCsvReader.GetField(endTimeField);

						var recorder = recorders.ContainsKey(room) ? recorders[room] : room;

						string prefix = _folderPrefixTextBox.Text.Trim();
						if (prefix.Length > 0) prefix += ' ';
						string folder = prefix + (eventName.EndsWith(" - " + room) ? eventName.Substring(0, eventName.Length - 3 - room.Length) : eventName);

						var start = DateTime.ParseExact(
							startDate + ' ' + startTime,
							"M/d/yyyy h:mm tt", CultureInfo.InvariantCulture,
							DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

						var end = DateTime.ParseExact(
							endDate + ' ' + endTime,
							"M/d/yyyy h:mm tt", CultureInfo.InvariantCulture,
							DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal)
							.Add(TimeSpan.FromMinutes(5));

						outputCsvWriter.WriteField(eventName);
						outputCsvWriter.WriteField(recorder);
						outputCsvWriter.WriteField(start.ToString("yyyy-MM-dd"));
						outputCsvWriter.WriteField(start.ToString("yyyy-MM-ddTHH\\:mm\\:ss"));
						outputCsvWriter.WriteField(end.ToString("yyyy-MM-ddTHH\\:mm\\:ss"));
						outputCsvWriter.WriteField(eventDescription);
						outputCsvWriter.WriteField(folder);

						outputCsvWriter.NextRecord();
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
	}
}
