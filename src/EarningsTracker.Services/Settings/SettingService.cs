using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using EarningsTracker.Services.SqlServer;

namespace EarningsTracker.Services.Settings
{
    public class SettingService : ISettingService
    {
        private readonly IRepository _repo;

        public SettingService(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<Setting> GetIncomePerOfficeAsync(int officeId)
        {
            // execute the stored procedure called GetSettings
            return await _repo.WithConnection(async c =>
            {
                var p = new DynamicParameters();
                // make sure "Id" is being used since this is the name of the input parameter of the stored procedure
                p.Add("Id", officeId, DbType.Int32);
                // map the result from stored procedure to Setting data model
                var result = await c.QueryAsync<Setting>("GetIncomePerOffice", p, commandType: CommandType.StoredProcedure);
                // return first record or NULL
                return result.FirstOrDefault();
            });
        }

        public async Task<int> UpdateIncomePerOfficeAsync(Setting item)
        {
            return await _repo.WithConnection(async c =>
            {
                // create a list of input parameters
                var p = new DynamicParameters();
                p.Add("Id", item.Id, DbType.Int32);
                p.Add("Revenue2013", item.Revenue2013, DbType.Decimal);
                p.Add("Profit2013", item.Profit2013, DbType.Decimal);
                p.Add("Revenue2014", item.Revenue2014, DbType.Decimal);
                p.Add("Profit2014", item.Profit2014, DbType.Decimal);
                p.Add("Revenue2015", item.Revenue2015, DbType.Decimal);
                p.Add("Profit2015", item.Profit2015, DbType.Decimal);
                p.Add("Revenue2016", item.Revenue2016, DbType.Decimal);
                p.Add("Profit2016", item.Profit2016, DbType.Decimal);
                p.Add("Revenue2017", item.Revenue2017, DbType.Decimal);
                p.Add("Profit2017", item.Profit2017, DbType.Decimal);
                // return a scalar value from stored procedure
                var result = await c.ExecuteScalarAsync<int>("UpdateIncomePerOffice", p, commandType: CommandType.StoredProcedure);
                return result;
            });
        }

    }
}

