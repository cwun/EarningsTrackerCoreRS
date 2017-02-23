using System.Collections.Generic;
using EarningsTracker.Services.Revenues;

namespace EarningsTracker.Services.Charts
{
    public class ChartData : IChartData
    {
        public RevenueData TotalRevenue { get; set; }
        public IEnumerable<decimal> YearlyProfit { get; set; }
        public IEnumerable<decimal> YearlyRevenue { get; set; }
    }
}
