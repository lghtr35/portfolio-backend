using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.Extensions.Configuration;

namespace backend.Services
{
    public interface IPostService
    {
        Task<Post> Create(Post post);

#nullable enable
        Task<IEnumerable<Post>> ReadAll(Dictionary<string, string> query);
        Task<Post> ReadOne(int id);
#nullable disable
        Task<Post> Update(Post post);
        Task<Post> Delete(int id);

    }
}