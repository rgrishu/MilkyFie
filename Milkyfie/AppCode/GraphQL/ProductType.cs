using HotChocolate.Types;
using Milkyfie.AppCode.Reops.Entities;

namespace Milkyfie.AppCode.GraphQL
{
    public class ProductType : ObjectType<Product>
    {
    }
    public class List<ProductType> : ObjectType<Product>
    {
    }
}
