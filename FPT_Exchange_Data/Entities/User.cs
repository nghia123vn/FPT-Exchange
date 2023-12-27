using System;
using System.Collections.Generic;

namespace FPT_Exchange_Data.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Avatar { get; set; }

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public Guid RoleId { get; set; }

    public Guid? StationId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<ProductActivy> ProductActivies { get; set; } = new List<ProductActivy>();

    public virtual ICollection<Product> ProductAddBies { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductBuyers { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductSellers { get; set; } = new List<Product>();

    public virtual ICollection<ProductTransfer> ProductTransfers { get; set; } = new List<ProductTransfer>();

    public virtual Role Role { get; set; } = null!;

    public virtual Station? Station { get; set; }

    public virtual Wallet? Wallet { get; set; }
}
