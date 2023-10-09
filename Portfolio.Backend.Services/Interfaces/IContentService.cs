using System;
using Portfolio.Backend.Common.Data.Requests.Content;
using Portfolio.Backend.Common.Data.Responses.Common;
using Portfolio.Backend.Common.Data.Responses.Content;

namespace Portfolio.Backend.Services.Interfaces
{
    public interface IContentService
    {
        Task<ContentResponse> CreateContent(ContentCreateRequest contentDTO);
        Task<IDictionary<string, ContentLayoutResponse>> GetContents();
        Task<ContentLayoutResponse> GetPageContent(ContentGetPageRequest contentDTO);
        Task<ContentResponse?> UpdateContent(ContentUpdateRequest contentDTO);
        Task<ContentResponse?> DeleteContent(int id);
        Task<ContentResponse?> GetContent(int id);
    }
}

