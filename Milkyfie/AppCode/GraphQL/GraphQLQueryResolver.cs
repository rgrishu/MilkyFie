using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Components;
using Milkyfie.AppCode.Interfaces;
using Milkyfie.AppCode.Reops.Entities;
using System.Collections.Generic;

namespace Milkyfie.AppCode.GraphQL.ProductQL
{
    [ExtendObjectType("Query")]
    public class GraphQLQueryResolver
    {
        public Reops.Entities.Product GetProductById([Service] IProduct productService, [Argument] int id)
        {
            var response = productService.GetByIdAsync(0).Result;
            return response.Result;
        }

        /// <Request>
        /* 
         **** sample Request on Banana cake pop ****
         
            query($searchByProduct:ProductInput){
                       allProduct(argument:$searchByProduct){
                       productID
                 }
            }
         
         **** sample Request on Postman ****
         
         * {
              "query": "query($searchByProduct:ProductInput){\r\n  allProduct(argument:$searchByProduct){\r\n    productID\r\n  }\r\n}",
              "variables": {
                "searchByProduct": {
                  "productID": 85,
                  "discount": 0,
                  "isDiscount": false,
                  "mRP": 0,
                  "sellingPrice": 0
                }
              }
            }*/
        /// </Request>
        public IEnumerable<Reops.Entities.Product> GetAllProduct([Service] IProduct productService, Reops.Entities.Product argument)
        {
            var response = productService.GetAllAsync(argument).Result;
            return response;
        }

        public IEnumerable<Category> GetAllCategory([Service] IRepository<Category> categoryService, Category argument)
        {
            var response = categoryService.GetAllAsync(argument).Result;
            return response;
        }
    }
}
