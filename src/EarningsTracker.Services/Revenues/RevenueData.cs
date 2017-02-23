namespace EarningsTracker.Services.Revenues
{
    public class RevenueData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Percent { get; set; }
        public decimal Revenue { get; set; }
        public decimal Others { get; set; }
    }
}
