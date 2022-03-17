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
    public class ReportRepo : IReport
    {
        private IDapperRepository _dapper;
        public ReportRepo(IDapperRepository dapper)
        {
            _dapper = dapper;
        }

        public async Task<JDataTable<Ledger>> Ledger(jsonAOData filter = null)
        {
            JDataTable<Ledger> d = new JDataTable<Ledger>();
            try
            {
                d = await _dapper.GetMultipleAsync<Ledger, ApplicationUser, Ledger>("proc_selectLedger", filter,
                    (ledger, applicationuser) =>
                    {
                        ledger.User = applicationuser;
                        return ledger;
                    }, splitOn: "LedgerID,UserID");
                  d.recordsFiltered = d.PageSetting.TotoalRows;//d.Data.Count();
               // d.recordsFiltered = d.Data.Count();
                d.recordsTotal = d.PageSetting.TotoalRows;
            }
            catch (Exception ex)
            {

            }
            return d;
        }

        public async Task<IEnumerable<Ledger>> GetAllLedger(Ledger entity = null)
        {
            IEnumerable<Ledger> ledger = new List<Ledger>();
            try
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("UserID", entity != null && entity.User != null ? entity.User.Id : 0);
                dbparams.Add("CreatedOn", entity != null ? entity.CreatedOn : String.Empty);
                string sqlQuery = @"proc_selectLedger";
                var res = await _dapper.GetAllAsyncProc<Ledger, ApplicationUser, Ledger>(entity ?? new Ledger(), sqlQuery,
                    dbparams, (ledger, applicationuser) =>
                    {
                        ledger.User = applicationuser;
                        return ledger;
                    }, splitOn: "LedgerID,UserID");
                ledger = res;
            }
            catch (Exception ex)
            {

            }
            return ledger;

        }

        private DynamicParameters prepareParam(jsonAOData param)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add(nameof(param.draw), param.draw);
            return p;
        }
    }
}
