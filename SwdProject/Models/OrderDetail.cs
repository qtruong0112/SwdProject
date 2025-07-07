using System;
using System.Collections.Generic;

namespace SwdProject.Models;

public partial class OrderDetail
{
    public long Id { get; set; }

    public double Price { get; set; }

    public long Quantity { get; set; }

    public long? OrderId { get; set; }

    public long? ProductId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
