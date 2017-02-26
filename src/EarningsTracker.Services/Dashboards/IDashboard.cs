using EarningsTracker.Services.Charts;

namespace EarningsTracker.Services.Dashboards
{
    public interface IDashboard
    {
        // data points on charts that represent the earnings report of the Dallas office
        ChartData Dallas { get; set; }
        // data points on charts that represent the earnings report of the Seattle office
        ChartData Seattle { get; set; }
        // data points on charts that represent the earnings report of the Boston office
        ChartData Boston { get; set; }
    }
}
