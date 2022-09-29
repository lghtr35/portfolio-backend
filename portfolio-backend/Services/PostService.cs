using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class PostService : IPostService
    {
        private readonly AppDatabaseContext _context;
        public PostService(AppDatabaseContext context)
        {
            _context = context;
        }
        public async Task<Post> Create(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }
#nullable enable
        public async Task<IEnumerable<Post>> ReadAll(Dictionary<string, string> query)
        {
            if (query.Count == 0)
            {
                IEnumerable<Post> res = await _context.Posts.ToListAsync();
                return res;
            }
            else
            {
                IQueryable<Post> queryable = this._context.Posts;
                queryable = this.MakeQuery(queryable, query);
                IEnumerable<Post> res = await queryable.ToListAsync();
                return res;
            }
        }
#nullable disable
        public IQueryable<Post> MakeQuery(IQueryable<Post> queryable, Dictionary<string, string> query)
        {
            if (query.ContainsKey("postid"))
            {
                queryable = queryable.Where(item => item.PostId == Int16.Parse(query["postid"]));
            }
            if (query.ContainsKey("header"))
            {
                queryable = queryable.Where(item => item.Header == query["header"]);
            }
            if (query.ContainsKey("message"))
            {
                queryable = queryable.Where(item => item.Message == query["message"]);
            }
            if (query.ContainsKey("email"))
            {
                queryable = queryable.Where(item => item.Email == query["email"]);
            }
            if (query.ContainsKey("name"))
            {
                queryable = queryable.Where(item => item.Name == query["name"]);
            }
            if (query.ContainsKey("createdat"))
            {
                queryable = queryable.Where(item => item.CreatedAt.ToString() == query["createdAt"]);
            }
            if (query.ContainsKey("updatedat"))
            {
                queryable = queryable.Where(item => item.UpdatedAt.ToString() == query["updatedat"]);
            }
            return queryable;
        }
        public async Task<Post> ReadOne(int id)
        {
            Post res = await _context.Posts.Where(prop => prop.PostId == id).FirstOrDefaultAsync();
            return res;
        }

        public async Task<Post> Update(Post post)
        {
            post.UpdatedAt = DateTime.UtcNow;
            var res = await _context.Posts.FindAsync(post.PostId);
            Type type = res.GetType();
            if (res == null)
            {
                return null;
            }
            foreach (var prop in type.GetProperties())
            {
                var newValue = type.GetProperty(prop.Name).GetValue(post);
                var oldValue = type.GetProperty(prop.Name).GetValue(res);
                if (newValue == null)
                    continue;
                if (Object.Equals(oldValue, newValue))
                    continue;
                else
                {
                    type.GetProperty(prop.Name).SetValue(res, newValue);
                }
            }
            await _context.SaveChangesAsync();
            return res;
        }
        public async Task<Post> Delete(int id)
        {
            var deleted = await _context.Posts.Where(item => item.PostId == id).FirstOrDefaultAsync();
            _context.Remove(deleted);
            await _context.SaveChangesAsync();
            return deleted;
        }
    }
}