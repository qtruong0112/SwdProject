using System;
using System.Collections.Generic;

namespace SwdProject.Models;

public partial class Order
{
    public long Id { get; set; }

    public double TotalPrice { get; set; }

    public long? UserId { get; set; }

    public string? ReceiverAddress { get; set; }

    public string? ReceiverName { get; set; }

    public string? ReceiverPhone { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User? User { get; set; }
}
