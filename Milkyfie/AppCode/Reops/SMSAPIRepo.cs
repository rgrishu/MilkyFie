using Dapper;
using Milkyfie.AppCode.DAL;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Milkyfie.AppCode.Reops
{
    public class SMSAPIRepo : ISMSAPI
    {
        private IDapperRepository _dapper;
        public SMSAPIRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }
        public async Task<Response> InsertSmsReportLog(SmsReport req)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("UserID", req.UserID);
            dbparams.Add("Name", req.Name);
            dbparams.Add("RequestUrl", req.RequestUrl);
            dbparams.Add("Response", req.Response);
            Response inires = await _dapper.InsertAsync<Response>("proc_InsertSmsReport", dbparams, commandType: CommandType.StoredProcedure);
            return inires;
        }
        public async Task<SMSAPI> GetSmsApi(string TemplateName)
        {
            SMSAPI smsapi=new SMSAPI();
            var dbparams = new DynamicParameters();
            dbparams.Add("TemplateName", TemplateName);

            string sqlQuery = @"proc_SelectSMSAPI";
            var res = await _dapper.GetAllAsyncProc<SMSAPI, SmsTemplate, SMSAPI>(new SMSAPI(), sqlQuery,
                dbparams, (smsapi, smsmtemplate) =>
                {
                    smsapi.SmsTemplate = smsmtemplate;
                    return smsapi;
                }, splitOn: "SMSApiID");
            smsapi = res.FirstOrDefault(); ;
            return smsapi;
        }
    }

}
