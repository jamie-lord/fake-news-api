using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Name { get; set; }

        public virtual ICollection<News> News { get; set; }
    }
}