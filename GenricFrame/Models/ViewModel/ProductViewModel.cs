using GenricFrame.AppCode.Reops.Entities;
using Microsoft.AspNetCore.Http;

namespace GenricFrame.Models.ViewModel
{
    public class ProductViewModel:Product
    {

        public IFormFile file { get; set; }



    }
}
