using System.Threading.Tasks;

namespace EarningsTracker.Services.Settings
{
    public interface ISettingService
    {
        Task<Income> GetIncomePerOfficeAsync(int id);
        Task<int> UpdateIncomePerOfficeAsync(Income item);
    }
}
