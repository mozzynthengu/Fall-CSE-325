using System.Globalization;
using System.Text;
using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");
var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");

Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);
var salesTotal = CalculateSalesTotal(salesFiles);

File.WriteAllText(
    Path.Combine(salesTotalDir, "totals.txt"),
    salesTotal.ToString("C", CultureInfo.GetCultureInfo("en-US"))
    + Environment.NewLine);

GenerateSalesSummary(
    salesFiles,
    Path.Combine(salesTotalDir, "salesSummary.txt"));

Console.WriteLine($"Total Sales: {salesTotal.ToString("C", CultureInfo.GetCultureInfo("en-US"))}");
Console.WriteLine("Sales summary created successfully.");

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new();

    var foundFiles = Directory.EnumerateFiles(
        folderName,
        "*.json",
        SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        salesFiles.Add(file);
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);

        SalesData? data =
            JsonConvert.DeserializeObject<SalesData?>(salesJson);

        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

void GenerateSalesSummary(
    IEnumerable<string> salesFiles,
    string outputPath)
{
    var us = CultureInfo.GetCultureInfo("en-US");
    var report = new StringBuilder();

    double total = CalculateSalesTotal(salesFiles);

    report.AppendLine("Sales Summary");
    report.AppendLine("----------------------------");
    report.AppendLine($" Total Sales: {total.ToString("C", us)}");
    report.AppendLine();
    report.AppendLine(" Details:");

    foreach (var file in salesFiles)
    {
        string json = File.ReadAllText(file);

        double fileTotal =
            JsonConvert.DeserializeObject<SalesData?>(json)?.Total ?? 0;

        report.AppendLine(
            $"  {Path.GetFileName(file)}: {fileTotal.ToString("C", us)}");
    }

    File.WriteAllText(outputPath, report.ToString());
}

record SalesData(double Total);
