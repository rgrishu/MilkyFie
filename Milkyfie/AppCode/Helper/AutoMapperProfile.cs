using AutoMapper;
using GenricFrame.AppCode.Reops.Entities;
using GenricFrame.Models;
using GenricFrame.Models.ViewModel;
namespace GenricFrame.AppCode.Helper
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
