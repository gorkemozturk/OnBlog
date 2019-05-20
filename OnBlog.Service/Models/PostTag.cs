using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnBlog.Service.Models
{
    public class PostTag
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int TagId { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; }

        [ForeignKey("TagId")]
        public Tag Tag { get; set; }
    }
}
