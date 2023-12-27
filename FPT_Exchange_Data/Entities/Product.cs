using System;
using System.Collections.Generic;

namespace FPT_Exchange_Data.Entities;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Price { get; set; }

    public Guid CategoryId { get; set; }

    public Guid StatusId { get; set; }

    public Guid StationId { get; set; }

    public Guid AddById { get; set; }

    public Guid? SellerId { get; set; }

    public Guid? BuyerId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User AddBy { get; set; } = null!;

    public virtual User? Buyer { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ImageProduct> ImageProducts { get; set; } = new List<ImageProduct>();

    public virtual ICollection<ProductActivy> ProductActivies { get; set; } = new List<ProductActivy>();

    public virtual ICollection<ProductTransferItem> ProductTransferItems { get; set; } = new List<ProductTransferItem>();

    public virtual User? Seller { get; set; }

    public virtual Station Station { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
