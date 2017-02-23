using System.Threading.Tasks;

namespace EarningsTracker.Services.Dashboards
{
    public interface IDashboardService
    {
        Task<Dashboard> GetDashboardSettingAsync();
    }
}
