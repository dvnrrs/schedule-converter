using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ScheduleConverter
{
	public static class ConfigFiles
	{
		public static Dictionary<string, string> ReadRecorders()
		{
			var filename = Path.Combine(
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
				"recorders.txt");

			var recorders = new Dictionary<string, string>();

			using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
			using (var reader = new StreamReader(file))
			{
				while (true)
				{
					var line = reader.ReadLine();
					if (line == null) break;
					var m = _re.Match(line);
					if (m.Success)
					{
						string key = m.Groups[1].Value;
						string value = m.Groups[2].Value;
						recorders[key] = value;
                    }
				}
			}

			return recorders;
		}

		private static Regex _re = new Regex(
			@"^\s*(\S+(?:\s+\S+)*)\s*=\s*(\S+(?:\s+\S+)*)\s*$",
			RegexOptions.Compiled);
	}
}