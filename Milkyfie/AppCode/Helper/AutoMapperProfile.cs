using AutoMapper;
using Milkyfie.AppCode.Reops.Entities;
using Milkyfie.Models;
using Milkyfie.Models.ViewModel;
namespace Milkyfie.AppCode.Helper
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<EmailConfig, EmailSettings>();
        }
    }
}
