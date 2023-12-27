using System;
using System.Collections.Generic;

namespace FPT_Exchange_Data.Entities;

public partial class Status
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ProductActivy> ProductActivyNewStatusNavigations { get; set; } = new List<ProductActivy>();

    public virtual ICollection<ProductActivy> ProductActivyOldStatusNavigations { get; set; } = new List<ProductActivy>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
