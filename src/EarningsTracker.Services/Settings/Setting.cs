using System;
using System.ComponentModel.DataAnnotations;

namespace EarningsTracker.Services.Settings
{
    public class Setting : ISetting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Revenue for 2013")]
        public decimal Revenue2013 { get; set; }

        [Required]
        [Display(Name = "Profit for 2013")]
        public decimal Profit2013 { get; set; }

        [Required]
        [Display(Name = "Revenue for 2014")]
        public decimal Revenue2014 { get; set; }

        [Required]
        [Display(Name = "Profit for 2014")]
        public decimal Profit2014 { get; set; }

        [Required]
        [Display(Name = "Revenue for 2015")]
        public decimal Revenue2015 { get; set; }

        [Required]
        [Display(Name = "Profit for 2015")]
        public decimal Profit2015 { get; set; }

        [Required]
        [Display(Name = "Revenue for 2016")]
        public decimal Revenue2016 { get; set; }

        [Required]
        [Display(Name = "Profit for 2016")]
        public decimal Profit2016 { get; set; }

        [Required]
        [Display(Name = "Revenue for 2017")]
        public decimal Revenue2017 { get; set; }

        [Required]
        [Display(Name = "Profit for 2017")]
        public decimal Profit2017 { get; set; }
    }
}
