using GenricFrame.AppCode.Reops.Entities;

namespace GenricFrame.Models
{
    public class EmailSettings: EmailConfig
    {
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
