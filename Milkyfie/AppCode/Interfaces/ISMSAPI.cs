using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Milkyfie.AppCode.Interfaces
{
    public interface ISMSAPI
    {
        Task<SMSAPI> GetSmsApi(string TemplateName);
        Task<Response> InsertSmsReportLog(SmsReport req);
    }
}
