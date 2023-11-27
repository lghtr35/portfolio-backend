namespace Portfolio.Backend.Common.Data.Responses.Common
{
    public class BaseControllerResponse
    {
        public string message { get; set; } = "";
        public bool succeed { get; set; } = true;
        public string reason { get; set; } = "";
    }
}

