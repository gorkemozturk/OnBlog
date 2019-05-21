using OnBlog.Service.Models.ViewModels.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnBlog.Service.Models.ViewModels.Post
{
    public class PostDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public DateTime PostedAt { get; set; }
        public string Owner { get; set; }
        public bool IsPublished { get; set; }
        public IEnumerable<TagViewModel> Tags { get; set; }
    }
}
