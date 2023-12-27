using System;
using System.Collections.Generic;

namespace FPT_Exchange_Data.Entities;

public partial class ProductTransferItem
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid ProductTransferId { get; set; }

    public string Status { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual ProductTransfer ProductTransfer { get; set; } = null!;
}
