using HotChocolate;
using HotChocolate.Types;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops;
using Milkyfie.AppCode.Reops.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milkyfie.AppCode.GraphQL.QueryResolver
{
    [ExtendObjectType("Query")]
    public class ProductQueryResolver
    {
        public Product GetById([Service] IProduct productService)
        {
            var response = productService.GetByIdAsync(0).Result;
            return response.Result;
        }

        public IEnumerable<Product> GetAllProduct([Service] IProduct productService)
        {
            var response = productService.GetAllAsync(new Product()).Result;
            return response;
        }
    }
}
