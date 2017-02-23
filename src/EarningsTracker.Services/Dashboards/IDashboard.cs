using EarningsTracker.Services.Charts;

namespace EarningsTracker.Services.Dashboards
{
    public interface IDashboard
    {
        ChartData Dallas { get; set; }
        ChartData Seattle { get; set; }
        ChartData Boston { get; set; }
    }
}
