using System;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Collections.Generic;

public class DataEncapsulate
{
    public DateTime Date { get; set; }
    public string Product { get; set; }
    public int Quantity { get; set; }
    public double Amount { get; set; }

    // Constructor
    public DataEncapsulate(DateTime date, string product, int quantity, double amount)
    {
        Date = date;
        Product = product;
        Quantity = quantity;
        Amount = amount;
    }

    // Method where the object is being represented as a string
    public override string ToString()
    {
        return $"Date: {Date}, Product: {Product}, Quantity: {Quantity}, Amount: {Amount}";
    }

    // An additional way to make a list that can be queried
    public static IQueryable<DataEncapsulate> AsQueryableList()
    {
        List<DataEncapsulate> dataList = new List<DataEncapsulate>
        {
            new DataEncapsulate(DateTime.Now, "ProductA", 10, 50.0),
            new DataEncapsulate(DateTime.Now, "ProductB", 5, 20.0),
            
        };

        return dataList.AsQueryable();
    }

 // Adding this method to make the class queryable
public static IQueryable<DataEncapsulate> AsQueryable()
    {
        return AsQueryableList().AsQueryable();
    }

}
