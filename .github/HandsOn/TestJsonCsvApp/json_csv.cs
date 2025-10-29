// Demo: convert JSON array (list of objects) into CSV file.
// Copilot: implement int convert_json_to_csv(const char *json_str, const char *csv_path)
using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

public static class JsonCsvConverter
{
    // Parses minimal JSON input, writes CSV headers and rows, returns 0 on success
    public static int convert_json_to_csv(string json_str, string csv_path)
    {
        try
        {
            var jsonArray = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json_str);
            if (jsonArray == null || jsonArray.Count == 0)
                return -1;

            var headers = new List<string>(jsonArray[0].Keys);
            using (var writer = new StreamWriter(csv_path))
            {
                // Write headers
                writer.WriteLine(string.Join(",", headers));
                // Write rows
                foreach (var obj in jsonArray)
                {
                    var row = new List<string>();
                    foreach (var header in headers)
                    {
                        var value = obj.ContainsKey(header) && obj[header] != null ? obj[header].ToString() : "";
                        // Proper escaping: wrap in quotes if contains comma or quote, and double quotes inside value
                        if (value.Contains(",") || value.Contains("\""))
                            value = $"\"{value.Replace("\"", "\"\"")}";
                        row.Add(value);
                    }
                    writer.WriteLine(string.Join(",", row));
                }
            }
            return 0;
        }
        catch
        {
            return -1;
        }
    }
}
