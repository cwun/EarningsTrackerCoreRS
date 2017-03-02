using System;

namespace EarningsTracker.Services.Settings
{
    public interface IIncome
    {
        int Id { get; set; }
        string Name { get; set; }
        decimal Revenue2013 { get; set; }
        decimal Profit2013 { get; set; }
        decimal Revenue2014 { get; set; }
        decimal Profit2014 { get; set; }
        decimal Revenue2015 { get; set; }
        decimal Profit2015 { get; set; }
        decimal Revenue2016 { get; set; }
        decimal Profit2016 { get; set; }
        decimal Revenue2017 { get; set; }
        decimal Profit2017 { get; set; }
    }
}
