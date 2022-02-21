using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenricFrame.Models
{
    public class EmailSettings
    {
        public string EmailFrom { get; set; } = "Mail@Roundpaytech.Com";
        public string EmailTo { get; set; }
        public string Password { get; set; } = "Info@2015";
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool enableSSL { get; set; } = true;
        public int Port { get; set; } = 587;
        public string smtpAddress { get; set; } = "mail.roundpaytech.com";
    }
}
