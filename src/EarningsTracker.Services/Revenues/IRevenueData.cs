namespace EarningsTracker.Services.Revenues
{
    public interface IRevenueData
    {
        int Id { get; set; }
        // name of a specific office
        string Name { get; set; }
        // total revenue percent for a specific office
        decimal Percent { get; set; }
        // total revenue amount for a specific office
        decimal Revenue { get; set; }
        // total revenue amount for other offices
        decimal Others { get; set; }
    }
}
