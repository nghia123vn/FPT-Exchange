using FPT_Exchange_Data.Entities;

namespace FPT_Exchange_Data.DTO.View
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Avatar { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Status { get; set; } = null!;


        //public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        //public virtual ICollection<ProductActivy> ProductActivies { get; set; } = new List<ProductActivy>();

        //public virtual ICollection<Product> ProductAddBies { get; set; } = new List<Product>();

        //public virtual ICollection<Product> ProductBuyers { get; set; } = new List<Product>();

        //public virtual ICollection<Product> ProductSellers { get; set; } = new List<Product>();

        //public virtual ICollection<ProductTransfer> ProductTransfers { get; set; } = new List<ProductTransfer>();

        public virtual RoleViewModel? Role { get; set; }

        public virtual StationViewModel? Station { get; set; }

        public virtual WalletViewModel? Wallet { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
