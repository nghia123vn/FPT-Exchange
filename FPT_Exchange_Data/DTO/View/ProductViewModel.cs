using FPT_Exchange_Data.Entities;

namespace FPT_Exchange_Data.DTO.View
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? Price { get; set; }

        public Guid AddById { get; set; }

        public Guid SellerId { get; set; }

        public Guid BuyerId { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual CategoryViewModel Category { get; set; } = null!;

        public virtual ICollection<ImageProductViewModel> ImageProducts { get; set; } = new List<ImageProductViewModel>();// create ImageProductViewModel

        public virtual StationViewModel Station { get; set; } = null!;// create StationProductViewModel

        public virtual StatusViewModel Status { get; set; } = null!;
    }

}

