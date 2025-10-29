using System;

class Program
{
    static void Main()
    {
        string json = @"[
            {""name"":""Alice"",""age"":30},
            {""name"":""Bob"",""age"":25}
        ]";
        string csvPath = "output.csv";
        int result = JsonCsvConverter.convert_json_to_csv(json, csvPath);
        Console.WriteLine(result == 0 ? "Success" : "Failure");
        Console.WriteLine($"CSV written to: {csvPath}");
    }
}