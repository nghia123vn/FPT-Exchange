using System;
using System.Collections.Generic;

namespace FPT_Exchange_Data.Entities;

public partial class Notification
{
    public Guid Id { get; set; }

    public string? Description { get; set; }

    public DateTime CreateAt { get; set; }

    public Guid SendTo { get; set; }

    public virtual User SendToNavigation { get; set; } = null!;
}
