using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FakeNewsAPI.Models
{
    public class Source
    {
        public int Id { get; set; }

        [Required]
        public string RSSurl { get; set; }

        public string Title { get; set; }

        public DateTime LastScrape { get; set; }

        [Required]
        [DefaultValue(0.0)]
        public double Score { get; set; }
    }
}