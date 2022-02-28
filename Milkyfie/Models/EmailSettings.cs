using Milkyfie.AppCode.Reops.Entities;

namespace Milkyfie.Models
{
    public class EmailSettings: EmailConfig
    {
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
