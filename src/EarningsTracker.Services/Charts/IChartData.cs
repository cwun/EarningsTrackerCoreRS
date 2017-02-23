using System.Collections.Generic;
using EarningsTracker.Services.Revenues;

namespace EarningsTracker.Services.Charts
{
    public interface IChartData
    {
        RevenueData TotalRevenue { get; set; }
        IEnumerable<decimal> YearlyProfit { get; set; }
        IEnumerable<decimal> YearlyRevenue { get; set; }
    }
}
