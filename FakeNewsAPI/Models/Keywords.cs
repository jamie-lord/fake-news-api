using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FakeNewsAPI.Models
{
    public class Keyword
    {
        public Keyword()
        {
            News = new HashSet<News>();
        }

        [Required]
        [Key]
        public string Name { get; set; }

        public virtual ICollection<News> News { get; set; }
    }
}