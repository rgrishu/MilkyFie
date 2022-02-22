
using GenricFrame.AppCode.CustomAttributes;
using GenricFrame.AppCode.Reops;
using GenricFrame.AppCode.Reops.Entities;
using GenricFrame.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GenricFrame.AppCode.Helper
{

    public class AppUtility
    {
        public static AppUtility O => instance.Value;
        private static Lazy<AppUtility> instance = new Lazy<AppUtility>(() => new AppUtility());
        private AppUtility() { }
        public Response UploadFile(FileUploadModel request)
        {
            var response = Validate.O.IsFileValid(request.file);
            if (response.StatusCode == Status.Success)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(request.FilePath);
                    if (!Directory.Exists(sb.ToString()))
                    {
                        Directory.CreateDirectory(sb.ToString());
                    }
                    var filename = ContentDispositionHeaderValue.Parse(request.file.ContentDisposition).FileName.Trim('"');
                    string originalExt = Path.GetExtension(filename).ToLower();
                    string[] Extensions = { ".png", ".jpeg", ".jpg" };
                    if (Extensions.Contains(originalExt))
                    {
                        //originalExt = ".jpg";
                    }
                    string originalFileName = Path.GetFileNameWithoutExtension(filename).ToLower() + originalExt;
                    if (!string.IsNullOrEmpty(request.FileName))
                    {
                        request.FileName = Path.GetFileNameWithoutExtension(request.FileName).ToLower() + originalExt;
                    }
                    request.FileName = string.IsNullOrEmpty(request.FileName) ? originalFileName.Trim() : request.FileName;
                    sb.Append(request.FileName);
                    using (FileStream fs = File.Create(sb.ToString()))
                    {
                        request.file.CopyTo(fs);
                        fs.Flush();
                    }
                    response.StatusCode = Status.Success;
                    response.ResponseText = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    response.ResponseText = "Error in file uploading. Try after sometime...";
                   // response.Error = ex.Message;
                }
            }
            return response;
        }

        public async Task SendMail(EmailSettings setting)
        {
            await Task.Delay(0);
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(setting.EmailFrom);
                    mail.To.Add(setting.EmailTo);
                    mail.Subject = setting.Subject;
                    mail.Body = setting.Body;
                    mail.IsBodyHtml = true;
                    //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment
                    using (SmtpClient smtp = new SmtpClient(setting.smtpAddress, setting.Port))
                    {
                        smtp.Credentials = new NetworkCredential(setting.EmailFrom, setting.Password);
                        smtp.EnableSsl = setting.enableSSL;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
