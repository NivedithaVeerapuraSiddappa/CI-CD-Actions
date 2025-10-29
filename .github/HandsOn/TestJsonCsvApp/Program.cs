using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
// Ensure JsonCsvConverter is accessible

using System;
using System.IO;

// Reference the JsonCsvConverter class
class Program
{
	static void Main()
	{
		string json = "[{'name':'Alice','age':30},{'name':'Bob','age':25}]"
			.Replace("'", "\"");
		string csvPath = "output.csv";
		int result = JsonCsvConverter.convert_json_to_csv(json, csvPath);
		Console.WriteLine(result == 0 ? "Success" : "Failure");
		Console.WriteLine($"CSV written to: {csvPath}");
		if (File.Exists(csvPath))
		{
			Console.WriteLine("CSV Content:");
			Console.WriteLine(File.ReadAllText(csvPath));
		}
	}
}
