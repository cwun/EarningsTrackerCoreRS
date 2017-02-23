using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EarningsTracker.Services.Charts;
using EarningsTracker.Services.SqlServer;
using EarningsTracker.Services.Revenues;

namespace EarningsTracker.Services.Dashboards
{
    public class DashboardService : IDashboardService
    {
        private readonly IRepository _repo;

        public DashboardService(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<Dashboard> GetDashboardSettingAsync()
        {
            return await _repo.WithConnection(async c =>
            {
                // execute the stored procedure called GetDashboardSetting
                var reader = await c.QueryMultipleAsync("GetDashboardSetting", commandType: CommandType.StoredProcedure);

                // map the result from stored procedure to Dashboard data model
                var results = new Dashboard
                {
                    Dallas = new ChartData
                    {
                        TotalRevenue = reader.ReadAsync<RevenueData>().Result.SingleOrDefault(),
                        YearlyProfit = reader.ReadAsync<decimal>().Result.AsEnumerable(),
                        YearlyRevenue = reader.ReadAsync<decimal>().Result.AsEnumerable()
                    },
                    Seattle = new ChartData
                    {
                        TotalRevenue = reader.ReadAsync<RevenueData>().Result.SingleOrDefault(),
                        YearlyProfit = reader.ReadAsync<decimal>().Result.AsEnumerable(),
                        YearlyRevenue = reader.ReadAsync<decimal>().Result.AsEnumerable()
                    },
                    Boston = new ChartData
                    {
                        TotalRevenue = reader.ReadAsync<RevenueData>().Result.SingleOrDefault(),
                        YearlyProfit = reader.ReadAsync<decimal>().Result.AsEnumerable(),
                        YearlyRevenue = reader.ReadAsync<decimal>().Result.AsEnumerable()
                    }
                };
                return results;
            });
        }
    }
}
