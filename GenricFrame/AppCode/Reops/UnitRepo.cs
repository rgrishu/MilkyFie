using Dapper;
using GenricFrame.AppCode.DAL;
using GenricFrame.AppCode.Interfaces;
using GenricFrame.AppCode.Reops.Entities;
using GenricFrame.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;

namespace GenricFrame.AppCode.Reops
{
    public class UnitRepo: IRepository<Unit>
    {
        private IDapperRepository _dapper;
        public UnitRepo(IDapperRepository dapper)
        {
           
            _dapper = dapper;
        }
        public Task<Response> AddAsync(Unit entity)
        {
            throw new System.NotImplementedException();
        }
        public Task<Response> DeleteAsync(int id)
        {
            throw new System.NotImplementedException();
        }
        public Task<IEnumerable<Unit>> GetAllAsync(Unit entity = null)
        {
            throw new System.NotImplementedException();
        }
        public Task<Response<Unit>> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }
        public async Task<IReadOnlyList<Unit>> GetDropdownAsync(Unit entity)
        {
            var dbparams = new DynamicParameters();
            var res = await _dapper.GetAllAsync<Unit>("select UnitID,concat(UnitName,' ( ',ShortName,' ) ')UnitName,ShortName,IsActive from Unit where IsActive=1 order by UnitName", dbparams, commandType: CommandType.Text);
            return res.ToList();
        }
    }
}
