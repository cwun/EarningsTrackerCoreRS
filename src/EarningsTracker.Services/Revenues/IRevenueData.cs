namespace EarningsTracker.Services.Revenues
{
    public interface IRevenueData
    {
        int Id { get; set; }
        string Name { get; set; }
        int Percent { get; set; }
        decimal Revenue { get; set; }
        decimal Others { get; set; }
    }
}
