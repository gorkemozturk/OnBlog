using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnBlog.Service.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        [MaxLength(250)]
        public string Summary { get; set; }

        [Required]
        public string Body { get; set; }
        public DateTime PostedAt { get; set; }

        public ApplicationUser Owner { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
