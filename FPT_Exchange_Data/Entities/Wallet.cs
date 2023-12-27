using System;
using System.Collections.Generic;

namespace FPT_Exchange_Data.Entities;

public partial class Wallet
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public int Score { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}
