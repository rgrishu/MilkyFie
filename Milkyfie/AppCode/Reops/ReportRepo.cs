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
        public Task<Response> AddAsync(Report entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Report>> GetAllAsync(Report entity = null)
        {
            throw new NotImplementedException();
        }

        public async Task<JDataTable<Ledger>> GetMultiSplits(Report entity = null)
        {
            JDataTable<Ledger> d = new JDataTable<Ledger>();
            try
            {
                d = await _dapper.GetMultipleAsync<Ledger, ApplicationUser, Ledger>("proc_selectLedger", new { UserID = 0 },
                    (ledger, applicationuser) =>
                    {
                        ledger.User = applicationuser;
                        return ledger;
                    }, splitOn: "LedgerID,UserID");
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

        public Task<Response<Report>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Report> GetDetails(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Report>> GetDropdownAsync(Report entity)
        {
            throw new NotImplementedException();
        }
    }

}
