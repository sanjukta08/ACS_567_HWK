using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class DataManager
{
    private static DataManager instance;

    // This is the List to store DataEncapsulation objects
    private List<DataEncapsulate> dataList;

    //This is the path to the data file
    private string filePath = "data.csv";

    // To enforce the singleton pattern, using a private constructor.

    private DataManager()
    {
        dataList = new List<DataEncapsulate>();
        ReadDataFromFile();
    }

    // This is a public method to get the singleton instance
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataManager();
            }
            return instance;
        }
    }

    // Method to read data from the file
    private void ReadDataFromFile()
    {
        try
        {
            if (File.Exists(filePath))
            {
                dataList = File.ReadAllLines(filePath)
                    .Skip(1) // If a header line is present, ignore it.
                    .Select(line => line.Split(','))
                    .Select(fields => new DataEncapsulate(
                        DateTime.Parse(fields[0]),
                        fields[1],
                        int.Parse(fields[2]),
                        double.Parse(fields[3])
                    ))
                    .ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"A reading error occurred from the file: {ex.Message}");
        }
    }
// This is a method to add new data
    public void AddData(DataEncapsulate newData)
{
    dataList.Add(newData);
    SaveDataToFile();
}

// Method for editing existing data based on a condition
public void EditData(string productName, Action<DataEncapsulate> editAction)
{
    var dataToEdit = dataList.Find(d => d.Product == productName);
    if (dataToEdit != null)
    {
        editAction.Invoke(dataToEdit);
        SaveDataToFile();
    }
}

// Method for deleting an existing data based on a condition
public void DeleteData(string productName)
{
    dataList.RemoveAll(d => d.Product == productName);
    SaveDataToFile();
}

// Method to calculate the mean of a particular field
public double GetMean()
{
    return dataList.Any() ? dataList.Average(d => d.Amount) : 0;
}

// Method to calculate the median of a particular field
public double GetMedian()
{
    return dataList.Any() ? dataList.OrderBy(d => d.Amount).Skip(dataList.Count / 2).First().Amount : 0;
}

// Method to filter data based on a condition
public List<DataEncapsulate> FilterData(Func<DataEncapsulate, bool> filterCondition)
{
    return dataList.Where(filterCondition).ToList();
}


// Method to save data to the file
private void SaveDataToFile()
{
    try
    {
        var lines = new List<string> { "Date,Product,Quantity,Amount" };
        lines.AddRange(dataList.Select(data => $"{data.Date},{data.Product},{data.Quantity},{data.Amount}"));
        File.WriteAllLines(filePath, lines);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error saving data to file: {ex.Message}");
    }
}

}
