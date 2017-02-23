using EarningsTracker.Services.Charts;

namespace EarningsTracker.Services.Dashboards
{
    public class Dashboard : IDashboard
    {
        public ChartData Dallas { get; set; }
        public ChartData Seattle { get; set; }
        public ChartData Boston { get; set; }
    }
}
