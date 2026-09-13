using CarRenter.DB.Models;

namespace CarRenter.DB.Utils;

public static class Utils
{
    public static decimal CalculateTotalPrice(DateTime startDate, DateTime endDate, Car car)
    {
        var totalHours = (decimal)(endDate - startDate).TotalHours;
        if (totalHours <= 0)
        {
            throw new ArgumentException("The end date must be greater than start date.");
        }

        return totalHours * car.HourlyPrice; 
    }
}