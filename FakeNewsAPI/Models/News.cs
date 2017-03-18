using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FakeNewsAPI.Models
{
    public class News
    {
        public int Id { get; set; }

        [Required]
        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Url { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public virtual Source Source { get; set; }

        public DateTime? Published { get; set; }

        public DateTime? Updated { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<string> Authors { get; set; }
        public string AuthorsString
        {
            get { return (Authors != null) ? string.Join("^", Authors) : null; }
            set { Authors = value.Split('^').ToList(); }
        }

        public string Summary { get; set; }

        public ICollection<string> Categories { get; set; }
        public string CategoriesString
        {
            get { return (Categories != null) ? string.Join("^", Categories) : null; }
            set { Categories = value.Split('^').ToList(); }
        }

        public ICollection<string> Keywords { get; set; }
        public string KeywordsString
        {
            get { return (Keywords != null) ? string.Join("^", Keywords) : null; }
            set { Keywords = value.Split('^').ToList(); }
        }

        [Required]
        [DefaultValue(0.0)]
        public double Score { get; set; }
    }
}