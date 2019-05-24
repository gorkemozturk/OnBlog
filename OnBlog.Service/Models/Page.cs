using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnBlog.Service.Models
{
    public class Page
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        [MaxLength(150)]
        public string Slug { get; set; }

        [Required]
        public string Body { get; set; }
        public DateTime PublishedAt { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsPublished { get; set; }
    }
}
