using System.Threading.Tasks;

namespace EarningsTracker.Services.Settings
{
    public interface ISettingService
    {
        Task<Setting> GetIncomePerOfficeAsync(int id);
        Task<int> UpdateIncomePerOfficeAsync(Setting item);
    }
}
