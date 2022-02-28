using Milkyfie.AppCode.Reops.Entities;
using Microsoft.AspNetCore.Http;

namespace Milkyfie.Models.ViewModel
{
    public class ProductViewModel:Product
    {

        public IFormFile file { get; set; }



    }
}
