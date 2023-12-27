using System;
using System.Collections.Generic;

namespace FPT_Exchange_Data.Entities;

public partial class ProductTransfer
{
    public Guid Id { get; set; }

    public Guid StationIdform { get; set; }

    public Guid StationIdto { get; set; }

    public Guid UserId { get; set; }

    public DateTime DateTime { get; set; }

    public virtual ICollection<ProductTransferItem> ProductTransferItems { get; set; } = new List<ProductTransferItem>();

    public virtual Station StationIdformNavigation { get; set; } = null!;

    public virtual Station StationIdtoNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
