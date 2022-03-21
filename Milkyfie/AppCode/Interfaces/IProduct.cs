using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Milkyfie.AppCode.Interfaces
{
    public interface IProduct : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProduct(Product entity = null);
        Task<Product> GetAllProductDetailApi(Product entity = null);
    }
}
