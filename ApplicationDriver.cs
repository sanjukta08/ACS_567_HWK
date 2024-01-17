using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;


public class ApplicationDriver
{
    private DataManager dataManager;

    public ApplicationDriver()
    {
        dataManager = DataManager.Instance;
    }

    public void Run()
    {
        Console.WriteLine("Welcome to the Data Management Application!");

        while (true)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Add Data");
            Console.WriteLine("2. Edit Data");
            Console.WriteLine("3. Delete Data");
            Console.WriteLine("4. Get Mean");
            Console.WriteLine("5. Get Median");
            Console.WriteLine("6. Filter Data");
            Console.WriteLine("0. Exit");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddData();
                    break;
                case "2":
                    EditData();
                    break;
                case "3":
                    DeleteData();
                    break;
                case "4":
                    GetMean();
                    break;
                case "5":
                    GetMedian();
                    break;
                case "6":
                    FilterData();
                    break;
                case "0":
                    Console.WriteLine("Exiting the application. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private void AddData()
    {
    Console.WriteLine("Adding new data:");
    Console.Write("Enter Date (MM/DD/YYYY): ");
    DateTime date;
    if (!DateTime.TryParse(Console.ReadLine(), out date))
    {
        Console.WriteLine("Invalid date format. Data not added.");
        return;
    }

    Console.Write("Enter Product: ");
    string product = Console.ReadLine();

    Console.Write("Enter Quantity: ");
    int quantity;
    if (!int.TryParse(Console.ReadLine(), out quantity))
    {
        Console.WriteLine("Invalid quantity format. Data not added.");
        return;
    }

    Console.Write("Enter Amount: ");
    double amount;
    if (!double.TryParse(Console.ReadLine(), out amount))
    {
        Console.WriteLine("Invalid amount format. Data not added.");
        return;
    }

    DataEncapsulate newData = new DataEncapsulate(date, product, quantity, amount);
    dataManager.AddData(newData);
    Console.WriteLine("Data added successfully.");
}

    private void EditData()
    {
        Console.WriteLine("Editing existing data:");
        Console.Write("Enter Product to edit: ");
    string productName = Console.ReadLine();

    dataManager.EditData(productName, dataToEdit =>
    {
        // Allow the user to edit specific fields
        Console.Write($"Edit Date ({dataToEdit.Date}): ");
        DateTime editedDate;
        if (DateTime.TryParse(Console.ReadLine(), out editedDate))
        {
            dataToEdit.Date = editedDate;
        }

        Console.Write($"Edit Quantity ({dataToEdit.Quantity}): ");
        int editedQuantity;
        if (int.TryParse(Console.ReadLine(), out editedQuantity))
        {
            dataToEdit.Quantity = editedQuantity;
        }

        Console.Write($"Edit Amount ({dataToEdit.Amount}): ");
        double editedAmount;
        if (double.TryParse(Console.ReadLine(), out editedAmount))
        {
            dataToEdit.Amount = editedAmount;
        }
    });

    Console.WriteLine("Data edited successfully.");
    }

    private void DeleteData()
    {
        Console.WriteLine("Deleting existing data:");
    Console.Write("Enter Product to delete: ");
    string productName = Console.ReadLine();

    dataManager.DeleteData(productName);
    Console.WriteLine("Data deleted successfully.");
    }

    private void GetMean()
    {
         double mean = dataManager.GetMean();
    Console.WriteLine($"Mean Amount: {mean:F2}");
    }

    private void GetMedian()
    {
        double median = dataManager.GetMedian();
    Console.WriteLine($"Median Amount: {median:F2}");
    }

    private void FilterData()
    {
        Console.WriteLine("Filtering data:");
    Console.Write("Enter filter condition (e.g., Product == 'XYZ'): ");
    string filterCondition = Console.ReadLine();

    // Using dynamic LINQ to apply the filter
    var filteredData = dataManager.FilterData(data => EvaluateFilterCondition(data, filterCondition));

    Console.WriteLine("Filtered Data:");
    foreach (var item in filteredData)
    {
        Console.WriteLine(item);
    }
    }

    
    private bool EvaluateFilterCondition(DataEncapsulate data, string condition)
{
    
    try
        {
            // var result = data.AsQueryable().Where(condition).Any();
            var result = DataEncapsulate.AsQueryable().Where(condition).Any();

            return result;
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid filter condition. Returning all data.");
            return true;
        }
}

private Expression<Func<DataEncapsulate, bool>> ParseFilterExpression(string condition)
{
    // Defining parameter for the lambda expression
    ParameterExpression parameter = Expression.Parameter(typeof(DataEncapsulate), "data");

    // Parse the condition and creating a lambda expression
    Expression body = DynamicExpressionParser.ParseLambda(new[] { parameter }, typeof(bool), condition).Body;

    // Creating a lambda expression with the specified parameter and parsed body
    return Expression.Lambda<Func<DataEncapsulate, bool>>(body, parameter);
}
public static void Main(string[] args)
    {
        // Creating an instance of ApplicationDriver and running the application
        ApplicationDriver appDriver = new ApplicationDriver();
        appDriver.Run();
    }
}
