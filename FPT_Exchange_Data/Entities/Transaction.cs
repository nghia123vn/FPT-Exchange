using System;
using System.Collections.Generic;

namespace FPT_Exchange_Data.Entities;

public partial class Transaction
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid WalletId { get; set; }

    public int Amount { get; set; }

    public int Fee { get; set; }

    public int Receive { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;
}
