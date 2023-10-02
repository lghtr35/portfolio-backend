using Microsoft.AspNetCore.Http;

namespace Portfolio.Backend.Common.Helpers
{
    public static class QueryConditionManager
    {
        public static bool IsFileExtensionAccepted(string[] accepted, IFormFile s)
        {
            var x = Path.GetExtension(s.FileName.ToLower());
            return accepted.Contains(x);
        }
    }
}

