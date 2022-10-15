using System;
namespace portfolio_backend.Data.DTOs.Common
{
    public class BaseControllerResponse
    {
        public string message { get; set; }
        public bool succeed { get; set; }
    }
}

