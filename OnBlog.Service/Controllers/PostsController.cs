using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnBlog.Service.Data;
using OnBlog.Service.Models;
using OnBlog.Service.Models.ViewModels.Post;

namespace OnBlog.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.Posts.ToListAsync();
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // PUT: api/Posts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, PostUpdateViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var post = await _context.Posts.FindAsync(id);

            post.Title = model.Title;
            post.Slug = model.Slug;
            post.Summary = model.Summary;
            post.Body = model.Body;
            post.IsPublished = model.IsPublished;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Posts
        [HttpPost]
        public async Task<ActionResult<PostCreateViewModel>> PostPost(PostCreateViewModel model)
        {
            var post = new Post()
            {
                UserId = model.UserId,
                Title = model.Title,
                Slug = model.Slug,
                Summary = model.Summary,
                Body = model.Body,
                IsPublished = model.IsPublished,
                PostedAt = DateTime.Now
            };

            _context.Posts.Add(post);

            foreach (var id in model.Tags)
            {
                var tag = await _context.Tags.FindAsync(id);
                var postTag = new PostTag() { PostId = post.Id, TagId = tag.Id };

                _context.PostTags.Add(postTag);
            }

            await _context.SaveChangesAsync();

            return model;
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return post;
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
