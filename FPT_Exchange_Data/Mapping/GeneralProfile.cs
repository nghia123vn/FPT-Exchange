using AutoMapper;
using FPT_Exchange_Data.DTO.View;
using FPT_Exchange_Data.Entities;

namespace FPT_Exchange_Data.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<Role, RoleViewModel>();
            CreateMap<Station, StationViewModel>();
            CreateMap<Wallet, WalletViewModel>();
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<ImageProduct, ImageProductViewModel>();
            CreateMap<Status, StatusViewModel>();
            CreateMap<ProductActivy, ProductActivityViewModel>();
            CreateMap<Product, ProductActivityItemViewModel>()
                      .ForMember(vm => vm.ImageProducts, otp => otp.MapFrom(product => product.ImageProducts.FirstOrDefault()));
        }
    }
}
