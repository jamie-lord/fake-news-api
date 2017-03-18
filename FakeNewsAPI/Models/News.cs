using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public Source Source { get; set; }

        public DateTime? Published { get; set; }

        public DateTime? Updated { get; set; }

        public string ImageUrl { get; set; }

        public List<string> Authors { get; set; }

        public string Summary { get; set; }

        [Index]
        public List<string> Keywords { get; set; }

        [Required]
        [DefaultValue(0.0)]
        public double Score { get; set; }
    }
}