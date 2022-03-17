using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Milkyfie.AppCode.Interfaces
{
    public interface IReport /*: IRepository<Report>*/
    {
        Task<IEnumerable<Ledger>> GetAllLedger(Ledger entity = null);
        Task<JDataTable<Ledger>> Ledger(jsonAOData filter = null);
    }
}
