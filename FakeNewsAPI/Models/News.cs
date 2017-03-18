using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FakeNewsAPI.Models
{
    public class News
    {
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Publisher { get; set; }

        public DateTime Published { get; set; }

        public string ImageUrl { get; set; }

        public string[] Keywords { get; set; }

        [Required]
        [DefaultValue(0.0)]
        public double Score { get; set; }
    }
}