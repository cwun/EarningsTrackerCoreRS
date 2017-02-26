using System.Collections.Generic;
using EarningsTracker.Services.Revenues;

namespace EarningsTracker.Services.Charts
{
    public interface IChartData
    {
        // total revenue information of a specific office
        RevenueData TotalRevenue { get; set; }
        // a set of data points on a chart that represents the yearly profit of a specific office
        IEnumerable<decimal> YearlyProfit { get; set; }
        // a set of data points on a chart that represents the yearly revenue of a specific office
        IEnumerable<decimal> YearlyRevenue { get; set; }
    }
}
