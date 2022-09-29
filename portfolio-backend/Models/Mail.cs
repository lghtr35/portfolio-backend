using System;


namespace backend.Models
{
    public class Mail
    {
        public string Destination { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
    }
}
