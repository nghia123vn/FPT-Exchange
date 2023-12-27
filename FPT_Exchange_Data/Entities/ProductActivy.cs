using System;
using System.Collections.Generic;

namespace FPT_Exchange_Data.Entities;

public partial class ProductActivy
{
    public Guid Id { get; set; }

    public string? ActionType { get; set; }

    public Guid UserId { get; set; }

    public Guid ProductId { get; set; }

    public Guid StationsId { get; set; }

    public Guid OldStatus { get; set; }

    public Guid? NewStatus { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual Status? NewStatusNavigation { get; set; }

    public virtual Status OldStatusNavigation { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Station Stations { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
