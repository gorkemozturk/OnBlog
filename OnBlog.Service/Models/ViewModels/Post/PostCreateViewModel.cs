using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnBlog.Service.Models.ViewModels.Post
{
    public class PostCreateViewModel
    {
        public string UserId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        [MaxLength(150)]
        public string Slug { get; set; }

        [Required]
        [MaxLength(250)]
        public string Summary { get; set; }

        [Required]
        public string Body { get; set; }
        public DateTime PostedAt { get; set; }
        public bool IsPublished { get; set; }

        public ICollection<int> Tags { get; set; }
    }
}
