using Microsoft.EntityFrameworkCore;
using Portfolio.Backend.Common;
using Portfolio.Backend.Common.Data.Entities;
using Portfolio.Backend.Common.Data.Repository;
using Portfolio.Backend.Common.Data.Requests.Content;
using Portfolio.Backend.Common.Data.Responses.Common;
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

        public async Task<IDictionary<string, ContentLayoutResponse>> GetContentsByPlace()
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

        public async Task<PageResponse<ContentResponse>> GetContents(ContentFilterRequest dto)
        {
            PageResponse<ContentResponse> response = new();
            IQueryable<Content> queryable = _context.Contents;
            queryable = MakeQuery(queryable, dto);
            IEnumerable<ContentResponse> list = await queryable.Select(c => new ContentResponse(c)).ToListAsync();
            response.TotalRecords = list.Count();
            int remaining = response.TotalRecords - dto.Page * dto.Size;
            response.Content = list.Skip(dto.Page * dto.Size).Take(dto.Size).ToArray();
            response.ItemsInPage = dto.Size;
            if (dto.Size > remaining)
            {
                response.ItemsInPage = response.TotalRecords - dto.Page * dto.Size;
            }
            response.PageSize = dto.Size;
            response.PageNumber = dto.Page;
            return response;
        }
        private static IQueryable<Content> MakeQuery(IQueryable<Content> queryable, ContentFilterRequest query)
        {
            if (query.IdList != null)
            {
                queryable = queryable.Where(item => query.IdList.Contains(item.ContentId));
            }
            if (query.PlaceSearchString != null)
            {
                queryable = queryable.Where(item => item.Place.Contains(query.PlaceSearchString));
            }
            if (query.LocationSearchString != null)
            {
                queryable = queryable.Where(item => item.Location.Contains(query.LocationSearchString));
            }
            if (query.PayloadSearchString != null)
            {
                queryable = queryable.Where(item => item.Payload.Contains(query.PayloadSearchString));
            }
            if (query.CreatedAtSearchString != null)
            {
                queryable = queryable.Where(item => item.CreatedAt.ToString().Contains(query.CreatedAtSearchString));
            }
            if (query.UpdatedAtSearchString != null)
            {
                queryable = queryable.Where(item => item.UpdatedAt.ToString().Contains(query.UpdatedAtSearchString));
            }
            return queryable.OrderBy(p => p.CreatedAt);
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

