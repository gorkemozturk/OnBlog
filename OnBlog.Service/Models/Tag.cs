using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnBlog.Service.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string TagName { get; set; }
    }
}
