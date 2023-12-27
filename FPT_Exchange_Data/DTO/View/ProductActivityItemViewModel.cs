namespace FPT_Exchange_Data.DTO.View
{
    public class ProductActivityItemViewModel
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

        public virtual StationViewModel Station { get; set; } = null!;// create StationProductViewModel

        public virtual StatusViewModel Status { get; set; } = null!;
        public virtual ImageProductViewModel ImageProducts { get; set; } = null!;// create ImageProductViewModel

    }
}
