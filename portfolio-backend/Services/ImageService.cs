using System.Data;
using portfolio_backend.Data.Repository;
using portfolio_backend.Data.Entities;
using portfolio_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace portfolio_backend.Services
{
    public class ImageService : IImageService
    {
        private readonly AppDatabaseContext context;
        public ImageService(AppDatabaseContext _context)
        {
            this.context = _context;
        }
        public async Task<Image> CreateImage(Image img)
        {
            context.Images.Add(img);
            await context.SaveChangesAsync();
            return img;
        }
#nullable enable
        public async Task<IEnumerable<Image>> GetImages(Dictionary<string, string> query)
        {
            if (query.Count == 0)
            {
                IEnumerable<Image> res = await context.Images.ToListAsync();
                return res;
            }
            else
            {
                IQueryable<Image> queryable = this.context.Images;
                queryable = MakeQuery(queryable, query);
                IEnumerable<Image> res = await queryable.ToListAsync();
                return res;
            }
        }
#nullable disable
        private static IQueryable<Image> MakeQuery(IQueryable<Image> queryable, Dictionary<string, string> query)
        {
            if (query.ContainsKey("imageid"))
            {
                queryable = queryable.Where(item => item.ImageId == Int32.Parse(query["imageid"]));
            }
            if (query.ContainsKey("place"))
            {
                queryable = queryable.Where(item => item.PlacePath == query["place"]);
            }
            if (query.ContainsKey("imagename"))
            {
                queryable = queryable.Where(item => item.ImageName == query["imagename"]);
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
        public async Task<Image> GetImage(int id)
        {
            Image res = await context.Images.Where(prop => prop.ImageId == id).FirstOrDefaultAsync();
            return res;
        }
        public async Task<Image> UpdateImage(Image img)
        {
            img.UpdatedAt = DateTime.UtcNow;
            var res = await context.Images.FindAsync(img.ImageId);
            if (res == null)
            {
                return null;
            }
            Type type = img.GetType();
            foreach (var (prop, newValue) in from prop in type.GetProperties()
                                             let newValue = type.GetProperty(prop.Name).GetValue(img)
                                             where newValue != null
                                             select (prop, newValue))
            {
                type.GetProperty(prop.Name).SetValue(res, newValue);
            }

            await context.SaveChangesAsync();
            return res;
        }
        public async Task<IEnumerable<Image>> Delete(int[] id)
        {
            var deleted = await context.Images.Where(item => Array.IndexOf(id, item.ImageId) > -1).ToListAsync();
            context.Remove(deleted);
            await context.SaveChangesAsync();
            return deleted;
        }
    }
}