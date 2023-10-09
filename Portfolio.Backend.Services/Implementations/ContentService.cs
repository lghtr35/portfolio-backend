using Microsoft.EntityFrameworkCore;
using Portfolio.Backend.Common.Data.Entities;
using Portfolio.Backend.Common.Data.Repository;
using Portfolio.Backend.Common.Data.Requests.Content;
using Portfolio.Backend.Common.Data.Responses.Content;
using Portfolio.Backend.Common.Exceptions;
using Portfolio.Backend.Services.Interfaces;

namespace Portfolio.Backend.Services.Implementations
{
    public class ContentService : IContentService
    {
        private readonly AppDatabaseContext _context;

        public ContentService(AppDatabaseContext context)
        {
            _context = context;
        }

        public async Task<ContentResponse> CreateContent(ContentCreateRequest contentDTO)
        {
            Content content = new();
            content.Location = contentDTO.Location;
            content.Place = contentDTO.Place;
            content.Payload = contentDTO.Payload;

            _context.Contents.Add(content);
            await _context.SaveChangesAsync();
            return new ContentResponse(content);
        }

        public async Task<ContentResponse?> DeleteContent(int id)
        {
            var deleted = await _context.Contents.Where(item => item.ContentId == id).FirstOrDefaultAsync();
            if (deleted == null)
            {
                throw new ObjectNotFoundException("No project with the given id have been found");
            }
            _context.Remove(deleted);
            await _context.SaveChangesAsync();
            return new ContentResponse(deleted);
        }

        public async Task<ContentResponse?> GetContent(int id)
        {
            Content? res = await _context.Contents.Where(prop => prop.ContentId == id).FirstOrDefaultAsync();
            if (res == null)
            {
                throw new ObjectNotFoundException("No project with the given id have been found");
            }
            return new ContentResponse(res);
        }

        public async Task<IDictionary<string, ContentLayoutResponse>> GetContents()
        {
            IList<string> places = await _context.Contents
                .Select(p => p.Place)
                .Distinct()
                .ToListAsync();

            IDictionary<string, ContentLayoutResponse> res = new Dictionary<string, ContentLayoutResponse>();

            foreach (string place in places)
            {
                res.Add(place, await GetPageContent(new ContentGetPageRequest(place)));
            }
            return res;
        }

        public async Task<ContentLayoutResponse> GetPageContent(ContentGetPageRequest contentDTO)
        {
            IList<Content> contents = await _context.Contents.Where(p => p.Place == contentDTO.Place).ToListAsync();
            IDictionary<string, ContentResponse> payloads = new Dictionary<string, ContentResponse>();
            foreach (var content in contents)
            {
                payloads.Add(content.Location, new ContentResponse(content));
            }
            ContentLayoutResponse layoutResponse = new();
            layoutResponse.Place = contentDTO.Place;
            layoutResponse.Contents = payloads;
            return layoutResponse;
        }

        public async Task<ContentResponse?> UpdateContent(ContentUpdateRequest contentDTO)
        {
            Content? res = await _context.Contents.Where(prop => prop.ContentId == contentDTO.ContentId).FirstOrDefaultAsync();
            if (res == null)
            {
                throw new ObjectNotFoundException("No project with the given id have been found");
            }

            if (!string.IsNullOrEmpty(contentDTO.Location))
            {
                res.Location = contentDTO.Location;
            }
            if (!string.IsNullOrEmpty(contentDTO.Payload))
            {
                res.Payload = contentDTO.Payload;
            }
            if (!string.IsNullOrEmpty(contentDTO.Place))
            {
                res.Place = contentDTO.Place;
            }

            res.UpdatedAt = DateTime.UtcNow;
            _context.Update(res);
            await _context.SaveChangesAsync();
            return new ContentResponse(res);
        }
    }
}

