namespace Milkyfie.AppCode.Reops.Entities
{
    public class SMSAPI
    {
        public int ID { get; set; }
        public string ApiName { get; set; }
        public string ApiUrl { get; set; }
        public string Status { get; set; }

        public SmsTemplate SmsTemplate { get; set; }
    }
    public class SmsTemplate
    {
        public int SmsTemplateID { get; set; }
        public int SMSApiID { get; set; }
        public string TemplateName { get; set; }
        public string TemplateID { get; set; }
        public string MessageTemplate { get; set; }

    }

    public class SmsReport
    {
        public int SmsReportID { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public string RequestUrl { get; set; }
        public string Response { get; set; }
      
    }

}
